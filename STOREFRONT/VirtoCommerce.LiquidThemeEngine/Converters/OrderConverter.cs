using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class OrderConverter
    {
        public static Order ToShopifyModel(this CustomerOrder order, IStorefrontUrlBuilder urlBuilder)
        {
            var result = new Order();
            result.InjectFrom<NullableAndEnumValueInjection>(order);

            result.Cancelled = order.IsCancelled.Value;
            result.CancelledAt = order.CancelledDate;
            result.CancelReasonLabel = order.CancelReason;
            result.CreatedAt = order.CreatedDate.Value;
            result.Name = order.Number;
            result.OrderNumber = order.Number;
            result.CustomerUrl = urlBuilder.ToAppAbsolute("/account/orders/" + order.Number);
            result.TotalPrice = order.Sum.Amount;

            if (order.Addresses != null)
            {
                result.BillingAddress = order.Addresses
                    .Where(a => (a.Type & AddressType.Billing) == AddressType.Billing)
                    .Select(a => a.ToShopifyModel())
                    .FirstOrDefault();

                result.ShippingAddress = order.Addresses
                    .Where(a => (a.Type & AddressType.Shipping) == AddressType.Shipping)
                    .Select(a => a.ToShopifyModel())
                    .FirstOrDefault();
            }

            return result;
        }
    }
}
