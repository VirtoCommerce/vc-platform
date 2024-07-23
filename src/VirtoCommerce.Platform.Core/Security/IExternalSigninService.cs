using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface IExternalSigninService
    {
        [Obsolete("Not being called. Use IExternalSignInService.SignInAsync()", DiagnosticId = "VC0009", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        public Task<string> ProcessCallbackAsync(string returnUrl, IUrlHelper urlHelper);
    }
}
