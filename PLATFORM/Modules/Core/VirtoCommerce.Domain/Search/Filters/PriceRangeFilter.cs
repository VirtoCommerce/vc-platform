using System.Text;

namespace VirtoCommerce.Domain.Search.Filters
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
