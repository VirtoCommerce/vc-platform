using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public abstract class ResponseCollection<T>
    {
        public ResponseCollection()
        {
            Items = new Collection<T>();
        }

        /// <summary>
        /// Gets or sets the collection of reposponse items
        /// </summary>
        [JsonProperty("items")]
        public ICollection<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the value of response items total count
        /// </summary>
        [JsonProperty("total")]
        public int TotalCount { get; set; }
    }
}
