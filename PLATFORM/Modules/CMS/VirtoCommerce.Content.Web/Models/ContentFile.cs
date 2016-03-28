using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
    /// <summary>
    /// Represent content file
    /// </summary>
    public class ContentFile : ContentItem
    {
        public ContentFile()
            :base("blob")
        {
        }

        public string MimeType { get; set; }
        public string Size { get; set; }
    }
}