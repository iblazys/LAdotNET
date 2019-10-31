using LAdotNET.Network.Packets;
using LAdotNET.WorldServer.Network.Packets.Client;
using LAdotNET.WorldServer.Network.Packets.Server;
using System;
using System.Collections.Generic;

namespace LAdotNET.WorldServer.Network
{
    public class WorldPacketFactory : PacketFactory
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static WorldPacketFactory()
        {
            Packets = new Dictionary<ushort, Type>
            {
                // ????
                { 0xA257,   typeof(CMSlaveLiberateRequest) },

                { 0x9E95,   typeof(SMBattlefieldVoteSuggestResult) },
                { 0x7123,   typeof(SMInitDynamicIsland) },
                { 0x4D44,   typeof(SMInitPaidBuff) },
                { 0x8C87,   typeof(SMInitHonorTitle) },
                { 0x662C,   typeof(SMInitShip) },
                { 0x6F49,   typeof(SMInitSkill) },
                { 0x663C,   typeof(SMInitLifeHuntingTrap) },
                { 0x566,    typeof(SMInitSkillPreset) },
                { 0x66E0,   typeof(SMCommonActionLifeResultNotify) },
                { 0x14CC,   typeof(SMInitPvp) },
                { 0x8193,   typeof(SMInitCharacterTendency) },
                { 0x6F42,   typeof(SMInitAdventureMatter) },
                { 0xD5C3,   typeof(SMInitCoopQuest) },
                { 0xA862,   typeof(SMInitRaid) },
                { 0xA4AB,   typeof(SMInitSail) },
                { 0x5DE4,   typeof(SMInitShipAvatar) },
                { 0x6966,   typeof(SMInitLocal) },
                { 0x3960,   typeof(SMImmuneStatusNotify) },
                { 0xE42B,   typeof(SMAddictionStateNotify) },
                { 0x6364,   typeof(SMInitSocialSkill) },
                { 0x5870,   typeof(SMInitAdvBook) },
                { 0x4973,   typeof(SMInitMedalReward) },
                { 0x54E5,   typeof(SMMusicRemoveNotify) },
                { 0x1A31,   typeof(SMInitTownSiteArea) },
                { 0x9496,   typeof(SMInitContentsConfig) },
                { 0x4219,   typeof(SMInitAbilityUnlock) },
                { 0x1FE,    typeof(SMInitCrew) },
                { 0x295,    typeof(SMInitCumulativePoint) },
                { 0x5ECB,   typeof(SMInitEnv) },
                { 0xB127,   typeof(SMInitLifeFishPot) },
                { 0x2EB1,   typeof(SMInitItem) },
                { 0xB146,   typeof(SMInitVehicle) },
                { 0x97B6,   typeof(SMInitQuestPointReward) },
                { 0x2819,   typeof(SMInitPropCollecting) },
                { 0xDCC9,   typeof(SMInitWorldIslandData) },
                //


                //{ 0x324B,   typeof(excess) },


                //

                { 0xA402,   typeof(SMInitPC) },
                { 0xEA1C,   typeof(SMReverseRuinRewardNotify) },
                { 0xF8A,    typeof(SMNewVehicle) },
                { 0x74D9,   typeof(SMPaidPurchaseUnpackResult) },
                { 0x6035,   typeof(SMImmuneNotify) },
            };

            ReverseLookup = new Dictionary<Type, ushort>();

            foreach (KeyValuePair<ushort, Type> entry in Packets)
            {
                ReverseLookup.Add(entry.Value, entry.Key);
            }
        }

        public static void Init()
        {
            Logger.Info($"Loaded {Packets.Count} worldserver packets.");
        }
    }
}
