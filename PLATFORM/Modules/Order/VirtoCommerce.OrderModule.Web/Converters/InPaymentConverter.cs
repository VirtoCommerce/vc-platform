using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;

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

            return retVal;
        }

        public static coreModel.PaymentIn ToCoreModel(this webModel.PaymentIn payment)
        {
            var retVal = new coreModel.PaymentIn();
            retVal.InjectFrom(payment);
            retVal.PaymentStatus = EnumUtility.SafeParse<PaymentStatus>(payment.Status, PaymentStatus.Custom);


            retVal.Currency = payment.Currency;


            if (payment.DynamicProperties != null)
                retVal.DynamicProperties = payment.DynamicProperties;


            return retVal;
        }


    }
}
