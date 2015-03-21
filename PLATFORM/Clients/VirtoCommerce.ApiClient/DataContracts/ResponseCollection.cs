using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace VirtoCommerce.ApiClient.DataContracts
{
    public class ResponseCollection<T>
    {
        [JsonProperty("total")]
        public int TotalCount { get; set; }

        [JsonProperty("items")]
        public Collection<T> Items { get { return _items ?? (_items = new Collection<T>()); } }
        private Collection<T> _items;
    }
}
