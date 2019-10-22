using System;
using System.Collections.Generic;

namespace LAdotNET.Network.Packets
{
    public class PacketFactory
    {
        public static Dictionary<ushort, Type> Packets { get; set; }
        public static Dictionary<Type, ushort> ReverseLookup { get; set; }
    }
}
