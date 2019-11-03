using DotNetty.Buffers;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using LAdotNET.WorldServer.Network.Packets.Server;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer.Network.Packets.Client
{
    class CMMove : Packet
    {

        private bool IsMoving;

        public CMMove(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {

        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }

        public override async Task HandleAsync()
        {
            if(IsMoving)
            {
                await Connection.SendAsync(new SMMoveNotify(Connection));
            } 
            else
            {
                // await Connection.SendAsync(new SMMoveStopNotify(Connection));
            }
        }

        public override void Serialize()
        {            
            Data.SkipBytes(2);
            Data.SkipBytes(4);
            Data.SkipBytes(2);

            var pos1 = Data.ReadFloatLE();
            Data.SkipBytes(1);
            var pos2 = Data.ReadFloatLE();
            Data.SkipBytes(1);
            var pos3 = Data.ReadFloatLE();

            Data.SkipBytes(1);

            var possibleZone = Data.ReadIntLE(); // this changes based on your position, doors are 16800

            var pos4 = Data.ReadFloatLE();

            Data.SkipBytes(4);
            IsMoving = Data.ReadIntLE() == 1;

            Logger.Debug($"POS1: {pos1} POS2: {pos2} POS3: {pos3} ZONE_FLAG: {possibleZone} ISMOVING: {IsMoving.ToString()}");

            // DEBUG INFO
            /*
            Logger.Debug(
                $"\nPos1: {pos1}\n" +
                $"Pos2: {pos2}\n" +
                $"Pos3: {pos3}\n" +
                $"PossibleZone: {possibleZone}\n" +
                $"UnkFloatOrInt: {pos4}\n" +
                $"IsMoving: {IsMoving}\n"
                );
                */
                
        }
    }
}
