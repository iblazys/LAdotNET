// This file is part of LAdotNET.
//
// LAdotNET is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// LAdotNET is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with LAdotNET.  If not, see <https://www.gnu.org/licenses/>.

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
