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
using LAdotNET.Extensions;
using LAdotNET.GameServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Client
{
    class CMSlideNoticeUpdate : Packet
    {
        private long UnkId { get; set; }
        private long PrevId { get; set; }
        private string UnkKey1 { get; set; }
        private string UnkKey2 { get; set; }

        private string AccountID { get; set; }

        private long UnkLong { get; set; }

        public CMSlideNoticeUpdate(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {
            //
        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }

        public override async Task HandleAsync()
        {
            // TODO: SendAsync(List<Packet>)

            await Connection.SendAsync(new SMPaidShopSettingNotify(Connection));
            await Connection.SendAsync(new SMWallpaperListResult(Connection));
            await Connection.SendAsync(new SMPCListResult(Connection));
            await Connection.SendAsync(new SMPCRoomStateNotify(Connection));
            await Connection.SendAsync(new SMPaidCashCheckResult(Connection));
            await Connection.SendAsync(new SMPaidPurchaseCheckResult(Connection));

            await Connection.SendAsync(new SMSlideNoticeUpdateNotify(Connection));
        }

        public override void Serialize()
        {
            Data.SkipBytes(4);
            Data.SkipBytes(4);

            UnkId = Data.ReadLongLE(); // some id?
            PrevId = Data.ReadLongLE(); // this is from SMWorldCancelResult

            UnkKey1 = Data.ReadString();
            UnkKey2 = Data.ReadString();

            Data.SkipBytes(1); // byte

            AccountID = Data.ReadUnicodeString();

            UnkLong = Data.ReadLongLE();

            // DEBUG INFO
            Logger.Debug(
                $"UnkID: {UnkId}\n" +
                $"PrevID: {PrevId}\n" +
                $"UnkKey1: {UnkKey1}\n" +
                $"UnkKey2: {UnkKey2}\n" +
                $"AccountID: {AccountID}\n" +
                $"UnkLong: {UnkLong}\n"
                );
        }
    }
}
