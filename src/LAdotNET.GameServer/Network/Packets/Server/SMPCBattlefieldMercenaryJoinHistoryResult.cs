﻿using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer.Network.Packets.Server
{
    class SMPCBattlefieldMercenaryJoinHistoryResult : Packet
    {
        public SMPCBattlefieldMercenaryJoinHistoryResult(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        public override void Deserialize()
        {
            Data.WriteBytes(new byte[]
            {
                0xB0, 0x36, 0x00, 0x00, 0x3A, 0x2C, 0x22, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00,
                0x4C, 0x00, 0x65, 0x00, 0x57, 0x00, 0x61, 0x00, 0x72, 0x00, 0x72, 0x00, 0x69, 0x00, 0x6F, 0x00,
                0x72, 0x00, 0x00, 0xE3, 0xA7, 0xD1, 0x12, 0xF8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1F, 0x18, 0x65, 0x00, 0x01, 0x00, 0x47,
                0x28, 0x00, 0x00, 0x00, 0x47, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69, 0x00, 0x00, 0x00, 0xC1, 0x01,
                0xB8, 0x00, 0x01, 0x00, 0x00, 0xDA, 0xDD, 0xE0, 0xFF, 0x37, 0x89, 0xC1, 0x3E, 0x00, 0x00, 0x00,
                0x00, 0x03, 0x65, 0x6D, 0x7C, 0xFF, 0x75, 0x93, 0x98, 0x3E, 0xF4, 0xFD, 0xD4, 0x3D, 0xD2, 0xD7,
                0xE3, 0xFF, 0xB7, 0xBE, 0xCB, 0xFF, 0x00, 0xCF, 0xD3, 0xE2, 0xFF, 0xAC, 0xBA, 0xCD, 0xFF, 0x00,
                0x01, 0x23, 0x18, 0x3F, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0xFF, 0x0D, 0x01, 0x08, 0x00, 0xF2, 0xE6,
                0x01, 0x0F, 0x00, 0x00, 0x09, 0x12, 0x00, 0x08, 0x09, 0x07, 0x0C, 0x00, 0x00, 0x00, 0x3F, 0x01,
                0x04, 0x0C, 0xD7, 0xA3, 0xF0, 0x3E, 0x01, 0x08, 0xCE, 0x04, 0x00, 0x01, 0x60, 0x01, 0x57, 0x6A,
                0x01, 0x00, 0x0C, 0x01, 0x00, 0x00, 0x00, 0x32, 0x00, 0x00, 0x00, 0x38, 0x6C, 0xAE, 0x5A, 0xBD,
                0xE3, 0x34, 0x7A, 0xAE, 0xF4, 0xC4, 0x69, 0x08, 0xE4, 0x98, 0x50, 0xB4, 0xC7, 0xC3, 0x71, 0xF5,
                0xED, 0x9C, 0x67, 0x92, 0xC3, 0xFF, 0x00, 0xFF, 0xFF, 0x2E, 0x04, 0x00, 0x3C, 0x00, 0x00, 0xFF,
                0xFF, 0xFF, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
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
