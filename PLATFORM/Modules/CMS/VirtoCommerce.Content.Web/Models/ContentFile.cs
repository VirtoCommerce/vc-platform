using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
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