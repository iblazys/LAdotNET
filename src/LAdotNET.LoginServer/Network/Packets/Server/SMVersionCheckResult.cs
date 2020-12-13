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
using System.Threading.Tasks;

namespace LAdotNET.LoginServer.Network.Packets.Server
{
    public class SMVersionCheckResult : Packet
    {
        public SMVersionCheckResult(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            IsEncrypted = false;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            
            Data.WriteBytes(new byte[]
            {
                0x20, 0xC9, 0xA7, 0x42 // first 4 bytes of xor key
            });

            Data.WriteUnicodeString("1.75.126.889862(CL:889862) (2019-10-30 13:38:49)");

            Data.WriteBytes(new byte[]
            {
                0xC2, 0x3B, 0x6D, 0xC8 // unk // THIS DOES NOT SEEM LIKE A CRC - I HAVE CHANGED IT AND CLIENT STILL ACCEPTED IT.
            });
            

            // RU TEST BELOW
            /*
            Data.WriteBytes(new byte[]
            {
                0x69, 0x2B, 0xAC, 0x1E // first 4 bytes of xor key
            });

            Data.WriteUnicodeString("+1.42.28.892284(CL:892284) (2019-11-4 4:1:9)");

            Data.WriteBytes(new byte[]
            {
                0xCF, 0xD4, 0xDB, 0x0C // unk
            });
            */
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
