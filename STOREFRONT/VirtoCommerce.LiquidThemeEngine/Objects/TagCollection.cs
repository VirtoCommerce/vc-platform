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
                return Root?.GroupBy(t => t.GroupName).Select(g => g.Key);
            }
        }
    }
}
