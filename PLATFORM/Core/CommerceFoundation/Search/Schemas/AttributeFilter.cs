using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Search.Schemas
{
    public partial class AttributeFilter
    {
        public string CacheKey
        {
            get
            {
                var key = new StringBuilder();
                key.Append("_af:" + Key);
                foreach (var field in this.Values)
                {
                    key.Append("_af:" + field.Id);
                }
                return key.ToString();
            }
        }
    }
}
