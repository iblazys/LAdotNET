using LAdotNET.LoginServer.Network.Packets.Client;
using LAdotNET.LoginServer.Network.Packets.Server;
using LAdotNET.Network.Packets;
using System;
using System.Collections.Generic;

namespace LAdotNET.LoginServer.Network
{
    public class LoginPacketFactory : PacketFactory
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static LoginPacketFactory()
        {
            Packets = new Dictionary<ushort, Type>
            {
                // These two opcodes do not change between updates
                { 0x1,  typeof(CMHelloServer) },
                { 0x2,  typeof(SMHelloClient) },

                { 0xB48E,   typeof(CMLoginChannelRequest) },
                { 0x8F86,   typeof(SMLoginChannelRequest) },
                { 0x3D96,   typeof(SMPCRoomRequestRewardResult) },
                { 0x43CA,    typeof(CMWorldCancelResult) },
                { 0x904A,   typeof(SMWorldCancelResult) }
            };

            ReverseLookup = new Dictionary<Type, ushort>();

            foreach (KeyValuePair<ushort, Type> entry in Packets)
            {
                ReverseLookup.Add(entry.Value, entry.Key);
            }
        }

        public static void Init()
        {
            Logger.Info($"Loaded {Packets.Count} loginserver packets.");
        }
    }
}
