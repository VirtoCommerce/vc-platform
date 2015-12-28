using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Shipping.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using System;

namespace VirtoCommerce.OrderModule.Web.Converters
{
    public static class ShipmentMethodConverter
    {
        public static webModel.ShippingMethod ToWebModel(this coreModel.ShippingMethod shippingMethod)
        {
            var retVal = new webModel.ShippingMethod();
            retVal.InjectFrom(shippingMethod);
            return retVal;
        }

    }
}
