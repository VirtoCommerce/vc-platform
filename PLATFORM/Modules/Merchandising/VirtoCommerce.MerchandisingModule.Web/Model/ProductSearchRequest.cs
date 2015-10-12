using System;
using System.Text;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ProductSearchRequest
    {
        public ProductSearchRequest()
        {
            ResponseGroup = ItemResponseGroup.ItemMedium;
            Outline = string.Empty;
            Language = "en-us";
            Currency = "USD";
            Take = 10;
        }

        /// <summary>
        /// Store ID
        /// </summary>
        public string Store { get; set; }

        /// <summary>
        /// Array of pricelist IDs
        /// </summary>
        public string[] Pricelists { get; set; }

        /// <summary>
        /// Response detalization scale (default value is ItemMedium)
        /// </summary>
        public ItemResponseGroup ResponseGroup { get; set; }

        /// <summary>
        /// Product category outline
        /// </summary>
        public string Outline { get; set; }

        /// <summary>
        /// Culture name (default value is "en-us")
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Currency (default value is "USD")
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the search phrase
        /// </summary>
        public string SearchPhrase { get; set; }

        /// <summary>
        /// Gets or sets the sort
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Gets or sets the sort order ascending or descending
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the start date
        /// </summary>
        public DateTime? StartDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the number of items to skip
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets the number of items to return
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Gets or sets search terms collection
        /// Item format: name:value1,value2,value3
        /// </summary>
        public string[] Terms { get; set; }

        /// <summary>
        /// Gets or sets the facets collection
        /// Item format: name:value1,value2,value3
        /// </summary>
        public string[] Facets { get; set; }

        public void Normalize()
        {
            SearchPhrase = SearchPhrase.EmptyToNull();
            Sort = Sort.EmptyToNull();
            SortOrder = SortOrder.EmptyToNull();

            if (!string.IsNullOrEmpty(SearchPhrase))
            {
                SearchPhrase = SearchPhrase.EscapeSearchTerm();
            }
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(SearchPhrase);
            builder.Append(Skip);
            builder.Append(Take);
            builder.Append(Sort);

            foreach (var facet in Facets)
            {
                builder.Append(facet);
            }

            return builder.ToString();
        }
    }
}
