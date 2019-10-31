using DotNetty.Buffers;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Client
{
    class CMUnknownPacket1 : Packet
    {
        /*
         * This packet is received after the server sends SMSlaveLiberateRequestResult
         */

        public CMUnknownPacket1(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {
            //
        }

        public override void Serialize()
        {
            Logger.Debug("\n" + HexUtils.Dump(Data));
        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }

        public override async Task HandleAsync()
        {
            // Do nothing
        }
    }
}
