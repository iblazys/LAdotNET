using LAdotNET.LoginServer.Config;
using LAdotNET.LoginServer.Network;
using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using System;
using System.Threading.Tasks;

namespace LAdotNET.LoginServer
{
    class Loginserver : TCPServer
    {
        // Instance
        public static readonly Loginserver Instance = new Loginserver();

        // Configuration
        public static LoginServerConfiguration Config { get; private set; }

        /// <summary>
        /// Loginserver loader
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            // Make the console look fine as hell'
            ConsoleUtils.WriteHeader(ConsoleColor.Cyan, ConsoleUtils.ServerType.LOGINSERVER);

            // Load Config
            Config = new LoginServerConfiguration();
            Config.Load();

            // Load Opcodes
            PacketFactory.Load();

            //await base.RunAsync(Config.Network, new LoginSessionInitializer());
            await base.RunAsync("127.0.0.1", 6010);
            
            Console.ReadKey();
        }
    }
}
