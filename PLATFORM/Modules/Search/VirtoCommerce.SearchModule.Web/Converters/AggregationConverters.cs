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
                Items = facetGroup.Facets.Select(f => f.ToModuleModel(appliedFilters)).ToArray()
            };

            if (facetGroup.Labels != null)
            {
                result.Labels = facetGroup.Labels.Select(ToModuleModel).ToArray();
            }

            return result;
        }

        public static coreModel.AggregationItem ToModuleModel(this searchModel.Facet facet, params string[] appliedFilters)
        {
            var result = new coreModel.AggregationItem
            {
                Value = facet.Key,
                Count = facet.Count,
                IsApplied = appliedFilters.Any(x => x.Equals(facet.Key, StringComparison.OrdinalIgnoreCase))
            };

            if (facet.Labels != null)
            {
                result.Labels = facet.Labels.Select(ToModuleModel).ToArray();
            }

            return result;
        }

        public static coreModel.AggregationLabel ToModuleModel(this searchModel.FacetLabel label)
        {
            return new coreModel.AggregationLabel
            {
                Language = label.Language,
                Label = label.Label,
            };
        }
    }
}
