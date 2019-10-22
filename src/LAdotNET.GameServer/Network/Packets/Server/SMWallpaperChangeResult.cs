using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Server
{
    class SMWallpaperChangeResult : Packet
    {
        public SMWallpaperChangeResult(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteBytes(new byte[]
            {
                0xA1, 0x86, 0x01, 0x00, 0x01, 0x00, 0xA1, 0x86, 0x01, 0x00, 0x01, 0xE3, 0xA7, 0xD1, 0x12, 0xC0,
                0x0B, 0x00, 0x00
            });
        }

        public override Task HandleAsync()
        {
            throw new NotImplementedException();
        }

        public override void Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
