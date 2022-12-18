using System;
using System.IO;
using System.Net;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules.External
{
    public class ExternalModulesClient : IExternalModulesClient
    {
        private readonly ExternalModuleCatalogOptions _options;

        public ExternalModulesClient(IOptions<ExternalModuleCatalogOptions> options)
        {
            _options = options.Value;
        }

        public Stream OpenRead(Uri address)
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.UserAgent] = "Virto Commerce Manager";

                if (!string.IsNullOrEmpty(_options.AuthorizationToken))
                {
                    webClient.Headers[HttpRequestHeader.Accept] = "application/octet-stream";
                    webClient.Headers[HttpRequestHeader.Authorization] = "Token " + _options.AuthorizationToken;
                }

                if (!string.IsNullOrEmpty(_options.AuthorizationSchema) &&
                    !string.IsNullOrEmpty(_options.AuthorizationParameter))
                {
                    webClient.Headers[HttpRequestHeader.Authorization] = $"{_options.AuthorizationSchema} {_options.AuthorizationParameter}";
                }


                return webClient.OpenRead(address);
            }
        }
    }
}
