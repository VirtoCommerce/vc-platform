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
        public static webModel.Facet ToWebModel(this coreModel.FacetGroup facetGroup)
        {
            var retVal = new webModel.Facet
            {
                FacetType = "",
                Field = facetGroup.FieldName,
                Values = facetGroup.Facets.Select(f => f.ToWebModel()).ToArray()
            };
            return retVal;
        }

        public static webModel.FacetValue ToWebModel(this coreModel.Facet facet)
        {
            var retVal = new webModel.FacetValue {Label = facet.Name, Value = facet.Key, Count = facet.Count};
            return retVal;
        }
    }
}
