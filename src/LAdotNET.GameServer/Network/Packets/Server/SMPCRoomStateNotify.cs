using LAdotNET.Extensions;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Server
{
    class SMPCRoomStateNotify : Packet
    {
        public SMPCRoomStateNotify(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteIntLE(13000); // RETURN CODE
            Data.WriteIntLE(1940933313); // unk

            Data.WriteUnicodeString("127.0.0.1:6040");

            Data.WriteLongLE(3182562); // some id? also in SMWorldCancelResult
            Data.WriteLongLE(153275889); // some session id? appears at end of CMSlaveLiberateRequest after character selection


            /*
            // If we send this followed by SMAuthError we can get the account passcode to show up

            Data.WriteIntLE(13010); // RETURN CODE
            Data.WriteLongLE(0);
            Data.WriteLongLE(0);
            Data.WriteIntLE(0);
            Data.WriteShortLE(0);

            */
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
