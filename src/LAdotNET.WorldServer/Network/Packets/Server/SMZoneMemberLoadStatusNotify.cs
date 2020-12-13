// This file is part of LAdotNET.
//
// LAdotNET is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// LAdotNET is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with LAdotNET.  If not, see <https://www.gnu.org/licenses/>.

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
