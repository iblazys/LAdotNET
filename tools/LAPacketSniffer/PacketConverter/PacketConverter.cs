using PacketConverter.Encryption;
using Snappy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PacketConverter
{
    public class PacketConverter
    {
        public int packetOrder = 0;
        public Counter counter = new Counter();
        public bool isFirstClientPacket = true;

        static List<BinFile> RawFiles = new List<BinFile>();
        static List<LAPacket> Processed = new List<LAPacket>();
        static string inputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"input");
        static string outputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"output");

        public void Run()
        {
            LoadFiles();

            ProcessRawFiles();

            DecryptAndDecompress(); // Decrypts using Processed List

            WriteOutput();
        }

        private void LoadFiles()
        {
            foreach (string file in Directory.EnumerateFiles(inputPath, "*.bin").OrderBy(n => Regex.Replace(Path.GetFileNameWithoutExtension(n), @"\d+", n => n.Value.PadLeft(4, '0'))))
            {
                string[] nameData = Path.GetFileNameWithoutExtension(file).Split('_');

                RawFiles.Add(new BinFile
                {
                    SourceFile = Path.GetFileName(file),

                    OrderNum = int.Parse(nameData[1]),
                    Date = nameData[2],
                    FileSize = int.Parse(nameData[3]),
                    PacketType = nameData[4],

                    Data = File.ReadAllBytes(file)
                });
            }
        }

        private void ProcessRawFiles()
        {
            foreach(var raw in RawFiles)
            {
                Console.WriteLine($"Processing {raw.OrderNum}_{raw.FileSize}_{raw.PacketType}");

                ProcessRawPacket(raw);
            }
        }

        private void ProcessRawPacket(BinFile raw)
        {
            using MemoryStream stream = new MemoryStream(raw.Data);
            using BinaryReader reader = new BinaryReader(stream);

            while(stream.Position != stream.Length)
            {
                var size = reader.ReadUInt16();
                var opcode = reader.ReadUInt16();
                var compType = reader.ReadByte();
                var encryption = reader.ReadByte();
                var data = reader.ReadBytes(size - 6);

                LAPacket packet = new LAPacket
                {
                    SourceFile = raw.SourceFile,
                    OrderNum = packetOrder, // raw.OrderNum
                    Origin = (Origin)Enum.Parse(typeof(Origin), raw.PacketType),

                    Size = size,
                    Opcode = opcode,
                    CompressionType = (CompressionType)compType,
                    EncryptionFlag = encryption,

                    Data = data
                };

                Processed.Add(packet);
                packetOrder++;
            }
        }

        private void DecryptAndDecompress()
        {
            foreach (var pkt in Processed)
            {
                // TRY FIND SEQUENCE NUMBER
                var guess1 = Crypt.xorKey.GetFirstOccurance((byte)(pkt.Data[0])) - 4;
                var guess2 = Crypt.xorKey.GetFirstOccurance((byte)(pkt.Data[0] + 1)) - 4;
                var guess3 = Crypt.xorKey.GetFirstOccurance((byte)(pkt.Data[0] - 1)) - 4;

                if (pkt.Origin == Origin.CLIENT)
                {
                    // DEBUG
                    //Console.WriteLine($"BEFORE:\n{HexDump.Dump(pkt.Data)}");

                    if (pkt.EncryptionFlag == 1)
                    {
                        if (pkt.Opcode == 0xA257 || pkt.Opcode == 0x8592)
                        {
                            Console.WriteLine("PACKET IS LOGIN/GAME HELLO - RESETTING THE COUNTER");
                            counter.Reset();
                        }

                        byte[] beforeDecrypt = new byte[pkt.Data.Length];
                        Array.Copy(pkt.Data, 0, beforeDecrypt, 0, beforeDecrypt.Length);

                        switch(pkt.Opcode)
                        {
                            case 0x6D08:
                                counter.Count = 8;
                                break;

                            case 0x38C3:
                                counter.Count = 9;
                                break;

                            case 0xBA09:
                                counter.Count = 4;
                                break;
                        }

                        Crypt.Xor(pkt.Data, (ushort)counter.Count);

                        if (pkt.Data[0] != counter.Count)
                        {
                            //{pkt.OrderNum}_{pkt.Origin}_0x{pkt.Opcode.ToString("X")}_{pkt.Size} 
                            Console.WriteLine($"CLIENT DECRYPTION FAILED - SOURCE FILE: {pkt.SourceFile} - WITH COUNTER VALUE {counter.Count}");
                            //Console.WriteLine($"BEFORE:\n{HexDump.Dump(beforeDecrypt)}");
                            //Console.WriteLine($"AFTER:\n{HexDump.Dump(pkt.Data)}");
                        } 
                        else
                        {
                            //{pkt.OrderNum}_{pkt.Origin}_0x{pkt.Opcode.ToString("X")}_{pkt.Size} 
                            Console.WriteLine($"DECRYPT/DECOMPRESS: - SOURCE FILE: {pkt.SourceFile} - WITH COUNTER VALUE {counter.Count}");
                            //Console.WriteLine($"BEFORE:\n{HexDump.Dump(beforeDecrypt)}");
                            //Console.WriteLine($"AFTER:\n{HexDump.Dump(pkt.Data)}");
                        }

                        counter.Increase();
                    }
                }

                if (pkt.Origin == Origin.SERVER)
                {
                    //Console.WriteLine($"DECRYPT/DECOMPRESS: {pkt.OrderNum}_{pkt.Origin}_0x{pkt.Opcode.ToString("X")}_{pkt.Size} - SOURCE FILE: {pkt.SourceFile}");

                    // DEBUG
                    //Console.WriteLine($"BEFORE:\n{HexDump.Dump(pkt.Data)}");

                    if (pkt.EncryptionFlag == 1)
                    {
                        Crypt.Xor(pkt.Data, pkt.Opcode);
                    }
                }

                if(pkt.CompressionType == CompressionType.SNAPPY)
                {
                    var uncompressed = SnappyCodec.Uncompress(pkt.Data);

                    var data = new byte[uncompressed.Length - 16];

                    Buffer.BlockCopy(uncompressed, 16, data, 0, data.Length);

                    pkt.Data = data;
                }

                if (pkt.CompressionType == CompressionType.LZ4)
                    Console.WriteLine("ENCOUNTERED LZ4");

                // DEBUG
                //if(pkt.Origin == Origin.CLIENT)
                   // Console.WriteLine($"AFTER:\n{HexDump.Dump(pkt.Data)}");
            }
        }

        public void WriteOutput()
        {
            foreach (var pkt in Processed)
            {
                string fileName = $"{pkt.OrderNum}_{Opcodes.GetPacketName(pkt.Opcode)}_0x{pkt.Opcode.ToString("X")}_{pkt.Size}_{pkt.Origin}.bin";

                using FileStream stream = new FileStream(Path.Combine(outputPath, fileName), FileMode.Create);
                using BinaryWriter writer = new BinaryWriter(stream);

                writer.Write(pkt.Size);
                writer.Write(pkt.Opcode);
                writer.Write((byte)pkt.CompressionType);
                writer.Write(pkt.EncryptionFlag);
                writer.Write(pkt.Data);
            }
        }
    }

    public static class Extensions
    {
        // EXTENSION MOVE ME
        public static int GetFirstOccurance(this byte[] byteArray, byte byteToFind)
        {
            return Array.IndexOf(byteArray, byteToFind);
        }
    }
}
