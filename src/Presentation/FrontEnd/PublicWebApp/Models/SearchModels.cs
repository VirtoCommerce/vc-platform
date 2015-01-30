using System.Collections.Generic;
using VirtoCommerce.ApiClient.DataContracts.Search;

namespace VirtoCommerce.Web.Models
{
    public class PagerModel
    {
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount { get; set; }
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Gets or sets the records per page.
        /// </summary>
        /// <value>The records per page.</value>
        public int RecordsPerPage { get; set; }
        /// <summary>
        /// Gets or sets the starting record.
        /// </summary>
        /// <value>The starting record.</value>
        public int StartingRecord { get; set; }

        /// <summary>
        /// Gets or sets the display starting record.
        /// </summary>
        /// <value>The display starting record.</value>
        public int DisplayStartingRecord { get; set; }
        /// <summary>
        /// Gets or sets the display ending record.
        /// </summary>
        /// <value>The display ending record.</value>
        public int DisplayEndingRecord { get; set; }
        /// <summary>
        /// Gets or sets the sort values.
        /// </summary>
        /// <value>The sort values.</value>
        public string[] SortValues { get; set; }
        /// <summary>
        /// Gets or sets the selected sort.
        /// </summary>
        /// <value>The selected sort.</value>
        public string SelectedSort { get; set; }

        /// <summary>
        /// Gets or sets the selected sort order.
        /// </summary>
        /// <value>The selected sort order.</value>
        public string SortOrder { get; set; }
    }


    public class SearchResult
    {

        public SearchResult()
        {
            Results = new List<ItemModel>();
            Pager = new PagerModel();
        }
        public PagerModel Pager { get; set; }

        public List<ItemModel> Results { get; set; }

        public Facet[] Facets { get; set; }

        public string Title { get; set; }
    }
}