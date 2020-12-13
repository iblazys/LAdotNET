using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using LAdotNET.Network.Packets;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LAdotNET.Network
{
    public abstract class TCPServer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public MultithreadEventLoopGroup BossGroup { get; set; }
        public MultithreadEventLoopGroup WorkerGroup { get; set; }

        public ServerBootstrap ServerBootstrap { get; set; }
        public IChannel ServerChannel { get; set; }

        public async Task RunAsync(string address, int port)
        {
            // MOVE TO CONFIG
            Environment.SetEnvironmentVariable("io.netty.allocator.maxOrder", "3");

            // DEBUG
            //InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));

            BossGroup = new MultithreadEventLoopGroup();
            WorkerGroup = new MultithreadEventLoopGroup();

            try
            {
                ServerBootstrap = new ServerBootstrap();
                ServerBootstrap.Group(BossGroup, WorkerGroup);
                ServerBootstrap.Channel<TcpServerSocketChannel>();

                ServerBootstrap
                    .Option(ChannelOption.SoBacklog, 100)
                    .Option(ChannelOption.SoReuseaddr, true) // mem optimization
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.SoKeepalive, true)
                    .Option(ChannelOption.SoRcvbuf, 4096) // ??
                    .Option(ChannelOption.Allocator, PooledByteBufferAllocator.Default)

                    .Handler(new LoggingHandler("ConsoleLogger", LogLevel.INFO))
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;

                        //pipeline.AddLast(new PacketDecoder());
                        pipeline.AddFirst(new LengthFieldBasedFrameDecoder(ByteOrder.LittleEndian, ushort.MaxValue, 0, 2, -2, 0, false));
                        pipeline.AddLast(new PacketHandler());
                        pipeline.AddLast(new PacketEncoder());
                    }));

                ServerChannel = await ServerBootstrap.BindAsync(IPAddress.Parse(address), port);

                Logger.Info($"Server is now listening on port: {address}:{port}");

                Console.ReadKey(); // blocks

                await ServerChannel.CloseAsync();
            }
            finally
            {
                Task.WaitAll(BossGroup.ShutdownGracefullyAsync(), WorkerGroup.ShutdownGracefullyAsync());
            }
        }
    }
}
