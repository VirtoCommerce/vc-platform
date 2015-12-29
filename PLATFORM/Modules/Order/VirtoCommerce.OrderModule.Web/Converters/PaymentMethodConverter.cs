using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Payment.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using System;

namespace VirtoCommerce.OrderModule.Web.Converters
{
    public static class PaymentMethodConverter
    {
        public static webModel.PaymentMethod ToWebModel(this coreModel.PaymentMethod paymentMethod)
        {
            var retVal = new webModel.PaymentMethod();
            retVal.InjectFrom(paymentMethod);
            retVal.PaymentMethodGroupType = paymentMethod.PaymentMethodGroupType;
            retVal.PaymentMethodType = paymentMethod.PaymentMethodType;
            retVal.IconUrl = paymentMethod.LogoUrl;
            return retVal;
        }

    }
}
