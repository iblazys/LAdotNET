using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Server
{
    class SMPCNameCheckResult : Packet
    {
        public SMPCNameCheckResult(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            // Did not change 1.6.2.3 - 1.6.4.1
            Data.WriteBytes(new byte[]
            {
                0x09, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x57, 0x00, 0x61, 0x00, 0x72, 0x00, 0x72, 0x00, 0x69, 0x00,
                0x6F, 0x00, 0x72, 0x00, 0xB0, 0x36, 0x00, 0x00
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
