﻿// This file is part of LAdotNET.
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

using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Server.Init
{
    class SMInitEnv : Packet
    {
        public SMInitEnv(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteBytes(new byte[]
            {
                0x06, 0x00, 0x03, 0x00, 0x45, 0x00, 0x74, 0x00, 0x63, 0x00, 0x13, 0x00, 0x55, 0x00, 0x73, 0x00,
                0x65, 0x00, 0x4D, 0x00, 0x6F, 0x00, 0x76, 0x00, 0x65, 0x00, 0x44, 0x00, 0x65, 0x00, 0x62, 0x00,
                0x75, 0x00, 0x67, 0x00, 0x48, 0x00, 0x69, 0x00, 0x73, 0x00, 0x74, 0x00, 0x6F, 0x00, 0x72, 0x00,
                0x79, 0x00, 0x05, 0x00, 0x66, 0x00, 0x61, 0x00, 0x6C, 0x00, 0x73, 0x00, 0x65, 0x00, 0x05, 0x00,
                0x50, 0x00, 0x61, 0x00, 0x72, 0x00, 0x74, 0x00, 0x79, 0x00, 0x1C, 0x00, 0x50, 0x00, 0x61, 0x00,
                0x72, 0x00, 0x74, 0x00, 0x79, 0x00, 0x49, 0x00, 0x6E, 0x00, 0x66, 0x00, 0x6F, 0x00, 0x4F, 0x00,
                0x70, 0x00, 0x74, 0x00, 0x69, 0x00, 0x6D, 0x00, 0x69, 0x00, 0x7A, 0x00, 0x61, 0x00, 0x74, 0x00,
                0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x45, 0x00, 0x6E, 0x00, 0x61, 0x00, 0x62, 0x00, 0x6C, 0x00,
                0x65, 0x00, 0x64, 0x00, 0x05, 0x00, 0x66, 0x00, 0x61, 0x00, 0x6C, 0x00, 0x73, 0x00, 0x65, 0x00,
                0x04, 0x00, 0x50, 0x00, 0x72, 0x00, 0x6F, 0x00, 0x70, 0x00, 0x19, 0x00, 0x53, 0x00, 0x74, 0x00,
                0x61, 0x00, 0x74, 0x00, 0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x41, 0x00, 0x75, 0x00, 0x74, 0x00,
                0x6F, 0x00, 0x52, 0x00, 0x65, 0x00, 0x66, 0x00, 0x69, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x43, 0x00,
                0x6F, 0x00, 0x6F, 0x00, 0x6C, 0x00, 0x64, 0x00, 0x6F, 0x00, 0x77, 0x00, 0x6E, 0x00, 0x03, 0x00,
                0x31, 0x00, 0x30, 0x00, 0x30, 0x00, 0x0B, 0x00, 0x5A, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x65, 0x00,
                0x43, 0x00, 0x68, 0x00, 0x61, 0x00, 0x6E, 0x00, 0x6E, 0x00, 0x65, 0x00, 0x6C, 0x00, 0x0E, 0x00,
                0x52, 0x00, 0x65, 0x00, 0x73, 0x00, 0x65, 0x00, 0x72, 0x00, 0x76, 0x00, 0x65, 0x00, 0x45, 0x00,
                0x6E, 0x00, 0x61, 0x00, 0x62, 0x00, 0x6C, 0x00, 0x65, 0x00, 0x64, 0x00, 0x04, 0x00, 0x74, 0x00,
                0x72, 0x00, 0x75, 0x00, 0x65, 0x00, 0x04, 0x00, 0x49, 0x00, 0x74, 0x00, 0x65, 0x00, 0x6D, 0x00,
                0x0E, 0x00, 0x56, 0x00, 0x65, 0x00, 0x72, 0x00, 0x73, 0x00, 0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00,
                0x45, 0x00, 0x6E, 0x00, 0x61, 0x00, 0x62, 0x00, 0x6C, 0x00, 0x65, 0x00, 0x64, 0x00, 0x05, 0x00,
                0x66, 0x00, 0x61, 0x00, 0x6C, 0x00, 0x73, 0x00, 0x65, 0x00, 0x0A, 0x00, 0x4C, 0x00, 0x6F, 0x00,
                0x67, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x51, 0x00, 0x75, 0x00, 0x65, 0x00, 0x75, 0x00, 0x65, 0x00,
                0x12, 0x00, 0x4C, 0x00, 0x6F, 0x00, 0x67, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x50, 0x00, 0x65, 0x00,
                0x65, 0x00, 0x72, 0x00, 0x53, 0x00, 0x6F, 0x00, 0x66, 0x00, 0x74, 0x00, 0x4C, 0x00, 0x69, 0x00,
                0x6D, 0x00, 0x69, 0x00, 0x74, 0x00, 0x05, 0x00, 0x32, 0x00, 0x32, 0x00, 0x30, 0x00, 0x30, 0x00,
                0x30, 0x00, 0xE4, 0xFD, 0xFF, 0xFF, 0xAC, 0x77, 0x04, 0x2B, 0x00, 0x00, 0x00, 0x00, 0xE3, 0xB7,
                0x02, 0xA4, 0xEC, 0x37, 0x00, 0x00, 0xE2, 0xE7, 0x41, 0x0B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x13,
                0x00, 0x4B, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x65, 0x00, 0x61, 0x00, 0x20, 0x00, 0x53, 0x00, 0x74,
                0x00, 0x61, 0x00, 0x6E, 0x00, 0x64, 0x00, 0x61, 0x00, 0x72, 0x00, 0x64, 0x00, 0x20, 0x00, 0x54,
                0x00, 0x69, 0x00, 0x6D, 0x00, 0x65, 0x00
            });
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
