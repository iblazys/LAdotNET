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
