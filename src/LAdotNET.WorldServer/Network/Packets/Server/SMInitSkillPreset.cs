﻿using LAdotNET.Network;
using LAdotNET.Network.Packets;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Server
{
    class SMInitSkillPreset : Packet
    {
        public SMInitSkillPreset(Connection connection) : base(connection)
        {
            CompressionType = CompressionType.SNAPPY;
            OpCode = PacketFactory.ReverseLookup[GetType()];
        }

        // 1.6.2.3 - 1.6.4.1 did not change
        public override void Deserialize()
        {
            Data.WriteBytes(new byte[]
            {
                0x13, 0x00, 0x31, 0xF2, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x32, 0xF2, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x34, 0xF2, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C,
                0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x36, 0xF2, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x00, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x37, 0xF2, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x39, 0xF2, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3B, 0xF2,
                0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x3D, 0xF2, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x3E, 0xF2, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x42, 0xF2, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x43, 0xF2, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C,
                0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46, 0xF2, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x00, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x48, 0xF2, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4A, 0xF2, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4B, 0xF2,
                0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x56, 0xF2, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x7C, 0x0F, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x7D, 0x0F, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x17, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7E, 0x0F, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6C,
                0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
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
