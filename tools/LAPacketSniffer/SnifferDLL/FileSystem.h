#pragma once
class FileSystem
{
public:
	static bool LogFiles;
	static void WriteToFile(uint8_t* packetData, int packetSize, PacketType packetType);
private:
	static int EncryptedFCounter;
	static int DecryptedFCounter;
	static int HttpFCounter;
};

