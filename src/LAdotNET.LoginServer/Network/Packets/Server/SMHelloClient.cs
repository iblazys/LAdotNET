using LAdotNET.Extensions;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.LoginServer.Network.Packets.Server
{
    public class SMHelloClient : Packet
    {
        public SMHelloClient(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            IsEncrypted = false;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            //Logger.Info($"Account ID: {conn.Account}");

            Data.WriteBytes(new byte[]
            {
                0x03, 0xCB, 0xEA, 0xB6, // first 4 bytes of xor key
            });

            Data.WriteLAString("1.284.314.875818(CL:875818) (2019-10-15 19:20:52)");

            Data.WriteBytes(new byte[]
            {
                0x07, 0x09, 0x71, 0x0D // unk // THIS DOES NOT SEEM LIKE A CRC - I HAVE CHANGED IT AND CLIENT STILL ACCEPTED IT.
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
