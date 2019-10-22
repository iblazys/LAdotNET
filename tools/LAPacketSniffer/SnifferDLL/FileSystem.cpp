#include "pch.h"
#include "FileSystem.h"

int FileSystem::EncryptedFCounter = 0;
int FileSystem::DecryptedFCounter = 0;
int FileSystem::HttpFCounter = 0;

bool FileSystem::LogFiles = true;

void FileSystem::WriteToFile(uint8_t* packetData, int packetSize, PacketType packetType)
{
	if (!LogFiles)
		return;

	std::ostringstream outputFile;
	//__int64 time = std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now().time_since_epoch()).count();

	auto t = std::time(nullptr);
	auto tm = *std::localtime(&t);
	auto time = std::put_time(&tm, "%d-%m-%Y-%H-%M-%S");

	//auto time = date::format("%F-%T", std::chrono::system_clock::now());
	// plogs/encrypted/pkt_order_time_size.bin
	// plogs/decrypted/pkt_order_time_size.bin
	switch (packetType) 
	{
	case PacketType::Server:
		outputFile << "plogs/encrypted/" << "pkt_" << FileSystem::EncryptedFCounter << "_" << time << "_" << packetSize << "_SERVER.bin";
		EncryptedFCounter++;
		break;
	case PacketType::Client:
		outputFile << "plogs/encrypted/" << "pkt_" << FileSystem::EncryptedFCounter << "_" << time << "_" << packetSize << "_CLIENT.bin";
		EncryptedFCounter++;
		break;
	case PacketType::DecryptedServer:
		outputFile << "plogs/decrypted/" << "pkt_" << FileSystem::DecryptedFCounter << "_" << time << "_" << packetSize << "_SERVER.bin";
		DecryptedFCounter++;
		break;
	case PacketType::DecryptedClient:
		outputFile << "plogs/decrypted/" << "pkt_" << FileSystem::DecryptedFCounter << "_" << time << "_" << packetSize << "_CLIENT.bin";
		DecryptedFCounter++;
		break;
	case PacketType::HTTP:
		outputFile << "plogs/http/" << "pkt_" << FileSystem::HttpFCounter << "_" << time << "_" << packetSize << ".bin";
		HttpFCounter++;
		break;
	}

	// Write it
	auto file = std::fstream(outputFile.str(), std::ios::out | std::ios::binary);
	file.write((char*)packetData, packetSize);
	file.close();
}
