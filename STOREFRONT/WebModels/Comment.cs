#region
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{

    #region
    #endregion

    public class Comment : Drop
    {
        #region Public Properties
        public string Author { get; set; }

        public string Content { get; set; }

        public string Email { get; set; }

        public string Id { get; set; }

        public CommentStatus Status { get; set; }

        public string Url { get; set; }
        #endregion
    }

    public enum CommentStatus
    {
        Unapproved,

        Published,

        Removed,

        Spam
    }
}