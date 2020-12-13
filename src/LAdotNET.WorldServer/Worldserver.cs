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
