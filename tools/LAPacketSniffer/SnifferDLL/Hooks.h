#pragma once
#ifndef HOOKS_H
#define HOOKS_H

enum class PacketType { HTTP, Server, Client, DecryptedServer, DecryptedClient };

namespace Hooks
{
	// lostark.exe packet tramps
	static __int64 (__fastcall * pClientPacketEncrypt_Original)(__int64* a1, __int64(__fastcall*** a2)(__int64, __int16*, signed __int64));
	static __int64(__fastcall* pServerPacketDecrypt_Original)(__int64 a1, __int64 a2, __int64 *a3);

	// lostark.exe packet callbacks
	static __int64 __fastcall hClientPacketEncrypt_Callback(__int64* a1, __int64(__fastcall*** a2)(__int64, __int16*, signed __int64));
	static __int64 __fastcall hServerPacketDecrypt_Callback(__int64 a1, __int64 a2, __int64 *a3);

	// packet tramps
	static int (WINAPI* pSend_Original)(SOCKET s, const char* buf, int len, int flags);
	static int (WINAPI* pRecv_Original)(SOCKET s, char* buf, int len, int flags);
	static int(__stdcall* pWSASend_Original)(SOCKET s, LPWSABUF lpBuffers, DWORD dwBufferCount, LPDWORD lpNumberOfBytesSent,
		DWORD dwFlags, LPWSAOVERLAPPED lpOverlapped, LPWSAOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine);
	static int(__stdcall* pWSARecv_Original)(int s, LPWSABUF lpBuffers, DWORD dwBufferCount, LPDWORD lpNumberOfBytesRecvd,
		LPDWORD lpFlags, LPWSAOVERLAPPED lpOverlapped, LPWSAOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine);

	// connection tramps
	static int (WINAPI* pConnect_Original)(SOCKET s, const struct sockaddr* name, int namelen);
	static HINTERNET(WINAPI* pInternetConnectW_Original)(HINTERNET, LPCWSTR, INTERNET_PORT, LPCWSTR, LPCWSTR, DWORD, DWORD, DWORD_PTR);
	
	// packet callbacks
	static int WINAPI hSend_Callback(SOCKET s, const char* buf, int len, int flags);
	static int __stdcall hWSASend_Callback(SOCKET s, LPWSABUF lpBuffers, DWORD dwBufferCount, LPDWORD lpNumberOfBytesSent,
		DWORD dwFlags, LPWSAOVERLAPPED lpOverlapped, LPWSAOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine);
	static int __stdcall hWSARecv_Callback(int s, LPWSABUF lpBuffers, DWORD dwBufferCount, LPDWORD lpNumberOfBytesRecvd,
		LPDWORD lpFlags, LPWSAOVERLAPPED lpOverlapped, LPWSAOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine);
	
	// connection callbacks
	static int WINAPI hConnect_Callback(SOCKET s, const struct sockaddr* name, int namelen);
	static HINTERNET WINAPI hInternetConnectW_Callback(HINTERNET, LPCWSTR, INTERNET_PORT, LPCWSTR, LPCWSTR, DWORD, DWORD, DWORD_PTR);

	void SetupHooks( );

	int GetPortNumber(SOCKET);
};

#endif // !HOOKS_H
