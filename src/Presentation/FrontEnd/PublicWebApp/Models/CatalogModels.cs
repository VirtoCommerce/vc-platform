using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ApiWebClient.Globalization;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.Web.Models
{
    public class ItemModel
    {
        public ItemModel(CatalogItem catalogItem)
        {
            CatalogItem = catalogItem;
            Availability = new ItemAvailabilityModel();
            DisplayTemplate = "Item";
        }
        public CatalogItem CatalogItem { get; set; }

        public Category Category { get; set; }

        public string CategoryId
        {
            get
            {
                return !string.IsNullOrEmpty(CatalogItem.Outline)
                    ? CatalogItem.Outline.Split("/".ToCharArray()).Last()
                    : null;
            }
        }

        public PriceModel Price { get; set; }

        public string DisplayName
        {
            get
            {
                var retVal = CatalogItem.Name;
                if (Properties.ContainsKey("Title"))
                {
                    retVal = this["Title"][0];
                }

                return retVal;
            }
        }

        public string DisplayTemplate { get; set; }

        public bool IsNew
        {
            get
            {
                return CatalogItem.StartDate.AddMonths(1) >= DateTime.UtcNow;
            }
        }

        public bool IsSale
        {
            get
            {
                return Price != null && Price.Type == PriceType.Sale;
            }
        }

        public ItemAvailabilityModel Availability { get; set; }

        #region Properties

        private IDictionary<string, string[]> _properties = new Dictionary<string, string[]>();

        public string[] this[string name]
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

        public IDictionary<string, string[]> Properties
        {
            get
            {
                return _properties;
            }
            set { _properties = value; }
        }

        #endregion
    }

    public class EditorialReviewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string ReviewType { get; set; }

    }

    public class CategoryPathModel
    {
        private string _url;

        public string Url
        {
            get { return _url; }
            set
            {
                Category = value.Split(new[] { '/' }).Last();
                _url = value;
            }
        }

        public string Category { get; set; }
    }

    public enum ItemStoreAvailabity
    {
        OutOfStore = 0,
        InStore,
        AvailableForBackOrder,
        AvailableForPreOrder
    }

    public class ItemAvailabilityModel
    {

        public DateTime? Date { get; set; }
        public ItemStoreAvailabity Availability { get; set; }
        public int MaxQuantity { get; set; }
        public int MinQuantity { get; set; }
        public string ItemId { get; set; }

        public bool IsAvailable
        {
            get
            {
                switch (Availability)
                {
                    case ItemStoreAvailabity.OutOfStore:
                        return false;
                    case ItemStoreAvailabity.AvailableForBackOrder:
                    case ItemStoreAvailabity.AvailableForPreOrder:
                        if (!Date.HasValue || Date.Value > DateTime.Now)
                        {
                            return false;
                        }
                        break;
                }
                return true;
            }
        }


        /// <summary>
        /// Gets the availability string.
        /// </summary>
        /// <value>The availability string.</value>
        public string AvailabilityString
        {
            get
            {
                var availabilityString = "";
                switch (Availability)
                {
                    case ItemStoreAvailabity.InStore:
                        availabilityString = MaxQuantity > 5 ? "In Stock".Localize() : string.Format("Only {0} left in stock".Localize(), ((int)MaxQuantity));
                        break;
                    case ItemStoreAvailabity.OutOfStore:
                        availabilityString = "Out Of Stock".Localize();
                        break;
                    case ItemStoreAvailabity.AvailableForBackOrder:
                        if (Date != null && Date.Value > DateTime.Now)
                        {
                            availabilityString = string.Format("Available on {0} for Back order".Localize(), Date.Value.ToLongDateString());
                        }
                        else
                        {
                            availabilityString = "Back Ordered".Localize();
                        }
                        break;
                    case ItemStoreAvailabity.AvailableForPreOrder:
                        if (Date != null && Date.Value > DateTime.Now)
                        {
                            availabilityString = string.Format("Available on {0} for Pre order".Localize(), Date.Value.ToLongDateString());
                        }
                        else
                        {
                            availabilityString = "Available for Preorder".Localize();
                        }
                        break;
                }

                return availabilityString;
            }
        }
    }
}