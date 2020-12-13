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

using LAdotNET.Config;

namespace LAdotNET.LoginServer.Config
{
    class LoginServerConfiguration : Configuration
    {
        public LoginConfig Login { get; set; }

        /// <summary>
        /// Initalize the configuration
        /// </summary>
        public LoginServerConfiguration() : base()
        {
            Login = new LoginConfig();
        }

        /// <summary>
        /// Loads all config files
        /// </summary>
        public void Load()
        {
            LoadDefaults();

            Login = LoadYaml<LoginConfig>("loginserver_config.yml");
        }
    }
}
