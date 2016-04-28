using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Customer.Model
{
    /// <summary>
    /// represent common members search criteria. More specialized criteria should be derived for this type.
    /// </summary>
	public class MembersSearchCriteria
	{
		public MembersSearchCriteria()
		{
            Take = 20;
		}
        /// <summary>
        /// Search member type (Contact, Organization etc)
        /// </summary>
        public string MemberType { get; set; }

        private string[] _memberTypes;
        public string[] MemberTypes
        {
            get
            {
                if (_memberTypes == null && !string.IsNullOrEmpty(MemberType))
                {
                    _memberTypes = new[] { MemberType };
                }
                return _memberTypes;
            }
            set
            {
                _memberTypes = value;
            }
        }

        /// <summary>
        /// Word, part of word or phrase to search
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// It used to limit search within an member (organization for example)
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// Deep search in  specified memberId  children members or in all if not memberId empty
        /// </summary>
        public bool DeepSearch { get; set; }

        /// <summary>
        /// Sorting expression property1:asc;property2:desc
        /// </summary>
        public string Sort { get; set; }

        public SortInfo[] SortInfos
        {
            get
            {
                return SortInfo.Parse(Sort).ToArray();
            }
        }

        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
