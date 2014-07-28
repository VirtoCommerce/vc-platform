using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Virto.Helpers;

namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class CatalogItemSearchModel.
	/// </summary>
    public class CatalogItemSearchModel
    {
		/// <summary>
		/// Gets or sets the criteria.
		/// </summary>
		/// <value>The criteria.</value>
        public CatalogItemSearchCriteria Criteria { get; set; }
		/// <summary>
		/// Gets or sets the catalog items.
		/// </summary>
		/// <value>The catalog items.</value>
        public CatalogItemWithPriceModel[] CatalogItems { get; set; }
		/// <summary>
		/// Gets or sets the suggestions.
		/// </summary>
		/// <value>The suggestions.</value>
        public string[] Suggestions { get; set; }
		/// <summary>
		/// Gets or sets the pager.
		/// </summary>
		/// <value>The pager.</value>
        public PagerModel Pager { get; set; }
		/// <summary>
		/// Gets or sets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
        public SearchParameters Parameters { get; set; }
		/// <summary>
		/// Gets or sets the filter groups.
		/// </summary>
		/// <value>The filter groups.</value>
        public FilterModel[] FilterGroups { get; set; }
		/// <summary>
		/// Gets or sets the selected filters.
		/// </summary>
		/// <value>The selected filters.</value>
        public List<SelectedFilterModel> SelectedFilters { get; set; }
    }

    //[KnownType(typeof(FilterModel))]
    //public class FilterModelList : List<FilterModel>
    //{
        
    //}

	/// <summary>
	/// Class FilterModel.
	/// </summary>
	[DataContract]
    [KnownType(typeof(FacetModel))]
    public class FilterModel
    {
		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>The key.</value>
        [DataMember] 
        public string Key { get; set; }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        [DataMember] 
        public string Name { get; set; }
		/// <summary>
		/// Gets or sets the facets.
		/// </summary>
		/// <value>The facets.</value>
        [DataMember] 
        public FacetModel[] Facets { get; set; }
    }

	/// <summary>
	/// Class FacetModel.
	/// </summary>
    [DataContract]
    public class FacetModel
    {
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		[DataMember]
        public string Name { get; set; }
		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>The key.</value>
        [DataMember]
        public string Key { get; set; }
		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		/// <value>The count.</value>
        [DataMember] 
        public int Count { get; set; }
    }

	/// <summary>
	/// Class SelectedFilterModel.
	/// </summary>
    public class SelectedFilterModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="SelectedFilterModel"/> class.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <param name="facet">The facet.</param>
        public SelectedFilterModel(FilterModel filter, FacetModel facet)
        {
            Filter = filter;
            Facet = facet;
        }

		/// <summary>
		/// Gets or sets the filter.
		/// </summary>
		/// <value>The filter.</value>
        public FilterModel Filter { get; set; }
		/// <summary>
		/// Gets or sets the facet.
		/// </summary>
		/// <value>The facet.</value>
        public FacetModel Facet { get; set; }
    }

	/// <summary>
	/// Class PagerModel.
	/// </summary>
    public class PagerModel
    {
		/// <summary>
		/// Gets or sets the total count.
		/// </summary>
		/// <value>The total count.</value>
        public int TotalCount { get; set; }
		/// <summary>
		/// Gets or sets the current page.
		/// </summary>
		/// <value>The current page.</value>
        public int CurrentPage { get; set; }
		/// <summary>
		/// Gets or sets the records per page.
		/// </summary>
		/// <value>The records per page.</value>
        public int RecordsPerPage { get; set; }
		/// <summary>
		/// Gets or sets the starting record.
		/// </summary>
		/// <value>The starting record.</value>
        public int StartingRecord { get; set; }

		/// <summary>
		/// Gets or sets the display starting record.
		/// </summary>
		/// <value>The display starting record.</value>
        public int DisplayStartingRecord { get; set; }
		/// <summary>
		/// Gets or sets the display ending record.
		/// </summary>
		/// <value>The display ending record.</value>
        public int DisplayEndingRecord { get; set; }
		/// <summary>
		/// Gets or sets the sort values.
		/// </summary>
		/// <value>The sort values.</value>
        public string[] SortValues { get; set; }
		/// <summary>
		/// Gets or sets the selected sort.
		/// </summary>
		/// <value>The selected sort.</value>
        public string SelectedSort { get; set; }

        /// <summary>
        /// Gets or sets the selected sort order.
        /// </summary>
        /// <value>The selected sort order.</value>
        public string SortOrder { get; set; }
    }

	/// <summary>
	/// Enum PriceType
	/// </summary>
    public enum PriceType
    {
		/// <summary>
		/// The none
		/// </summary>
        None,
		/// <summary>
		/// The regular
		/// </summary>
        Regular,
		/// <summary>
		/// The sale
		/// </summary>
        Sale
    }

	/// <summary>
	/// Class FormattedPriceModel.
	/// </summary>
    public class FormattedPriceModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="FormattedPriceModel"/> class.
		/// </summary>
		/// <param name="price">The price.</param>
		/// <param name="currency">The currency.</param>
        public FormattedPriceModel(Price price, string currency)
        {
            Price = price;
            Currency = currency;
        }

		/// <summary>
		/// Gets or sets the price.
		/// </summary>
		/// <value>The price.</value>
        public Price Price { get; set; }

		/// <summary>
		/// Gets or sets the currency.
		/// </summary>
		/// <value>The currency.</value>
        public string Currency { get; set; }

		/// <summary>
		/// Gets the list price formatted.
		/// </summary>
		/// <value>The list price formatted.</value>
        public string ListPriceFormatted
        {
            get { return Price == null ? String.Empty : StoreHelper.FormatCurrency(Price.List, Currency); }
        }

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
        public PriceType Type
        {
            get
            {
                if (Price != null && Price.Sale != null && Price.Sale < Price.List)
                {
                    return PriceType.Sale;
                }

                return PriceType.Regular;
            }
        }

		/// <summary>
		/// Gets the sale price formatted.
		/// </summary>
		/// <value>The sale price formatted.</value>
        public string SalePriceFormatted
        {
            get
            {
                if (Price == null || Price.Sale == null)
                {
                    return String.Empty;
                }

                return StoreHelper.FormatCurrency((decimal)Price.Sale, Currency);
            }
        }
    }

	/// <summary>
	/// Class AssociatedCatalogItemWithPriceModel.
	/// </summary>
    public class AssociatedCatalogItemWithPriceModel : CatalogItemWithPriceModel
    {
		/// <summary>
		/// The _association type
		/// </summary>
        private readonly string _associationType;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssociatedCatalogItemWithPriceModel"/> class.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="price">The price.</param>
		/// <param name="availability">The availability.</param>
		/// <param name="associationType">Type of the association.</param>
        public AssociatedCatalogItemWithPriceModel(ItemModel item, PriceModel price, ItemAvailabilityModel availability,
                                                   string associationType) :
            base(item, price, availability)
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

	/// <summary>
	/// Class CatalogItemWithPriceModel.
	/// </summary>
    public class CatalogItemWithPriceModel
    {
		/// <summary>
		/// The _availability
		/// </summary>
        private readonly ItemAvailabilityModel _availability;
		/// <summary>
		/// The _item
		/// </summary>
        private readonly ItemModel _item;
		/// <summary>
		/// The _price
		/// </summary>
        private readonly PriceModel _price;

	    private readonly ReviewTotals _itemReviews;

		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogItemWithPriceModel"/> class.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="price">The price.</param>
		/// <param name="availability">The availability.</param>
        public CatalogItemWithPriceModel(ItemModel item, PriceModel price, ItemAvailabilityModel availability)
        {
            _item = item;
            _price = price;
            _availability = availability;
		    _itemReviews = new ReviewTotals {ItemId = item.ItemId};
        }

		/// <summary>
		/// Gets the catalog item.
		/// </summary>
		/// <value>The catalog item.</value>
        public ItemModel CatalogItem
        {
            get { return _item; }
        }

		/// <summary>
		/// Gets the price.
		/// </summary>
		/// <value>The price.</value>
        public PriceModel Price
        {
            get { return _price; }
        }

		/// <summary>
		/// Gets the availability.
		/// </summary>
		/// <value>The availability.</value>
        public ItemAvailabilityModel Availability
        {
            get { return _availability; }
        }

		/// <summary>
		/// Gets the display name.
		/// </summary>
		/// <value>The display name.</value>
        public string DisplayName
        {
            get { return CatalogItem.Item.DisplayName(); }
        }

	    public bool IsNew
	    {
	        get
	        {
	            return CatalogItem.Item.StartDate.AddMonths(1) >= DateTime.UtcNow;
	        }
	    }

	    public bool IsSale
	    {
	        get
	        {
	            return Price != null && Price.Type == PriceType.Sale;
	        }
	    }

	    public string SearchOutline { get; set; }

	    public ReviewTotals ItemReviewTotals
	    {
	        get { return _itemReviews; }
	    }

	    /// <summary>
		/// Gets the <see cref="System.String"/> with the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>System.String.</returns>
        public string this[string name]
        {
            get
            {
                var firstOrDefault = _item.GetType().GetProperties().FirstOrDefault(x => x.Name == name);
                return firstOrDefault != null ? firstOrDefault.ToString() : null;
            }
        }
    }

	/// <summary>
	/// Class ItemAvailabilityModel.
	/// </summary>
    public class ItemAvailabilityModel
    {
		/// <summary>
		/// The _availability
		/// </summary>
        private ItemAvailability _availability;

		/// <summary>
		/// Initializes a new instance of the <see cref="ItemAvailabilityModel"/> class.
		/// </summary>
		/// <param name="availability">The availability.</param>
        public ItemAvailabilityModel(ItemAvailability availability)
        {
            _availability = availability;
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
                switch (_availability.Availability)
                {
                    case ItemStoreAvailabity.InStore:
						availabilityString = _availability.MaxQuantity > 5 ? "In Stock".Localize() : string.Format("Only {0} left in stock".Localize(), ((int)_availability.MaxQuantity));
                        break;
                    case ItemStoreAvailabity.OutOfStore:
						availabilityString = "Out Of Stock".Localize();
                        break;
                    case ItemStoreAvailabity.AvailableForBackOrder :
                        if (_availability.Date != null && _availability.Date.Value > DateTime.Now)
                        {
							availabilityString = string.Format("Available on {0} for Back order".Localize(), _availability.Date.Value.ToLongDateString());
                        }
                        else
                        {
							availabilityString = "Back Ordered".Localize();
                        }
                        break;
					case ItemStoreAvailabity.AvailableForPreOrder:
                        if (_availability.Date != null && _availability.Date.Value > DateTime.Now)
                        {
							availabilityString = string.Format("Available on {0} for Pre order".Localize(), _availability.Date.Value.ToLongDateString());
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

		/// <summary>
		/// Gets the minimum quantity.
		/// </summary>
		/// <value>The minimum quantity.</value>
		public int MinQuantity
		{
			get { return (int)_availability.MinQuantity; }
		}

		/// <summary>
		/// Gets the maximum quantity.
		/// </summary>
		/// <value>The maximum quantity.</value>
        public int MaxQuantity
        {
            get { return (int)_availability.MaxQuantity; }
        }

		/// <summary>
		/// Gets a value indicating whether this instance is available.
		/// </summary>
		/// <value><c>true</c> if this instance is available; otherwise, <c>false</c>.</value>
        public bool IsAvailable
        {
            get { return _availability.IsAvailable; }
        }
    }

	/// <summary>
	/// Class SearchParameters.
	/// </summary>
    public class SearchParameters
    {
		/// <summary>
		/// The default page size
		/// </summary>
        public const int DefaultPageSize = 8;

		/// <summary>
		/// Initializes a new instance of the <see cref="SearchParameters"/> class.
		/// </summary>
        public SearchParameters()
        {
            Facets = new Dictionary<string, string[]>();
            PageSize = 0;
            PageIndex = 1;
        }

		/// <summary>
		/// Gets or sets the free search.
		/// </summary>
		/// <value>The free search.</value>
        public string FreeSearch { get; set; }
		/// <summary>
		/// Gets or sets the index of the page.
		/// </summary>
		/// <value>The index of the page.</value>
        public int PageIndex { get; set; }
		/// <summary>
		/// Gets or sets the size of the page.
		/// </summary>
		/// <value>The size of the page.</value>
        public int PageSize { get; set; }
		/// <summary>
		/// Gets or sets the facets.
		/// </summary>
		/// <value>The facets.</value>
        public IDictionary<string, string[]> Facets { get; set; }
		/// <summary>
		/// Gets or sets the sort.
		/// </summary>
		/// <value>The sort.</value>
        public string Sort { get; set; }

        /// <summary>
        /// Gets or sets the sort order asc or desc.
        /// </summary>
        /// <value>True if descending, false if ascending</value>
        public string SortOrder { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append(FreeSearch);
			builder.Append(PageIndex);
			builder.Append(PageSize);
			builder.Append(Sort);

			foreach (var facet in Facets)
			{
				builder.Append(facet);
			}

			return builder.ToString();
		}
    }

	/// <summary>
	/// Enum ItemDisplayOptions
	/// </summary>
	[Flags]
	public enum ItemDisplayOptions
	{
		/// <summary>
		/// The item only
		/// </summary>
		ItemOnly = 0,
		/// <summary>
		/// The item price
		/// </summary>
		ItemPrice = 1 << 1,
		/// <summary>
		/// The item availability
		/// </summary>
		ItemAvailability = 1 << 2,
		/// <summary>
		/// The item property sets
		/// </summary>
		ItemPropertySets = 1 << 3,
		/// <summary>
		/// The item small
		/// </summary>
		ItemSmall = ItemOnly | ItemPrice,
		/// <summary>
		/// The item medium
		/// </summary>
		ItemMedium = ItemOnly | ItemPrice | ItemAvailability,
		/// <summary>
		/// The item large
		/// </summary>
		ItemLarge = ItemOnly | ItemPrice | ItemAvailability | ItemPropertySets
	}
}