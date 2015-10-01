using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class SearchParameters
    {
        /// <summary>
        /// The value of default page size
        /// </summary>
        public const int DefaultPageSize = 8;

        /// <summary>
        /// The facet regex
        /// </summary>
        private static readonly Regex _facetRegex = new Regex("^f_", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Used to apply facet filtering
        /// </summary>
        private static readonly Regex _termRegex = new Regex("^t_", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchParameters" /> class.
        /// </summary>
        public SearchParameters()
        {
            Facets = new StringKeyValues[0];
            PageSize = 0;
            StartingRecord = 0;
        }

        /// <summary>
        /// Gets or sets the facets
        /// </summary>
        /// <value>The facets</value>
        public StringKeyValues[] Facets { get; set; }

        /// <summary>
        /// Gets or sets the free search
        /// </summary>
        /// <value>The free search</value>
        public string FreeSearch { get; set; }

        /// <summary>
        /// Gets or sets the size of the page
        /// </summary>
        /// <value>The size of the page</value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the sort
        /// </summary>
        /// <value>The sort</value>
        public string Sort { get; set; }

        /// <summary>
        /// Gets or sets the sort order ascending or descending
        /// </summary>
        /// <value>True if descending, false if ascending</value>
        public string SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the start date
        /// </summary>
        public DateTime? StartDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the index of the page
        /// </summary>
        /// <value>The index of the page</value>
        public int StartingRecord { get; set; }

        /// <summary>
        /// Gets or sets search terms collection
        /// </summary>
		public StringKeyValues[] Terms { get; set; }

        public static bool TryParse(string s, out SearchParameters result)
        {
            var qs = HttpUtility.ParseQueryString(s);

            var qsDict = NvToDict(qs);

            // parse facets
            var facets =
                qsDict.Where(k => _facetRegex.IsMatch(k.Key))
                    .Select(k => k.WithKey(_facetRegex.Replace(k.Key, "")))
                    .ToDictionary(x => x.Key, y => y.Value.Split(','))
                    .Select(p => new StringKeyValues { Key = p.Key, Value = p.Value })
                    .ToArray();

            // parse filters
            var terms =
                qsDict.Where(k => _termRegex.IsMatch(k.Key))
                    .Select(k => k.WithKey(_termRegex.Replace(k.Key, "")))
                    .ToDictionary(x => x.Key, y => y.Value.Split(','))
                    .Select(p => new StringKeyValues { Key = p.Key, Value = p.Value })
                    .ToArray();

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
            builder.Append(FreeSearch);
            builder.Append(StartingRecord);
            builder.Append(PageSize);
            builder.Append(Sort);

            foreach (var facet in Facets)
            {
                builder.Append(facet);
            }

            return builder.ToString();
        }

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
    }
}
