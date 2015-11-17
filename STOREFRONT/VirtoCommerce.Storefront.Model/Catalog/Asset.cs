using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class Asset : ValueObject<Asset>
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

    }
}
