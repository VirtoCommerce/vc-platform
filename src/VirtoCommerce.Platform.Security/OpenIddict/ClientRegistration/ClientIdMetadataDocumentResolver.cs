using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Extensions;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

/// <summary>
/// Fetches client ID metadata documents from public addresses only, so that a client id can't make the platform reach
/// its own network. Only valid documents are cached.
/// </summary>
public class ClientIdMetadataDocumentResolver : IClientIdMetadataDocumentResolver, IDisposable
{
    private const string _cacheKeyPrefix = "OAuthClientIdMetadataDocument:";

    private static readonly HashSet<HttpStatusCode> _transientStatusCodes = [HttpStatusCode.RequestTimeout, HttpStatusCode.TooManyRequests];

    private static readonly HashSet<HttpStatusCode> _redirectStatusCodes =
    [
        HttpStatusCode.MovedPermanently,
        HttpStatusCode.Found,
        HttpStatusCode.SeeOther,
        HttpStatusCode.TemporaryRedirect,
        HttpStatusCode.PermanentRedirect,
    ];

    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;
    private readonly IOptionsMonitor<AuthorizationOptions> _options;
    private readonly IClientMetadataValidator _validator;
    private readonly ILogger<ClientIdMetadataDocumentResolver> _logger;

    public ClientIdMetadataDocumentResolver(
        IPlatformMemoryCache memoryCache,
        IOptionsMonitor<AuthorizationOptions> options,
        IClientMetadataValidator validator,
        IHostEnvironment hostEnvironment,
        ILogger<ClientIdMetadataDocumentResolver> logger)
        : this(CreateHandler(() => hostEnvironment.IsDevelopment() && options.CurrentValue.ClientRegistration.ClientIdMetadataDocuments.AllowLoopbackInDevelopment),
            memoryCache, options, validator, logger)
    {
    }

    protected internal ClientIdMetadataDocumentResolver(
        HttpMessageHandler handler,
        IMemoryCache memoryCache,
        IOptionsMonitor<AuthorizationOptions> options,
        IClientMetadataValidator validator,
        ILogger<ClientIdMetadataDocumentResolver> logger)
    {
        _httpClient = new HttpClient(handler) { Timeout = Timeout.InfiniteTimeSpan };
        _memoryCache = memoryCache;
        _options = options;
        _validator = validator;
        _logger = logger;
    }

    protected OAuthClientIdMetadataDocumentOptions Options => _options.CurrentValue.ClientRegistration.ClientIdMetadataDocuments;

    public virtual bool IsClientIdUrl(string clientId)
    {
        return Options.Enabled && _validator.IsClientIdUrl(clientId);
    }

