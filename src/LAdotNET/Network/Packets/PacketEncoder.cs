using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using LAdotNET.Utils;
using System.Threading.Tasks;

namespace LAdotNET.Network.Packets
{
    public class PacketEncoder : ChannelHandlerAdapter
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public override Task WriteAsync(IChannelHandlerContext context, object msg)
        {
            if (!(msg is Packet packet)) return base.WriteAsync(context, null);

            packet.Deserialize();

            // DEBUG
            Logger.Debug("[S] - {0} - op[0x{1}] - length[{2}] - encrypted[{3}] - compression[{4}]",
                packet.GetType().Name,
                packet.OpCode.ToString("X"),
                packet.Length,
                packet.IsEncrypted.ToString(),
                packet.CompressionType.ToString());

            Logger.Debug($"\n{HexUtils.Dump(packet.Data)}");

            if (packet.CompressionType != CompressionType.NONE)
                packet.Compress(); // TODO: LZ4

            if (packet.IsEncrypted)
                packet.Encrypt();

            var header = Unpooled.Buffer();

            packet.Length = (ushort)(packet.Data.ReadableBytes + 6); // current packet size + header length
            //packet.Length = (ushort)(packet.Data.ReadableBytes + 10); // 10 BYTE HEADER FOR RU

            header.WriteUnsignedShortLE((ushort)packet.Length);
            header.WriteUnsignedShortLE(packet.OpCode);
            header.WriteByte((byte)packet.CompressionType); // TODO: LZ4 (Flag: 1)
            header.WriteByte(packet.IsEncrypted ? 1 : 0); // Crypt Flag

            // RU TEST
            //header.WriteIntLE(0);

            base.WriteAsync(context, header);

            return base.WriteAsync(context, packet.Data);
        }
    }
}
