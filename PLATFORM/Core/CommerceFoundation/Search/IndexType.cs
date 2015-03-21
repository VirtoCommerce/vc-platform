using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Search
{
    public partial class IndexType
    {
        /// <summary>
        /// Values are parsed to tokens
        /// </summary>
        public static string ANALYZED = "Index.Analyzed";
        /// <summary>
        /// Values are indexed as is
        /// </summary>
        public static string NOT_ANALYZED = "Index.NotAnalyzed";
        /// <summary>
        /// Values are not indexed
        /// </summary>
        public static string NO = "Index.No";
    }
}
