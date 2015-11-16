using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Builders
{
    public interface ICartBuilder
    {
        Task<CartBuilder> GetOrCreateNewTransientCartAsync(Store store, Customer customer, Currency currency);

        CartBuilder AddItem(LineItem lineItem);

        CartBuilder RemoveItem(string id);

        CartBuilder UpdateItem(string id, int quantity);

        CartBuilder MergeWith(ShoppingCart cart);

        Task SaveAsync();

        ShoppingCart Cart { get; }
    }
}