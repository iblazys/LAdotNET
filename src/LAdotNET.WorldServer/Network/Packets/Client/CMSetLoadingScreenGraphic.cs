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
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using LAdotNET.WorldServer.Network.Packets.Server;
using LAdotNET.WorldServer.Network.Packets.Server.Init;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Client
{
    class CMSetLoadingScreenGraphic : Packet
    {
        public CMSetLoadingScreenGraphic(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {

        }

        public override void Serialize()
        {
            Logger.Debug("\n" + HexUtils.Dump(Data));
        }

        public override async Task HandleAsync()
        {
            // DISCONNECTING FROM THE WORLDSERVER AT THIS POINT WILL CAUSE THE CINEMATIC TO PLAY AND WORLD ENTRANCE TO OCCUR

            // FINISH WORLD ENTRANCE

            // SMNewPropExist
            // SMNewNPCExist

            
            await Connection.SendAsync(new SMZoneTraitNotify(Connection));
            await Connection.SendAsync(new SMImmuneStatusNotify(Connection));
            await Connection.SendAsync(new SMIdentityChangeNotify(Connection));
            await Connection.SendAsync(new SMBattleSlotUpdateNotify(Connection));
            await Connection.SendAsync(new SMInstanceZoneEnteredNotify(Connection));
            await Connection.SendAsync(new SMInitDungeonQuest(Connection));
            await Connection.SendAsync(new SMStatusEffectAddNotify(Connection));
            await Connection.SendAsync(new SMImmuneStatusNotify(Connection));
            await Connection.SendAsync(new SMStatChangeNotify(Connection));
            await Connection.SendAsync(new SMZoneMemberLoadStatusNotify(Connection, 188874281, 1191182336));
            await Connection.SendAsync(new SMZoneMemberLoadStatusNotify(Connection, 188864280, 1191182337));
            await Connection.SendAsync(new SMDungeonFirstVisitorNotify(Connection));

            // none of the above packets cause the loading screen to disappear
            await Connection.SendAsync(new SMLoadCompleteResult(Connection));
            await Connection.SendAsync(new SMInitCoopQuest(Connection));
            await Connection.SendAsync(new SMServerTimeNotify(Connection));
        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }
    }
}
