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
            Data.WriteIntLE(11000);   // RESULT CODE
            Data.WriteUnicodeString("127.0.0.1:6020"); // GAMESERVER IP AND PORT
            Data.WriteLongLE(3182562);    // SOME ID - ACCOUNT OR SESSION??
            Data.WriteShortLE(0);         // UNK
            Data.WriteIntLE(473741366);   // UNK - CHANGES EVERY TIME
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
