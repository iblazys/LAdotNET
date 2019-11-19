#pragma once
class Settings
{
	public:

		// Global settings
		uintptr_t ModuleBase = 0;
		DWORD ModuleSize = 0;

		// Xor Key
		uint8_t* XorKey;
		uintptr_t KeyGrabberADDR = 0;

		// Ini Settings
		bool LoggingEnabled;

		bool RedirectConnections;
		std::string RedirectIP;

		// Counters
		Counter* LoginCounter;
		Counter* GameCounter;
		Counter* WorldCounter;

		// Opcodes
		std::map<int, std::string> m_Opcodes;

		Settings();

		bool Load();
		bool LoadOpcodes();
		void SetXorKey(uint8_t* key);
};

