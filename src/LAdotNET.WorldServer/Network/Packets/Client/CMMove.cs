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
        // Temporary
        private enum MoveType
        {
            STOP,
            NORMAL,
            KNOCKDOWN
        }

        public CMMove(Connection connection, IByteBuffer buffer) : base(connection, buffer)
        {

        }

        public override void Deserialize()
        {
            throw new NotImplementedException();
        }

        public override async Task HandleAsync()
        {
            
        }

        public override void Serialize()
        {          
            Data.SkipBytes(8);

            Data.ReadIntLE(); // unk
            Data.ReadShortLE(); // unk

            var PosY = Data.ReadShortLE();
            Data.ReadShortLE(); // unk
            var PosX = Data.ReadShortLE(); // CONFIRMED CORRECT

            Data.SkipBytes(15); // unk stuff

            MoveType moveType = (MoveType)Data.ReadByte();

            Data.ReadIntLE(); // unk zeros

            Data.ResetReaderIndex();
            Logger.Debug($"\n{HexUtils.Dump(Data)}");
            Logger.Debug($"X,Y[{PosX}, {PosY}] MoveType[{moveType.ToString()}]");

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
