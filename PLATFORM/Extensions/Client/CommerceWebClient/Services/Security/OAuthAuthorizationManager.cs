using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using VirtoCommerce.Foundation.Security;
using VirtoCommerce.Foundation.Security.Swt;

namespace VirtoCommerce.Web.Client.Services.Security
{
    using System.Threading;

    public class OAuthAuthorizationManager : ServiceAuthorizationManager
    {
        /// <summary>
        /// Checks authorization for the given operation context based on default policy evaluation.
        /// </summary>
        /// <param name="operationContext">The <see cref="T:System.ServiceModel.OperationContext" /> for the current authorization request.</param>
        /// <returns>
        /// true if access is granted; otherwise, false. The default is true.
        /// </returns>
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            var retVal = base.CheckAccessCore(operationContext);

            SimpleWebToken token = null;

            if (retVal)
            {
                // Extract authorization data.
                var requestMessage = operationContext.RequestContext.RequestMessage;
                var httpDetails = requestMessage.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                var requestUri = WebOperationContext.Current != null && WebOperationContext.Current.IncomingRequest.UriTemplateMatch != null ? WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri : requestMessage.Headers.To;

                token = ReadAuthToken(httpDetails);
                retVal = token != null && IsValidToken(token, requestUri);
            }

            var securityContext = ServiceSecurityContext.Anonymous;
            ClaimsPrincipal principal = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            var identity = principal.Identity;

            if (retVal)
            {
                var claims = token.Claims.Select(keyValuePair => new Claim(keyValuePair.Key, keyValuePair.Value));
                identity = new ClaimsIdentity(claims, "OAUTH-SWT");
                principal = new ClaimsPrincipal(identity);
                Thread.CurrentPrincipal = principal;
            }
            securityContext.AuthorizationContext.Properties["Principal"] = principal;
            securityContext.AuthorizationContext.Properties["Identities"] = new List<IIdentity> { identity };
            operationContext.IncomingMessageProperties.Security.ServiceSecurityContext = securityContext;

            return retVal;
            //return true;
        }

        /// <summary>
        /// Reads the auth token.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">requestMessage</exception>
        private SimpleWebToken ReadAuthToken(HttpRequestMessageProperty requestMessage)
        {
            if (requestMessage == null)
            {
                throw new ArgumentNullException("requestMessage");
            }

            SimpleWebToken retVal = null;
            var header = requestMessage.Headers["Authorization"];

            if (header != null)
            {
                // The header should be in the form 'WRAP access_token="1234567890"'
                const string headerPrefix = "WRAP access_token=";
                if (header.StartsWith(headerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    var rawToken = header.Substring(headerPrefix.Length).Trim('"');
                    retVal = SimpleWebToken.Parse(rawToken);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Determines whether [is valid token] [the specified web token].
        /// </summary>
        /// <param name="webToken">The web token.</param>
        /// <param name="requestedUri">The requested URI.</param>
        /// <returns>
        ///   <c>true</c> if [is valid token] [the specified web token]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidToken(SimpleWebToken webToken, Uri requestedUri)
        {
            var tokenValidator = SecurityConfiguration.Instance.TokenValidator;

            //return webToken.Issuer == tokenValidator.TrustedIssuerUri
            //    && (webToken.Audience.IsBaseOf(requestedUri) || (IsLocal(webToken.Audience) && IsLocal(requestedUri)))
            //    && webToken.ExpiresOn > DateTime.UtcNow
            //    && webToken.IsValidSignature(tokenValidator.SignatureKey);

            var isTrustedUssuerUri = webToken.Issuer == tokenValidator.TrustedIssuerUri;
            var isAudience = webToken.Audience.IsBaseOf(requestedUri);
            var isLocal = (IsLocal(webToken.Audience) && IsLocal(requestedUri));
            
            if (!isLocal)
            {
                isLocal = IsLocalWithFQDNCheck(requestedUri);
            }

            var isExpires = webToken.ExpiresOn > DateTime.UtcNow;
            var isValidSignature = webToken.IsValidSignature(tokenValidator.SignatureKey);
            var result = isTrustedUssuerUri && (isAudience || isLocal) && isExpires && isValidSignature;

            return result;
        }

        /// <summary>
        /// Determines whether the specified URI is local.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        ///   <c>true</c> if the specified URI is local; otherwise, <c>false</c>.
        /// </returns>
        private bool IsLocal(Uri uri)
        {
            var isLocalMachine = uri.IsLoopback || string.Equals(uri.Host, Environment.MachineName, StringComparison.OrdinalIgnoreCase);
            return isLocalMachine;
        }


        /// <summary>
        /// Determines whether [is local with FQDN check] [the specified URI].
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        ///   <c>true</c> if [is local with FQDN check] [the specified URI]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsLocalWithFQDNCheck(Uri uri)
        {
            var domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            var hostName = Dns.GetHostName();
            var fqdn = "";
            if (!hostName.Contains(domainName))
            {
                fqdn = hostName + "." + domainName;
            }
            else
            {
                fqdn = hostName;
            }

            var isLocalMachine = string.Equals(uri.Host, fqdn, StringComparison.OrdinalIgnoreCase);
            return isLocalMachine;

        }
        
    }
}