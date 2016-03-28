using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Web.Models
{
    /// <summary>
    /// Base class for content items
    /// </summary>
    public abstract class ContentItem 
    {
        public ContentItem(string type)
        {
            Type = type;
        }
        public string Name { get; set; }
        /// <summary>
        /// content item type (ContentFile, ContentFolder etc)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Absolute url (which may be used to acccess or download content item directly)
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Relative content item url for access only through provider 
        /// </summary>
        public string RelativeUrl { get; set; }
        /// <summary>
        /// Parent folder url
        /// </summary>
        public string ParentUrl { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}