#include "pch.h"
#include "NamedPipes.h"

HANDLE NamedPipes::pipeHandle;
bool NamedPipes::IsConnected = false;

int NamedPipes::StartPipeServer()
{
	std::wcout << "Creating an instance of a named pipe..." << std::endl;

	// Create a pipe to send data
	pipeHandle = CreateNamedPipe(
		L"\\\\.\\pipe\\win_defender", // name of the pipe
		PIPE_ACCESS_DUPLEX, // 1-way pipe -- send only
		PIPE_TYPE_BYTE, // send data as a byte stream
		1, // only allow 1 instance of this pipe
		4096, // no outbound buffer
		0, // no inbound buffer
		0, // use default wait time
		NULL // use default security attributes
	);

	if (pipeHandle == NULL || pipeHandle == INVALID_HANDLE_VALUE) {
		std::wcout << "Failed to create outbound pipe instance.";
		// look up error code here using GetLastError()
		return -1;
	}

	std::wcout << "Waiting for a client to connect to the pipe..." << std::endl;

	// This call blocks until a client process connects to the pipe
	BOOL result = ConnectNamedPipe(pipeHandle, NULL);
	if (!result) {
		std::wcout << "Failed to make connection on named pipe." << std::endl;
		// look up error code here using GetLastError()
		CloseHandle(pipeHandle); // close the pipe
		return -1;
	}

	std::wcout << "Client connected... exiting method" << std::endl;
	NamedPipes::IsConnected = true;
	return 0;
}

int NamedPipes::SendPipeBytes(const uint8_t* byteData, int length) 
{
	if (!IsConnected) 
	{
		std::wcout << "Client is disconnected, aborting sending data" << std::endl;
		return 1;
	}
		
	std::wcout << "Sending data to pipe..." << std::endl;

	// This call blocks until a client process reads all the data
	//const wchar_t *data = L"*** Hello Pipe World ***";	//original

	DWORD numBytesWritten = 0;
	bool result = WriteFile(
		NamedPipes::pipeHandle, // handle to our outbound pipe
		byteData, // data to send
		length * sizeof(uint8_t), // length of data to send (bytes)
		&numBytesWritten, // will store actual amount of data sent
		NULL // not using overlapped IO
	);


	if (result) {
		std::wcout << "Number of bytes sent: " << numBytesWritten << std::endl;
	}
	else {
		std::wcout << "Failed to send data." << std::endl;
		// look up error code here using GetLastError()
	}

	delete[] byteData;

	return 1;
}