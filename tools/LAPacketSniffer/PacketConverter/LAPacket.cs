namespace PacketConverter
{
    public enum Origin
    {
        CLIENT,
        SERVER
    };

    public enum CompressionType
    {
        NONE,
        LZ4,
        SNAPPY
    }

    public class LAPacket
    {
        // For use by this program only
        public string SourceFile;
        public int OrderNum;
        public Origin Origin;

        // Actual Packet
        public ushort Size;
        public ushort Opcode;
        public CompressionType CompressionType;
        public byte EncryptionFlag;

        public byte[] Data;
    }
}
