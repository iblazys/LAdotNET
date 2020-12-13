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

using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetty.Buffers;
using LAdotNET.LoginServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;

namespace LAdotNET.LoginServer.Network.Packets.Client
{
    class CMVersionCheckRequest : Packet
    {
        private int DataEZ { get; set; }

        public CMVersionCheckRequest(Connection connection, IByteBuffer pkt) : base(connection, pkt) { }

        public override void Deserialize()
        {
            DataEZ = Data.ReadIntLE();
        }

        public override async Task HandleAsync()
        {
            await Connection.SendAsync(new SMVersionCheckResult(Connection));
        }

        public override void Serialize()
        {

        }
    }
}
