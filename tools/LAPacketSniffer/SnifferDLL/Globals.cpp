#include "pch.h"
#include "Globals.h"

// THESE MUST BE UPDATED WITH NEW PATCHES
// USE LAOffsetUpdater.exe

// 1.5.9.2,241
//const DWORD Globals::EncryptOffset = 0xBABEF0;
//const DWORD Globals::DecryptOffset = 0x1A8EAF0;

// 1.5.10.1
//const DWORD Globals::EncryptOffset = 0xBACFC0;
//const DWORD Globals::DecryptOffset = 0x1A98120;

// 1.6.2.3
const DWORD Globals::EncryptOffset = 0xC742D0;
const DWORD Globals::DecryptOffset = 0x1BBD090;

// SET THIS TO ENABLE / DISABLE CONNECTION RE-ROUTING
const bool Globals::RerouteConnections = true;

uint64_t Globals::BaseAddress = 0; // gets set in dllmain