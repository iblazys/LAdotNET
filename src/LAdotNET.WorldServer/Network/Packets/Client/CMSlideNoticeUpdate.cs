using DotNetty.Buffers;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using LAdotNET.WorldServer.Network.Packets.Server;
using LAdotNET.WorldServer.Network.Packets.Server.Init;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Client
{
    class CMSlideNoticeUpdate : Packet
    {
        public CMSlideNoticeUpdate(Connection connection, IByteBuffer buffer) : base(connection, buffer)
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
                new SMBattlefieldWeeklyRewardNotify(Connection),
                new SMInitEnv(Connection),
                new SMInitPC(Connection),
                new SMInitItemAddMaxCount(Connection),
                new SMInitShip(Connection),
                new SMInitSkillPreset(Connection),
                new SMInitLifeSkill(Connection),
                new SMInitSocialSkill(Connection),
                new SMCommonActionListAddNotify(Connection),
                new SMInitQuest(Connection),
                new SMInitCompletedQuest(Connection),
                new SMInitBattleSlot(Connection),
                new SMInitCoopQuest(Connection),
                new SMInitSail(Connection),
                new SMInitShipAvatar(Connection),
                new SMInitShip(Connection),
                new SMInitMail(Connection),
                new SMInitAbility(Connection),
                new SMAddonSkillFeatureChangeNotify(Connection),
                new SMInitSquareHole(Connection),
                new SMInitAdventureMatter(Connection),
                new SMInitMusic(Connection),
                new SMMusicReturnPositionNotify(Connection),
                new SMInitVehicle(Connection),
                new SMInitContinent(Connection),
                new SMInitAchievementActive(Connection, 0),
                new SMInitAchievementActive(Connection, 1),
                new SMInitCumulativePoint(Connection),
                new SMInitCumulativePointReward(Connection),
                new SMInitEquipPreset(Connection),
                new SMInitLifeHuntingTrap(Connection),
                new SMInitLifeFishPot(Connection),
                new SMInitVoyageExploration(Connection),
                new SMInitRaid(Connection),
                new SMInitPvp(Connection),
                new SMInitZonePiece(Connection),
                new SMInitExpedition(Connection),
                new SMInitPropCollecting(Connection),
                new SMModStatisticNotify(Connection),
                new SMInitContentsConfig(Connection),
                new SMInitCharacterTendency(Connection),
                new SMInitNpcFriendship(Connection),
                new SMInitNpcFriendshipTalk(Connection),
                new SMInitNpcFriendshipAction(Connection),
                new SMInitQuestPointReward(Connection),
                new SMInitPaidAction(Connection),
                new SMInitChaosDungeonRewardCount(Connection),
                new SMVoyageLuckNotify(Connection),
                new SMInitCalendar(Connection),
                new SMImmuneStatusNotify(Connection), // immune notify
                new SMPermanentAttrAcquireNotify(Connection),
                new SMInitCard(Connection),
                new SMInitCardDeck(Connection),
                new SMInitPaidBuff(Connection),
                new SMInitPet(Connection),
                new SMReverseRuinStageInit(Connection),
                new SMInitWarpPoint(Connection),
                new SMNewZone(Connection),
                new SMPaidServiceCheckResult(Connection),
                new SMImmuneStatusNotify(Connection)

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
