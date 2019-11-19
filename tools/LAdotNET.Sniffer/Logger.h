#pragma once
class Logger 
{
	private:
		Settings* settings;

	public:
		Logger(Settings* settings);

		void WriteToConsole(const char* format, ...);

		void WriteToFile(int packetSize, 
			u_short opcode, 
			uint8_t compFlag, 
			bool cryptFlag, 
			uint8_t* packetData, 
			ServerType serverType, 
			std::string name);

		void WriteToFile(uint8_t* data, int size);
};