using System;
using System.Threading.Tasks;

namespace LAdotNET.WorldServer
{
    class Program
    {
        static async Task Main()
        {
            Worldserver server = new Worldserver();
            await server.Start();
        }
    }
}
