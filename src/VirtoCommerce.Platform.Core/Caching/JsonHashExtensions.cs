using System;
using System.Buffers;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Core.Caching;

/// <summary>
/// Stable content hash of an object graph, for use as a cache key.
/// </summary>
/// <remarks>
/// Prefer this over a key assembled from selected fields: a hand-written projection silently becomes
/// under-inclusive - and starts serving the wrong data - the day a field is added to the hashed type.
/// Hash a request or criteria object, not a materialized entity (serialization walks every readable property,
/// so a lazily-loaded one issues queries) and not a graph carrying secrets (the pooled buffers go back to
/// <see cref="ArrayPool{T}.Shared"/> uncleared, readable by whoever rents them next).
/// </remarks>
public static class JsonHashExtensions
{
    // 4096 chars keeps a typical (~2 KB) payload to a single drain, and both pooled buffers - this one and
    // the byte buffer derived from it (12291 requested, 16384 returned) - under the 85 KB large-object-heap
    // threshold.
    private const int CharBufferSize = 4096;

    private static readonly JsonSerializerSettings _cacheKeySerializerSettings = CreateCacheKeySettings();

    /// <summary>
    /// The serializer settings <see cref="GetJsonSha256Hex(object)"/> uses, as a fresh instance the caller
    /// may adjust - add a converter, say - before passing it to the overload that takes settings.
    /// </summary>
    /// <remarks>
    /// Two obligations come with them. Serialize the root as <c>typeof(object)</c>, the way
    /// <see cref="GetJsonSha256Hex(object, JsonSerializerSettings)"/> does - <c>Auto</c> discriminates against
    /// the DECLARED type, which a root does not have. And do not read a payload back with them: emitting type
    /// names is safe only because this JSON is exclusively hashed.
    /// </remarks>
    public static JsonSerializerSettings CreateCacheKeySettings()
    {
        return new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            // A cyclic graph should yield a truncated but stable key, not an exception, on a path whose only
            // job is to produce a string.
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            // Load-bearing: two sibling types that declare the same members serialize identically, so
            // without a discriminator their graphs hash alike and a cache keyed on the digest serves one
            // caller another's result.
            TypeNameHandling = TypeNameHandling.Auto,
        };
    }

    /// <summary>
    /// SHA-256 of the object's JSON form, lowercase hex, using settings vetted for cache keys.
    /// </summary>
    /// <param name="value">
    /// Graph to hash, and not one carrying secrets - see the remarks on this type for why. <c>null</c> is
    /// legal and hashes as the JSON literal <c>null</c>.
    /// </param>
    /// <returns>64 lowercase hex characters.</returns>
    public static string GetJsonSha256Hex(this object value)
    {
        return value.GetJsonSha256Hex(_cacheKeySerializerSettings);
    }

    /// <summary>
    /// SHA-256 of the object's JSON form, lowercase hex, using caller-supplied settings.
    /// </summary>
    /// <param name="value">
    /// Graph to hash. <c>null</c> is legal and hashes as the JSON literal <c>null</c>. A root that
    /// serializes to a bare JSON scalar carries no type discriminator - two enums sharing a numeric value,
    /// or an <c>int</c> and a <c>long</c> of equal value, hash alike.
    /// </param>
    /// <param name="settings">
    /// Must make the JSON DISCRIMINATING, not merely valid - see <see cref="CreateCacheKeySettings"/>.
    /// Settings without a type discriminator produce a plausible digest that collides across types.
    /// </param>
    /// <returns>64 lowercase hex characters.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
    /// <remarks>
    /// The digest is UTF8-based and does not depend on the host platform, whatever <see cref="Formatting"/>
    /// the settings ask for.
    /// </remarks>
    public static string GetJsonSha256Hex(this object value, JsonSerializerSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        using var incremental = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        using (var textWriter = new Utf8HashingTextWriter(incremental))
        {
            // CloseOutput=false: this writer owns its pooled buffers and returns them in its own Dispose.
            using var jsonWriter = new JsonTextWriter(textWriter) { CloseOutput = false };
            // typeof(object) is load-bearing, not a filler argument: TypeNameHandling.Auto emits $type only
            // where the runtime type differs from the DECLARED one, and the overload without a type leaves
            // the root with none to differ from - so nothing is written for the hashed object itself, the
            // very thing the key is taken over, and two sibling roots collide.
            JsonSerializer.Create(settings).Serialize(jsonWriter, value, typeof(object));
        }

        Span<byte> digest = stackalloc byte[SHA256.HashSizeInBytes];
        incremental.GetCurrentHash(digest);

        // Lowercase, not ToHexString: CacheKey.Normalize returns the same instance for an already-lowercase
        // key and allocates a lowered copy otherwise, and PlatformMemoryCache normalizes on every get, set
        // and remove - an uppercase digest would pay that copy for the lifetime of the key.
        return Convert.ToHexStringLower(digest);
    }

    /// <summary>
    /// <see cref="TextWriter"/> that UTF8-encodes into a pooled buffer and feeds an
    /// <see cref="IncrementalHash"/>, so no JSON string or byte array is ever materialized. Collapsing it
    /// back into serialize-then-hash produces the SAME digest, so no test fails - the only casualty is the
    /// allocation win, which <c>JsonHashBenchmarks</c> quantifies.
    /// </summary>
    private sealed class Utf8HashingTextWriter : TextWriter
    {
        private readonly IncrementalHash _hash;
        // A stateful Encoder, not Encoding.GetBytes: a surrogate pair can straddle two drains, and only the
        // encoder carries the high surrogate across the boundary instead of emitting a replacement char.
        private readonly Encoder _encoder = Encoding.UTF8.GetEncoder();
        // ArrayPool may return a LARGER array than requested, so the working capacity is this field rather
        // than _chars.Length - the byte buffer is sized from it, and the two must not drift apart.
        private readonly int _capacity = CharBufferSize;
        private char[] _chars = ArrayPool<char>.Shared.Rent(CharBufferSize);
        private byte[] _bytes = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(CharBufferSize));
        private int _pending;
        private bool _disposed;

        public Utf8HashingTextWriter(IncrementalHash hash)
        {
            _hash = hash;
            // Pinned, so a caller asking for indented formatting cannot make the digest depend on
            // Environment.NewLine: JsonTextWriter routes indentation through this property.
            NewLine = "\n";
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            if (_pending == _capacity)
            {
                Drain(flush: false);
            }

            _chars[_pending++] = value;
        }

        public override void Write(char[] buffer, int index, int count) => Write(buffer.AsSpan(index, count));

        public override void Write(string value) => Write(value.AsSpan());

        public override void Write(ReadOnlySpan<char> buffer)
        {
            while (!buffer.IsEmpty)
            {
                if (_pending == _capacity)
                {
                    Drain(flush: false);
                }

                var take = Math.Min(buffer.Length, _capacity - _pending);
                buffer[..take].CopyTo(_chars.AsSpan(_pending));
                _pending += take;
                buffer = buffer[take..];
            }
        }

        public override void Flush() => Drain(flush: false);

        private void Drain(bool flush)
        {
            if (_disposed || (_pending == 0 && !flush))
            {
                return;
            }

            var written = _encoder.GetBytes(_chars.AsSpan(0, _pending), _bytes, flush);
            if (written > 0)
            {
                _hash.AppendData(_bytes.AsSpan(0, written));
            }

            _pending = 0;
        }

        // Must be idempotent: returning the same rented arrays twice corrupts ArrayPool.Shared process-wide,
        // surfacing as unrelated code receiving an aliased buffer.
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                try
                {
                    // Drain BEFORE setting the flag - Drain() bails out on it, which would truncate the hash.
                    // flush: true so a trailing high surrogate is emitted rather than dropped.
                    Drain(flush: true);
                }
                finally
                {
                    _disposed = true;
                    ArrayPool<char>.Shared.Return(_chars);
                    ArrayPool<byte>.Shared.Return(_bytes);
                    _chars = null;
                    _bytes = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
