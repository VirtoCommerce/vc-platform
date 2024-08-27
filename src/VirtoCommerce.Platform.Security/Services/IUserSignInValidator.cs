using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Services
{
    [Obsolete("Use VirtoCommerce.Platform.Security.OpenIddict.ITokenRequestValidator", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
    public interface IUserSignInValidator
    {
        public int Priority { get; set; }

        Task<IList<TokenLoginResponse>> ValidateUserAsync(SignInValidatorContext context);
    }
}
