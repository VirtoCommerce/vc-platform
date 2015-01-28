using System;
using System.Linq;
using VirtoCommerce.ApiClient.DataContracts;
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
            Price = new PriceModel();
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
                if (CatalogItem.Properties.ContainsKey("Title"))
                {
                    retVal = CatalogItem["Title"].ToString();
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

     
    }

    public class AssociatedItemModel : ItemModel
    {
        private readonly string _associationType;


        public AssociatedItemModel(CatalogItem catalogItem, string associationType) : base(catalogItem)
        {
            _associationType = associationType;
        }

        /// <summary>
        /// Gets the type of the association.
        /// </summary>
        /// <value>The type of the association.</value>
        public string AssociationType
        {
            get { return _associationType; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is required.
        /// </summary>
        /// <value><c>true</c> if this instance is required; otherwise, <c>false</c>.</value>
        public bool IsRequired
        {
            get { return string.Equals(_associationType, AssociationTypes.required.ToString(), StringComparison.OrdinalIgnoreCase); }
        }
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