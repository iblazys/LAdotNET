// This file is part of LAdotNET.
//
// LAdotNET is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// LAdotNET is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with LAdotNET.  If not, see <https://www.gnu.org/licenses/>.

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
