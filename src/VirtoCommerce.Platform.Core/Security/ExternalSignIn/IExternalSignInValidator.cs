using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security.ExternalSignIn;

public interface IExternalSignInValidator
{
    Task<bool> ValidateAsync(ExternalSignInRequest request);
}
