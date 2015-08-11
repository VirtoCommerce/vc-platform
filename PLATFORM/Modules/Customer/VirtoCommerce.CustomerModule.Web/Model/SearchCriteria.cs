namespace VirtoCommerce.CustomerModule.Web.Model
{
	public class SearchCriteria
	{
		public SearchCriteria()
		{
			Count = 20;
		}

        /// <summary>
        /// Word, part of word or phrase to search
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// It used to limit search within an organization
        /// </summary>
		public string OrganizationId { get; set; }
	    
        /// <summary>
        /// It used to skip some first search results 
        /// </summary>
		public int Start { get; set; }
        
        /// <summary>
        /// It used to limit the number of search results
        /// </summary>
        /// <value>20 by default</value>
		public int Count { get; set; }
	}
}
