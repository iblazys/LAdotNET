// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

void PipeStarter()
{
	NamedPipes::StartPipeServer();
}

BOOL APIENTRY DllMain(HMODULE hDLL, DWORD Reason, LPVOID Reserved)
{
    switch (Reason)
    {
    case DLL_PROCESS_ATTACH:

		//DisableThreadLibraryCalls(hDLL);

		// Initialize MinHook.
		if (MH_Initialize() != MH_OK)
		{
			OutputDebugString(L"MH_Initialize failed");
			return 1;
		}

		AllocConsole();
		freopen_s((FILE**)stdout, "CONOUT$", "w", stdout);
		Logger::DoLog = true; // Console logging

		FileSystem::LogFiles = true;
		Globals::BaseAddress = (uint64_t)GetModuleHandleA("LOSTARK.exe");

		// NAMED PIPES ARE NOT BEING USED

		Hooks::SetupHooks();

		//CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)PipeStarter, NULL, NULL, NULL);

    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

