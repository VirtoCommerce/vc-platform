using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Builders
{
    public interface ICartBuilder
    {
        Task<CartBuilder> GetOrCreateNewTransientCartAsync(Store store, Customer customer, Currency currency);

        CartBuilder AddItem(Product product, int quantity);

        CartBuilder UpdateItem(string id, int quantity);

        CartBuilder RemoveItem(string id);

        CartBuilder AddAddress(Address address);

        Task SaveAsync();

        ShoppingCart Cart { get; }
    }
}