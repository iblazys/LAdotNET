using DotNetty.Buffers;
using System.Text;

namespace LAdotNET.Extensions
{
    public static class ByteBufferExt
    {
        /// <summary>
        /// Write a length prefixed unicode string
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="v"></param>
        public static void WriteUnicodeString(this IByteBuffer buffer, string v)
        {
            var bytes = Encoding.Unicode.GetBytes(v);

            buffer.WriteShortLE(bytes.Length / 2);
            buffer.WriteBytes(bytes);
        }

        /// <summary>
        /// Write a length prefixed ascii string
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="v"></param>
        public static void WriteString(this IByteBuffer buffer, string v)
        {
            var bytes = Encoding.ASCII.GetBytes(v);

            buffer.WriteShortLE(bytes.Length / 2);
            buffer.WriteBytes(bytes);
        }

        /// <summary>
        /// Read a length prefixed unicode string
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="v"></param>
        public static string ReadUnicodeString(this IByteBuffer buffer)
        {
            var len = (buffer.ReadShortLE() * 2);
            return buffer.ReadString(len, Encoding.Unicode);
        }

        /// <summary>
        /// Read a length prefixed ascii string
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="v"></param>
        public static string ReadString(this IByteBuffer buffer)
        {
            var len = buffer.ReadShortLE();
            return buffer.ReadString(len, Encoding.ASCII);
        }
    }
}
