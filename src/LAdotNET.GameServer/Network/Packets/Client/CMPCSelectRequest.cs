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
using LAdotNET.GameServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Client
{
    class CMPCSelectRequest : Packet
    {
        /*
         * This packet is received after selecting a character
         * Controls world entrance?
         */

        public CMPCSelectRequest(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {
            //
        }

        public override void Serialize()
        {
            Logger.Debug("\n" + HexUtils.Dump(Data));
        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }

        public override async Task HandleAsync()
        {
            // If using auxilary passcode

                // send with return code 13010 - pc_select_result_failure_aux_password_required
                // await Connection.SendAsync(new SMPCRoomStateNotify(Connection)); 
                // Connection.SendAsync(new SMAuthError(Connection)); // Send auth error with ret code 1?

            // If not using aux passcode
            
            // send connection to worldserver
            await Connection.SendAsync(new SMPCSelectResult(Connection)); // ret code pc_select_result_success
            await Connection.SendAsync(new SMEnterWorldCompleted(Connection));
        }
    }
}
