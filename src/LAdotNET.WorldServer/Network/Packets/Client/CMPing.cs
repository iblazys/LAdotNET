using DotNetty.Buffers;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Client
{
    class CMPing : Packet
    {
        public CMPing(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {

        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }

        public override async Task HandleAsync()
        {
            // Send SMPong
        }

        public override void Serialize()
        {
            //Logger.Debug("\n"+HexUtils.Dump(Data));
        }
    }
}
