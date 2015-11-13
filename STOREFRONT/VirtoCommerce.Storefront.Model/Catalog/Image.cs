using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class Image : Entity
    {
        /// <summary>
        /// Relative url of image
        /// </summary>
        public string RelativeUrl { get; set; }

        /// <summary>
        /// Full url of image
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Asset type identifier
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// Asset group name
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Asset name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Asset language code
        /// </summary>
        public string LanguageCode { get; set; }
    }
}
