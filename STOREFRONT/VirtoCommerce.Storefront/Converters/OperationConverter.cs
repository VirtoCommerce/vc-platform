using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;

namespace VirtoCommerce.Storefront.Converters
{
    public static class OperationConverter
    {
        public static Operation ToWebModel(this VirtoCommerceOrderModuleWebModelOperation operation)
        {
            var operationWebModel = new Operation();

            var currency = new Currency(EnumUtility.SafeParse(operation.Currency, CurrencyCodes.USD));

            operationWebModel.InjectFrom(operation);

            operationWebModel.Currency = currency;

            if (operation.ChildrenOperations != null)
            {
                operationWebModel.ChildrenOperations = operation.ChildrenOperations.Select(co => co.ToWebModel()).ToList();
            }

            if (operation.DynamicProperties != null)
            {
                operationWebModel.DynamicProperties = operation.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            operationWebModel.Sum = new Money(operation.Sum ?? 0, currency.Code);
            operationWebModel.Tax = new Money(operation.Tax ?? 0, currency.Code);

            return operationWebModel;
        }
    }
}