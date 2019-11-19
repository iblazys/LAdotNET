#include "pch.h"
#include "scanner.h"

Scanner::Scanner(Settings* settings, Logger* logger)
{
	this->settings = settings;
	this->logger = logger;
}

bool Scanner::Init()
{
	MODULEINFO modInfo = { 0 };
	HMODULE hModule = GetModuleHandle(nullptr);

	if (hModule != 0)
	{
		GetModuleInformation(GetCurrentProcess(), hModule, &modInfo, sizeof(MODULEINFO));

		settings->ModuleBase = (uintptr_t)modInfo.lpBaseOfDll;
		settings->ModuleSize = (DWORD)modInfo.SizeOfImage;

		return true;
	}
	
	return false;
}

bool Scanner::RunScans()
{
	// Xor key grabber scan
	settings->KeyGrabberADDR = (uintptr_t)Scan(
		"\x48\x8B\xC4\x56\x41\x54\x41\x55\x48\x83\xEC\x70\x45\x8B\x48\x14\x45\x8B\x50\x18\x4D\x8B\xE0\x45\x2B\xD1\x4C\x8B\xEA\x48\x8B\xF1\x41\x83\xFA\x06",
		"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", (char*)settings->ModuleBase, settings->ModuleSize);

	if (settings->KeyGrabberADDR == 0)
	{
		logger->WriteToConsole("Failed to find keygrabber address. \n");
		return false;
	}

	// Add moar

	return true;
}

char* Scanner::Scan(const char* pattern, const char* mask, char* begin, unsigned int size)
{
	unsigned int patternLength = strlen(mask);

	for (unsigned int i = 0; i < size - patternLength; i++)
	{
		bool found = true;
		for (unsigned int j = 0; j < patternLength; j++)
		{
			if (mask[j] != '?' && pattern[j] != *(begin + i + j))
			{
				found = false;
				break;
			}
		}
		if (found)
		{
			return (begin + i);
		}
	}
	return nullptr;
}
