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
