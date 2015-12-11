using System.Linq;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class AggregationConverters
    {
        public static webModel.Aggregation ToWebModel(this coreModel.Aggregation aggregation)
        {
            var result = new webModel.Aggregation();
            result.InjectFrom(aggregation);

            if (aggregation.Items != null)
            {
                result.Items = aggregation.Items.Select(i => i.ToWebModel()).ToArray();
            }

            return result;
        }

        public static webModel.AggregationItem ToWebModel(this coreModel.AggregationItem item)
        {
            var result = new webModel.AggregationItem();
            result.InjectFrom(item);
            return result;
        }
    }
}
