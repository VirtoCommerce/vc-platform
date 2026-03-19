using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Core.Security.ExternalSignIn;

public interface IExternalSignInUserBuilder
{
    public Task BuildNewUser(ApplicationUser user, ExternalLoginInfo externalLoginInfo);
}
