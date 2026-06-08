using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using VirtoCommerce.Platform.Core.ExportImport;
using SharpZipFile = ICSharpCode.SharpZipLib.Zip.ZipFile;

namespace VirtoCommerce.Platform.BackupRestore;

/// <summary>
/// SharpZipLib-backed implementation. Write mode uses <see cref="ZipOutputStream"/>
/// (sequential, one entry at a time). Read mode uses SharpZipLib's ZipFile
/// (random access by entry name) — aliased as <c>SharpZipFile</c> to avoid colliding
/// with the <c>VirtoCommerce.Platform.Data.ZipFile</c> namespace under which this
/// class lives.
/// </summary>
/// <remarks>
/// Write/Read are mutually exclusive — an instance is either a writer (created via
/// the writing constructor) or a reader (created via the reading constructor),
/// never both.
/// </remarks>
public class SharpZipBackupArchive : IZipBackupArchive
{
    // Write mode
    private readonly ZipOutputStream _zipOut;
    private readonly string _password;
    private EntryWriteStream _currentEntryStream;

    // Read mode
    private readonly SharpZipFile _zipFile;

    public SharpZipBackupArchive(Stream output, string password)
    {
        _password = string.IsNullOrEmpty(password) ? null : password;
        _zipOut = new ZipOutputStream(output)
        {
            IsStreamOwner = false,
        };
        _zipOut.SetLevel(9); // matches CompressionLevel.Optimal from the previous BCL usage
        if (_password != null)
        {
            _zipOut.Password = _password;
        }
    }

    public SharpZipBackupArchive(Stream input, string password, bool _reader)
    {
        _zipFile = new SharpZipFile(input)
        {
            IsStreamOwner = false,
        };
        if (!string.IsNullOrEmpty(password))
        {
            _zipFile.Password = password;
        }
    }

    public Task<Stream> CreateEntryAsync(string name, bool leaveUnencrypted = false)
    {
        if (_zipOut == null)
        {
            throw new InvalidOperationException("Archive was opened for reading; cannot create entries.");
        }
        if (_currentEntryStream is { IsClosed: false })
        {
            throw new InvalidOperationException(
                $"Entry '{_currentEntryStream.Name}' is still open. Dispose its stream before creating another entry.");
        }

        var entry = new ZipEntry(ZipEntry.CleanName(name))
        {
            DateTime = DateTime.UtcNow,
            // CompressionMethod.Deflated is the SharpZipLib default — entries get compressed.
        };

        if (leaveUnencrypted || _password == null)
        {
            // Temporarily clear the password so this entry is written without encryption.
            // (The manifest entry uses this path so the import side can read it before
            // prompting the user for the password.)
            _zipOut.Password = null;
        }
        else
        {
            entry.AESKeySize = 256;
            _zipOut.Password = _password;
        }

        _zipOut.PutNextEntry(entry);

        _currentEntryStream = new EntryWriteStream(_zipOut, name, () =>
        {
            _zipOut.CloseEntry();
            // Restore the password setting so subsequent entries are encrypted by default
            // (only relevant if the JUST-closed entry was the temporarily-unencrypted one).
            if (_password != null)
            {
                _zipOut.Password = _password;
            }
        });

        return Task.FromResult<Stream>(_currentEntryStream);
    }

    public Task<Stream> OpenEntryAsync(string name)
    {
        if (_zipFile == null)
        {
            throw new InvalidOperationException("Archive was opened for writing; cannot read entries.");
        }
        var entry = _zipFile.GetEntry(ZipEntry.CleanName(name));
        if (entry == null)
        {
            return Task.FromResult<Stream>(null);
        }
        // GetInputStream returns a fresh decompressing (and decrypting, if applicable) stream
        // over the underlying random-access ZipFile. Safe to return directly — caller is
        // responsible for disposing it.
        return Task.FromResult(_zipFile.GetInputStream(entry));
    }

    public void Dispose()
    {
        if (_currentEntryStream is { IsClosed: false })
        {
            // Best-effort: a writer leaked an entry stream. Close it so the archive
            // doesn't end up with a half-written entry that breaks the central directory.
            try
            { _currentEntryStream.Dispose(); }
            catch { /* swallow during dispose */ }
        }

        try
        { _zipOut?.Finish(); }
        catch { /* nothing to do during dispose */ }
        _zipOut?.Dispose();
        _zipFile?.Close();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Per-entry write stream. Delegates to the underlying <see cref="ZipOutputStream"/>;
    /// on dispose, calls <see cref="ZipOutputStream.CloseEntry"/> instead of closing the
    /// archive stream. The constructor's <c>onClose</c> callback fires after CloseEntry
    /// so the parent archive can restore encryption state.
    /// </summary>
    private sealed class EntryWriteStream : Stream
    {
        private readonly ZipOutputStream _underlying;
        private readonly Action _onClose;
        private bool _disposed;

        public string Name { get; }
        public bool IsClosed => _disposed;

        public EntryWriteStream(ZipOutputStream underlying, string name, Action onClose)
        {
            _underlying = underlying;
            Name = name;
            _onClose = onClose;
        }

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => !_disposed;
        public override long Length => throw new NotSupportedException();
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush() => _underlying.Flush();
        public override Task FlushAsync(CancellationToken cancellationToken) => _underlying.FlushAsync(cancellationToken);

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => _underlying.Write(buffer, offset, count);
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => _underlying.WriteAsync(buffer, offset, count, cancellationToken);
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
            => _underlying.WriteAsync(buffer, cancellationToken);

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            if (disposing)
            {
                _onClose?.Invoke();
            }
            base.Dispose(disposing);
        }

        public override ValueTask DisposeAsync()
        {
            Dispose(true);
            return ValueTask.CompletedTask;
        }
    }
}
