using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public interface IGrantTypeHandler
{
    string GrantType { get; }

    Task<ActionResult> HandleAsync(TokenRequestContext context);
}
