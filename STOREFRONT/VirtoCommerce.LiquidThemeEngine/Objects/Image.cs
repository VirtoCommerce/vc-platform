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
        private readonly Storefront.Model.Image _image;

        public Image(Storefront.Model.Image image)
        {
            _image = image;
        }

        public string Alt
        {
            get
            {
                return _image.AlternateText;
            }
        }

        public bool? AttachedToVariant
        {
            get
            {
                //TODO no info about it
                return true;
            }
        }

        public string ProductId
        {
            get
            {
                return "";
            }
        }

        public int Position
        {
            get
            {
                //TODO no info about it
                return 0;
            }
        }

        public string Src
        {
            get
            {
                return _image.Url;
            }
        }

        public string Name
        {
            get
            {
                return _image.Title;
            }
        }

        public IEnumerable<Variant> Variants
        {
            get
            {
                //TODO no info
                return null;
            }
        }
    }
}
