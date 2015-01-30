using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreModel = VirtoCommerce.Foundation.Search.Facets;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class FacetConverters
    {
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
                IsApplied = appliedFilters.Any(x=>x.Equals(facet.Key, StringComparison.OrdinalIgnoreCase))
            };
            return retVal;
        }
    }
}
