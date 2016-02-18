using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotLiquid;
using DotLiquid.Exceptions;

namespace VirtoCommerce.LiquidThemeEngine.Tags
{
    public class PoweredByTag : Block
    {
        private readonly string[] _poweredLinks = {
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">.NET ecommerce platform</a> by Virto",
                                             "<a href=\"http://virtocommerce.com/shopping-cart\" rel=\"nofollow\" target=\"_blank\">Shopping Cart</a> by Virto",
                                             "<a href=\"http://virtocommerce.com/shopping-cart\" rel=\"nofollow\" target=\"_blank\">.NET Shopping Cart</a> by Virto",
                                             "<a href=\"http://virtocommerce.com/shopping-cart\" rel=\"nofollow\" target=\"_blank\">ASP.NET Shopping Cart</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">.NET ecommerce</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">.NET ecommerce framework</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">ASP.NET ecommerce</a> by Virto Commerce",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">ASP.NET ecommerce platform</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">ASP.NET ecommerce framework</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">Enterprise ecommerce</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">Enterprise ecommerce platform</a> by Virto",
                                         };
 
        #region Public Methods and Operators
 
        public override void Render(Context context, TextWriter result)
        {
            var themeEngine = (ShopifyLiquidThemeEngine)Template.FileSystem;
            var storeName = themeEngine.WorkContext.CurrentStore.Name;
            var hashCode = (uint)storeName.GetHashCode();

            result.WriteLine(_poweredLinks[hashCode % _poweredLinks.Length]);
        }

        protected override void AssertMissingDelimitation()
        {
        }
        #endregion
    }
}
