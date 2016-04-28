using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Customer.Model
{
	public class MembersSearchResult
	{
        public MembersSearchResult()
        {
            Members = new List<Member>();
        }
		public int TotalCount { get; set; }
		public List<Member> Members { get; set; }
	}
}
