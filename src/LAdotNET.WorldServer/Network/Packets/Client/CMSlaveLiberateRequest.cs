using DotNetty.Buffers;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using LAdotNET.WorldServer.Network.Packets.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Client
{
    class CMSlaveLiberateRequest : Packet
    {
        public CMSlaveLiberateRequest(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {
            //
        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }

        public override async Task HandleAsync()
        {
            // TEST DUMMY WORLD ENTRANCE

            List<Packet> pkts = new List<Packet>
            {
                // top
                new SMBattlefieldVoteSuggestResult(Connection),
                new SMInitDynamicIsland(Connection),
                new SMInitPaidBuff(Connection),
                new SMInitHonorTitle(Connection),
                new SMInitShip(Connection),
                new SMInitSkill(Connection),
                new SMInitLifeHuntingTrap(Connection),
                new SMInitSkillPreset(Connection),
                new SMCommonActionLifeResultNotify(Connection),
                new SMInitPvp(Connection),
                new SMInitCharacterTendency(Connection),
                new SMInitAdventureMatter(Connection),
                new SMInitCoopQuest(Connection),
                new SMInitRaid(Connection),
                new SMInitSail(Connection),
                new SMInitShipAvatar(Connection),
                new SMInitLocal(Connection),
                new SMImmuneStatusNotify(Connection),
                new SMAddictionStateNotify(Connection),
                new SMInitSocialSkill(Connection),
                new SMInitAdvBook(Connection),
                new SMInitMedalReward(Connection),
                new SMMusicRemoveNotify(Connection),
                new SMInitTownSiteArea(Connection),
                new SMInitContentsConfig(Connection),
                new SMInitAbilityUnlock(Connection, 0),
                new SMInitAbilityUnlock(Connection, 1),
                new SMInitCrew(Connection),
                new SMInitCumulativePoint(Connection),
                new SMInitEnv(Connection),
                new SMInitLifeFishPot(Connection),
                new SMInitItem(Connection),
                new SMInitVehicle(Connection),
                new SMInitQuestPointReward(Connection),
                new SMInitPropCollecting(Connection),
                new SMInitWorldIslandData(Connection),
                new SMInitExcessMoney(Connection),
                new SMInitPet(Connection),
                new SMModSignetCountNotify(Connection),
                new SMInitContentsBookmark(Connection),
                new SMInitChaosDungeonRewardCount(Connection),
                new SMInitNpcFriendshipAction(Connection),
                new SMInitNpcFriendship(Connection),
                new SMInitMusic(Connection),
                new SMInitQuest(Connection),
                new SMInitNpcFriendshipTalk(Connection),
                new SMInitCard(Connection),
                new SMVoyageLinerUpdateNotify(Connection),
                new SMInitBattleSlot(Connection),
                new SMImmuneNotify(Connection), // immune notify
                new SMPeriodUpdateStatNotify(Connection),
                new SMInitCardDeck(Connection),
                new SMInitCalendar(Connection),
                new SMInitPaidAction(Connection),
                new SMInitPC(Connection),
                new SMReverseRuinRewardNotify(Connection),
                new SMInitVoyageExploration(Connection),
                new SMNewVehicle(Connection),
                new SMPaidPurchaseUnpackResult(Connection),
                new SMImmuneNotify(Connection)

                // cm packet should be received after this, 0x51B0
            };

            await Connection.SendAsync(pkts);
        }

        public override void Serialize()
        {
            Logger.Debug("\n" + HexUtils.Dump(Data));
        }
    }
}
