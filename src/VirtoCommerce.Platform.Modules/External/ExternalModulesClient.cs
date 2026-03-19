using System;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.External
{
    public class ExternalModulesClient : IExternalModulesClient
    {
        private readonly ExternalModuleCatalogOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalModulesClient(IOptions<ExternalModuleCatalogOptions> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public Stream OpenRead(Uri address)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, address)
            {
                Version = httpClient.DefaultRequestVersion,
                VersionPolicy = httpClient.DefaultVersionPolicy,
                Headers = { { HeaderNames.UserAgent, "Virto Commerce Manager" } },
            };

            if (!string.IsNullOrEmpty(_options.AuthorizationToken))
            {
                request.Headers.Add(HeaderNames.Accept, "application/octet-stream");
                request.Headers.Add(HeaderNames.Authorization, "Token " + _options.AuthorizationToken);
            }

            if (!string.IsNullOrEmpty(_options.AuthorizationSchema) &&
                !string.IsNullOrEmpty(_options.AuthorizationParameter))
            {
                request.Headers.Add(HeaderNames.Authorization, $"{_options.AuthorizationSchema} {_options.AuthorizationParameter}");
            }

            var response = httpClient.Send(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStream();
        }
    }
}
