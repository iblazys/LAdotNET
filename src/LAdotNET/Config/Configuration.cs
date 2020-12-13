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

using NLog;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LAdotNET.Config
{
    public abstract class Configuration
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static string ConfigDir = Path.Combine(Environment.CurrentDirectory, "config");

        public NetworkConfig Network { get; private set; }

        /// <summary>
        /// Initialize the default configuration files shared by servers
        /// </summary>
        public Configuration()
        {
            Network = new NetworkConfig();
        }

        /// <summary>
        /// Loads all default config files
        /// </summary>
        public void LoadDefaults()
        {
            Network = LoadYaml<NetworkConfig>("network_config.yml");
        }

        /// <summary>
        /// Loads the specified file from the /config/ directory
        /// </summary>
        /// <typeparam name="T">the config type</typeparam>
        /// <param name="fileName">the file name including extension</param>
        /// <returns></returns>
        public T LoadYaml<T>(string fileName)
        {
            Logger.Info($"Loading configuration file: {fileName}");

            var deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

            using (var reader = new StreamReader(Path.Combine(ConfigDir, fileName)))
            {
                return deserializer.Deserialize<T>(reader);
            }
        }
    }
}
