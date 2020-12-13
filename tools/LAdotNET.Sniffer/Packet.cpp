#include "pch.h"

LAPacket::LAPacket(Settings* _settings, Logger* _logger)
{
	settings = _settings;
	logger = _logger;

	Size = 0;
	Opcode = 0;
	CompressionFlag = 0;
	IsEncrypted = false;

	Order = 0;
	DataSize = 0;

	Data = nullptr;
}

void LAPacket::Parse(ByteBuffer* buf, Origin sender, int port)
{
	// Parses multiple packets from one request
	while(buf->bytesRemaining() > 0) 
	{
		Size = buf->getShort();
		Opcode = buf->getShort();
		CompressionFlag = buf->getChar();
		IsEncrypted = buf->getChar() ? true : false;

		DataSize = Size - 6;
		Data = new uint8_t[DataSize];
		buf->getBytes(Data, DataSize);

		Name = GetPacketName();
		Type = GetServerType(port);
		Order = GetOrder();
		Sender = sender;

		if (IsEncrypted) 
		{
			if (Sender == Origin::SERVER) 
			{
				Decrypt(Opcode);
			}
			else
			{
				Counter* _counter = GetCounterType();

				// TODO: Move these two opcodes to the ini file
				if (Opcode == 0xB48E || Opcode == 0x3AED)
				{
					// Reset counter on first client packet of each connection (back to server select screen etc)
					_counter->Reset();
				}

				Decrypt(_counter->getCount());

				logger->WriteToConsole("Decrypting client packet with count [%i] \n", _counter->getCount());
				_counter->Increase();
			}
		}

		if (CompressionFlag == 2)
			Decompress();

		// Verbose Loggerino
		logger->WriteToConsole("%s -> [%s] len[%i] op[0x%04X] compressed[%i] encrypted[%s] - requestSize[%i] \n",
			sender ? "SERVER" : "CLIENT",
			Name.c_str(),
			DataSize,
			Opcode,
			CompressionFlag,
			IsEncrypted ? "TRUE" : "FALSE",
			buf->size()
		);

		// TODO: Log Raw Packets
		logger->WriteToFile(DataSize, Opcode, CompressionFlag, IsEncrypted, Data, Type, Name, Order);
		util::hexdump(Data, DataSize);
	}
}

/**
 * Decrypts packet data
 *
 * @param seed opcode if server packet, sequence number if client packet
 */
void LAPacket::Decrypt(int seed)
{
	// Super insane encryption by Smilegate - very very advanced
	for (int i = 0; i < DataSize; i++)
	{
		Data[i] ^= settings->XorKey[(seed & 0xFF) + 4];
		seed++;
	}
}

void LAPacket::Decompress()
{
	size_t result;
	auto res = snappy_uncompressed_length(reinterpret_cast<const char*>(Data), DataSize, &result);

	if (res == SNAPPY_OK) 
	{
		uint8_t* newData = new uint8_t[result];

		auto uncompressRes = snappy_uncompress((const char*)Data, DataSize, (char*)newData, &result);
		if (uncompressRes == SNAPPY_OK)
		{
			delete[] Data;

			DataSize = result - 16;
			Data = new uint8_t[DataSize];
			std::memcpy(Data, newData + 16, DataSize);

			logger->WriteToConsole("Decompressing packet [%s] size before[%i] size after[%i] \n", Name.c_str(), Size - 6, DataSize);

			delete[] newData;
		}
		else
		{
			logger->WriteToConsole("Failed to decompress packet, snappy result [%d] \n", (int)uncompressRes);
		}
	}
	else
	{
		logger->WriteToConsole("Failed to get uncompressed length, snappy result [%d] \n", (int)res);
	}

	//util::hexdump(Data, result);
}

std::string LAPacket::GetPacketName() 
{
	auto it = settings->m_Opcodes.find(Opcode);

	if (it != settings->m_Opcodes.end())
		return it->second;
	else
		return "UNKNOWN";
}

ServerType LAPacket::GetServerType(int port)
{
	switch (port) 
	{
		case 6010:
			return LOGINSERVER;
		case 6020:
			return GAMESERVER;
		case 6040:
			return WORLDSERVER;
	}
}

Counter* LAPacket::GetCounterType() 
{
	switch (Type) 
	{
		case LOGINSERVER:
			return settings->LoginCounter;
		case GAMESERVER:
			return settings->GameCounter;
		case WORLDSERVER:
			return settings->WorldCounter;
	}
}

int LAPacket::GetOrder()
{
	int order = 0;

	switch (Type)
	{
		case LOGINSERVER:
			order = settings->LoginOrder;
			settings->LoginOrder++;
			break;
		case GAMESERVER:
			order = settings->GameOrder;
			settings->GameOrder++;
			break;
		case WORLDSERVER:
			order = settings->WorldOrder;
			settings->WorldOrder++;
			break;
	}

	return order;
}
