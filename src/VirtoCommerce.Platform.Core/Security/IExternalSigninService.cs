using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface IExternalSigninService
    {
        public Task<string> ProcessCallbackAsync(string returnUrl, IUrlHelper urlHelper);
    }
}
