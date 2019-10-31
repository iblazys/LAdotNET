using LAdotNET.Extensions;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Server
{
    class SMPCRoomStateNotify : Packet
    {
        /// <summary>
        /// Sends the actual gameserver ip address and result code
        /// </summary>
        public SMPCRoomStateNotify(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteIntLE(3182562);
            Data.WriteIntLE(0);
            Data.WriteIntLE(575880581);

            Data.WriteLAString("127.0.0.1:6040");

            Data.WriteLongLE(153234490); // some id? appears at end of CMSlaveLiberateRequest after character selection

            Data.WriteIntLE(13000); // RETURN CODE
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
