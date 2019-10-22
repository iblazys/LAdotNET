#pragma once
class NamedPipes
{

public:
	static bool IsConnected;
	static HANDLE pipeHandle;

	NamedPipes()
	{
		IsConnected = false;
		pipeHandle = NULL;
	}

	static int StartPipeServer();
	static int SendPipeBytes(const uint8_t* byteData, int length);
};

