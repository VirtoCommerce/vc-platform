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
        private IDictionary<string, object> _variationProperties = new Dictionary<string, object>();

        #endregion

        #region Public Properties

        public string ManufacturerPartNumber { get; set; }

        public string Gtin { get; set; }

        public Association[] Associations { get; set; }

        public string CatalogId { get; set; }

        public string[] Categories { get; set; }

        public string Code { get; set; }

        public EditorialReview[] EditorialReviews { get; set; }

        public string Id { get; set; }

        public ItemImage[] Images { get; set; }

        public string MainProductId { get; set; }

        public bool? TrackInventory { get; set; }

        public bool? IsBuyable { get; set; }

        public bool? IsActive { get; set; }

        public int? MaxQuantity { get; set; }
        public int? MinQuantity { get; set; }

        public string Name { get; set; }

        public string Outline { get; set; }

        public IDictionary<string, object> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        public IDictionary<string, object> VariationProperties
        {
            get { return _variationProperties; }
            set { _variationProperties = value; }
        }

        public double Rating { get; set; }

        public int ReviewsTotal { get; set; }

        public SeoKeyword[] Seo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ProductType { get; set; }

        public string WeightUnit { get; set; }

        public decimal? Weight { get; set; }

        public string MeasureUnit { get; set; }

        public decimal? Height { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public bool? EnableReview { get; set; }

        public int? MaxNumberOfDownload { get; set; }

        public DateTime? DownloadExpiration { get; set; }

        public string DownloadType { get; set; }

        public bool? HasUserAgreement { get; set; }

        public string ShippingType { get; set; }

        public string TaxType { get; set; }

        public string Vendor { get; set; }

        public Asset[] Assets { get; set; }
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
