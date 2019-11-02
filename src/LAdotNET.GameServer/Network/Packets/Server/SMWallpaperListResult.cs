using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Server
{
    class SMWallpaperListResult : Packet
    {
        public SMWallpaperListResult(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            // Only thing that changed was the ID at the end of the packet
            Data.WriteBytes(new byte[]
            {
                /* 1.6.2.3
                0xA1, 0x86, 0x01, 0x00,
                0x01, 0x00, 
                0xA1, 0x86, 0x01, 0x00, 
                0x01, 
                0xE3, 0xA7, 0xD1, 0x12, 
                0xC0, 0x0B, 0x00, 0x00
                */

                0xA1, 0x86, 0x01, 0x00, 
                0x01, 0x00, 
                0xA1, 0x86, 0x01, 0x00, 
                0x01, 0xE3, 
                0xB7, 0x02, 0xA4, 
                0xC3, 0x39, 0x00, 0x00
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
