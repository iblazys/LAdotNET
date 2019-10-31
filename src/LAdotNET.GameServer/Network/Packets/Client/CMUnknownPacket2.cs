using DotNetty.Buffers;
using LAdotNET.GameServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Client
{
    class CMUnknownPacket2 : Packet
    {
        /*
         * This packet is received after selecting a character
         */

        public CMUnknownPacket2(Connection connection, IByteBuffer buffer) : base(connection, buffer)
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
            await Connection.SendAsync(new SMPCRoomStateNotify(Connection));
            //Connection.SendAsync(new SMAuthError(Connection));
        }
    }
}
