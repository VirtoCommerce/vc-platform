using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class Asset : Entity
    {
        /// <summary>
        /// Size of asset in bytes
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// Mime type of asset
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Relative url of asset
        /// </summary>
        public string RelativeUrl { get; set; }

        /// <summary>
        /// Full url of asset
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
