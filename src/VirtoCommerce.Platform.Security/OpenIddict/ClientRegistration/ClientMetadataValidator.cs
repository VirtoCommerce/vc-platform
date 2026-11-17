using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

public class ClientMetadataValidator : IClientMetadataValidator
{
    private const string HttpsScheme = "https://";

    private static readonly HashSet<string> _dotSegments = new(StringComparer.OrdinalIgnoreCase) { ".", "..", "%2e", "%2e%2e", ".%2e", "%2e." };

    private readonly IOAuthAppRules _appRules;

    public ClientMetadataValidator(IOAuthAppRules appRules)
    {
        _appRules = appRules;
    }

    /// <summary>
    /// HTTPS, with a path, and without a fragment, user info or dot segments.
    /// </summary>
    public virtual bool IsClientIdUrl(string clientId)
    {
        if (string.IsNullOrEmpty(clientId) || clientId.Length > ClientMetadata.MaxClientIdLength || !clientId.StartsWith(HttpsScheme, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (clientId.Contains('#') || clientId.Contains('\\'))
        {
            return false;
        }

        if (!Uri.TryCreate(clientId, UriKind.Absolute, out var uri) || !string.IsNullOrEmpty(uri.UserInfo) || uri.AbsolutePath.Length <= 1)
        {
            return false;
        }

        return !HasDotSegment(clientId);
    }

    /// <summary>
    /// A client that names no authentication method is taken as a public one.
    /// </summary>
    public virtual ClientMetadataResult Validate(JsonElement metadata)
    {
        if (metadata.ValueKind != JsonValueKind.Object)
        {
            return ClientMetadataResult.Fail(ClientMetadata.Errors.InvalidClientMetadata, "The client metadata must be a JSON object.");
        }

        var authMethod = GetString(metadata, ClientMetadata.Names.TokenEndpointAuthMethod);
        if (authMethod != null && authMethod != ClientAuthenticationMethods.None)
        {
            return ClientMetadataResult.Fail(ClientMetadata.Errors.InvalidClientMetadata,
                $"Only public clients may register themselves: the token_endpoint_auth_method must be '{ClientAuthenticationMethods.None}'.");
        }

        if (!IncludesWhenGiven(metadata, ClientMetadata.Names.GrantTypes, GrantTypes.AuthorizationCode))
        {
            return ClientMetadataResult.Fail(ClientMetadata.Errors.InvalidClientMetadata,
                $"The grant_types must be a list that includes '{GrantTypes.AuthorizationCode}'.");
        }

        if (!IncludesWhenGiven(metadata, ClientMetadata.Names.ResponseTypes, ResponseTypes.Code))
        {
            return ClientMetadataResult.Fail(ClientMetadata.Errors.InvalidClientMetadata,
                $"The response_types must be a list that includes '{ResponseTypes.Code}'.");
        }

        if (!TryGetStrings(metadata, ClientMetadata.Names.RedirectUris, out var redirectUris) || redirectUris == null || redirectUris.Count == 0)
        {
            return ClientMetadataResult.Fail(ClientMetadata.Errors.InvalidRedirectUri, "The redirect_uris are required.");
        }

        var redirectUriError = redirectUris.Select(_appRules.GetRedirectUriError).FirstOrDefault(x => x != null);
        if (redirectUriError != null)
        {
            return ClientMetadataResult.Fail(ClientMetadata.Errors.InvalidRedirectUri, redirectUriError);
        }

        return ClientMetadataResult.Success(new ClientMetadata
        {
            ClientName = GetClientName(metadata),
            RedirectUris = redirectUris.Distinct(StringComparer.Ordinal).ToList(),
        });
    }

    public virtual ClientIdMetadataDocumentResult ParseDocument(string clientId, string json)
    {
        JsonDocument jsonDocument;
        try
        {
            jsonDocument = JsonDocument.Parse(json);
        }
        catch (JsonException)
        {
            return ClientIdMetadataDocumentResult.Fail("The client metadata document is not valid JSON.");
        }

        using (jsonDocument)
        {
            var root = jsonDocument.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
            {
                return ClientIdMetadataDocumentResult.Fail("The client metadata document is not a JSON object.");
            }

            if (GetString(root, ClientMetadata.Names.ClientId) != clientId)
            {
                return ClientIdMetadataDocumentResult.Fail("The client_id of the client metadata document is not the URL of the document.");
            }

            // A document is public, so it can't hold a secret
            if (root.TryGetProperty(ClientMetadata.Names.ClientSecret, out _) || root.TryGetProperty(ClientMetadata.Names.ClientSecretExpiresAt, out _))
            {
                return ClientIdMetadataDocumentResult.Fail("The client metadata document can't contain a client secret.");
            }

            // Without a method, a client would authenticate with a secret (RFC 7591), which a document can't hold
            if (GetString(root, ClientMetadata.Names.TokenEndpointAuthMethod) == null)
            {
                return ClientIdMetadataDocumentResult.Fail(
                    $"The token_endpoint_auth_method of the client metadata document must be '{ClientAuthenticationMethods.None}'.");
            }

            var result = Validate(root);
            if (result.Metadata == null)
            {
                return ClientIdMetadataDocumentResult.Fail(result.ErrorDescription);
            }

            return ClientIdMetadataDocumentResult.Success(new ClientIdMetadataDocument
            {
                ClientId = clientId,
                ClientName = result.Metadata.ClientName,
                RedirectUris = result.Metadata.RedirectUris,
            });
        }
    }

    protected static string GetString(JsonElement metadata, string name)
    {
        return metadata.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String ? value.GetString() : null;
    }

    /// <summary>
    /// A missing property is valid and gives null values.
    /// </summary>
    protected static bool TryGetStrings(JsonElement metadata, string name, out IList<string> values)
    {
        values = null;

        if (!metadata.TryGetProperty(name, out var value))
        {
            return true;
        }

        if (value.ValueKind != JsonValueKind.Array || value.EnumerateArray().Any(x => x.ValueKind != JsonValueKind.String))
        {
            return false;
        }

        values = value.EnumerateArray().Select(x => x.GetString()).ToList();
        return true;
    }

    /// <summary>
    /// A missing list is valid; a given one must include the value.
    /// </summary>
    protected static bool IncludesWhenGiven(JsonElement metadata, string name, string value)
    {
        return TryGetStrings(metadata, name, out var values) && (values == null || values.Contains(value));
    }

    private static string GetClientName(JsonElement metadata)
    {
        var name = GetString(metadata, ClientMetadata.Names.ClientName);
        var clientName = name == null ? null : new string(name.Where(IsPrintable).ToArray()).Trim();

        if (string.IsNullOrEmpty(clientName))
        {
            return null;
        }

        if (clientName.Length <= ClientMetadata.MaxClientNameLength)
        {
            return clientName;
        }

        var length = char.IsHighSurrogate(clientName[ClientMetadata.MaxClientNameLength - 1])
            ? ClientMetadata.MaxClientNameLength - 1
            : ClientMetadata.MaxClientNameLength;

        return clientName[..length];
    }

    private static bool IsPrintable(char value)
    {
        return !char.IsControl(value) && CharUnicodeInfo.GetUnicodeCategory(value) != UnicodeCategory.Format;
    }

    // Uri removes dot segments, so they are looked for in the client id as it is
    private static bool HasDotSegment(string clientId)
    {
        var pathStart = clientId.IndexOf('/', HttpsScheme.Length);
        var queryStart = clientId.IndexOf('?');
        var path = queryStart > pathStart ? clientId[pathStart..queryStart] : clientId[pathStart..];

        return path.Split('/').Any(_dotSegments.Contains);
    }
}
