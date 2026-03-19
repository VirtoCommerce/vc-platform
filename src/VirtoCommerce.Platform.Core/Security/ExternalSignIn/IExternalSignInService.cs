using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security.ExternalSignIn;

public interface IExternalSignInService
{
    public Task<ExternalSignInResult> SignInAsync();
}
