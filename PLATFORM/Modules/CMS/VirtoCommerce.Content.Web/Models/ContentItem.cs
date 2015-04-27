namespace VirtoCommerce.Content.Web.Models
{
    #region

    using System;

    #endregion

    public class ContentItem
    {
        #region Public Properties

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public string Name { get; set; }

        public string ParentContentItemId { get; set; }

        public string Path { get; set; }

        #endregion
    }
}