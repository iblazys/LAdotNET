using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Server
{
    class SMZoneMemberLoadStatusNotify : Packet
    {
        private int ZoneSessionID;
        private int Unk;

        public SMZoneMemberLoadStatusNotify(Connection connection, int zoneSessId, int unk) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];

            ZoneSessionID = zoneSessId;
            Unk = unk;
        }

        public override void Deserialize()
        {
            Data.WriteShortLE(0);
            Data.WriteIntLE(0);

            Data.WriteIntLE(153275889); // ID AGAIN

            Data.WriteIntLE(ZoneSessionID); // 188874281 / 188864280 zone session id?

            Data.WriteIntLE(0);

            Data.WriteIntLE(Unk); // 1191182336 / 1191182337 da fuq?

            Data.WriteIntLE(40); // zone id?
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
