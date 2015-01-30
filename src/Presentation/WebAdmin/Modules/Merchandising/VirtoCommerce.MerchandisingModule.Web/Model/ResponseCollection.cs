using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ResponseCollection<T>
    {
		public ResponseCollection()
		{
			Items = new Collection<T>();
		}

        [JsonProperty("total")]
        public int TotalCount { get; set; }

		[JsonProperty("items")]
		public ICollection<T> Items { get; set; }
        
    }
}
