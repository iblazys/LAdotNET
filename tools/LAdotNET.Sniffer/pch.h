// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H
#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers

#pragma comment(lib, "Ws2_32.lib")

#include <windows.h>
#include <stdio.h>
#include <iostream>
#include <fstream>
#include "WS2tcpip.h" // WINSOCK
#include <WinInet.h> // WININET
#include "psapi.h"
#include <mutex>
#include <sstream>
#include <map>

#include "MinHook.h"
#include "snappy-c.h"
#include "INIReader.h"
#include "ByteBuffer.hpp"

#include "enums.h"
#include "utils.h"

#include "Counter.h"
#include "settings.h"
#include "logger.h"
#include "scanner.h"
#include "Packet.h"
#include "hooks.h"

#endif //PCH_H
