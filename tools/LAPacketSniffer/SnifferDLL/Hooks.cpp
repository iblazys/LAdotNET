#include "pch.h"
#include "Hooks.h"

void Hooks::SetupHooks()
{
	// WS2 Functions - packets are encrypted when received / sent through here.
	MH_CreateHookApi(L"Ws2_32.dll", "send", Hooks::hSend_Callback, (LPVOID*)&Hooks::pSend_Original);
	MH_CreateHookApi(L"Ws2_32.dll", "WSARecv", Hooks::hWSARecv_Callback, (LPVOID*)&Hooks::pWSARecv_Original);
	MH_CreateHookApi(L"Ws2_32.dll", "WSASend", Hooks::hWSASend_Callback, (LPVOID*)&Hooks::pWSASend_Original);

	// In game encryption functions - get decrypted packets
	MH_CreateHook((LPVOID)(Globals::BaseAddress + Globals::EncryptOffset), Hooks::hClientPacketEncrypt_Callback, (LPVOID*)&Hooks::pClientPacketEncrypt_Original);
	MH_CreateHook((LPVOID)(Globals::BaseAddress + Globals::DecryptOffset), Hooks::hServerPacketDecrypt_Callback, (LPVOID*)&Hooks::pServerPacketDecrypt_Original);

	// Connection rerouting
	if (Globals::RerouteConnections)
	{
		MH_CreateHookApi(L"wininet.dll", "InternetConnectW", Hooks::hInternetConnectW_Callback, (LPVOID*)&Hooks::pInternetConnectW_Original);
		MH_CreateHookApi(L"Ws2_32.dll", "connect", Hooks::hConnect_Callback, (LPVOID*)&Hooks::pConnect_Original);
	}
		
	// Enable all the hooks
	if (MH_EnableHook(MH_ALL_HOOKS) == MH_OK)
	{
		Beep(523, 100);
	}
}

__int64 __fastcall Hooks::hClientPacketEncrypt_Callback(__int64* a1, __int64(__fastcall*** a2)(__int64, __int16*, signed __int64))
{
	auto ret = pClientPacketEncrypt_Original(a1, a2);

	//uint8_t* byteCheck = *reinterpret_cast<uint8_t**>(&a1);
	uint8_t* ptr = (uint8_t*)&a1;
	uint8_t* packetData = (ptr + 0x50);

	//printf("a1: %p\n", a1);
	//printf("byteCheck: %p\n", byteCheck);
	//printf("ptr: %p\n", ptr);
	//printf("packetData: %p\n", packetData);

	unsigned short packetSize = packetData[1] << 8 | packetData[0];
	//std::cout << "PKT SIZE: " << packetSize << std::endl;
	FileSystem::WriteToFile(packetData, packetSize, PacketType::DecryptedClient);

	/*
	uint8_t* tempBuf = new uint8_t[packetSize];
	memcpy(tempBuf, packetData, packetSize);
	std::thread(NamedPipes::SendPipeBytes, tempBuf, packetSize).detach(); // fire and forget?
	*/

	return ret;
}

__int64 __fastcall Hooks::hServerPacketDecrypt_Callback(__int64 a1, __int64 a2, __int64 *a3)
{
	auto ret = pServerPacketDecrypt_Original(a1, a2, a3);

	uint8_t* byteCheck = *reinterpret_cast<uint8_t**>(&a3);

	uint32_t checkFlag = *((uint32_t*)&byteCheck[0x14]);

	if (checkFlag >= 6) 
	{
		uint8_t* packetData = reinterpret_cast<uint8_t*>(*(a3+0x1)); // *(uint8_t**)(*(a3 + 1)); // add 8 bytes to ptr, and go into the pointer we are now sitting on top of
		unsigned short packetSize = packetData[1] << 8 | packetData[0];

		FileSystem::WriteToFile(packetData, packetSize, PacketType::DecryptedServer);

		/*
		uint8_t* tempBuf = new uint8_t[packetSize];
		memcpy(tempBuf, packetData, packetSize);
		std::thread(NamedPipes::SendPipeBytes, tempBuf, packetSize).detach(); // fire and forget?
		*/
	}

	return ret;
}

int WINAPI Hooks::hSend_Callback(SOCKET s, const char* buf, int len, int flags)
{
	PacketType pType = PacketType::Client;
	int port = GetPortNumber(s);

	if (port == 10011 || port == 8888)
		pType = PacketType::HTTP;

	FileSystem::WriteToFile(*(uint8_t**)(&buf), len, pType);
	
	/*
	uint8_t* tempBuf = new uint8_t[len];
	memcpy(tempBuf, *(uint8_t**)(&buf), len);
	std::thread(NamedPipes::SendPipeBytes, tempBuf, len).detach(); // fire and forget?
	*/

	return pSend_Original(s, buf, len, flags);
}

