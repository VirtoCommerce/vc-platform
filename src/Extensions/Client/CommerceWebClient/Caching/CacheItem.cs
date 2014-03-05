using System;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Client.Caching
{
    [Serializable, DataContract]
    public class CacheItem
    {
        /// <summary>
        /// Gets or sets content type.
        /// </summary>
        /// <value>
        /// The content type.
        /// </value>
        [DataMember(Order = 1)]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content to be cached.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [DataMember(Order = 2)]
        public string Content { get; set; }
    }
}
