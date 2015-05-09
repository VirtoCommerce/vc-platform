using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;

namespace VirtoCommerce.Web.Models.Banners
{
    public class PlaceHolder : Drop
    {
        public string Name { get; set; }

        public BannerCollection Banners { get; set; }
    }
}