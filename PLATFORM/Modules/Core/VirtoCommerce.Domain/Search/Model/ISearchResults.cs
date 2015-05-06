namespace VirtoCommerce.Domain.Search.Model
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
