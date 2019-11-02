using LAdotNET.GameServer.Network;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.GameServer
{
    class Gameserver : TCPServer
    {
        // Instance
        public static readonly Gameserver Instance = new Gameserver();

        // Configuration
        //public static LoginServerConfiguration Config { get; private set; }

        /// <summary>
        /// Loginserver loader
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            // Make the console look fine as hell'
            ConsoleUtils.WriteHeader(ConsoleColor.Cyan, ConsoleUtils.ServerType.GAMESERVER);

            // Load Config
            //Config = new LoginServerConfiguration();
            //Config.Load();

            // Load Opcodes
            PacketFactory.Load();

            //await base.RunAsync(Config.Network, new LoginSessionInitializer());
            await base.RunAsync("127.0.0.1", 6020);

            Console.ReadKey();
        }
    }
}
