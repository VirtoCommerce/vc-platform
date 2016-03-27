using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/article
    /// </summary>
    public class Article : Page
    {
        public Article()
        {
            this.User = new ArticleUser();
        }

        /// <summary>
        /// Returns the relative URL where POST requests are sent to when creating new comments.
        /// </summary>
        public string CommentPostUrl { get; set; }

        public IMutablePagedList<Comment> Comments { get; set; }

        public string CommentsCount { get; set; }

        public bool CommentsEnabled { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Returns the excerpt of an article.
        /// </summary>
        public string Excerpt { get; set; }

        public string ExcerptOrContent { get; set; }

        public bool Moderated { get; set; }

        public string[] Tags { get; set; }

        public ArticleUser User { get; set; }

    }

   
}
