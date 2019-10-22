using DotNetty.Buffers;
using LAdotNET.Network.Encryption;
using NLog;
using Snappy;
using System;
using System.Threading.Tasks;

namespace LAdotNET.Network.Packets
{
    public enum CompressionType
    {
        NONE,
        LZ4,
        SNAPPY
    }

    public abstract class Packet
    {
        /// <summary>
        /// Logger for packets
        /// </summary>
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The client connection
        /// </summary>
        public Connection Connection { get; set; }

        /// <summary>
        /// The packet length
        /// </summary>
        public ushort Length { get; set; }

        /// <summary>
        /// The packet opcode
        /// </summary>
        public ushort OpCode { get; set; }

        /// <summary>
        /// The compression type used on the data
        /// </summary>
        public CompressionType CompressionType { get; set; }

        /// <summary>
        /// Is the packet encrypted
        /// </summary>
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// The packet data
        /// </summary>
        public IByteBuffer Data { get; set; }

        /// <summary>
        /// Builds a packet from a IByteBuffer
        /// </summary>
        /// <param name="buffer"></param>
        public Packet(Connection connection, IByteBuffer buffer)
        {
            Connection = connection;

            Length = buffer.ReadUnsignedShortLE();
            OpCode = buffer.ReadUnsignedShortLE();
            CompressionType = (CompressionType)buffer.ReadByte();
            IsEncrypted = buffer.ReadByte() == 1;

            Data = buffer.ReadBytes(Length - 6);
        }

        public Packet(Connection connection)
        {
            Connection = connection;

            Data = Unpooled.Buffer();
            CompressionType = CompressionType.NONE;
            IsEncrypted = true;
        }

        /// <summary>
        /// Encrypts the packet data - generally used for server packets
        /// </summary>
        /// <returns></returns>
        public bool Encrypt()
        {
            LACrypt.Xor(Data, OpCode);
            return true;
        }

        /// <summary>
        /// Decrypts the packet data
        /// </summary>
        /// <returns></returns>
        public bool Decrypt()
        {
            LACrypt.Xor(Data, Connection.Counter.Count);
            Connection.Counter.Increase();

            return true;
        }

        /// <summary>
        /// Compresses the packet data
        /// </summary>
        /// <returns></returns>
        public bool Compress()
        {
            if (CompressionType != CompressionType.SNAPPY)
                return false;

            // We have to add 16 bytes to the start of the packet data
            // in order for the client to be able to uncompress it.

            var padded = new byte[Data.ReadableBytes + 16];
            Buffer.BlockCopy(Data.Array, 0, padded, 16, padded.Length - 16);

            Data.Release();

            // Set the new data
            Data = Unpooled.WrappedBuffer(SnappyCodec.Compress(padded));
            return true;
        }

        /// <summary>
        /// Uncompresses the packet data
        /// </summary>
        /// <returns></returns>
        public bool Uncompress()
        {
            throw new NotImplementedException();
        }

        public void SetClient(Connection connection)
        {
            Connection = connection;
        }

        public abstract void Serialize();
        public abstract void Deserialize();
        public abstract Task HandleAsync();
    }
}
