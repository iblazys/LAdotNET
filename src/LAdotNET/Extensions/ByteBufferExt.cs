using DotNetty.Buffers;
using System.Text;

namespace LAdotNET.Extensions
{
    public static class ByteBufferExt
    {
        public static void WriteLAString(this IByteBuffer buffer, string v)
        {
            var bytes = Encoding.Unicode.GetBytes(v);

            buffer.WriteShortLE(bytes.Length / 2);
            buffer.WriteBytes(bytes);
        }
    }
}
