#region
using System;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{

    #region
    #endregion

    public class Page : Drop
    {
        #region Public Properties
        public string Author { get; set; }

        public string Content { get; set; }

        public string Handle { get; set; }

        public string Id { get; set; }

        public string Layout { get; set; }

        public DateTime PublishedAt { get; set; }

        public string TemplateSuffix { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
        #endregion
    }
}