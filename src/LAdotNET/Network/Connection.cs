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
