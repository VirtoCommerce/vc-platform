using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class AggregationConverters
    {
        public static Aggregation ToWebModel(this VirtoCommerceCatalogModuleWebModelAggregation aggregation, string currentLanguage)
        {
            var result = new Aggregation();
            result.InjectFrom<NullableAndEnumValueInjecter>(aggregation);

            if (aggregation.Items != null)
            {
                result.Items = aggregation.Items
                    .Select(i => i.ToWebModel(currentLanguage))
                    .ToArray();
            }

            if (aggregation.Labels != null)
            {
                result.Label =
                    aggregation.Labels.Where(l => string.Equals(l.Language, currentLanguage, StringComparison.OrdinalIgnoreCase))
                        .Select(l => l.Label)
                        .FirstOrDefault();
            }

            if (string.IsNullOrEmpty(result.Label))
            {
                result.Label = aggregation.Field;
            }

            return result;
        }

        public static AggregationItem ToWebModel(this VirtoCommerceCatalogModuleWebModelAggregationItem item, string currentLanguage)
        {
            var result = new AggregationItem();
            result.InjectFrom<NullableAndEnumValueInjecter>(item);

            if (item.Labels != null)
            {
                result.Label =
                    item.Labels.Where(l => string.Equals(l.Language, currentLanguage, StringComparison.OrdinalIgnoreCase))
                        .Select(l => l.Label)
                        .FirstOrDefault();
            }

            if (string.IsNullOrEmpty(result.Label) && item.Value != null)
            {
                result.Label = item.Value.ToString();
            }

            return result;
        }
    }
}
