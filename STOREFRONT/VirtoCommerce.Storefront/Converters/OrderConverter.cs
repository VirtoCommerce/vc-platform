using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;
using System.Collections.Generic;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class OrderConverter
    {
        public static CustomerOrder ToWebModel(this VirtoCommerceOrderModuleWebModelCustomerOrder order, IEnumerable<Currency> availCurrencies, Language language)
        {
            var webModel = new CustomerOrder();

            var currency = availCurrencies.FirstOrDefault(x=> x.Equals(order.Currency)) ?? new Currency(language, order.Currency);

            webModel.InjectFrom(order);

            if (order.Addresses != null)
            {
                webModel.Addresses = order.Addresses.Select(a => a.ToWebModel()).ToList();
            }

            if (order.ChildrenOperations != null)
            {
                webModel.ChildrenOperations = order.ChildrenOperations.Select(co => co.ToWebModel(availCurrencies, language)).ToList();
            }

            webModel.Currency = currency;

            webModel.DiscountAmount = new Money(order.DiscountAmount ?? 0, currency);

            if (order.DynamicProperties != null)
            {
                webModel.DynamicProperties = order.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            if (order.InPayments != null)
            {
                webModel.InPayments = order.InPayments.Select(p => p.ToWebModel(availCurrencies, language)).ToList();
            }

            if (order.Items != null)
            {
                webModel.Items = order.Items.Select(i => i.ToWebModel(availCurrencies, language)).ToList();
            }

            if (order.Shipments != null)
            {
                webModel.Shipments = order.Shipments.Select(s => s.ToWebModel(availCurrencies, language)).ToList();
            }

            webModel.Sum = new Money(order.Sum ?? 0, currency);
            webModel.Tax = new Money(order.Tax ?? 0, currency);

            if (order.TaxDetails != null)
            {
                webModel.TaxDetails = order.TaxDetails.Select(td => td.ToWebModel(currency)).ToList();
            }

            return webModel;
        }
    }
}