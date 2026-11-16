using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

public class DynamicClientRegistrationResult
{
    public string ClientId { get; init; }

    public DateTimeOffset IssuedAt { get; init; }

    public string ClientName { get; init; }

    public IList<string> RedirectUris { get; init; } = [];

    public string Error { get; init; }

    public string ErrorDescription { get; init; }
}
