﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.StaticContent
{
    public class Blog : ContentItem
    {
        public Blog(string url, Language language)
            : base(url, language)
        {
        }

        public IStorefrontPagedList<BlogArticle> Articles { get; set; }
    }
}
