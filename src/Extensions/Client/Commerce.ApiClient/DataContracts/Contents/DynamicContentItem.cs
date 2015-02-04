using System.Collections.Generic;
using Newtonsoft.Json;

namespace VirtoCommerce.ApiClient.DataContracts.Contents
{
    public class DynamicContentItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ContentType
        {
            get; set; 
        }

        public bool IsMultilingual
        {
            get; set; 
        }

        private IDictionary<string, string> _properties = new Dictionary<string, string>();

        [JsonIgnore]
        public string this[string name]
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

        public IDictionary<string, string> Properties
        {
            get
            {
                return _properties;
            }
            set { _properties = value; }
        }
    }
}
