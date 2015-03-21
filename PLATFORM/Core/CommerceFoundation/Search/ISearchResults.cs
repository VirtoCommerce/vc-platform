using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Search.Facets;

namespace VirtoCommerce.Foundation.Search
{
    public interface ISearchResults
    {
        ISearchCriteria SearchCriteria { get; }

        /// <summary>
        /// Gets the total count of all results that we can potentially return.
        /// </summary>
        /// <value>The total count.</value>
        int TotalCount { get; }

        int DocCount { get; }

        /// <summary>
        /// Gets or sets the facet groups.
        /// </summary>
        /// <value>The facet groups.</value>
        FacetGroup[] FacetGroups { get; set; }

        ResultDocumentSet[] Documents {get;}
    }
}
