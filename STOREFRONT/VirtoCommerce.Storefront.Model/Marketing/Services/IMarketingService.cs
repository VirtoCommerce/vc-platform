using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Services
{
    public interface IMarketingService
    {
        Task<string> GetDynamicContentHtmlAsync(string storeId, string placeholderName);
    }
}