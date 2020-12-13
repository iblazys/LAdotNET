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

using DotNetty.Transport.Channels;
using LAdotNET.Network.Encryption;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAdotNET.Network
{
    /// <summary>
    /// Represents a client connection
    /// </summary>
    public class Connection
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The Netty Channel
        /// </summary>
        public IChannel Channel { get; set; }

        /// <summary>
        /// The encryption counter for client packets
        /// </summary>
        public Counter Counter { get; set; }

        /// <summary>
        /// The account for this connection
        /// </summary>
        public Account Account { get; set; }

        public Connection()
        {
            Counter = new Counter();
        }

        /// <summary>
        /// Sets the channel
        /// </summary>
        /// <param name="channel"></param>
        public void SetChannel(IChannel channel)
        {
            Channel = channel;
        }

        /// <summary>
        /// Sends a packet asynchronously
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public async Task SendAsync(Packet packet)
        {
            await Channel.WriteAndFlushAsync(packet);
            
            /*
            Logger.Debug("[S] - {0} - op[0x{1}] - length[{2}] - encrypted[{3}] - compression[{4}]",
                packet.GetType().Name,
                packet.OpCode.ToString("X"),
                packet.Length,
                packet.IsEncrypted.ToString(),
                packet.CompressionType.ToString());
                */
        }

        /// <summary>
        /// Sends a list of packets asynchronously
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public async Task SendAsync(IEnumerable<Packet> packets)
        {
            foreach(Packet packet in packets)
            {
                await Channel.WriteAndFlushAsync(packet);
            }
        }
    }
}
