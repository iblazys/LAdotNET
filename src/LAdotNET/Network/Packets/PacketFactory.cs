using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LAdotNET.Network.Packets
{
    /// <summary>
    /// Loads packet names/opcodes and searches for a matching class
    /// </summary>
    public class PacketFactory
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static Dictionary<ushort, Type> Packets = new Dictionary<ushort, Type>();
        public static Dictionary<Type, ushort> ReverseLookup = new Dictionary<Type, ushort>();

        public static void Load()
        {
            Logger.Info("Loading packet definitions.");

            Stopwatch s1 = new Stopwatch();

            s1.Start();

            LoadPackets("..\\..\\..\\..\\..\\data\\sm_opcodes.dat"); // TODO: Fix path
            LoadPackets("..\\..\\..\\..\\..\\data\\cm_opcodes.dat");

            CheckMissingOpcodes();

            s1.Stop();

            Logger.Info($"Loaded {Packets.Count} packets in {s1.Elapsed.TotalMilliseconds} ms");
        }

        private static void LoadPackets(string filePath)
        {
            var file = Path.Combine(Environment.CurrentDirectory, filePath);

            if (!File.Exists(file))
            {
                Logger.Error($"Unable to find opcode file: {file}");
                return;
            }

            ReadFile(file);
        }

        private static void ReadFile(string file)
        {
            var lines = File.ReadLines(file);

            foreach (var line in lines)
            {
                // Ignore "comments"
                if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                    continue;

                var data = line.Split(',');
                var opcode = Convert.ToUInt16(data[0], 16);
                var name = data[1].Trim();

                Type packetType = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes().Where(t => t.IsSubclassOf(typeof(Packet))))
                    .FirstOrDefault(t => t.Name.Equals(name));

                if (packetType != null)
                {
                    Packets.Add(opcode, packetType);
                    ReverseLookup.Add(packetType, opcode);
                }
            }

            lines = null;
        }

        private static void CheckMissingOpcodes()
        {
            var packetHandlers = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes().Where(t => t.IsSubclassOf(typeof(Packet)))).ToList();

            foreach(var packet in packetHandlers)
            {
                if(!ReverseLookup.ContainsKey(packet))
                {
                    Logger.Warn($"No opcode found for packet {packet.Name}");
                }
            }
        }
    }
}
