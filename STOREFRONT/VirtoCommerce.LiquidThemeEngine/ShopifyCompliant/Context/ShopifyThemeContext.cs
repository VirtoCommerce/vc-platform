using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.ShopifyCompliant.Context
{
    public class ShopifyThemeContext : WorkContext
    {
        public Settings Settings { get; set; }
    }
}
