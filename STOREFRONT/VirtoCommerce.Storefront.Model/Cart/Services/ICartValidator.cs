using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Cart.Services
{
    public interface ICartValidator
    {
        Task ValidateItemsAsync(IEnumerable<string> productIds);

        Task ValidateShipmentsAsync(IEnumerable<string> shipmentIds);

        Task ValidateCartAsync();
    }
}