using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Services
{
    public interface IUserSignInValidator
    {
        public int Priority { get; set; }

        Task<IList<TokenLoginResponse>> ValidateUserAsync(SignInValidatorContext context);
    }
}
