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
