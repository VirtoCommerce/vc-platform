using System;
using System.Linq;
using coreModel = VirtoCommerce.Domain.Search;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class FacetConverters
    {
        #region Public Methods and Operators

        public static webModel.Facet ToWebModel(this coreModel.FacetGroup facetGroup, params string[] appliedFilters)
        {
            var retVal = new webModel.Facet
                         {
                             FacetType = facetGroup.FacetType,
                             Field = facetGroup.FieldName,
                             Values = facetGroup.Facets.Select(f => f.ToWebModel(appliedFilters)).ToArray()
                         };
            return retVal;
        }

        public static webModel.FacetValue ToWebModel(this coreModel.Facet facet, params string[] appliedFilters)
        {
            var retVal = new webModel.FacetValue
                         {
                             Label = facet.Name,
                             Value = facet.Key,
                             Count = facet.Count,
                             IsApplied =
                                 appliedFilters.Any(x => x.Equals(facet.Key, StringComparison.OrdinalIgnoreCase))
                         };
            return retVal;
        }

        #endregion
    }
}
