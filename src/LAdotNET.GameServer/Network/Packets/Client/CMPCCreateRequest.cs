using DotNetty.Buffers;
using LAdotNET.GameServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Client
{
    class CMPCCreateRequest : Packet
    {
        public CMPCCreateRequest(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {
            //
        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }

        public override async Task HandleAsync()
        {
            await Connection.SendAsync(new SMPCCreateResult(Connection));
        }

        public override void Serialize()
        {
            // contains character name etc etc
            Logger.Debug("\n" + HexUtils.Dump(Data));
        }
    }
}
