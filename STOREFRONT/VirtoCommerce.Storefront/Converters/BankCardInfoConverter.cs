using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class BankCardInfoConverter
    {
        public static VirtoCommerceDomainPaymentModelBankCardInfo ToServiceModel(this BankCardInfo webModel)
        {
            var serviceModel = new VirtoCommerceDomainPaymentModelBankCardInfo();

            serviceModel.InjectFrom(webModel);

            return serviceModel;
        }
    }
}