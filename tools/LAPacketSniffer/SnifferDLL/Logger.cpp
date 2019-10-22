#include "pch.h"
#include "Logger.h"

bool Logger::DoLog = true;

void Logger::ConsoleLog(std::string msg)
{
	if (Logger::DoLog)
		std::cout << msg << std::endl;
}