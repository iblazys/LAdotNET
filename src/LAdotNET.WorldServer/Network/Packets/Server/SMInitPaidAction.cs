using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Server
{
    class SMInitPaidAction : Packet
    {
        public SMInitPaidAction(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteBytes(new byte[]
            {
                0x03, 0x00, 0x01, 0x0B, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x6C, 0x17,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xFF, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00
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
