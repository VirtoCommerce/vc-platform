using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Services
{
    public interface IUserSignInValidator
    {
        public int Priority { get; set; }

        Task<IList<TokenLoginResponse>> ValidateUserAsync(ApplicationUser user, SignInResult signInResult, IDictionary<string, object> context);
    }
}
