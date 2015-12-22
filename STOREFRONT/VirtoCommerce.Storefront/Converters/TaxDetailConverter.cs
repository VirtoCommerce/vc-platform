using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class TaxDetailConverter
    {
        public static TaxDetail ToWebModel(this VirtoCommerceDomainCommerceModelTaxDetail taxDetail)
        {
            var taxDetailWebModel = new TaxDetail();

            taxDetailWebModel.InjectFrom(taxDetail);

            return taxDetailWebModel;
        }

        public static VirtoCommerceDomainCommerceModelTaxDetail ToServiceModel(this TaxDetail taxDetail)
        {
            var taxDetailServiceModel = new VirtoCommerceDomainCommerceModelTaxDetail();

            taxDetailServiceModel.InjectFrom(taxDetail);

            return taxDetailServiceModel;
        }
    }
}