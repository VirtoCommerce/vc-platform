using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace VirtoCommerce.Web.Core.DataContracts
{
    using System.Collections;
    using System.Collections.Generic;

    public class ResponseCollection<T>
    {
        [JsonProperty("total")]
        public int TotalCount { get; set; }

        [JsonProperty("items")]
        public Collection<T> Items { get { return _items ?? (_items = new Collection<T>()); } }
        private Collection<T> _items;
    }
}
