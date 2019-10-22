using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using NLog;
using System;
using System.Collections.Generic;

namespace LAdotNET.Network.Packets
{
    /// <summary>
    /// Decrypts and decompresses packets
    /// </summary>
    public class PacketDecoder : MessageToMessageDecoder<IByteBuffer>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Prepares packets to be handled by the PacketHandler
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <param name="output"></param>
        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
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

                if (!(Activator.CreateInstance(PacketFactory.Packets[opcode], message) is Packet packet))
                    return;

                Logger.Debug("[C] - {0} - op[0x{1}] - length[{2}] - encrypted[{3}] - compression[{4}]", 
                    packet.GetType().Name,
                    packet.OpCode.ToString("X"),
                    packet.Length,
                    packet.IsEncrypted.ToString(),
                    packet.CompressionType.ToString());

                output.Add(packet);
            }
        }

        public override bool IsSharable => true;
    }
}
