using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Assets.AzureBlobStorage
{

    /// <summary>
    /// Wraps stream that can't flush multiple times (like BlockBlobWriteStream).
    /// Flushes only once at Dispose.
    /// </summary>
    public class FlushLessStream : Stream
    {
        private readonly Stream _underStream;
        public FlushLessStream(Stream stream)
        {
            _underStream = stream;
        }

        public override bool CanRead => _underStream.CanRead;

        public override bool CanSeek => _underStream.CanSeek;

        public override bool CanWrite => _underStream.CanWrite;

        public override long Length => _underStream.Length;

        public override long Position { get => _underStream.Position; set => _underStream.Position = value; }

        public override void Flush()
        {
            // The method especially left empty
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _underStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _underStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _underStream.SetLength(value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _underStream.Flush();
                _underStream.Dispose();
            }

            base.Dispose(disposing);
        }

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return base.WriteAsync(buffer, cancellationToken);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _underStream.Write(buffer, offset, count);
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            base.Write(buffer);
        }
    }
}
