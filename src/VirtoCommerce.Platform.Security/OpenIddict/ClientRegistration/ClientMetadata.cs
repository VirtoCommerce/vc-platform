using System.Collections.Generic;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

/// <summary>
/// What the platform takes of the client metadata (RFC 7591) of a self-registered client; the rest is ignored.
/// </summary>
public class ClientMetadata
{
    public const int MaxClientNameLength = 100;

    public const int MaxClientIdLength = 100;

    public string ClientName { get; init; }

    public IList<string> RedirectUris { get; init; } = [];

    public static class Names
    {
        public const string ClientId = "client_id";
        public const string ClientIdIssuedAt = "client_id_issued_at";
        public const string ClientName = "client_name";
        public const string ClientSecret = "client_secret";
        public const string ClientSecretExpiresAt = "client_secret_expires_at";
        public const string GrantTypes = "grant_types";
        public const string RedirectUris = "redirect_uris";
        public const string ResponseTypes = "response_types";
        public const string TokenEndpointAuthMethod = "token_endpoint_auth_method";
    }

    public static class Errors
    {
        public const string InvalidClientMetadata = "invalid_client_metadata";
        public const string InvalidRedirectUri = "invalid_redirect_uri";
    }
}
