using System.Globalization;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Tag : Drop
    {
        public Tag(string groupName, string value)
        {
            GroupName = groupName;
            Value = value;
            Id = string.Concat(GroupName, "_", Value).ToLowerInvariant();
        }

        public string GroupName { get; set; }
        public string GroupLabel { get; set; }
        public string Label { get; set; }
        public int Count { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            var filters = (Context["collection_sidebar_filters"] ?? string.Empty).ToString();

            if (filters == "groups")
            {
                // eliminate count for now, since it problematic to make it work in some templates, especially when determine active tag
                return Id;
            }

            if (filters == "facets")
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}_{1} ({2})", GroupName, Label, Count);
            }

            return Id;
        }

        public override bool Equals(object obj)
        {
            var tag = obj as Tag;
            return tag != null && Id.Equals(tag.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
