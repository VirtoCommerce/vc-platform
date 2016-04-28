using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Descriptions : ItemCollection<Description>
    {
        public Descriptions(IEnumerable<Description> descriptions)
            : base(descriptions)
        {
        }

        public override object BeforeMethod(string method)
        {
            return this.SingleOrDefault(x => x.Type == method);
        }
    }
}