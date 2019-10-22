using LAdotNET.GameServer.Network.Packets.Client;
using LAdotNET.GameServer.Network.Packets.Server;
using LAdotNET.Network.Packets;
using System;
using System.Collections.Generic;

namespace LAdotNET.GameServer.Network
{
    class GamePacketFactory : PacketFactory
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static GamePacketFactory()
        {
            Packets = new Dictionary<ushort, Type>
            {
                // These two opcodes do not change between updates
                { 0xA257,   typeof(CMSlaveLiberateRequest) },
                // 0xCDA9, length 29 -- back to login server
                
                { 0xFF9,   typeof(SMPaidShopLimitList) },
                { 0xA813,   typeof(SMPaidBuffChangeNotify) },
                { 0x80FD,   typeof(SMWallpaperChangeResult) },
                { 0xDF3B,   typeof(SMPCListExcessResult) },
                { 0xBB44,   typeof(SMPCRoomRequestRewardResult) },
                { 0x5DA6,   typeof(SMPaidCashCheckResult) },
                { 0xA44E,   typeof(SMSlaveLiberateRequestResult) },

                { 0x38A4, typeof(CMUnknownPacket1) },

                { 0xE36E,   typeof(CMPCNameChangeRequest) },
                { 0xB57F, typeof(SMPCNameChangeResult) },

                { 0xB2BF, typeof(CMPCBattlefieldMercenaryJoinHistoryRequest) },
                { 0xE11C, typeof(SMPCBattlefieldMercenaryJoinHistoryResult) }
            };

            ReverseLookup = new Dictionary<Type, ushort>();

            foreach (KeyValuePair<ushort, Type> entry in Packets)
            {
                ReverseLookup.Add(entry.Value, entry.Key);
            }
        }

        public static void Init()
        {
            Logger.Info($"Loaded {Packets.Count} gameserver packets.");
        }
    }
}
