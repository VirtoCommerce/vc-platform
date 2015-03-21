using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Search.Schemas
{
    public partial class PriceRangeFilter
    {
        public string CacheKey
        {
            get
            {
                var key = new StringBuilder();
                key.Append("_pr:" + Key);
                foreach (var field in this.Values)
                {
                    key.Append("_pr:" + field.Id);
                }
                return key.ToString();
            }
        }
    }
}
