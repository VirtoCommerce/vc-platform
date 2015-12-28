using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Settings;
using coreModel = VirtoCommerce.Domain.Payment.Model;
using webModel = VirtoCommerce.CartModule.Web.Model;

namespace VirtoCommerce.CartModule.Web.Converters
{
    public static class PaymentMethodConverter
    {
        public static webModel.PaymentMethod ToWebModel(this coreModel.PaymentMethod paymentMethod)
        {
            var retVal = new webModel.PaymentMethod();
            retVal.InjectFrom(paymentMethod);

            retVal.GatewayCode = paymentMethod.Code;
            retVal.Name = paymentMethod.Description;
            retVal.IconUrl = paymentMethod.LogoUrl;
            retVal.Type = paymentMethod.PaymentMethodType.ToString();
            retVal.Group = paymentMethod.PaymentMethodGroupType.ToString();
            retVal.Description = paymentMethod.Description;
            retVal.Priority = paymentMethod.Priority;
            retVal.IsAvailableForPartial = paymentMethod.IsAvailableForPartial;
            if(paymentMethod.Settings != null)
            {
                retVal.Settings = paymentMethod.Settings.Where(x => x.ValueType != SettingValueType.SecureString).ToList();
            }

            return retVal;
        }

    

    }
}
