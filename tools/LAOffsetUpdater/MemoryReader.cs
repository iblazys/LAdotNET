using System;
using System.Text;

namespace LAOffsetUpdater
{
    public class MemoryReader
    {
        public int Handle { get; set; }

        public MemoryReader()
        {
            
        }

        public byte ReadByte(long address)
        {
            byte[] buffer = new byte[sizeof(byte)];
            int bytesRead = 0;

            Win32.ReadProcessMemory(Handle, address, buffer, buffer.Length, ref bytesRead);

            return buffer[0];
        }

        public byte ReadByte(int index, byte[] data)
        {
            if (index >= data.Length || index < 0)
                return 0;

            return data[index];
        }

        public byte[] ReadByteArray(long address, int Size)
        {
            byte[] buffer = new byte[Size];
            int bytesRead = 0;

            Win32.ReadProcessMemory(Handle, address, buffer, buffer.Length, ref bytesRead);

            return buffer;
        }

        public long ReadInt64(long address)
        {
            byte[] buffer = new byte[sizeof(long)];
            int bytesRead = 0;

            Win32.ReadProcessMemory(Handle, address, buffer, buffer.Length, ref bytesRead);

            return BitConverter.ToInt64(buffer, 0);
        }

        public long ReadInt64(int index, byte[] data)
        {
            if (index + sizeof (long) > data.Length)
                return 0;

           return BitConverter.ToInt64(data, index);
        }

        public int ReadInt32(int index, byte[] data)
        {
            if (index + sizeof(int) > data.Length)
                return 0;

            return BitConverter.ToInt32(data, index);
        }

        public string ReadStringUnicode(long address)
        {
            string rtn = "";

            byte[] buffer = new byte[512];
            int bytesRead = 0;

            byte[] b = new byte[1];

            int count = 0;
            int countB = 0;

            for (int i = 0; i < 512; i++)
            {
                Win32.ReadProcessMemory(Handle, address + i, b, b.Length, ref bytesRead);

                buffer[i] = b[0];

                if (b[0] == 0)
                    count++;
                if (b[0] != 0)
                {
                    count--;
                    countB++;
                }

                if (count >= 2)
                    break;

                if (count < -1)
                    return rtn;
            }

            byte[] bufferB = new byte[countB * 2];
            Array.Copy(buffer, bufferB, countB * 2);

            rtn = Encoding.Unicode.GetString(bufferB);

            return rtn;
        }

        public string ReadStringUnicode(int index, byte[] data)
        {
            string rtn = "";

            byte[] buffer = new byte[512];

            int length = buffer.Length;

            if (index + length > data.Length)
                length = data.Length - index;

            byte[] b = new byte[1];

            int count = 0;
            int countB = 0;

            for (int i = 0; i < length; i++)
            {
                b[0] = data[index + i];

                buffer[i] = b[0];

                if (b[0] == 0)
                    count++;
                if (b[0] != 0)
                {
                    count--;
                    countB++;
                }

                if (count >= 2)
                    break;

                if (count < -1)
                    return rtn;
            }

            byte[] bufferB = new byte[countB * 2];
            Array.Copy(buffer, bufferB, countB * 2);

            rtn = Encoding.Unicode.GetString(bufferB);

            return rtn;
        }

        public string ReadStringASCII(long address)
        {
            byte[] buffer = new byte[512];
            int bytesRead = 0;

            Win32.ReadProcessMemory(Handle, address, buffer, buffer.Length, ref bytesRead);

            int counter = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                    break;

                counter++;
            }

            byte[] a = new byte[counter];
            Array.Copy(buffer, 0, a, 0, counter);

            string output = Encoding.ASCII.GetString(a);

            return output;
        }

        public string ReadStringASCII(int index, byte[] data)
        {
            byte[] buffer = new byte[512];

            int length = buffer.Length;

            if (index + length > data.Length)
                length = data.Length - index;

            Array.Copy(data, index, buffer, 0, length);

            int counter = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                    break;

                counter++;
            }

            byte[] a = new byte[counter];
            Array.Copy(buffer, 0, a, 0, counter);

            string output = Encoding.ASCII.GetString(a);

            return output;
        }
    }
}
