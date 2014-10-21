using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;

namespace VirtoCommerce.Shipping
{
    public class WeightedShippingGateway : IShippingGateway
    {
        private decimal defaultWeghtUnitPriceDecimal = 0.49M;
        public const string UnitWeightPricePropertyName = "UnitWeightPrice";
        public ShippingRate GetRate(string shippingMethod, LineItem[] items, ref string message)
        {
            if (items == null || items.Length == 0)
            {
                message = "No line items provided";
                return null;
            }


            var repository = ServiceLocator.Current.GetInstance<IShippingRepository>();
            var options = repository.ShippingOptions.ExpandAll().ToArray();

            var method = (from o in options from m in o.ShippingMethods where m.ShippingMethodId.Equals(shippingMethod, StringComparison.OrdinalIgnoreCase) select m).SingleOrDefault();

            if (method == null)
            {
                message = "The shipping method could not be loaded.";
                return null;
            }

            var retVal = new ShippingRate(method.ShippingMethodId, method.Name, method.BasePrice, method.Currency);

            var prop = method.ShippingOption.ShippingGatewayPropertyValues.FirstOrDefault(
                x => x.Name == UnitWeightPricePropertyName);

            if (prop != null && !string.IsNullOrWhiteSpace(prop.ShortTextValue) && decimal.TryParse(prop.ShortTextValue, NumberStyles.Number, CultureInfo.InvariantCulture, out defaultWeghtUnitPriceDecimal))
            {
                //just ok
            }

            //Calculate price for item weight
            foreach (var item in items)
            {
                retVal.Price += item.Weight * defaultWeghtUnitPriceDecimal * item.Quantity;
            }

            return retVal;
        }
    }
}
