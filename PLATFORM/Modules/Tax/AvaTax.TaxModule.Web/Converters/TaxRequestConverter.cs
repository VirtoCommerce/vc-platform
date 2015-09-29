using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AvaTaxCalcREST;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Domain.Tax.Model;
using domainModel = VirtoCommerce.Domain.Commerce.Model;

namespace AvaTax.TaxModule.Web.Converters
{
    public static class TaxDetailConverter
    {
        public static domainModel.TaxDetail ToDomainTaxDetail(this TaxDetail taxDetail)
        {
            return new domainModel.TaxDetail
            {
                Amount = taxDetail.Tax,
                Name = taxDetail.TaxName,
                Rate = taxDetail.Rate
            };
        }
    }
}