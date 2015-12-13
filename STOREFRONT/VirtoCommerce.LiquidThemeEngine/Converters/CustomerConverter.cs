using System.Globalization;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using storefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CustomerConverter
    {
        public static Customer ToShopifyModel(this storefrontModel.Customer customer, storefrontModel.WorkContext workContext, IStorefrontUrlBuilder urlBuilder)
        {
            var result = new Customer();
            result.InjectFrom<NullableAndEnumValueInjection>(customer);

            result.DefaultAddress = customer.DefaultAddress.ToShopifyModel();
            result.DefaultBillingAddress = customer.DefaultBillingAddress.ToShopifyModel();
            result.DefaultShippingAddress = customer.DefaultShippingAddress.ToShopifyModel();

            if (customer.Tags != null)
            {
                result.Tags = customer.Tags.ToList();
            }

            if (customer.Addresses != null)
            {
                var addresses = customer.Addresses.Select(a => a.ToShopifyModel()).ToList();

                // Add virtual ID to each address
                var id = 1;
                foreach (var address in addresses)
                {
                    address.Id = id.ToString(CultureInfo.InvariantCulture);
                    id++;
                }

                result.Addresses = new StorefrontPagedList<Address>(addresses, 1, 10, addresses.Count, page => workContext.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());
            }

            if (customer.Orders != null)
            {
                var orders = customer.Orders.Select(o => o.ToShopifyModel(urlBuilder)).ToList();
                result.Orders = new StorefrontPagedList<Order>(orders, 1, 10, customer.OrdersCount, page => workContext.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());
            }

            return result;
        }
    }
}
