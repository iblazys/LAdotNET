using LAdotNET.Network;
using LAdotNET.Network.Packets;
using LAdotNET.Utils;
using LAdotNET.WorldServer.Network;
using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer
{
    class Worldserver : TCPServer
    {
        // Instance
        public static readonly Worldserver Instance = new Worldserver();

        // Configuration
        //public static LoginServerConfiguration Config { get; private set; }

        /// <summary>
        /// Worldserver loader
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            // Make the console look fine as hell'
            ConsoleUtils.WriteHeader(ConsoleColor.Cyan, ConsoleUtils.ServerType.WORLDSERVER);

            // Load Config
            //Config = new LoginServerConfiguration();
            //Config.Load();

            // Load Opcodes
            PacketFactory.Load();

            await base.RunAsync("127.0.0.1", 6040);

            Console.ReadKey();
        }
    }
}
