using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Server
{
    class SMServerTimeNotify : Packet
    {
        public SMServerTimeNotify(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteBytes(new byte[]
            {
                0x00, 0x00, 0x14, 0x21, 0x45, 0x0B, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE3, 0xB7, 0x02, 0xB4, 0xD5,
                0xE8, 0x00, 0x00, 0x04
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
