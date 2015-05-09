using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Web.Models.Banners
{
    public class BannerCollection : ItemCollection<Banner>
    {
        public BannerCollection(IEnumerable<Banner> collections)
            : base(collections)
        {
        }

        public override int TotalCount
        {
            get { return Size; }
        }
    }
}