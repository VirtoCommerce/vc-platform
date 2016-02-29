using System.Globalization;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Customer;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CustomerConverter
    {
        public static Customer ToShopifyModel(this CustomerInfo customer, StorefrontModel.WorkContext workContext, StorefrontModel.Common.IStorefrontUrlBuilder urlBuilder)
        {
            var result = new Customer();
            result.InjectFrom<StorefrontModel.Common.NullableAndEnumValueInjecter>(customer);
            result.Name = customer.FullName;
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
                //TODO: make customer.Addresses as IPagedList
                result.Addresses = new StorefrontModel.Common.StorefrontPagedList<Address>(addresses, 1, 10, addresses.Count, page => workContext.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());
                result.AddressesCount = addresses.Count;
            }

            if (customer.Orders != null)
            {
                var orders = customer.Orders.Select(o => o.ToShopifyModel(urlBuilder)).ToList();
                result.Orders = new StorefrontModel.Common.StorefrontPagedList<Order>(orders, customer.Orders, customer.Orders.GetPageUrl);
                result.OrdersCount = orders.Count;
            }

            if (customer.QuoteRequests != null)
            {
                var quoteRequests = customer.QuoteRequests.Select(qr => qr.ToShopifyModel()).ToList();
                result.QuoteRequests = new StorefrontModel.Common.StorefrontPagedList<QuoteRequest>(quoteRequests, customer.QuoteRequests, customer.QuoteRequests.GetPageUrl);
            }

            return result;
        }
    }
}
