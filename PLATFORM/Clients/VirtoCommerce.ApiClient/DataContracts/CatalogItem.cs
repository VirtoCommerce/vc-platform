using System.Collections.Generic;
using Newtonsoft.Json;

namespace VirtoCommerce.ApiClient.DataContracts
{
    public class CatalogItem : Resource
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public ItemImage[] Images { get; set; }

        private IDictionary<string, object> _properties = new Dictionary<string, object>();
        
        [JsonIgnore]
        public object this[string name]
        {
            get
            {
                return _properties[name];
            }
            set
            {
                if (_properties.ContainsKey(name))
                {
                    _properties[name] = value;
                }
                else
                {
                    _properties.Add(name, value);
                }
            }
        }

        public IDictionary<string, object> Properties
        {
            get
            {
                return _properties;
            }
            set { _properties = value; }
        }

        public string Catalog { get; set; }
    }
}
