using DotNetty.Buffers;
using LAdotNET.LoginServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.LoginServer.Network.Packets.Client
{
    class CMWorldSelectRequest : Packet
    {
        public CMWorldSelectRequest(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {
            // Set State etc
        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }

        public override async Task HandleAsync()
        {
            await Connection.SendAsync(new SMWorldSelectResult(Connection));
        }

        public override void Serialize()
        {
            // contains server id
        }
    }
}
