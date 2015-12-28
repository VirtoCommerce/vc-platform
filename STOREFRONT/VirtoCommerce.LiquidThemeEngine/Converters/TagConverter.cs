using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class TagConverter
    {
        public static Tag ToShopifyModel(this Term term)
        {
            return new Tag(term.Name, term.Value);
        }

        public static Tag ToShopifyModel(this AggregationItem item, string groupName, string groupLabel)
        {
            return new Tag(groupName, item.Value != null ? item.Value.ToString() : null)
            {
                GroupLabel = groupLabel,
                Label = item.Label,
                Count = item.Count,
            };
        }
    }
}
