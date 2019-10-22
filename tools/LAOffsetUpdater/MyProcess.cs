using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LAOffsetUpdater
{
    public class StringReference
    {
        public long Address { get; set; }
        public string Name { get; set; }
        public List<long> Usage { get; set; }

        public StringReference(long address, string name)
        {
            Address = address;
            Name = name;

            Usage = new List<long>();
        }
    }

    public class FunctionReference
    {
        public long Address { get; set; }
        public string Name { get; set; }
        public List<long> Usage { get; set; }

        public FunctionReference(long address, string name)
        {
            Address = address;
            Name = name;

            Usage = new List<long>();
        }
    }

    public class MyProcess : MemoryReader
    {
        private Process Process { get; set; }
        
        public ProcessModule Module { get; set; }
        public byte[] Dump_MainModule { get; set; }

        public Dictionary<long, StringReference> ASCII_StringReferences { get; set; }
        public Dictionary<long, StringReference> UTF16_StringReferences { get; set; }
        public Dictionary<long, FunctionReference> FunctionReferences { get; set; }

        public MyProcess(string name)
        {
            Process = Process.GetProcessesByName(name).FirstOrDefault();
            Handle = (int)GetHandle();

            if (Process == null)
            {
                Console.WriteLine("Cant find " + name);
                Console.ReadKey();
                Environment.Exit(0);
            }

            Dump_MainModule = DumpModule("LOSTARK.exe");

            ASCII_StringReferences = GetASCIIStringReferences();
            UTF16_StringReferences = GetUTF16StringReferences();

            FunctionReferences = new Dictionary<long, FunctionReference>();

            GenerateStringReferenceUsages();
        }

        private IntPtr GetHandle()
        {
            if (Process != null)
                return Win32.OpenProcess(0x1F0FFF, false, Process.Id);
            
            return IntPtr.Zero;
        }

        private byte[] DumpModule(string name)
        {
            Console.WriteLine("Dumping Module " + name);

            Module = null;

            foreach (ProcessModule m in Process.Modules)
            {
                if (m.ModuleName == name)
                    Module = m;
            }

            if(Module == null)
                return null;

            byte[] dump = ReadByteArray((long)Module.BaseAddress, Module.ModuleMemorySize);

            return dump;
        }

        private Dictionary<long, StringReference> GetASCIIStringReferences()
        {
            Console.WriteLine("Collecting ASCII String-References");

            Dictionary<long, StringReference> buffer = new Dictionary<long, StringReference>();

            for (int i = 0; i < Dump_MainModule.Length; i+=4)
            {
                
                string s = ReadStringASCII(i, Dump_MainModule);

                if (s != "")
                {
                    StringReference sr = new StringReference((long)Module.BaseAddress + i, s);

                    buffer.Add(sr.Address, sr);
                }
            }

            return buffer;
        }

        private Dictionary<long, StringReference> GetUTF16StringReferences()
        {
            Console.WriteLine("Collecting UTF16 String-References");

            Dictionary<long, StringReference> buffer = new Dictionary<long, StringReference>();

            for (int i = 0; i < Dump_MainModule.Length; i += 4)
            {

                string s = ReadStringUnicode(i, Dump_MainModule);

                if (s != "")
                {
                    StringReference sr = new StringReference((long)Module.BaseAddress + i, s);

                    buffer.Add(sr.Address, sr);
                }
            }

            return buffer;
        }

        public void GenerateStringReferenceUsages()
        {
            Console.WriteLine("Generating String-Reference Usages");

            for (int i = 0; i < Dump_MainModule.Length; i++)
            {

                var adr = ((long)Module.BaseAddress + i);

                if (ReadByte(i, Dump_MainModule) == 0x48 || ReadByte(i, Dump_MainModule) == 0x4C)
                {
                    if (ReadByte(i + 1, Dump_MainModule) == 0x8D)
                    {
                        var value = (adr + 7) + ReadInt32(i + 3, Dump_MainModule);

                        if(ASCII_StringReferences.ContainsKey(value))
                            ASCII_StringReferences[value].Usage.Add(adr);
                        if (UTF16_StringReferences.ContainsKey(value))
                            UTF16_StringReferences[value].Usage.Add(adr);
                    }
                    else if (ReadByte(i + 1, Dump_MainModule) == 0x83)
                    {
                        var value = (adr + 8) + ReadInt32(i + 3, Dump_MainModule);

                        if (ASCII_StringReferences.ContainsKey(value))
                            ASCII_StringReferences[value].Usage.Add(adr);
                        if (UTF16_StringReferences.ContainsKey(value))
                            UTF16_StringReferences[value].Usage.Add(adr);
                    }
                }
            }
        }

        public void GenerateFunctionCallReferenceUsages()
        {
            Console.WriteLine("Generating Function-Call-Reference Usages");

            for (int i = 0; i < Dump_MainModule.Length; i++)
            {

                var adr = ((long)Module.BaseAddress + i);

                if (ReadByte(i, Dump_MainModule) == 0xE8)
                {
                    var value = (adr + 5) + ReadInt32(i + 1, Dump_MainModule);

                    if (FunctionReferences.ContainsKey(value))
                        FunctionReferences[value].Usage.Add(adr);

                }
            }
        }

        public List<long> FindFunctionCallUsage(long funcAdr, string name)
        {
            Console.WriteLine("Looking for Function-Call-Reference Usage of " + name);

            List<long> buffer = new List<long>();

            for (int i = 0; i < Dump_MainModule.Length; i++)
            {

                var adr = ((long)Module.BaseAddress + i);

                if (ReadByte(i, Dump_MainModule) == 0xE8)
                {

                    var value = ReadInt32(i + 1, Dump_MainModule);

                    if ((adr + 5) + value == funcAdr)
                    {
                        buffer.Add(adr);

                        Console.WriteLine("Usage found at " + adr.ToString("X"));
                    }

                }
            }

            return buffer;
        }

        public List<long> FindStringReferenceUsage(long stringRef, string name)
        {
            Console.WriteLine("Looking for String-Reference Usage of " + name);

            List<long> buffer = new List<long>();

            for (int i = 0; i < Dump_MainModule.Length; i++)
            {
                
                var adr = ((long) Module.BaseAddress + i);
                
                if (ReadByte(i, Dump_MainModule) == 0x48 || ReadByte(i, Dump_MainModule) == 0x4C)
                {
                    if (ReadByte(i + 1, Dump_MainModule) == 0x8D)
                    {
                        var value = ReadInt32(i + 3, Dump_MainModule);

                        if ((adr + 7) + value == stringRef)
                        {
                            buffer.Add(adr);

                            Console.WriteLine("Usage found at " + adr.ToString("X"));
                        }
                    }
                    else if (ReadByte(i + 1, Dump_MainModule) == 0x83)
                    {
                        var value = ReadInt32(i + 3, Dump_MainModule);

                        if ((adr + 8) + value == stringRef)
                        {
                            buffer.Add(adr);

                            Console.WriteLine("Usage found at " + adr.ToString("X"));
                        }
                    }
                }
            }

            return buffer;
        }

        public long GetQWordPointerValue(long address)
        {
            long offset = address - (long) Module.BaseAddress;

            int instrSize = 0;

            if (ReadByte((int) offset, Dump_MainModule) == 0x48 || ReadByte((int)offset, Dump_MainModule) == 0x4C)
            {
                if (ReadByte((int) offset + 1, Dump_MainModule) == 0x8B || ReadByte((int)offset + 1, Dump_MainModule) == 0x89 || ReadByte((int)offset + 1, Dump_MainModule) == 0x8D)
                {
                    instrSize = 7;

                    return address + instrSize + ReadInt32((int)offset + 3, Dump_MainModule);
                }
                else
                {
                    instrSize = 8;

                    return address + instrSize + ReadInt32((int)offset + 3, Dump_MainModule);
                }
                
            }
            else if (ReadByte((int)offset, Dump_MainModule) == 0x44)
            {
                if (ReadByte((int)offset + 1, Dump_MainModule) == 0x39)
                {
                    instrSize = 7;

                    return address + instrSize + ReadInt32((int)offset + 3, Dump_MainModule);
                }

            }
            else if (ReadByte((int)offset, Dump_MainModule) == 0xE8 || ReadByte((int)offset, Dump_MainModule) == 0xE9)
            {
                instrSize = 5;

                return address + instrSize + ReadInt32((int)offset + 1, Dump_MainModule);
            }

            else if (ReadByte((int)offset, Dump_MainModule) == 0x39)
            {
                instrSize = 6;

                return address + instrSize + ReadInt32((int)offset + 2, Dump_MainModule);
            }

            else if (ReadByte((int)offset, Dump_MainModule) == 0x88)
            {
                instrSize = 6;

                return address + instrSize + ReadInt32((int)offset + 2, Dump_MainModule);
            }

            return 0;
        }

        public long GetQWordPointerValue(long address, long[] offsets)
        {

            long _base = address;

            foreach (var offset in offsets)
            {
                if (_base == 0)
                    break;

                _base = GetQWordPointerValue(_base + offset);
            }

            return _base;
        }

        public List<long> FindBytePattern(byte[] pattern, string[] mask)
        {
            List<long> buffer = new List<long>();

            int startOffset = 0;

            for (int i = 0; i < Dump_MainModule.Length; i++)
            {
                if (Dump_MainModule[i] == pattern[0])
                {
                    startOffset = i;

                    bool found = true;

                    for (int x = 1; x < pattern.Length; x++)
                    {
                        if (Dump_MainModule[i + x] != pattern[x] && mask[x] == "x")
                        {
                            found = false;

                            break;
                        }
                    }

                    if(found)
                        buffer.Add((long)Module.BaseAddress + startOffset);
                }
            }

            return buffer;
        } 
    }
}
