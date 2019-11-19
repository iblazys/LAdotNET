// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

void InitSniffer() 
{
	Settings* settings = new Settings();
	Logger* logger = new Logger(settings);
	Scanner* scanner = new Scanner(settings, logger);

	settings->Load();

	if (settings->LoggingEnabled)
	{
		AllocConsole();
		freopen_s((FILE**)stdout, "CONOUT$", "w", stdout);
		logger->WriteToConsole("LAdotNET packet sniffer for Lost Ark KR \n");
	}

	settings->LoadOpcodes();
	logger->WriteToConsole("Loaded %i opcodes \n", settings->m_Opcodes.size());

	// Initialize MinHook.
	if (MH_Initialize() != MH_OK)
	{
		logger->WriteToConsole("MH_Initialize failed \n");
		return;
	}

	scanner->Init();

	if (!scanner->RunScans())
	{
		logger->WriteToConsole("One or more scans failed to complete. Closing in 5 seconds. \n");
		Sleep(5000);
		return;
	}

	logger->WriteToConsole("Loaded Base Address: %llx \n", settings->ModuleBase);
	logger->WriteToConsole("Loaded Module Size: %i \n", settings->ModuleSize);
	logger->WriteToConsole("Found KeyGrabber Address: %p \n", settings->KeyGrabberADDR);
	logger->WriteToConsole("Enabling hooks... \n");

	// Init Hooks
	Hooks::Setup(settings, logger);

	logger->WriteToConsole("Launching game.... \n\n");
	
}

BOOL APIENTRY DllMain(HMODULE hDLL, DWORD Reason, LPVOID Reserved)
{
	switch (Reason)
	{
	case DLL_PROCESS_ATTACH:
		DisableThreadLibraryCalls(hDLL);
		InitSniffer();
		break;
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

