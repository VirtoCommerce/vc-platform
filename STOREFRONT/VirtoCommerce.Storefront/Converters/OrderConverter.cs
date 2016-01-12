using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Converters
{
    public static class OrderConverter
    {
        public static CustomerOrder ToWebModel(this VirtoCommerceOrderModuleWebModelCustomerOrder order, IEnumerable<Currency> availCurrencies)
        {
            var webModel = new CustomerOrder();

            var currency = availCurrencies.FirstOrDefault(x=> x.IsHasSameCode(order.Currency)) ?? new Currency(order.Currency, 1);

            webModel.InjectFrom(order);

            if (order.Addresses != null)
            {
                webModel.Addresses = order.Addresses.Select(a => a.ToWebModel()).ToList();
            }

            if (order.ChildrenOperations != null)
            {
                webModel.ChildrenOperations = order.ChildrenOperations.Select(co => co.ToWebModel(availCurrencies)).ToList();
            }

            webModel.Currency = currency;

            webModel.DiscountAmount = new Money(order.DiscountAmount ?? 0, currency);

            if (order.DynamicProperties != null)
            {
                webModel.DynamicProperties = order.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            if (order.InPayments != null)
            {
                webModel.InPayments = order.InPayments.Select(p => p.ToWebModel(availCurrencies)).ToList();
            }

            if (order.Items != null)
            {
                webModel.Items = order.Items.Select(i => i.ToWebModel(availCurrencies)).ToList();
            }

            if (order.Shipments != null)
            {
                webModel.Shipments = order.Shipments.Select(s => s.ToWebModel(availCurrencies)).ToList();
            }

            webModel.Sum = new Money(order.Sum ?? 0, currency);
            webModel.Tax = new Money(order.Tax ?? 0, currency);

            if (order.TaxDetails != null)
            {
                webModel.TaxDetails = order.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            return webModel;
        }
    }
}