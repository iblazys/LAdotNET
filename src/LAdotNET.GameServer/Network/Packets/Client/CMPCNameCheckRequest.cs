using DotNetty.Buffers;
using LAdotNET.GameServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Client
{
    class CMPCNameCheckRequest : Packet
    {
        public CMPCNameCheckRequest(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {
            //
        }

        public override void Serialize()
        {
            Logger.Debug("\n" + HexUtils.Dump(Data));
        }

        public override async Task HandleAsync()
        {
            await Connection.SendAsync(new SMPCNameCheckResult(Connection));
        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }
    }
}
