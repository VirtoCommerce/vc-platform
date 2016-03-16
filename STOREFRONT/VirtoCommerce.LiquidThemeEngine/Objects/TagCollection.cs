using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class TagCollection : ItemCollection<Tag>
    {
        public TagCollection(IEnumerable<Tag> tags)
            : base(tags)
        {
        }

        public IEnumerable<string> Groups
        {
            get
            {
                var retVal = this.GroupBy(t => t.GroupLabel).Select(g => g.Key);
                return retVal;
            }
        }

        public override bool Contains(object value)
        {
            var tag = value as Tag;
            var str = value as string;
            var retVal = false;

            if (tag != null)
            {
                retVal = this.Any(x => x.Equals(tag));
            }

            if (str != null)
            {
                retVal = this.Any(x => string.Equals(x.Value, str, StringComparison.OrdinalIgnoreCase));
            }

            return retVal;
        }
    }
}
