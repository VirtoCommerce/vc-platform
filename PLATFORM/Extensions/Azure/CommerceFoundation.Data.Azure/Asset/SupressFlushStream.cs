namespace VirtoCommerce.Foundation.Data.Azure.Asset
{
    using System;
    using System.IO;
    using System.Runtime.Remoting;

    sealed class SuppressFlushForStream : Stream
    {
        private readonly Stream _inner;

        public SuppressFlushForStream(Stream inner)
        {
            this._inner = inner;
        }

        public override bool CanRead
        {
            get { return this._inner.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this._inner.CanSeek; }
        }

        public override bool CanTimeout
        {
            get { return this._inner.CanTimeout; }
        }

        public override bool CanWrite
        {
            get { return this._inner.CanWrite; }
        }

        public override long Length
        {
            get { return this._inner.Length; }
        }

        public override long Position
        {
            get { return this._inner.Position; }
            set { this._inner.Position = value; }
        }

        public override int ReadTimeout
        {
            get { return this._inner.ReadTimeout; }
            set { this._inner.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { return this._inner.WriteTimeout; }
            set { this._inner.WriteTimeout = value; }
        }

        public new object GetLifetimeService()
        {
            return this._inner.GetLifetimeService();
        }

        public override object InitializeLifetimeService()
        {
            return this._inner.InitializeLifetimeService();
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return this._inner.CreateObjRef(requestedType);
        }

        public new void CopyTo(Stream destination)
        {
            this._inner.CopyTo(destination);
        }

        public new void CopyTo(Stream destination, int bufferSize)
        {
            this._inner.CopyTo(destination, bufferSize);
        }

        public override void Close()
        {
            this._inner.Close();
        }

        public new void Dispose()
        {
            this._inner.Dispose();
        }

        public override void Flush()
        {
            // yeah, that's just the hack
            //_inner.Flush();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this._inner.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return this._inner.EndRead(asyncResult);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this._inner.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            this._inner.EndWrite(asyncResult);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this._inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this._inner.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._inner.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            return this._inner.ReadByte();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this._inner.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            this._inner.WriteByte(value);
        }
    }
}
