using DotNetty.Buffers;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Client
{
    class CMUnknownOnLoadingScreen : Packet
    {
        public CMUnknownOnLoadingScreen(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {

        }

        public override void Serialize()
        {
            Logger.Debug("\n" + HexUtils.Dump(Data));
        }

        public override async Task HandleAsync()
        {
            // FINISH WORLD ENTRANCE?

        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }
    }
}
