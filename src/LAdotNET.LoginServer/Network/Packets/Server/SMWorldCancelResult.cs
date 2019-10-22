using LAdotNET.Extensions;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.LoginServer.Network.Packets.Server
{
    class SMWorldCancelResult : Packet
    {
        public SMWorldCancelResult(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteLongLE(3182562);    // UNK - could be an int instead?
            Data.WriteShortLE(0);         // UNK

            Data.WriteLAString("127.0.0.1:6020"); // GAMESERVER IP AND PORT

            Data.WriteIntLE(672421763);   // UNK

            Data.WriteIntLE(11000);   // RESULT CODE
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
