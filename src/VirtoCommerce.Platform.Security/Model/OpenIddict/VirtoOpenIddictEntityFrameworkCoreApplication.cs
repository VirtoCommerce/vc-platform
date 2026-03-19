using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace VirtoCommerce.Platform.Security.Model.OpenIddict;

public class VirtoOpenIddictEntityFrameworkCoreApplication : OpenIddictEntityFrameworkCoreApplication<string, VirtoOpenIddictEntityFrameworkCoreAuthorization, VirtoOpenIddictEntityFrameworkCoreToken>
{
    public VirtoOpenIddictEntityFrameworkCoreApplication()
    {
        Id = Guid.NewGuid().ToString();
    }
}
