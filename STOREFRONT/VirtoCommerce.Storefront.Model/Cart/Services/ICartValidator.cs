using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Cart.Services
{
    public interface ICartValidator
    {
        Task ValidateAsync(ShoppingCart cart);
    }
}