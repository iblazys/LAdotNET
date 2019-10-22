using System.Threading.Tasks;

namespace LAdotNET.LoginServer
{
    class Program
    {
        static async Task Main()
        {
            Loginserver server = new Loginserver();
            await server.Start();
        }
    }
}
