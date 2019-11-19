#include "pch.h"

LAPacket::LAPacket(Settings* _settings, Logger* _logger)
{
	settings = _settings;
	logger = _logger;

	Size = 0;
	Opcode = 0;
	CompressionFlag = 0;
	IsEncrypted = false;

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

		Data = new uint8_t[Size - 6];
		buf->getBytes(Data, Size - 6);

		Name = GetPacketName();
		Type = GetServerType(port);
		Sender = sender;

		if (IsEncrypted) 
		{
			if (Sender == SERVER) 
			{
				Decrypt(Opcode);
			}
			else
			{
				int count = GetCounterType()->getCount();
				Decrypt(count);
				logger->WriteToConsole("Decrypting client packet with count [%i] \n", count);
			}
		}

		if (CompressionFlag == 2)
			Decompress();

		logger->WriteToConsole("%s -> [%s] len[%i] op[0x%04X] compressed[%i] encrypted[%s] - requestSize[%i] \n",
			sender ? "SERVER" : "CLIENT",
			Name.c_str(),
			Size,
			Opcode,
			CompressionFlag,
			IsEncrypted ? "TRUE" : "FALSE",
			buf->size()
		);

		// SAVE TO FILE
		logger->WriteToFile(Size, Opcode, CompressionFlag, IsEncrypted, Data, Type, Name);
	}
}

void LAPacket::Decrypt(int seed)
{
	int dataLength = Size - 6;

	for (int i = 0; i < dataLength; i++)
	{
		Data[i] ^= settings->XorKey[(seed & 0xFF) + 4];

		seed++;
	}

	if(Sender == CLIENT)
		GetCounterType()->Increase();
}

void LAPacket::Decompress()
{
	size_t result;
	snappy_uncompressed_length(reinterpret_cast<const char*>(Data), Size - 6, &result);

	uint8_t* newData = new uint8_t[result];

	snappy_uncompress((const char*)Data, Size - 6, (char*)newData, &result);

	delete[] Data;

	Data = new uint8_t[result];
	std::memcpy(Data, newData, result);

	delete[] newData;

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
