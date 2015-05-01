namespace VirtoCommerce.Web.Models.Routing
{
    public static class Constants
    {
        #region Constants
        /// <summary>
        ///     The category route key
        /// </summary>
        public const string Category = "category";

        /// <summary>
        ///     The item route key
        /// </summary>
        public const string Item = "item";

        /// <summary>
        ///     The language route key
        /// </summary>
        public const string Language = "lang";

        public const string Tags = "tags";

        /// <summary>
        ///     The language regex
        /// </summary>
        public const string LanguageRegex = "[a-z]{2}(-[A-Z]{2})?";

        /// <summary>
        ///     The store route key
        /// </summary>
        public const string Store = "store";
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the category route. {lang}/{store}/{category}
        /// </summary>
        /// <value>
        ///     The category route.
        /// </value>
        public static string CategoryRoute
        {
            get
            {
                return string.Format("{0}/{{{1}}}", StoreRoute, Category);
            }
        }

        /// <summary>
        ///     Gets the category route. {lang}/{store}/{category}
        /// </summary>
        /// <value>
        ///     The category route.
        /// </value>

        public static string CategoryRouteWithTags
        {
            get
            {
                return string.Format("{0}/{{{1}}}/{{{2}}}", StoreRoute, Category, Tags);
            }
        }

        public static string CategoryRouteCodeWithTags
        {
            get
            {
                return string.Format("{0}/{{{1}}}/collection/{{{2}}}", StoreRoute, Category, Tags);
            }
        }

        /// <summary>
        ///     Gets the item route. {lang}/{store}/{category}/{item}
        /// </summary>
        /// <value>
        ///     The item route.
        /// </value>
        public static string ItemRoute
        {
            get
            {
                return string.Format("{0}/{{{1}}}", CategoryRoute, Item);
            }
        }

        public static string ItemRouteWithCode
        {
            get
            {
                return string.Format("{0}/itm-{{{1}}}", CategoryRoute, Item);
            }
        }

        /// <summary>
        ///     Gets the store route. {lang}/{store}
        /// </summary>
        /// <value>
        ///     The store route.
        /// </value>
        public static string StoreRoute
        {
            get
            {
                return string.Format("{{{0}}}/{{{1}}}", Language, Store);
            }
        }

        #endregion
    }
}