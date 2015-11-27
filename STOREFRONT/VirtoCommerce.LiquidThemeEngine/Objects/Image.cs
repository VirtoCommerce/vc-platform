using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/product#product-image
    /// </summary>
    public class Image : Drop
    {
      
        public string Alt { get; set; }

        public bool? AttachedToVariant { get; set; }

        public string ProductId { get; set; }

        public int Position { get; set; }

        public string Src { get; set; }

        public string Name { get; set; }

        public Variant[] Variants { get; set; }
    }
}
