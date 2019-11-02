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
