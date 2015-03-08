using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class SearchParameters
    {
        #region Constants

        /// <summary>
        ///     The default page size
        /// </summary>
        public const int DefaultPageSize = 8;

        #endregion

        #region Static Fields

        /// <summary>
        ///     The facet regex
        /// </summary>
        private static readonly Regex FacetRegex = new Regex("^f_", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // used to apply facet filtering

        private static readonly Regex TermRegex = new Regex("^t_", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        // used to filter by any property

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchParameters" /> class.
        /// </summary>
        public SearchParameters()
        {
            this.Facets = new Dictionary<string, string[]>();
            this.PageSize = 0;
            this.StartingRecord = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the facets.
        /// </summary>
        /// <value>The facets.</value>
        public IDictionary<string, string[]> Facets { get; set; }

        /// <summary>
        ///     Gets or sets the free search.
        /// </summary>
        /// <value>The free search.</value>
        public string FreeSearch { get; set; }

        /// <summary>
        ///     Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; set; }

        /// <summary>
        ///     Gets or sets the sort.
        /// </summary>
        /// <value>The sort.</value>
        public string Sort { get; set; }

        /// <summary>
        ///     Gets or sets the sort order asc or desc.
        /// </summary>
        /// <value>True if descending, false if ascending</value>
        public string SortOrder { get; set; }

        public DateTime? StartDateFrom { get; set; }

        /// <summary>
        ///     Gets or sets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
        public int StartingRecord { get; set; }

        public IDictionary<string, string[]> Terms { get; set; }

        #endregion

        #region Public Methods and Operators

        public static bool TryParse(string s, out SearchParameters result)
        {
            result = null;

            var qs = HttpUtility.ParseQueryString(s);

            var qsDict = NvToDict(qs);

            // parse facets
            var facets =
                qsDict.Where(k => FacetRegex.IsMatch(k.Key))
                    .Select(k => k.WithKey(FacetRegex.Replace(k.Key, "")))
                    .ToDictionary(x => x.Key, y => y.Value.Split(','));

            // parse facets
            var terms =
                qsDict.Where(k => TermRegex.IsMatch(k.Key))
                    .Select(k => k.WithKey(TermRegex.Replace(k.Key, "")))
                    .ToDictionary(x => x.Key, y => y.Value.Split(','));

            var sp = new SearchParameters
                     {
                         FreeSearch = qs["q"].EmptyToNull(),
                         StartingRecord = qs["skip"].TryParse(0),
                         PageSize = qs["take"].TryParse(10),
                         Sort = qs["sort"].EmptyToNull(),
                         SortOrder = qs["sortorder"].EmptyToNull(),
                         Facets = facets,
                         Terms = terms
                     };
            if (!string.IsNullOrEmpty(sp.FreeSearch))
            {
                sp.FreeSearch = sp.FreeSearch.EscapeSearchTerm();
            }

            DateTime startDate;
            if (!string.IsNullOrEmpty(qs["startdatefrom"]) && DateTime.TryParse(qs["startdatefrom"], out startDate))
            {
                sp.StartDateFrom = startDate;
            }

            result = sp;

            return true;
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(this.FreeSearch);
            builder.Append(this.StartingRecord);
            builder.Append(this.PageSize);
            builder.Append(this.Sort);

            foreach (var facet in this.Facets)
            {
                builder.Append(facet);
            }

            return builder.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Name values to dictionary.
        /// </summary>
        /// <param name="nv">The nv.</param>
        /// <returns>IDictionary{System.StringSystem.String}.</returns>
        private static Dictionary<string, string> NvToDict(NameValueCollection nv)
        {
            var d = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var k in nv.AllKeys)
            {
                d[k] = nv[k];
            }
            return d;
        }

        #endregion
    }
}
