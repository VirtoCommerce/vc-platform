using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Search.Model;
using VirtoCommerce.Foundation.Search.Factories;

namespace VirtoCommerce.Foundation.Search.Services
{
    [ServiceContract(Name = "SearchService", Namespace = "http://schemas.virtocommerce.com/1.0/search/")]
	public interface ISearchService
	{
        /// <summary>
        /// Builds the index in the specified scope for the specified index document types. Pass empty document type to index all items.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="indexDocumentType">Type of the index document.</param>
        /// <param name="rebuild">if set to <c>true</c> [rebuild].</param>
		[OperationContract]
		void BuildIndex(string scope, string indexDocumentType, bool rebuild);

        /// <summary>
        /// Searches the specified criteria in the specified scope.
        /// </summary>
        /// <param name="scope">The scope which is a global namespace for the search.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [OperationContract]
        ISearchResults Search(string scope, ISearchCriteria criteria);

        /*
        #region Filter Methods
        [OperationContract]
        void SaveFilters(Filter[] filters);
        [OperationContract]
        Filter[] GetAllFilters(string storeId, bool useCache);
        #endregion

        #region Build Settings Methods
        [OperationContract]
        void SaveBuildSettings(BuildSettings settings);
        [OperationContract]
        BuildSettings GetAllBuildSettings(string storeId, string documentType);
        #endregion         
         * */
    }
}
