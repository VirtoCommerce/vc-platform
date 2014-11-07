using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor.Parser;

namespace VirtoCommerce.Web.Client.Extensions.Routing
{
    public static class Constants
    {
        /// <summary>
        /// The language route key
        /// </summary>
        public const string Language = "lang";
        /// <summary>
        /// The store route key
        /// </summary>
        public const string Store = "store";
        /// <summary>
        /// The category route key
        /// </summary>
        public const string Category = "category";
        /// <summary>
        /// The item route key
        /// </summary>
        public const string Item = "item";
        /// <summary>
        /// The language regex
        /// </summary>
        public const string LanguageRegex = "[a-z]{2}(-[A-Z]{2})?";

        /// <summary>
        /// Gets the item route. {lang}/{store}/{category}/{item}
        /// </summary>
        /// <value>
        /// The item route.
        /// </value>
        public static string ItemRoute
        {
            get
            {
                return string.Format("{0}/{{{1}}}", CategoryRoute, Item);
            }
        }

        /// <summary>
        /// Gets the category route. {lang}/{store}/{category}
        /// </summary>
        /// <value>
        /// The category route.
        /// </value>
        public static string CategoryRoute
        {
            get
            {
                return string.Format("{0}/{{{1}}}", StoreRoute, Category);
            }
        }

        /// <summary>
        /// Gets the store route. {lang}/{store}
        /// </summary>
        /// <value>
        /// The store route.
        /// </value>
        public static string StoreRoute
        {
            get
            {
                return string.Format("{{{0}}}/{{{1}}}", Language, Store);
            }
        }

    }
}
