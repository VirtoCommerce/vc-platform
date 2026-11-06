using System.Collections.Generic;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.OpenIddict;

/// <summary>
/// What the platform checks and derives when an OAuth application is saved. Replace the registration, or derive
/// from <see cref="OAuthAppRules"/>, to change the defaults or the validation rules.
/// </summary>
public interface IOAuthAppRules
{
    /// <summary>
    /// Adds the permissions of a new application with the platform defaults.
    /// </summary>
    void AddDefaultPermissions(OpenIddictApplicationDescriptor descriptor);

    /// <summary>
    /// Brings an application to be saved to its canonical form: the client, application and consent types in lower
    /// case, the client type of a new application inferred when it is missing (the stored one for an existing
    /// application), and the endpoint and response type permissions its grant types need. A client that names no
    /// grant type gets the platform defaults. <paramref name="stored"/> is the saved state of an existing
    /// application, or null for a new one.
    /// </summary>
    void Normalize(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationDescriptor stored);

    /// <summary>
    /// The reasons the application can't be saved. <paramref name="stored"/> is the saved state of an existing
    /// application, or null for a new one.
    /// </summary>
    IList<string> Validate(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationDescriptor stored);

    /// <summary>
    /// Why a redirect URI can't be used, or null.
    /// </summary>
    string GetRedirectUriError(string uri);
}
