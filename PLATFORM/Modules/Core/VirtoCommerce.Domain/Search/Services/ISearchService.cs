using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Domain.Search.Services
{
  public interface ISearchService
	{
        /// <summary>
        /// Builds the index in the specified scope for the specified index document types. Pass empty document type to index all items.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="indexDocumentType">Type of the index document.</param>
        /// <param name="rebuild">if set to <c>true</c> [rebuild].</param>
		void BuildIndex(string scope, string indexDocumentType, bool rebuild);

        /// <summary>
        /// Searches the specified criteria in the specified scope.
        /// </summary>
        /// <param name="scope">The scope which is a global namespace for the search.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        ISearchResults Search(string scope, ISearchCriteria criteria);

   
    }
}
