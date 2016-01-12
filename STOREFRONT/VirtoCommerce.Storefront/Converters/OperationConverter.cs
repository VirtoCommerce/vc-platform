using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Converters
{
    public static class OperationConverter
    {
        public static Operation ToWebModel(this VirtoCommerceOrderModuleWebModelOperation operation, IEnumerable<Currency> availCurrencies)
        {
            var operationWebModel = new Operation();

            var currency = availCurrencies.FirstOrDefault(x => x.IsHasSameCode(operation.Currency)) ?? new Currency(operation.Currency, 1); ;

            operationWebModel.InjectFrom(operation);

            operationWebModel.Currency = currency;

            if (operation.ChildrenOperations != null)
            {
                operationWebModel.ChildrenOperations = operation.ChildrenOperations.Select(co => co.ToWebModel(availCurrencies)).ToList();
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