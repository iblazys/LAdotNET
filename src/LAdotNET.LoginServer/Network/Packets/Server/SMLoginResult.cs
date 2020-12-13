// This file is part of LAdotNET.
//
// LAdotNET is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// LAdotNET is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with LAdotNET.  If not, see <https://www.gnu.org/licenses/>.

using LAdotNET.Extensions;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Text;
using System.Threading.Tasks;

namespace LAdotNET.LoginServer.Network.Packets.Server
{
    class SMLoginResult : Packet
    {
        public SMLoginResult(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteUnicodeString("88080929");   // ACCOUNT ID

            Data.WriteIntLE(0);   // UNK
            Data.WriteIntLE(0);   // UNK

            // Server List
            Data.WriteShortLE(Loginserver.Config.Login.ServerList.Count); // SERVER COUNT

            foreach (var serv in Loginserver.Config.Login.ServerList)
            {
                var serverName = Encoding.Unicode.GetBytes(serv.Name);

                Data.WriteByte(serv.Id);    // SERVER ID - APPEARS IN NEXT CLIENT PACKET

                Data.WriteShortLE(serverName.Length / 2);
                Data.WriteBytes(serverName);

                Data.WriteByte(1);    // unk - 0 =  makes server text red
                Data.WriteByte(1);    // unk - 0 = no changes
                Data.WriteByte(1);    // unk - 0 makes server status text disappear
                Data.WriteByte(16);   // Server Load [0-100]
                Data.WriteByte(7);    // unk
                Data.WriteByte(0);    // unk
                Data.WriteByte(5);    // Character Count
            }
            // End Server List

            Data.WriteUnicodeString(Loginserver.Config.Network.ClientVersion); // CLIENT_VERSION

            Data.WriteShortLE(0); // UNK

            Data.WriteIntLE(10000); // RESULT CODE

            Data.WriteByte(0);    // UNK
            Data.WriteIntLE(0);   // UNK

            Data.WriteUnicodeString("88080929");   // ACCOUNT ID

            Data.WriteIntLE(8);     // UNK
            Data.WriteShortLE(0);   // UNK
        }

        public override Task HandleAsync()
        {
            throw new NotImplementedException();
        }

        public override void Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
