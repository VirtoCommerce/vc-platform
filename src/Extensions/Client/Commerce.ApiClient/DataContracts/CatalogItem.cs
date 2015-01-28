using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Core.DataContracts
{
    public class CatalogItem : Resource
    {
        public string Id { get; set; }

        public string CatalogId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public ItemImage[] Images { get; set; }

        public EditorialReview[] EditorialReviews { get; set; }

        public Association[] Associations { get; set; }

        public int ReviewsTotal { get; set; }

        public double Rating { get; set; }

        public string[] Categories { get; set; }

        public string Outline { get; set; }

        public SeoKeyword[] SeoKeywords { get; set; }

        public DateTime StartDate { get; set; }

        public string MainProductId { get; set; }

        #region Properties

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

        #endregion
    }
}
