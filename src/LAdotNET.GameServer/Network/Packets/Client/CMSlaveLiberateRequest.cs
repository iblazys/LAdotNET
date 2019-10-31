using DotNetty.Buffers;
using LAdotNET.GameServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Client
{
    class CMSlaveLiberateRequest : Packet
    {
        public CMSlaveLiberateRequest(Connection connection, IByteBuffer buffer) : base(connection, buffer)
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

            await Connection.SendAsync(new SMPaidShopLimitList(Connection));
            await Connection.SendAsync(new SMWallpaperChangeResult(Connection));
            await Connection.SendAsync(new SMPCListExcessResult(Connection));
            await Connection.SendAsync(new SMPCRoomRequestRewardResult(Connection));
            await Connection.SendAsync(new SMPaidBuffChangeNotify(Connection));
            await Connection.SendAsync(new SMPaidCashCheckResult(Connection));

            await Connection.SendAsync(new SMSlaveLiberateRequestResult(Connection));
        }

        public override void Serialize()
        {
            Logger.Debug("\n" + HexUtils.Dump(Data));
        }
    }
}
