namespace PacketConverter
{
    class BinFile
    {
        public string SourceFile;

        public int OrderNum;
        public string Date;
        public int FileSize; // FILES CAN CONTAIN MORE THAN ONE PACKET
        public string PacketType;

        public byte[] Data;
    }
}
