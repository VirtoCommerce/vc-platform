using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace VirtoCommerce.Platform.Security.Model.OpenIddict;

public class VirtoOpenIddictEntityFrameworkCoreToken : OpenIddictEntityFrameworkCoreToken<string, VirtoOpenIddictEntityFrameworkCoreApplication, VirtoOpenIddictEntityFrameworkCoreAuthorization>
{
    public VirtoOpenIddictEntityFrameworkCoreToken()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string IpAddress { get; set; }

    public string UserAgent { get; set; }
}
