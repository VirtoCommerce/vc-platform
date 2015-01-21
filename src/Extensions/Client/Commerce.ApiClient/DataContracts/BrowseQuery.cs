using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace VirtoCommerce.Web.Core.DataContracts
{
    /// <summary>
    /// Model for a query, including sorting, paging and filter string
    /// </summary>
    [DataContract(Namespace = "http://schemas.virtocommerce.com/2.0")]
    public class BrowseQuery
    {

        /// <summary>
        /// The default page size
        /// </summary>
        public const int DefaultPageSize = 8;

        /// <summary>
        /// Gets or sets the number of items to skip as part of this query
        /// </summary>
        [DataMember]
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or sets the number of items to take as part of this query
        /// </summary>
        [DataMember]
        public int? Take { get; set; }

        /// <summary>
        /// Gets or sets the property to sort on as part of this query
        /// </summary>
        [DataMember]
        public string SortProperty { get; set; }

        /// <summary>
        /// Gets or sets the direction on which to sort on as part of this query
        /// </summary>
        [DataMember]
        public string SortDirection { get; set; }

        /// <summary>
        /// Gets or sets the string on which to filter as part of this query
        /// </summary>
        [DataMember, DefaultValue("")]
        public string Search { get; set; }

        /// <summary>
        /// Gets or sets the start date from.
        /// </summary>
        public DateTime? StartDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the outline to filter products. Its category path.
        /// </summary>
        [DataMember, DefaultValue("")]
        public string Outline { get; set; }

        [DataMember]
        public Dictionary<string, string[]> Filters { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Search);
            builder.Append(Skip);
            builder.Append(Take);
            builder.Append(Outline);
            builder.Append(SortProperty);

            foreach (var facet in Filters)
            {
                builder.Append(facet);
            }

            return builder.ToString();
        }
    }
}
