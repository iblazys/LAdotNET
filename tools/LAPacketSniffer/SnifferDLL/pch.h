// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

#define WIN32_LEAN_AND_MEAN // Exclude rarely-used stuff from Windows headers

#pragma comment(lib, "Ws2_32.lib")

#include <windows.h>
#include <iostream>
#include <MinHook.h>
#include <Winsock2.h>
#include <thread> // for pipes - this could be bad af
#include <string>
#include <WinInet.h>
#include <iomanip>
#include <ctime>
#include "WS2tcpip.h"

#include <sstream>
#include <fstream>

#include "NamedPipes.h"
#include "Hooks.h"
#include "Logger.h"
#include "Hexdump.h"
#include "Globals.h"
#include "FileSystem.h"
#endif //PCH_H
