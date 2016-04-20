using System.Collections.Generic;
using DotLiquid;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/blog
    /// </summary>
    public class Blog : Drop
    {
        public Blog()
        {
            AllTags = new List<Tag>();
            Tags = new List<Tag>();
        }
        /// <summary>
        /// Returns all tags of all articles of a blog. This includes tags of articles that are not in the current pagination view.
        /// </summary>
        public ICollection<Tag> AllTags { get; set; }

        /// <summary>
        /// Returns all tags in a blog. Similar to all_tags, but only returns tags of articles that are in the filtered view.
        /// </summary>
        public ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// Returns an array of all articles in a blog. See this page for a list of all available attributes for article.
        /// </summary>
        public IMutablePagedList<Article> Articles { get; set; }

        /// <summary>
        /// Returns the total number of articles in a blog. This total does not include hidden articles.
        /// </summary>
        public int ArticlesCount { get { return Articles.GetTotalCount(); } }

        /// <summary>
        /// Returns true if comments are enabled, or false if they are disabled.
        /// </summary>
        public bool CommentsEnabled { get; set; }

        public string Handle { get; set; }

        public string Id { get; set; }

        public bool Moderated { get; set; }

        /// <summary>
        /// Returns the title of the blog.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns the relative URL of the blog.
        /// </summary>
        public string Url { get; set; }
    }
}
