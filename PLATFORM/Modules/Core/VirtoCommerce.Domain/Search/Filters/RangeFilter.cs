using System.Text;

namespace VirtoCommerce.Domain.Search.Filters
{
    public partial class RangeFilter
    {
        public string CacheKey
        {
            get
            {
                var key = new StringBuilder();
                key.Append("_rf:" + Key);
                foreach (var field in this.Values)
                {
                    key.Append("_rf:" + field.Id);
                }
                return key.ToString();
            }
        }
    }
}
