#pragma once
class LAPacket {

	private:
		Logger* logger;
		Settings* settings;

		void Decrypt(int seed);
		void Decompress();

		ServerType GetServerType(int port);
		Counter* GetCounterType();
		int GetOrder();
		std::string GetPacketName();

	public:
		uint16_t Size;
		uint16_t Opcode;
		uint8_t CompressionFlag;
		bool IsEncrypted;
		uint8_t* Data;
		uint16_t DataSize;

		ServerType Type;
		std::string	Name;
		Origin Sender;
		int Order;

		LAPacket(Settings* settings, Logger* logger);

		void Parse(ByteBuffer* buf, Origin sender, int port);
};

