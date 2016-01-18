using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;
using System.Collections.Generic;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class OperationConverter
    {
        public static Operation ToWebModel(this VirtoCommerceOrderModuleWebModelOperation operation, IEnumerable<Currency> availCurrencies, Language language)
        {
            var operationWebModel = new Operation();

            var currency = availCurrencies.FirstOrDefault(x => x.Equals(operation.Currency)) ?? new Currency(language, operation.Currency); ;

            operationWebModel.InjectFrom(operation);

            operationWebModel.Currency = currency;

            if (operation.ChildrenOperations != null)
            {
                operationWebModel.ChildrenOperations = operation.ChildrenOperations.Select(co => co.ToWebModel(availCurrencies, language)).ToList();
            }

            if (operation.DynamicProperties != null)
            {
                operationWebModel.DynamicProperties = operation.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            operationWebModel.Sum = new Money(operation.Sum ?? 0, currency);
            operationWebModel.Tax = new Money(operation.Tax ?? 0, currency);

            return operationWebModel;
        }
    }
}