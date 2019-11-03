using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Server
{
    class SMMoveNotify : Packet
    {
        public SMMoveNotify(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteBytes(new byte[]
            {
                0xC4, 0xE5, 0x69, 0x27, 0x80, 0xF8, 0x03, 0xC4, 0x3F, 0x00, 0xEC, 0xE7, 0x04, 0x2B, 0x00, 0x00,
                0x00, 0x00, 0x01, 0x01, 0x42, 0x06, 0x65, 0x27, 0xE0, 0xF8, 0x03, 0xC4, 0x3F, 0x00
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
