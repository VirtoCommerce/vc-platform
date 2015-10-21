using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represent SEO information and contains common SEO fields  
    /// </summary>
    public class SeoInfo
    {
        public string ImageAltDescription { get; set; }

        public string Language { get; set; }

        public string MetaDescription { get; set; }

        public string Slug { get; set; }

        public string MetaKeywords { get; set; }

        public string Title { get; set; }

    }
}