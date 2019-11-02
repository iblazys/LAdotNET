using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Server
{
    class SMInitPropCollecting : Packet
    {
        public SMInitPropCollecting(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteBytes(new byte[]
            {
                0x01, 0x00, 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0xDC, 0x05, 0x00, 0x00, 0xDC, 0x05, 0x00, 0x00, 0xDC, 0x05, 0x00, 0x00, 0xDC, 0x05, 0x00, 0x00,
                0xDC, 0x05, 0x00, 0x00
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
