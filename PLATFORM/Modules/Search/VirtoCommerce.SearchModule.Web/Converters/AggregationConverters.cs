using System;
using System.Linq;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using searchModel = VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.SearchModule.Web.Converters
{
    public static class AggregationConverters
    {
        public static coreModel.Aggregation ToModuleModel(this searchModel.FacetGroup facetGroup, params string[] appliedFilters)
        {
            var result = new coreModel.Aggregation
            {
                AggregationType = facetGroup.FacetType,
                Field = facetGroup.FieldName,
                Label = facetGroup.FieldDisplayName,
                Items = facetGroup.Facets.Select(f => f.ToModuleModel(appliedFilters)).ToArray()
            };
            return result;
        }

        public static coreModel.AggregationItem ToModuleModel(this searchModel.Facet facet, params string[] appliedFilters)
        {
            var result = new coreModel.AggregationItem
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
