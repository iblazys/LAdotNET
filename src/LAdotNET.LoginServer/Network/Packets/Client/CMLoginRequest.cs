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

using DotNetty.Buffers;
using LAdotNET.LoginServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.LoginServer.Network.Packets.Client
{
    class CMLoginRequest : Packet
    {
        public CMLoginRequest(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {
            // Set State etc
        }

        public override void Serialize()
        {
            Logger.Debug("\n"+HexUtils.Dump(Data));
        }

        public override async Task HandleAsync()
        {
            await Connection.SendAsync(new SMLoginResult(Connection));
            await Connection.SendAsync(new SMPCRoomStateNotify(Connection));
        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }
    }
}
