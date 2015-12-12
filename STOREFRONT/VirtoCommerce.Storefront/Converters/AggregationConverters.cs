using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Converters
{
    public static class AggregationConverters
    {
        public static Aggregation ToWebModel(this VirtoCommerceCatalogModuleWebModelAggregation aggregation)
        {
            var result = new Aggregation();
            result.InjectFrom<NullableAndEnumValueInjection>(aggregation);

            if (aggregation.Items != null)
            {
                result.Items = aggregation.Items.Select(i => i.ToWebModel()).ToArray();
            }

            return result;
        }

        public static AggregationItem ToWebModel(this VirtoCommerceCatalogModuleWebModelAggregationItem item)
        {
            var result = new AggregationItem();
            result.InjectFrom<NullableAndEnumValueInjection>(item);
            return result;
        }
    }
}
