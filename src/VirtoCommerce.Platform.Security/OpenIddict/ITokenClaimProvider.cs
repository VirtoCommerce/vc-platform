using System.Security.Claims;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public interface ITokenClaimProvider
{
    Task SetClaimsAsync(ClaimsPrincipal principal, TokenRequestContext context);
}
