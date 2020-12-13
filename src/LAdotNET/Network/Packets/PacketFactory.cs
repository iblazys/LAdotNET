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
