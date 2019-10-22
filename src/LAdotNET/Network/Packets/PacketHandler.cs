using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;

namespace LAdotNET.Network.Packets
{
    public class PacketHandler : ChannelHandlerAdapter
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Connection Connection { get; set; }

        public override bool IsSharable => true;

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            Connection = new Connection();
            Connection.SetChannel(context.Channel);
            
            Logger.Info($"Connected - {Connection.Channel}");
            base.ChannelRegistered(context);
        }

        public override async void ChannelRead(IChannelHandlerContext context, object msg)
        {
            var message = (IByteBuffer)msg;

            while (message.ReadableBytes > 0)
            {
                message.MarkReaderIndex();

                var length = message.ReadUnsignedShortLE();
                var opcode = message.ReadUnsignedShortLE();

                if (!PacketFactory.Packets.ContainsKey(opcode))
                {
                    Logger.Error($"Unknown packet recieved! - OP: 0x{opcode.ToString("X")}, L: {length}");
                    return;
                }

                message.ResetReaderIndex();

                if (!(Activator.CreateInstance(PacketFactory.Packets[opcode], Connection, message) is Packet packet))
                    return;

                Logger.Debug("[C] - {0} - op[0x{1}] - length[{2}] - encrypted[{3}] - compression[{4}]",
                    packet.GetType().Name,
                    packet.OpCode.ToString("X"),
                    packet.Length,
                    packet.IsEncrypted.ToString(),
                    packet.CompressionType.ToString());

                if (packet.IsEncrypted)
                    packet.Decrypt();

                if (packet.CompressionType != CompressionType.NONE)
                    packet.Uncompress();

                packet.Serialize();

                await packet.HandleAsync();                
            }
        }

        /*
        protected override async void ChannelRead0(IChannelHandlerContext ctx, Packet packet)
        {
            packet.SetClient(Connection);

            if (packet.IsEncrypted)
                packet.Decrypt();

            if (packet.CompressionType != CompressionType.NONE)
                packet.Uncompress();

            packet.Serialize();

            await packet.HandleAsync();

            // test
            packet.Data.Release();
        }
        */

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            Logger.Info($"Disconnected - {Connection.Channel}");

            Connection = null;

            base.ChannelUnregistered(context);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Logger.Error($"Unhandled Exception! {exception}");
            context.CloseAsync();
        }
    }
}
