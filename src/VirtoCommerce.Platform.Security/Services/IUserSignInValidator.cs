using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Services
{
    public interface IUserSignInValidator
    {
        public int Priority { get; set; }

        Task<IList<TokenLoginResponse>> ValidateUserAsync(ApplicationUser user, IDictionary<string, object> context);
    }
}
