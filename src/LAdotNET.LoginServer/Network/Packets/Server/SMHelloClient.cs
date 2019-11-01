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
                0x20, 0xC9, 0xA7, 0x42 // first 4 bytes of xor key
            });

            //Data.WriteLAString("1.284.314.875818(CL:875818) (2019-10-15 19:20:52)");
            Data.WriteLAString("1.75.126.889862(CL:889862) (2019-10-30 13:38:49)");

            Data.WriteBytes(new byte[]
            {
                0xC2, 0x3B, 0x6D, 0xC8 // unk // THIS DOES NOT SEEM LIKE A CRC - I HAVE CHANGED IT AND CLIENT STILL ACCEPTED IT.
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
