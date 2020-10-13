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
                if (!string.IsNullOrEmpty(_options.AuthorizationToken))
                {
                    webClient.Headers["User-Agent"] = "Virto Commerce Manager";
                    webClient.Headers["Accept"] = "application/octet-stream";
                    webClient.Headers["Authorization"] = "token " + _options.AuthorizationToken;
                }
                return webClient.OpenRead(address);
            }
        }
    }
}
