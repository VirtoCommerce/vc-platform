using System.Collections.Generic;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

public class ClientIdMetadataDocument
{
    public string ClientId { get; set; }

    public string ClientName { get; set; }

    public IList<string> RedirectUris { get; set; } = [];
}
