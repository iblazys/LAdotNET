using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LAOffsetUpdater
{
    class Program
    {
        private static string version = "1.6.2.3,226";
        private static MyProcess process;

        private static List<AddressObject> scans = new List<AddressObject>()
        {
            new AddressObject("_clientEncryptPacket_MAIN", ScanType.BytePatternSearch, ValueType.InsideFunction, null, @"\x40\x56\x57\x41\x56\xB8\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x48\x2B\xE0\x48\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x48\x8B\x05\x06\x97\x24\x01", "xxxxxx????x????xxxx????????????????xxxxxxx", null, 0),
            new AddressObject("_clientEncryptPacket_ACTUAL", ScanType.BytePatternSearch, ValueType.InsideFunction, null, @"\x48\x89\x5C\x24\x10\x48\x89\x74\x24\x18\x57\x48\x83\xEC\x20\x48\x8B\x01\x48\x8B\xFA\x48\x8B\xF1\xFF\x50\x10\x48\x8D\x54\x24\x30", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", null, 0),
            new AddressObject("_serverDecryptPacket_ACTUAL", ScanType.BytePatternSearch, ValueType.InsideFunction, null, @"\x48\x8B\xC4\x56\x41\x54\x41\x55\x48\x83\xEC\x70\x45\x8B\x48\x14\x45\x8B\x50\x18\x4D\x8B\xE0\x45\x2B\xD1\x4C\x8B\xEA\x48\x8B\xF1\x41\x83\xFA\x06", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", null, 0),
        };

        private static string CreateHexBytePattern(byte[] pattern)
        {
            string buffer = @"\";

            foreach (var c in pattern)
            {
                buffer += "x" + c.ToString("x") + @"\";
            }

            return buffer;
        }

        static void Main(string[] args)
        {
            process = new MyProcess("LOSTARK");

            Stopwatch s1 = new Stopwatch();
            s1.Start();

            Task[] tasks = new Task[2];

            tasks[0] = Task.Factory.StartNew(() => GenerateAddresses());
            tasks[1] = Task.Factory.StartNew(() => GenerateServerOpcodes());

            Task.WaitAll(tasks);

            s1.Stop();
            Console.WriteLine("---- Total Time: " + s1.Elapsed.TotalSeconds + " sec");


            Console.ReadKey();
        }

        private static void GenerateAddresses()
        {
            Console.WriteLine("Generating useful addresses...");

            Stopwatch s1 = new Stopwatch();
            s1.Start();

            List<Task> scanTasks = new List<Task>();

            int index = 1;

            foreach (var scan in scans)
            {
                //Console.Title = "Address " + index + "/" + scans.Count;

                scanTasks.Add(Task.Factory.StartNew(() => scan.Find(process)));

                //scan.Find(p);

                index++;
            }

            Task.WaitAll(scanTasks.ToArray());

            string fileOutput = "";
            string fileOutputCpp = "";

            foreach (var scan in scans)
            {
                if (scan.BytePatternSearch == null)
                    scan.BytePatternSearch = new byte[] { 0 };

                if (scan.BytePatternMask == null)
                    scan.BytePatternMask = new string[] { "0" };

                if (scan.StringSearch == null)
                    scan.StringSearch = "0";

                string offsets = "";

                foreach (var offset in scan.ResultOffset)
                {
                    if (offset >= 0)
                        offsets += "0x" + offset.ToString("X") + "\t";
                    else if (offset < 0)
                    {
                        var b = offset * (-1);

                        offsets += "-0x" + b.ToString("X") + "\t";
                    }
                }

                string pattern = "";
                if (scan.BytePatternSearch != null)
                    pattern = CreateHexBytePattern(scan.BytePatternSearch);

                string mask = "";
                if (scan.BytePatternMask != null)
                    mask = String.Join("", scan.BytePatternMask);

                string entry = "public const long " + scan.Name + " = ";
                string entryCpp = "const __int64 " + scan.Name + " = ";

                scan.Results = scan.Results.Where(x => x != 0).ToList();

                long _first = 0;

                if (scan.Results.Count > 0)
                    _first = scan.Results[scan.Pick];

                entry += "0x" + (_first + scan.AddOffset).ToString("X") + ";\t// offset: 0x" + (_first - process.Module.BaseAddress.ToInt64()).ToString("X");
                // entry += "0x" + (_first - process.Module.BaseAddress.ToInt64()).ToString("X") + ";\t//\n";
                entryCpp += "0x" + (_first + scan.AddOffset).ToString("X") + ";\t//";

                foreach (var result in scan.Results.Where(x => x != 0))
                {
                    if (result == _first)
                        continue;

                    entry += "0x" + (result + scan.AddOffset).ToString("X") + "\t";
                    entryCpp += "0x" + (result + scan.AddOffset).ToString("X") + "\t";
                }

                entry += "\n";
                entryCpp += "\n";

                fileOutput += entry;
                fileOutputCpp += entryCpp;
            }

            File.WriteAllText(@".\addresses_" + version + ".txt", fileOutput);
            File.WriteAllText(@".\addresses_" + version + "_CPP.txt", fileOutputCpp);

            s1.Stop();
            TimeSpan t1 = s1.Elapsed;

            Console.WriteLine("Addresses Done! (" + scans.Count + " generated in " + t1.TotalSeconds + "sec)");
        }

        private static void GenerateServerOpcodes()
        {
            Console.WriteLine("Generating SM opcodes...");

            Stopwatch s1 = new Stopwatch();
            s1.Start();

            var references =
                process.UTF16_StringReferences.Where(x => x.Value.Name.StartsWith("PKT")).ToDictionary(x => x.Key, y => y.Value);

            string fileOutput = "";

            Console.WriteLine(references.Count + " References found");

            List<OpCode> _opCodes = new List<OpCode>();

            foreach (var _ref in references)
            {
                Console.WriteLine($"Processing {_ref.Value.Name } - address of string: {_ref.Key.ToString("X")}");

                var strLen = _ref.Value.Name.Length * 2;

                //var search = (_ref.Key + strLen); - old searching below string

                var search = _ref.Key;
                var found = false;
                var opcode = 0;

                while (!found)
                {
                    var b = process.ReadByte(search - 2);

                    if (b != 0x00)
                    {
                        if(b == 0xF7)
                        {
                            search -= 6;

                            var opcodeAddr = process.ReadInt64(search - 0x18);

                            if (process.ReadByte(opcodeAddr) == 0xB8) // mov, opcode
                            {
                                opcode = process.ReadInt32((int)(opcodeAddr - (long)process.Module.BaseAddress) + 0x1, process.Dump_MainModule); // add 1 to remove the mov byte
                                found = true;
                            }

                            //found = true;
                        }
                    }

                    search -= 2;
                }

                OpCode _oc = new OpCode(_ref.Value.Name);
                _oc.Opcode = opcode;

                _opCodes.Add(_oc);
            }

            foreach (var op in _opCodes)
            {
                var smName = "SM" + op.Name.Substring(3);
                //fileOutput += "{ 0x" + op.Opcode.ToString("X") + ",  \"" + op.Name + "\" },\n";
                fileOutput += "0x" + op.Opcode.ToString("X") + ", " + smName + "\n";
            }

            File.WriteAllText(@".\opCodes_sm_" + version + ".txt", fileOutput);

            s1.Stop();

            TimeSpan t1 = s1.Elapsed;

            Console.WriteLine("SM packets done! (" + _opCodes.Count + " generated in " + t1.TotalSeconds + "sec)");
        }
    }

    public class OpCode
    {
        public string Name { get; set; }
        public int Opcode { get; set; }

        public OpCode(string name)
        {
            Name = name;
        }
    }
}
