#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts
{

    #region

    #endregion

    /// <summary>
    ///     Model for a query, including sorting, paging and filter string
    /// </summary>
    [DataContract(Namespace = "http://schemas.virtocommerce.com/2.0")]
    public class BrowseQuery
    {
        #region Constants

        /// <summary>
        ///     The default page size
        /// </summary>
        public const int DefaultPageSize = 8;

        #endregion

        #region Public Properties

        [DataMember]
        public Dictionary<string, string[]> Filters { get; set; }

        /// <summary>
        ///     Gets or sets the outline to filter products. Its category path.
        /// </summary>
        [DataMember]
        [DefaultValue("")]
        public string Outline { get; set; }

        [DataMember]
        public string[] PriceLists { get; set; }

        /// <summary>
        ///     Gets or sets the string on which to filter as part of this query
        /// </summary>
        [DataMember]
        [DefaultValue("")]
        public string Search { get; set; }

        /// <summary>
        ///     Gets or sets the number of items to skip as part of this query
        /// </summary>
        [DataMember]
        public int? Skip { get; set; }

        /// <summary>
        ///     Gets or sets the direction on which to sort on as part of this query
        /// </summary>
        [DataMember]
        public string SortDirection { get; set; }

        /// <summary>
        ///     Gets or sets the property to sort on as part of this query
        /// </summary>
        [DataMember]
        public string SortProperty { get; set; }

        /// <summary>
        ///     Gets or sets the start date from.
        /// </summary>
        public DateTime? StartDateFrom { get; set; }

        /// <summary>
        ///     Gets or sets the number of items to take as part of this query
        /// </summary>
        [DataMember]
        [DefaultValue(20)]
        public int? Take { get; set; }

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Search);
            builder.Append(Skip);
            builder.Append(Take);
            builder.Append(Outline);
            builder.Append(SortProperty);

            if (PriceLists != null)
            {
                builder.Append(string.Join(",", PriceLists));
            }

            foreach (var facet in Filters)
            {
                builder.Append(facet);
            }

            return builder.ToString();
        }

        #endregion
    }
}
