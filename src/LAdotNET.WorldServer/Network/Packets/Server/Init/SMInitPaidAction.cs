﻿// This file is part of LAdotNET.
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

namespace LAdotNET.WorldServer.Network.Packets.Server.Init
{
    class SMInitPaidAction : Packet
    {
        public SMInitPaidAction(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteBytes(new byte[]
            {
                0x0A, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x1F, 0x18, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03,
                0x00, 0x00, 0x00, 0x00, 0x1F, 0x18, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x00, 0xE2, 0x17, 0xC3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0xE2, 0x17,
                0xC3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x02, 0x00, 0x00, 0x00, 0x1F, 0x18, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x07, 0x02, 0x00, 0x00, 0x00, 0x1F, 0x18, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x1C, 0x0C, 0x00, 0x00, 0x00, 0x1F, 0x18, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1D, 0x02, 0x00,
                0x00, 0x00, 0x1F, 0x18, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x23, 0x00, 0x00, 0x00, 0x00, 0xE2,
                0x17, 0xC3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3E, 0x00, 0x00, 0x00, 0x00, 0x1F, 0x18, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00
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
