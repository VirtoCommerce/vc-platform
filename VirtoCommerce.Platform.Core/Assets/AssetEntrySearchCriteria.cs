using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Assets
{
    public class AssetEntrySearchCriteria : ValueObject
    {
        /// <summary>
        /// Phrase to search in Name and RelativeUrl
        /// </summary>
        public string SearchPhrase { get; set; }

        public TenantIdentity[] Tenants { get; set; }

        /// <summary>
        /// Asset language
        /// </summary>
        public string LanguageCode { get; set; }

        public string Group { get; set; }

        /// <summary>
        /// Sorting expression property1:asc;property2:desc
        /// </summary>
        public string Sort { get; set; }
        public virtual SortInfo[] SortInfos => SortInfo.Parse(Sort).ToArray();

        public int Skip { get; set; }
        public int Take { get; set; } = 20;
    }
}
