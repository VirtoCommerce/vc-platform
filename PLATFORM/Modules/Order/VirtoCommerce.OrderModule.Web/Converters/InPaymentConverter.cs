using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using corePaymentModel = VirtoCommerce.Domain.Payment.Model;
using System.Collections.Generic;
using System;

namespace VirtoCommerce.OrderModule.Web.Converters
{
    public static class InPaymentConverter
    {
        public static webModel.PaymentIn ToWebModel(this coreModel.PaymentIn payment)
        {
            var retVal = new webModel.PaymentIn();
            retVal.InjectFrom(payment);
            retVal.Currency = payment.Currency;


            retVal.ChildrenOperations = payment.GetFlatObjectsListWithInterface<coreModel.IOperation>().Except(new[] { payment }).Select(x => x.ToWebModel()).ToList();

            if (payment.DynamicProperties != null)
                retVal.DynamicProperties = payment.DynamicProperties;

            retVal.PaymentMethod = new webModel.PaymentMethod();
            retVal.PaymentMethod.Code = payment.GatewayCode;
            retVal.PaymentMethod.Description = payment.GatewayCode;
            retVal.PaymentMethod.Name = payment.GatewayCode;
 
            if (payment.PaymentMethod != null)
            {
                retVal.PaymentMethod = payment.PaymentMethod.ToWebModel();
            }
            return retVal;
        }

        public static coreModel.PaymentIn ToCoreModel(this webModel.PaymentIn payment)
        {
            var retVal = new coreModel.PaymentIn();
            retVal.InjectFrom(payment);
            retVal.PaymentStatus = EnumUtility.SafeParse<PaymentStatus>(payment.Status, PaymentStatus.Custom);

            if(payment.PaymentMethod != null)
            {
                retVal.GatewayCode = payment.PaymentMethod.Code;
            }
            retVal.Currency = payment.Currency;


            if (payment.DynamicProperties != null)
                retVal.DynamicProperties = payment.DynamicProperties;


            return retVal;
        }


    }
}
