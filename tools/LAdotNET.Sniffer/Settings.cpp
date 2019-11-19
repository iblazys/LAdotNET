#include "pch.h"
#include "settings.h"

Settings::Settings() 
{
	LoginCounter = new Counter();
	GameCounter = new Counter();
	WorldCounter = new Counter();
}

bool Settings::Load() 
{
	INIReader reader("../../x64lahook.ini"); // Take us to LOSTARK Dir

	if (reader.ParseError() != 0) {
		std::cout << "Can't load 'x64lahook.ini'\n";
		return false;
	}

	// Settings
	LoggingEnabled = reader.GetBoolean("Logging", "Enabled", false);
	RedirectConnections = reader.GetBoolean("Connections", "Redirect", false);
	RedirectIP = reader.Get("Connections", "RedirectIP", "127.0.0.1");

	return true;
}

bool Settings::LoadOpcodes() 
{
	std::string opcode;
	std::string name;

	std::fstream infile("plogs/opcodes.dat");
	std::string textline;
	while (std::getline(infile, textline))
	{
		std::string comma_string;
		std::istringstream text_stream(textline);
		text_stream >> opcode;
		std::getline(text_stream, name, '\n');
		text_stream >> name;

		auto intOpcode = strtoul(opcode.c_str(), NULL, 0);

		name.erase(std::remove_if(name.begin(), name.end(), isspace), name.end());

		m_Opcodes.insert(std::pair<int, std::string>(intOpcode, name));

		//std::cout << "OP: " << opcode << " NAME: [" << name << "] OPCODE INT: " << intOpcode << std::endl;
	}
}

void Settings::SetXorKey(uint8_t* key)
{
	XorKey = new uint8_t[260];
	std::memcpy(XorKey, key, 260);
}