#include "pch.h"

static Settings* settings;
static Logger* logger;

bool keyGrabbed = false;

void Hooks::Setup(Settings* _settings, Logger* _logger)
{
	settings = _settings;
	logger = _logger;

	// WS2 Functions - packets are encrypted when received / sent through here.
	MH_CreateHookApi(L"Ws2_32.dll", "send", Hooks::hSend_Callback, (LPVOID*)&Hooks::pSend_Original);
	MH_CreateHookApi(L"Ws2_32.dll", "WSARecv", Hooks::hWSARecv_Callback, (LPVOID*)&Hooks::pWSARecv_Original);
	MH_CreateHookApi(L"Ws2_32.dll", "WSASend", Hooks::hWSASend_Callback, (LPVOID*)&Hooks::pWSASend_Original);

	// KeyGrabber (Also has decrypted server packets)
	MH_CreateHook((LPVOID)settings->KeyGrabberADDR, Hooks::KeyGrabber_Callback, (LPVOID*)&Hooks::KeyGrabber_Original);

	// Connection rerouting
	if (settings->RedirectConnections)
	{
		logger->WriteToConsole("Enabling connection redirection. \n");
		MH_CreateHookApi(L"wininet.dll", "InternetConnectW", Hooks::hInternetConnectW_Callback, (LPVOID*)&Hooks::pInternetConnectW_Original);
		MH_CreateHookApi(L"Ws2_32.dll", "connect", Hooks::hConnect_Callback, (LPVOID*)&Hooks::pConnect_Original);
	}

	// Enable all the hooks
	if (MH_EnableHook(MH_ALL_HOOKS) == MH_OK)
	{
		Beep(523, 100);
	}
}

// PACKET LOGGING

// CLIENT -> SERVER
int WINAPI Hooks::hSend_Callback(SOCKET s, const char* buf, int sendLen, int flags)
{
	int port = util::GetPortNumber(s);

	// TODO: HTTP
	if(port != 10011 || port != 8888) 
	{
		LAPacket packet(settings, logger);
		ByteBuffer buffer(sendLen);
		buffer.putBytes(*(uint8_t**)(&buf), sendLen);

		packet.Parse(&buffer, CLIENT, port);
	}

	return pSend_Original(s, buf, sendLen, flags);
}

// CLIENT -> SERVER
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
				DWORD bytesSent = *lpNumberOfBytesSent;
				int port = util::GetPortNumber(s);

				logger->WriteToConsole("wsasend called \n");
			}
		}
	}

	return ret;
}

// SERVER -> CLIENT
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
				DWORD bytesRecv = *lpNumberOfBytesRecvd;
				
				int port = util::GetPortNumber(s);

				LAPacket packet(settings, logger);
				ByteBuffer buffer(bytesRecv);
				buffer.putBytes((uint8_t*)lpBuffers[i].buf, bytesRecv);

				packet.Parse(&buffer, SERVER, port);
			}
		}
	}

	return ret;
}

// XOR KEY
__int64 __fastcall Hooks::KeyGrabber_Callback(__int64* a1, __int64 a2, __int64* a3)
{
	auto ret = KeyGrabber_Original(a1, a2, a3);

	if (!keyGrabbed)
	{
		uint8_t* xorKey = reinterpret_cast<uint8_t*>(*(a1 + 0x7)); // add 56 bytes to ptr, (7 * 8 = 56)
		settings->SetXorKey(xorKey);
		logger->WriteToFile(xorKey, 260);
		logger->WriteToConsole("KeyGrabber: Success - 'xorkey.bin'\n");
		keyGrabbed = true;
	}

	// Can read packet data here but meh
	return ret;
}

// REDIRECT
HINTERNET WINAPI Hooks::hInternetConnectW_Callback(HINTERNET hInternet, LPCWSTR ServerName, INTERNET_PORT InternetPort, LPCWSTR UserName, LPCWSTR Password, DWORD dwService, DWORD dwFlags, DWORD_PTR dwContext)
{
	std::wstring original(ServerName);

	std::wstring stemp = std::wstring(settings->RedirectIP.begin(), settings->RedirectIP.end());
	LPCWSTR redirect = stemp.c_str();

	logger->WriteToConsole("HTTP Redirect: [%s:%i] -> [%s:%i]\n",
		std::string(original.begin(), original.end()).c_str(),
		InternetPort,
		settings->RedirectIP.c_str(),
		InternetPort
	);
	
	return Hooks::pInternetConnectW_Original(hInternet, redirect, InternetPort, UserName, Password, dwService, dwFlags, dwContext);
}

int WINAPI Hooks::hConnect_Callback(SOCKET s, const struct sockaddr* name, int namelen)
{
	struct sockaddr_in* addr_in = (struct sockaddr_in*)name;
	char* ip = inet_ntoa(addr_in->sin_addr);
	u_short port = ntohs(addr_in->sin_port);

	logger->WriteToConsole("TCP Redirect: [%s:%i] -> [%s:%i]\n", 
		ip,
		port,
		settings->RedirectIP.c_str(),
		port
	);

	struct sockaddr_in newTarget;
	newTarget.sin_family = AF_INET;
	newTarget.sin_addr.s_addr = inet_addr(settings->RedirectIP.c_str());
	newTarget.sin_port = addr_in->sin_port; //htons(addr_in->sin_port); //htons(6010);

	return  Hooks::pConnect_Original(s, (struct sockaddr*) & newTarget, namelen);
}
