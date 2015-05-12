#region

using System;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models.Blogs
{
    public class Article : Drop
    {
        #region Constructors and Destructors
        public Article()
        {
            this.User = new ArticleUser();
        }
        #endregion

        #region Public Properties
        public string Author { get; set; }

        public string CommentPostUrl { get; set; }

        public Comment[] Comments { get; set; }

        public string CommentsCount { get; set; }

        public bool CommentsEnabled { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Excerpt { get; set; }

        public string ExcerptOrContent { get; set; }

        public string Id { get; set; }

        public bool Moderated { get; set; }

        public DateTime PublishedAt { get; set; }

        public string[] Tags { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public ArticleUser User { get; set; }
        #endregion
    }

    public class ArticleUser : Drop
    {
        #region Public Properties
        public string AccountOwner { get; set; }

        public string Bio { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string Homepage { get; set; }

        public string LastName { get; set; }
        #endregion
    }
}