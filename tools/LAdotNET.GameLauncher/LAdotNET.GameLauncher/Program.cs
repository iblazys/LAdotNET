using Memory;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LAdotNET.GameLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            Mem m = new Mem();

            if (File.Exists("Binaries//Win64//LOSTARK.exe")) // Check EXE
            {
                try
                {
                    Process startProcess = new Process();
                    startProcess.StartInfo.Arguments = "--launch_from_launcher -AUTOLOGIN -ID=TESTUSER -PASSWORD=TESTPASS -PCNAME=TESTPCNAME";
                    startProcess.StartInfo.FileName = "LOSTARK.exe";
                    startProcess.StartInfo.WorkingDirectory = "Binaries//Win64";
                    startProcess.Start();
                }
                catch
                {
                    Console.WriteLine("Failed to start LOSTARK.exe");
                }

                Thread.Sleep(1000);

                Console.WriteLine("Starting LOSTARK.exe");

                if (m.OpenProcess("LOSTARK"))
                {
                    Console.WriteLine("Attached to LOSTARK.exe");
                    Console.WriteLine("Injecting x64lahook.dll");

                    Thread.Sleep(1500);

                    // FULL PATH TO DLL FILE
                    m.InjectDLL("E:\\Games\\LOSTARK\\x64lahook.dll");

                    //var baseAddress = m.theProc.MainModule.BaseAddress.ToInt64();
                    //Console.WriteLine("BASE ADDRESS: 0x" + baseAddress.ToString("X"));
                }
                
                m.closeProcess();
                
            }
            else
            {
                Console.WriteLine("Cant find LOSTARK.exe");
            }

            StartPipes();

            Console.ReadKey();
        }

        public static void StartPipes()
        {
            Console.WriteLine("Starting pipe server: sgup_ipc_server");

            using (NamedPipeServerStream server = new NamedPipeServerStream("sgup_ipc_server"))
            {
                Console.WriteLine("Waiting for connection");
                server.WaitForConnection(); // blocks
                Console.WriteLine("LOSTARK Client connected");

                while(server.IsConnected)
                {
                    byte[] buffer = new byte[516]; // client communicates in blocks of 516 bytes
                    server.Read(buffer, 0, 516);

                    Console.WriteLine(HexDump.Dump(buffer));

                    // SEND BACK PASSWORD
                    //server.Write(Reply.TestReply, 0, Reply.TestReply.Length);

                    byte[] reply;

                    using (MemoryStream stream = new MemoryStream())
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        writer.Write((byte)0);
                        writer.Write((byte)7);
                        writer.Write((byte)8);
                        writer.Write((byte)16);

                        writer.Write((int)318926848); // must be random each time?

                        writer.Write((long)1); // must be 1

                        var user = Encoding.ASCII.GetBytes("testuser");

                        writer.Write(user);
                        writer.Write(new byte[(512 - user.Length)]);

                        writer.Write(new byte[512]); // spacer

                        var pass = Encoding.ASCII.GetBytes("testpass");
                        //var pass = Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiJ9.eyJhcHBsaWNhdGlvbl9ubyI6MzIwMDAsInRva2VuIjoiMmQwMjIyOTAwMDk5NDBkODhkMzlkNjg0ZGEwODc3MWFlMjMyMzI0NTRmNzJjMGJiMGFiZjk2ODQ0YmNkZWFhYSIsImV4cGlyZV90aW1lIjoxNTcyNzU0MTQxNDYzLCJtZW1iZXJfbm8iOjg4MDgwOTI5fQ.JgfQmKtz0yDhffcxoQQw0e3RaXQqcZt21OF6wnyLE1A");

                        writer.Write(pass);
                        writer.Write(new byte[(512 - pass.Length)]);

                        writer.Write(new byte[512]); // spacer

                        writer.Write(21599); // ??

                        reply = stream.ToArray();
                    }

                    server.Write(reply, 0, reply.Length);
                    server.Flush();
                    
                    Console.WriteLine($"REPLIED WITH\n{HexDump.Dump(reply)}");
                }
                Console.WriteLine("Client disconnected. Launcher closing...");
            }
        }
        
        //182.162.7.47:44333
    }
}
