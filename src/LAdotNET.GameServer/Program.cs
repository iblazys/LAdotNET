using System.Threading.Tasks;

namespace LAdotNET.GameServer
{
    class Program
    {
        static async Task Main()
        {
            Gameserver server = new Gameserver();
            await server.Start();
        }
    }
}
