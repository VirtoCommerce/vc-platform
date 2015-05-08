    #region
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Web.Models.Convertors;
using VirtoCommerce.Web.Models.Services;

#endregion

namespace VirtoCommerce.Web.Models.Helpers
{
    public class CartHelper
    {
        private readonly CommerceService _commerceService;

        public CartHelper(CommerceService service)
        {
            _commerceService = service;
        }

        #region Properties
        private Cart ShoppingCart
        {
            get
            {
                return SiteContext.Current.Cart;
            }
        }
        #endregion

        #region Public Methods and Operators
        public async Task<LineItem> AddAsync(string variantId)
        {
            var product = await _commerceService.GetProductAsync(SiteContext.Current, variantId);

            var lineItem = product.AsLineItem();

            var cart = this.ShoppingCart;
            cart.Items.Add(lineItem);

            await this._commerceService.SaveChangesAsync(cart);

            return lineItem;
        }

        public async Task<Cart> ChangeAsync(string id, int quantity)
        {
            var cart = this.ShoppingCart;

            if (cart.Items.Any())
            {
                var item = cart.Items.SingleOrDefault(i => i.Id.Equals(id));
                if (item != null)
                {
                    if (quantity == 0) // remove item
                    {
                        cart.Items.Remove(item);
                    }
                    else
                    {
                        item.Quantity = quantity;
                    }
                }
            }

            cart = await this._commerceService.SaveChangesAsync(cart);

            return cart;
        }

        public async Task<Cart> ChangeAsync(int index, int quantity)
        {
            var cart = this.ShoppingCart;

            if (cart.Items.Any() && cart.Items.Count > index)
            {
                if (quantity == 0) // remove item
                {
                    cart.Items.RemoveAt(index);
                }
                else
                {
                    cart.Items[index].Quantity = quantity;
                }
            }

            cart = await this._commerceService.SaveChangesAsync(cart);

            return cart;
        }

        public async Task<Cart> ClearAsync()
        {
            var cart = this.ShoppingCart;

            if (cart.Items.Any())
            {
                cart.Items.RemoveAll(x => x != null);
            }

            cart = await this._commerceService.SaveChangesAsync(cart);

            return cart;
        }

        public async Task<Cart> UpdateAsync(int[] updates, string note, string action)
        {
            var cart = this.ShoppingCart;
            cart.Note = note;

            if (cart.Items.Any() && updates != null)
            {
                var index = 0;
                foreach (var update in updates)
                {
                    if (cart.Items.Count <= index)
                    {
                        break;
                    }

                    if (update == 0)
                    {
                        cart.Items.RemoveAt(index);
                    }
                    else
                    {
                        cart.Items[index].Quantity = update;
                    }

                    index++;
                }
            }

            cart = await this._commerceService.SaveChangesAsync(cart);

            return cart;
        }
        #endregion
    }
}