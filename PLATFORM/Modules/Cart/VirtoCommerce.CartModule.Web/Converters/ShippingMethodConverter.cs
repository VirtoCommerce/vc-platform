using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Settings;
using coreModel = VirtoCommerce.Domain.Shipping.Model;
using webModel = VirtoCommerce.CartModule.Web.Model;

namespace VirtoCommerce.CartModule.Web.Converters
{
    public static class ShippingMethodConverter
    {
        public static webModel.ShippingMethod ToWebModel(this coreModel.ShippingRate shippingRate)
        {
            var retVal = new webModel.ShippingMethod();
            retVal.InjectFrom(shippingRate.ShippingMethod);
            retVal.InjectFrom(shippingRate);

            retVal.Currency = shippingRate.Currency;
            retVal.Name = shippingRate.ShippingMethod.Description;
            retVal.Price = shippingRate.Rate;
            retVal.ShipmentMethodCode = shippingRate.ShippingMethod.Code;
            
            if (shippingRate.ShippingMethod.Settings != null)
            {
                retVal.Settings = shippingRate.ShippingMethod.Settings.Where(x => x.ValueType != SettingValueType.SecureString).ToList();
            }

            return retVal;
        }

    

    }
}
