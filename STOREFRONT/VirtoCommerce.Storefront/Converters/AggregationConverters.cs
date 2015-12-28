using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class AggregationConverters
    {
        public static Aggregation ToWebModel(this VirtoCommerceCatalogModuleWebModelAggregation aggregation)
        {
            var result = new Aggregation();
            result.InjectFrom<NullableAndEnumValueInjecter>(aggregation);

            if (aggregation.Items != null)
            {
                result.Items = aggregation.Items.Select(i => i.ToWebModel()).ToArray();
            }

            return result;
        }

        public static AggregationItem ToWebModel(this VirtoCommerceCatalogModuleWebModelAggregationItem item)
        {
            var result = new AggregationItem();
            result.InjectFrom<NullableAndEnumValueInjecter>(item);
            return result;
        }
    }
}
