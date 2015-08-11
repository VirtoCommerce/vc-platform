using System.Collections.Generic;

namespace VirtoCommerce.CustomerModule.Web.Model
{
	public class SearchResult
	{
		public SearchResult()
		{
			Members = new List<Member>();
		}
		
        /// <summary>
        /// Total count of objects satisfied Search Criteria
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Part of objects satisfied Search Criteria. See Skip and Count parameters of Search Criteria
        /// </summary>
		public List<Member> Members { get; set; }

	}
}
