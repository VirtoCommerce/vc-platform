using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Security.OpenIddict
{
    public interface ITokenRequestValidator
    {
        public int Priority { get; set; }

        Task<IList<TokenResponse>> ValidateAsync(TokenRequestContext context);
    }
}
