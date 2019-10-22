using System.Collections.Generic;

namespace LAdotNET.LoginServer.Config
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
    }

    public class LoginConfig
    {
        public List<Server> ServerList = new List<Server>();
    }
}