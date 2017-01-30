using System;
using System.Configuration;
using System.Net;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Modularity
{
    public static class WebClientExtensions
    {
        /// <summary>
        /// Adds authorization token to download modules from private GitHub repositories.
        /// Applies to https://api.github.com and https://raw.githubusercontent.com
        /// </summary>
        /// <param name="webClient"></param>
        /// <param name="address"></param>
        public static void AddAuthorizationTokenForGitHub(this WebClient webClient, string address)
        {
            Uri url;
            if (Uri.TryCreate(address, UriKind.Absolute, out url))
            {
                AddAuthorizationTokenForGitHub(webClient, url);
            }
        }

        /// <summary>
        /// Adds authorization token to download modules from private GitHub repositories.
        /// Applies to https://api.github.com and https://raw.githubusercontent.com
        /// </summary>
        /// <param name="webClient"></param>
        /// <param name="address"></param>
        public static void AddAuthorizationTokenForGitHub(this WebClient webClient, Uri address)
        {
            if (address.Scheme == Uri.UriSchemeHttps && (address.Host == "api.github.com" || address.Host == "raw.githubusercontent.com"))
            {
                var gitHubToken = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Modules:GitHubAuthorizationToken", string.Empty);
                if (!string.IsNullOrEmpty(gitHubToken))
                {
                    webClient.Headers["User-Agent"] = "Virto Commerce Manager";
                    webClient.Headers["Accept"] = "application/octet-stream";
                    webClient.Headers["Authorization"] = "token " + gitHubToken;
                }
            }
        }
    }
}
