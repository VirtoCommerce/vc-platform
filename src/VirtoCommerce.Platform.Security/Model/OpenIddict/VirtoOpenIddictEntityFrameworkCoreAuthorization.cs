using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace VirtoCommerce.Platform.Security.Model.OpenIddict;

public class VirtoOpenIddictEntityFrameworkCoreAuthorization : OpenIddictEntityFrameworkCoreAuthorization<string, VirtoOpenIddictEntityFrameworkCoreApplication, VirtoOpenIddictEntityFrameworkCoreToken>
{
    public VirtoOpenIddictEntityFrameworkCoreAuthorization()
    {
        Id = Guid.NewGuid().ToString();
    }
}
