using System.Globalization;
using System.Linq;
using Omu.ValueInjecter;
using PagedList;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CustomerConverter
    {
        public static Customer ToShopifyModel(this CustomerInfo customer, StorefrontModel.WorkContext workContext, IStorefrontUrlBuilder urlBuilder)
        {
            var result = new Customer();
            result.InjectFrom<NullableAndEnumValueInjecter>(customer);
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
                result.Addresses = new MutablePagedList<Address>(addresses);
            }

            if (customer.Orders != null)
            {
                result.Orders = new MutablePagedList<Order>((pageNumber, pageSize) =>
                {
                    customer.Orders.Slice(pageNumber, pageSize);
                    return new StaticPagedList<Order>(customer.Orders.Select(x => x.ToShopifyModel(urlBuilder)), customer.Orders);
                }, customer.Orders.PageNumber, customer.Orders.PageSize);
            }

            if (customer.QuoteRequests != null)
            {
                result.QuoteRequests = new MutablePagedList<QuoteRequest>((pageNumber, pageSize) =>
                {
                    customer.QuoteRequests.Slice(pageNumber, pageSize);
                    return new StaticPagedList<QuoteRequest>(customer.QuoteRequests.Select(x => x.ToShopifyModel()), customer.QuoteRequests);
                }, customer.QuoteRequests.PageNumber, customer.QuoteRequests.PageSize);
            }

            return result;
        }
    }
}