int __stdcall Hooks::hWSASend_Callback(SOCKET s, LPWSABUF lpBuffers, DWORD dwBufferCount, LPDWORD lpNumberOfBytesSent,
	DWORD dwFlags, LPWSAOVERLAPPED lpOverlapped, LPWSAOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine)
{
	int ret = pWSASend_Original(s, lpBuffers, dwBufferCount, lpNumberOfBytesSent, dwFlags, lpOverlapped, lpCompletionRoutine);

	if (!ret)
	{
		if (lpBuffers != NULL && dwBufferCount > 0)
		{
			for (DWORD i = 0; i < dwBufferCount; i++)
			{
				PacketType pType = PacketType::Client;
				DWORD bytesSent = *lpNumberOfBytesSent;
				int port = GetPortNumber(s);

				if (port == 10011 || port == 8888)
					pType = PacketType::HTTP;

				FileSystem::WriteToFile((uint8_t*)lpBuffers[i].buf, bytesSent, pType);

				/*
				uint8_t* tempBuf = new uint8_t[bytesSent];
				memcpy(tempBuf, (uint8_t*)lpBuffers[i].buf, bytesSent);
				std::thread(NamedPipes::SendPipeBytes, tempBuf, bytesSent).detach(); // fire and forget?
				*/
			}
		}
	}

	return ret;
}

int __stdcall Hooks::hWSARecv_Callback(int s, LPWSABUF lpBuffers, DWORD dwBufferCount, LPDWORD lpNumberOfBytesRecvd,
	LPDWORD lpFlags, LPWSAOVERLAPPED lpOverlapped, LPWSAOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine)
{
	int ret = pWSARecv_Original(s, lpBuffers, dwBufferCount, lpNumberOfBytesRecvd, lpFlags, lpOverlapped, lpCompletionRoutine);

	if (!ret)
	{
		if (lpBuffers != NULL && dwBufferCount > 0)
		{
			for (DWORD i = 0; i < dwBufferCount; i++)
			{
				PacketType pType = PacketType::Server;
				DWORD bytesRecv = *lpNumberOfBytesRecvd;
				int port = Hooks::GetPortNumber(s);

				if (port == 10011 || port == 8888)
					pType = PacketType::HTTP;

				FileSystem::WriteToFile((uint8_t*)lpBuffers[i].buf, bytesRecv, pType);

				/*
				uint8_t* tempBuf = new uint8_t[bytesRecv];
				memcpy(tempBuf, (uint8_t*)lpBuffers[i].buf, bytesRecv);
				std::thread(NamedPipes::SendPipeBytes, tempBuf, bytesRecv).detach(); // fire and forget?
				*/
			}
		}
	}

	return ret;
}

HINTERNET WINAPI Hooks::hInternetConnectW_Callback(HINTERNET hInternet, LPCWSTR ServerName, INTERNET_PORT InternetPort, LPCWSTR UserName, LPCWSTR Password, DWORD dwService, DWORD dwFlags, DWORD_PTR dwContext)
{
	std::wstring wide(ServerName);

	//Logger::ConsoleLog("Reroute [" + std::string(wide.begin(), wide.end()) + "] to [127.0.0.1]");

	return Hooks::pInternetConnectW_Original(hInternet, L"127.0.0.1", InternetPort, UserName, Password, dwService, dwFlags, dwContext);
}

int WINAPI Hooks::hConnect_Callback(SOCKET s, const struct sockaddr* name, int namelen)
{
	struct sockaddr_in* addr_in = (struct sockaddr_in*)name;
	char* ip = inet_ntoa(addr_in->sin_addr);

	Logger::ConsoleLog("- Attempted connection to [" + std::string(ip) + ":" + std::to_string(ntohs(addr_in->sin_port)) + "] rerouting to [127.0.0.1]");

	struct sockaddr_in newTarget;
	newTarget.sin_family = AF_INET;
	newTarget.sin_addr.s_addr = inet_addr("127.0.0.1");
	newTarget.sin_port = addr_in->sin_port; //htons(addr_in->sin_port); //htons(6010);

	return  Hooks::pConnect_Original(s, (struct sockaddr*) & newTarget, namelen);
}

// MOVE ME TO UTILS.CPP
int Hooks::GetPortNumber(SOCKET s)
{
	sockaddr_in sa = { 0 }; /* init to all zeros */
	socklen_t sl = sizeof(sa);
	if (getpeername(s, (sockaddr*)&sa, &sl))
		return -1;
	else
		return ntohs(sa.sin_port);
}