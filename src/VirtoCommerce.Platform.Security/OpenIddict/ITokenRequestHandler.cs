using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public interface ITokenRequestHandler
{
    Task HandleAsync(ApplicationUser user, TokenRequestContext context);
}
