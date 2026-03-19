using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace VirtoCommerce.Platform.Security.Model.OpenIddict;

public class VirtoOpenIddictEntityFrameworkCoreScope : OpenIddictEntityFrameworkCoreScope<string>
{
    public VirtoOpenIddictEntityFrameworkCoreScope()
    {
        Id = Guid.NewGuid().ToString();
    }
}
