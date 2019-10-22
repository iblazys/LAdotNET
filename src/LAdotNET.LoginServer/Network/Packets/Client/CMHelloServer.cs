using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetty.Buffers;
using LAdotNET.LoginServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;

namespace LAdotNET.LoginServer.Network.Packets.Client
{
    class CMHelloServer : Packet
    {
        private int DataEZ { get; set; }

        public CMHelloServer(Connection connection, IByteBuffer pkt) : base(connection, pkt) { }

        public override void Deserialize()
        {
            DataEZ = Data.ReadIntLE();
        }

        public override async Task HandleAsync()
        {
            await Connection.SendAsync(new SMHelloClient(Connection));
        }

        public override void Serialize()
        {

        }
    }
}
