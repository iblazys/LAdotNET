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
