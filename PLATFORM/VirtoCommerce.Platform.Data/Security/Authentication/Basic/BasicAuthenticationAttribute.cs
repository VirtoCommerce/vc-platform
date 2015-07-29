using System;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace VirtoCommerce.Platform.Data.Security.Authentication.Basic
{
    public abstract class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public const string AuthenticationType = "Basic";

        public string Realm { get; set; }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            if (authorization.Scheme != AuthenticationType)
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);

            if (userNameAndPasword == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
                return;
            }

            string userName = userNameAndPasword.Item1;
            string password = userNameAndPasword.Item2;

            IPrincipal principal = await AuthenticateAsync(userName, password, context, cancellationToken);

            if (principal == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
            }
            else
            {
                // Authentication was attempted and succeeded. Set Principal to the authenticated user.
                context.Principal = principal;
            }
        }

        protected abstract Task<IPrincipal> AuthenticateAsync(string userName, string password, HttpAuthenticationContext context, CancellationToken cancellationToken);

        private static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (String.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            int colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }

            var userName = decodedCredentials.Substring(0, colonIndex);
            var password = decodedCredentials.Substring(colonIndex + 1);
            return new Tuple<string, string>(userName, password);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter;

            if (String.IsNullOrEmpty(Realm))
            {
                parameter = null;
            }
            else
            {
                // A correct implementation should verify that Realm does not contain a quote character unless properly
                // escaped (preceded by a backslash that is not itself escaped).
                parameter = "realm=\"" + Realm + "\"";
            }

            context.ChallengeWith(AuthenticationType, parameter);
        }

        public virtual bool AllowMultiple
        {
            get { return false; }
        }
    }
}
