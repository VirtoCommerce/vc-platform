using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class UserSearchCriteria : SearchCriteriaBase
    {
        public string MemberId { get; set; }
        public IList<string> MemberIds { get; set; }
        public DateTime? ModifiedSinceDate { get; set; }
        //Search users by their role names
        public string[] Roles { get; set; }
        [Obsolete("Use LoginStartDate/LoginEndDate instead", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public DateTime? LasLoginDate { get; set; }
        public bool OnlyUnlocked { get; set; }
        public bool OnlyLocked { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string UserType { get; set; }
        public string Status { get; set; }
        public string StoreId { get; set; }
        public DateTime? LoginStartDate { get; set; }
        public DateTime? LoginEndDate { get; set; }
    }
}
