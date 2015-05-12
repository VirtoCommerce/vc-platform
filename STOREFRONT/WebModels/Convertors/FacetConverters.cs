#region

using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Web.Models.Searching;
using Data = VirtoCommerce.ApiClient.DataContracts.Search;

#endregion

namespace VirtoCommerce.Web.Models.Convertors
{
    public static class FacetConverters
    {
        #region Public Methods and Operators
        public static Tag AsWebModel(this FacetFilterValue facetValue, string field)
        {
            return new Tag { Field = field, Label = facetValue.Label, Count = facetValue.Count, Value = facetValue.Value };
        }

        public static FacetFilter AsWebModel(this Data.Facet facet)
        {
            var ret = new FacetFilter();
            ret.InjectFrom(facet);
            if(facet.Values.Any())
                ret.Values = facet.Values.Select(x => x.AsWebModel()).ToArray();

            return ret;
        }

        public static FacetFilterValue AsWebModel(this Data.FacetValue val)
        {
            var ret = new FacetFilterValue();
            ret.InjectFrom(val);
            return ret;
        }
        #endregion
    }
}