    public virtual async Task<ClientIdMetadataDocumentResult> ResolveAsync(string clientId, CancellationToken cancellationToken = default)
    {
        if (!IsClientIdUrl(clientId))
        {
            return ClientIdMetadataDocumentResult.Fail("The client id is not a client metadata document URL.");
        }

        var host = new Uri(clientId).Host;
        if (!IsAllowedHost(host, Options))
        {
            return ClientIdMetadataDocumentResult.Fail($"The host '{host}' is not allowed to serve client metadata documents.");
        }

        ClientIdMetadataDocumentResult failure = null;

        var cacheKey = _cacheKeyPrefix + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(clientId)));

        var document = await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async options =>
        {
            var (result, lifetime) = await FetchAsync(clientId, cancellationToken);
            if (result.Document == null)
            {
                failure = result;
                return null;
            }

            options.AbsoluteExpirationRelativeToNow = lifetime;
            return result;
        }, cacheNullValue: false);

        if (document != null)
        {
            return document;
        }

        _logger.LogWarning("The client metadata document {ClientId} can't be used: {Error}", clientId, failure.Error);
        return failure;
    }

    /// <summary>
    /// Connects only to public addresses. They are checked when the connection is made, so a host can't resolve to
    /// another address after the check.
    /// </summary>
    public static SocketsHttpHandler CreateHandler(Func<bool> allowLoopback = null)
    {
        return new SocketsHttpHandler
        {
            AllowAutoRedirect = false,
            UseProxy = false,
            UseCookies = false,
            AutomaticDecompression = DecompressionMethods.None,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            ConnectCallback = (context, cancellationToken) => ConnectToPublicAddressAsync(context, allowLoopback?.Invoke() == true, cancellationToken),
        };
    }

    protected virtual async Task<(ClientIdMetadataDocumentResult Result, TimeSpan Lifetime)> FetchAsync(string clientId, CancellationToken cancellationToken)
    {
        var options = Options;
        var uri = new Uri(clientId);

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(options.Timeout);

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token);

            return (await ReadDocumentAsync(clientId, response, options, timeout.Token), GetLifetime(response.Headers.CacheControl, options));
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return (ClientIdMetadataDocumentResult.Fail("The client metadata document can't be fetched in time.", isTransient: true), TimeSpan.Zero);
        }
        catch (HttpRequestException ex) when (FindInnerException<NonPublicAddressException>(ex) is { } nonPublic)
        {
            return (ClientIdMetadataDocumentResult.Fail(nonPublic.Message), TimeSpan.Zero);
        }
        catch (HttpRequestException ex)
        {
            return (ClientIdMetadataDocumentResult.Fail($"The client metadata document can't be fetched: {ex.Message}", isTransient: true), TimeSpan.Zero);
        }
        catch (IOException ex)
        {
            return (ClientIdMetadataDocumentResult.Fail($"The client metadata document can't be read: {ex.Message}", isTransient: true), TimeSpan.Zero);
        }
    }

    protected virtual async Task<ClientIdMetadataDocumentResult> ReadDocumentAsync(string clientId, HttpResponseMessage response,
        OAuthClientIdMetadataDocumentOptions options, CancellationToken cancellationToken)
    {
        // The document must be served at the client id itself
        if (_redirectStatusCodes.Contains(response.StatusCode))
        {
            return ClientIdMetadataDocumentResult.Fail("The client metadata document is redirected; redirects are not followed.");
        }

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return ClientIdMetadataDocumentResult.Fail($"The client metadata document can't be fetched: HTTP {(int)response.StatusCode}.",
                isTransient: _transientStatusCodes.Contains(response.StatusCode) || response.StatusCode >= HttpStatusCode.InternalServerError);
        }

        var json = await ReadContentAsync(response.Content, options.MaxDocumentSize, cancellationToken);
        if (json == null)
        {
            return ClientIdMetadataDocumentResult.Fail($"The client metadata document is larger than {options.MaxDocumentSize} bytes.");
        }

        return _validator.ParseDocument(clientId, json);
    }

    protected virtual bool IsAllowedHost(string host, OAuthClientIdMetadataDocumentOptions options)
    {
        if (options.AllowedHosts == null || options.AllowedHosts.Count == 0)
        {
            return true;
        }

        return options.AllowedHosts
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Any(x => x.StartsWith("*.", StringComparison.Ordinal)
                ? host.EndsWith(x[1..], StringComparison.OrdinalIgnoreCase) && host.Length > x.Length - 1
                : string.Equals(host, x, StringComparison.OrdinalIgnoreCase));
    }

    protected virtual TimeSpan GetLifetime(CacheControlHeaderValue cacheControl, OAuthClientIdMetadataDocumentOptions options)
    {
        var lifetime = GetRequestedLifetime(cacheControl, options);

        if (lifetime < options.MinCacheLifetime)
        {
            return options.MinCacheLifetime;
        }

        return lifetime > options.MaxCacheLifetime ? options.MaxCacheLifetime : lifetime;
    }

    private static TimeSpan GetRequestedLifetime(CacheControlHeaderValue cacheControl, OAuthClientIdMetadataDocumentOptions options)
    {
        if (cacheControl == null)
        {
            return options.MaxCacheLifetime;
        }

        if (cacheControl.NoStore || cacheControl.NoCache)
        {
            return options.MinCacheLifetime;
        }

        return cacheControl.MaxAge ?? options.MaxCacheLifetime;
    }

    // Null when the content is larger than the limit
    private static async Task<string> ReadContentAsync(HttpContent content, int maxSize, CancellationToken cancellationToken)
    {
        if (content.Headers.ContentLength > maxSize)
        {
            return null;
        }

        await using var stream = await content.ReadAsStreamAsync(cancellationToken);
        using var buffer = new MemoryStream();
        var chunk = new byte[4096];
        int read;

        while ((read = await stream.ReadAsync(chunk, cancellationToken)) > 0)
        {
            if (buffer.Length + read > maxSize)
            {
                return null;
            }

            buffer.Write(chunk, 0, read);
        }

        return Encoding.UTF8.GetString(buffer.GetBuffer(), 0, (int)buffer.Length).TrimStart('\uFEFF');
    }

    private static async ValueTask<Stream> ConnectToPublicAddressAsync(SocketsHttpConnectionContext context, bool allowLoopback, CancellationToken cancellationToken)
    {
        var host = context.DnsEndPoint.Host;
        var addresses = IPAddress.TryParse(host, out var literal)
            ? [literal]
            : await Dns.GetHostAddressesAsync(host, cancellationToken);

        // Every address must be public: a host that resolves to a private address too is refused as a whole
        if (addresses.Length == 0 || !addresses.All(x => x.IsPublic() || (allowLoopback && IPAddress.IsLoopback(x))))
        {
            throw new NonPublicAddressException($"The host '{host}' doesn't resolve to a public address.");
        }

        var socket = new Socket(SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
        try
        {
            await socket.ConnectAsync(addresses, context.DnsEndPoint.Port, cancellationToken);
            return new NetworkStream(socket, ownsSocket: true);
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }

    private static T FindInnerException<T>(Exception exception) where T : Exception
    {
        for (var current = exception; current != null; current = current.InnerException)
        {
            if (current is T result)
            {
                return result;
            }
        }

        return null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _httpClient.Dispose();
        }
    }

    protected sealed class NonPublicAddressException(string message) : IOException(message);
}
