using System;
using System.Linq;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using searchModel = VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.SearchModule.Web.Converters
{
    public static class FacetConverters
    {
        public static moduleModel.Facet ToModuleModel(this searchModel.FacetGroup facetGroup, params string[] appliedFilters)
        {
            var result = new moduleModel.Facet
            {
                FacetType = facetGroup.FacetType,
                Field = facetGroup.FieldName,
                Values = facetGroup.Facets.Select(f => f.ToModuleModel(appliedFilters)).ToArray()
            };
            return result;
        }

        public static moduleModel.FacetValue ToModuleModel(this searchModel.Facet facet, params string[] appliedFilters)
        {
            var result = new moduleModel.FacetValue
            {
                Label = facet.Name,
                Value = facet.Key,
                Count = facet.Count,
                IsApplied = appliedFilters.Any(x => x.Equals(facet.Key, StringComparison.OrdinalIgnoreCase))
            };
            return result;
        }
    }
}
