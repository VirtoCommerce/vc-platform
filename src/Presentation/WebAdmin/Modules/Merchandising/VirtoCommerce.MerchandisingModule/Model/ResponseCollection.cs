using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VirtoCommerce.MerchandisingModule.Model
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
