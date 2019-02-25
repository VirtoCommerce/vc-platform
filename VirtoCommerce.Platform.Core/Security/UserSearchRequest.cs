using System;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class UserSearchRequest
    {
        public UserSearchRequest()
        {
            TakeCount = 20;
        }
        public string ResponseGroup { get; set; }
        /// <summary>
        /// Sorting expression property1:asc;property2:desc
        /// </summary>
        public string Sort { get; set; }
        public virtual SortInfo[] SortInfos => SortInfo.Parse(Sort).ToArray();

        public DateTime? ModifiedSinceDate { get; set; }
        public string[] AccountTypes { get; set; }
        public string Keyword { get; set; }
        public string MemberId { get; set; }
        public string[] MemberIds { get; set; }
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
    }
}
