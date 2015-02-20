#region

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts
{

    #region

    #endregion

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

        public string Outline { get; set; }

        public IDictionary<string, object> Properties
        {
            get
            {
                return this._properties;
            }
            set
            {
                this._properties = value;
            }
        }

        public double Rating { get; set; }

        public int ReviewsTotal { get; set; }

        public SeoKeyword[] SeoKeywords { get; set; }

        public DateTime StartDate { get; set; }

        #endregion

        #region Public Indexers

        [JsonIgnore]
        public object this[string name]
        {
            get
            {
                return this._properties[name];
            }
            set
            {
                if (this._properties.ContainsKey(name))
                {
                    this._properties[name] = value;
                }
                else
                {
                    this._properties.Add(name, value);
                }
            }
        }

        #endregion
    }
}
