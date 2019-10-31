using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Server
{
    class SMVoyageLinerUpdateNotify : Packet
    {
        public SMVoyageLinerUpdateNotify(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteBytes(new byte[] 
            {
                0x00, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0xA0, 0x0F, 0x00, 0x00, 0x86, 0xA0, 0x4D, 0x08,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xA0, 0x0F, 0x00, 0x00, 0x00, 0x00
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
