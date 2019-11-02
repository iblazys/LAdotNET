using DotNetty.Buffers;
using LAdotNET.GameServer.Network.Packets.Server;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Client
{
    class CMPCRoomStateRequest : Packet
    {
        /*
         * This packet is received after selecting a character
         * Controls world entrance?
         */

        public CMPCRoomStateRequest(Connection connection, IByteBuffer buffer) : base(connection, buffer)
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
            // If using auxilary passcode

                // send with return code 13010 - pc_select_result_failure_aux_password_required
                // await Connection.SendAsync(new SMPCRoomStateNotify(Connection)); 
                // Connection.SendAsync(new SMAuthError(Connection)); // Send auth error with ret code 1?

            // If not using aux passcode
            
            // send connection to worldserver
            await Connection.SendAsync(new SMPCSelectResult(Connection)); // ret code pc_select_result_success
            await Connection.SendAsync(new SMEnterWorldCompleted(Connection));
        }
    }
}
