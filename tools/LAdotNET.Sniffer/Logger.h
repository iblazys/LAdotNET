#pragma once
class Logger 
{
	private:
		Settings* settings;

	public:
		Logger(Settings* settings);

		void WriteToConsole(const char* format, ...);

		void WriteToFile(int dataSize, 
			u_short opcode, 
			uint8_t compFlag, 
			bool cryptFlag, 
			uint8_t* packetData, 
			ServerType serverType, 
			std::string name,
			int order);

		void WriteToFile(uint8_t* data, int size);
};