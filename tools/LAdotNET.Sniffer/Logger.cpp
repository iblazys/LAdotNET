#include "pch.h"
#include "logger.h"

Logger::Logger(Settings* settings) 
{
	this->settings = settings;
}

void Logger::WriteToConsole(const char* format, ...)
{
	if (!settings->LoggingEnabled)
		return;

	va_list args;
	va_start(args, format);

	vprintf(format, args);

	va_end(args);
}

void Logger::WriteToFile(int dataSize, u_short opcode, uint8_t compFlag, bool cryptFlag, uint8_t* packetData, ServerType serverType, std::string name, int order)
{
	std::ostringstream fileName;

	int sizeWithHeader = dataSize + 6;

	switch (serverType)
	{
		case LOGINSERVER:
			fileName << "plogs/LOGINSERVER/" << order << "_" << name << "_" << sizeWithHeader << ".bin";
			break;
		case GAMESERVER:
			fileName << "plogs/GAMESERVER/" << order << "_" << name << "_" << sizeWithHeader << ".bin";
			break;
		case WORLDSERVER:
			fileName << "plogs/WORLDSERVER/" << order << "_" << name << "_" << sizeWithHeader << ".bin";
			break;
	}

	// Write it

	unsigned char crypt = cryptFlag ? 1 : 0;
	auto file = std::fstream(fileName.str(), std::ios::out | std::ios::binary);
	file.write(reinterpret_cast<const char*>(&sizeWithHeader), sizeof(unsigned short));
	file.write(reinterpret_cast<const char*>(&opcode), sizeof(unsigned short));
	file.write(reinterpret_cast<const char*>(&compFlag), sizeof(unsigned char));
	file.write(reinterpret_cast<const char*>(&crypt), sizeof(unsigned char));
	file.write((char*)packetData, dataSize);
	file.close();
}

void Logger::WriteToFile(uint8_t* data, int size)
{
	auto file = std::fstream("plogs/xorKey.bin", std::ios::out | std::ios::binary);
	file.write((char*)data, size);
	file.close();
}