#region

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts
{
    public class CatalogItem : Resource
    {
        #region Fields

        private IDictionary<string, object> _properties = new Dictionary<string, object>();

        #endregion

        #region Public Properties

        public Association[] Associations { get; set; }

        public string CatalogId { get; set; }

        public string[] Categories { get; set; }

        public string Code { get; set; }

        public EditorialReview[] EditorialReviews { get; set; }

        public string Id { get; set; }

        public ItemImage[] Images { get; set; }

        public string MainProductId { get; set; }

        public string Name { get; set; }

        public bool? TrackInventory { get; set; }

        public bool? IsBuyable { get; set; }

        public bool? IsActive { get; set; }


        public string Outline { get; set; }

        public IDictionary<string, object> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        public double Rating { get; set; }

        public int ReviewsTotal { get; set; }

        public SeoKeyword[] Seo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? MaxQuantity { get; set; }

        public int? MinQuantity { get; set; }

        #endregion

        #region Public Indexers

        [JsonIgnore]
        public object this[string name]
        {
            get { return _properties[name]; }
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

        #endregion
    }
}
