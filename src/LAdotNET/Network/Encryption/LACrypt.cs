﻿using DotNetty.Buffers;

namespace LAdotNET.Network.Encryption
{
    public class LACrypt
    {
        // 1.6.2.3
        private static byte[] xorKey = new byte[]
        {
            0x03, 0xCB, 0xEA, 0xB6, 0x32, 0xCF, 0x2E, 0xA8, 0x78, 0x52, 0x00, 0x3A, 0x7C, 0x59, 0xC8, 0xD9,
            0x98, 0x70, 0x68, 0x2D, 0x40, 0x1C, 0x18, 0xE1, 0xF8, 0xD8, 0xDC, 0xA5, 0x47, 0x9E, 0xA2, 0x8A,
            0x87, 0x62, 0x77, 0x4C, 0x05, 0x5B, 0x67, 0x10, 0x22, 0x9C, 0xA0, 0x44, 0xC7, 0x63, 0x65, 0xC2,
            0x11, 0x61, 0x79, 0x5E, 0x95, 0x8D, 0xA9, 0x3E, 0x49, 0x9A, 0xE9, 0x4E, 0x35, 0xE3, 0xE5, 0x14,
            0xCC, 0xB6, 0x97, 0x5A, 0xA4, 0x8C, 0x06, 0x01, 0xDB, 0x31, 0x73, 0xBA, 0xB9, 0x41, 0x0E, 0x81,
            0x64, 0x0C, 0xD7, 0x6D, 0xE8, 0x36, 0xD3, 0x94, 0x2B, 0x89, 0x21, 0x12, 0xA7, 0x04, 0x9F, 0x76,
            0x53, 0xFD, 0x15, 0x58, 0xBE, 0x4A, 0xB4, 0x93, 0xFA, 0xE7, 0x85, 0x16, 0x23, 0x96, 0xEA, 0xC4,
            0x66, 0xCB, 0xD2, 0x42, 0x0A, 0x29, 0xEF, 0xF4, 0x3C, 0x2A, 0x82, 0xAD, 0xD4, 0xBB, 0x45, 0x0B,
            0x1B, 0x92, 0x28, 0xC1, 0xFF, 0x56, 0x69, 0x3D, 0x2F, 0x7A, 0xDE, 0x7E, 0xB1, 0x7B, 0x5D, 0x48,
            0x39, 0x9D, 0x83, 0x17, 0x19, 0x50, 0xA1, 0x38, 0xB0, 0xBC, 0x1A, 0x26, 0x02, 0x4D, 0x34, 0x37,
            0x57, 0xE2, 0xEE, 0x8F, 0x25, 0x7D, 0xF1, 0x43, 0xC3, 0x6A, 0xFB, 0x6C, 0x71, 0x08, 0xB7, 0xF2,
            0x80, 0xE4, 0x03, 0x54, 0xAA, 0x3B, 0xD1, 0x2C, 0x6F, 0xEB, 0x3F, 0xD5, 0xBD, 0xF0, 0x4F, 0xA3,
            0x91, 0x8E, 0xDD, 0x5C, 0xF3, 0x86, 0x5F, 0x0F, 0x60, 0xA6, 0xFC, 0xC5, 0xE6, 0xF6, 0xEC, 0xB2,
            0xD6, 0x09, 0xC9, 0x4B, 0x84, 0xAF, 0xDA, 0xF7, 0x20, 0xE0, 0xED, 0x27, 0x6E, 0x6B, 0x30, 0x74,
            0xD0, 0xB3, 0x9B, 0xB8, 0x24, 0x51, 0xB5, 0xCE, 0x1D, 0x99, 0x90, 0x1E, 0x33, 0xAB, 0x55, 0x13,
            0xAE, 0x07, 0xDF, 0x7F, 0x75, 0x88, 0xC6, 0xCA, 0xC0, 0xBF, 0x0D, 0x1F, 0xF5, 0xFE, 0x8B, 0xCD,
            0x72, 0x46, 0xF9, 0xAC
        };


        public static void Xor(IByteBuffer data, int seed)
        {
            for(int i = 0; i < data.ReadableBytes; i++)
            {
                data.SetByte(i, (byte)(data.GetByte(i) ^ xorKey[(seed & 0xFF) + 4]));

                seed++;
            }
        }
    }
}
