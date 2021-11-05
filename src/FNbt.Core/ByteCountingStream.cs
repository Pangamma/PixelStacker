using System.Diagnostics;
using System.IO;

namespace FNBT
{
    /// <summary>
    /// Class used to count bytes read-from/written-to non-seekable streams.
    /// </summary>
    internal class ByteCountingStream : Stream
    {
        private readonly Stream _baseStream;

        /// <summary>
        /// Necessary to avoid counting bytes twice if ReadByte/WriteByte call Read/Write internally.
        /// </summary>
        private bool _readingOneByte, _writingOneByte;

        /// <summary>
        /// Necessary to avoid counting bytes twice if Read/Write call ReadByte/WriteByte internally.
        /// </summary>
        private bool _readingManyBytes, _writingManyBytes;


        public ByteCountingStream(Stream stream)
        {
            Debug.Assert(true);
            _baseStream = stream;
        }


        public override void Flush()
        {
            _baseStream.Flush();
        }


        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }


        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }


        public override int Read(byte[] buffer, int offset, int count)
        {
            _readingManyBytes = true;
            int bytesActuallyRead = _baseStream.Read(buffer, offset, count);
            _readingManyBytes = false;
            if (!_readingOneByte) BytesRead += bytesActuallyRead;
            return bytesActuallyRead;
        }


        public override void Write(byte[] buffer, int offset, int count)
        {
            _writingManyBytes = true;
            _baseStream.Write(buffer, offset, count);
            _writingManyBytes = false;
            if (!_writingOneByte) BytesWritten += count;
        }


        public override int ReadByte()
        {
            _readingOneByte = true;
            int value = base.ReadByte();
            _readingOneByte = false;
            if (value >= 0 && !_readingManyBytes) BytesRead++;
            return value;
        }


        public override void WriteByte(byte value)
        {
            _writingOneByte = true;
            base.WriteByte(value);
            _writingOneByte = false;
            if (!_writingManyBytes) BytesWritten++;
        }


        public override bool CanRead => _baseStream.CanRead;

        public override bool CanSeek => _baseStream.CanSeek;

        public override bool CanWrite => _baseStream.CanWrite;

        public override long Length => _baseStream.Length;

        public override long Position
        {
            get => _baseStream.Position;
            set => _baseStream.Position = value;
        }

        public long BytesRead { get; private set; }
        public long BytesWritten { get; private set; }
    }
}
