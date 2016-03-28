using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Web.Models
{
    public abstract class ContentItem 
    {
        public ContentItem(string type)
        {
            Type = type;
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string RelativeUrl { get; set; }
        public string ParentUrl { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}