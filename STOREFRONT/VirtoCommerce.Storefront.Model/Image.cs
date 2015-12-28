using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class Image : ValueObject<Image>
    {
        /// <summary>
        /// Full url of image
        /// </summary>
        public string Url { get; set; }

        public string FullSizeImageUrl { get; set; }

        /// <summary>
        /// Image title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Image alt text
        /// </summary>
        public string Alt { get; set; }

      
    }
}
