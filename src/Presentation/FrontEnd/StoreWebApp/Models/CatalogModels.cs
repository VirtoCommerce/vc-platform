using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Web.Virto.Helpers;

namespace VirtoCommerce.Web.Models
{
    using VirtoCommerce.Foundation.Catalogs.Services;

    /// <summary>
	/// Class ItemModel.
	/// </summary>
    public class ItemModel
    {
		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
	    public Item Item { get; set; }

		/// <summary>
		/// Gets or sets the item identifier.
		/// </summary>
		/// <value>The item identifier.</value>
        public string ItemId
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the CategoryName.
        /// </summary>
        /// <value>The name.</value>
        public string CategoryName
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the start date.
		/// </summary>
		/// <value>The start date.</value>
        public DateTime StartDate
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the end date.
		/// </summary>
		/// <value>The end date.</value>
        public DateTime? EndDate
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is active.
		/// </summary>
		/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is buyable.
		/// </summary>
		/// <value><c>true</c> if this instance is buyable; otherwise, <c>false</c>.</value>
        public bool IsBuyable
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether [track inventory].
		/// </summary>
		/// <value><c>true</c> if [track inventory]; otherwise, <c>false</c>.</value>
        public bool TrackInventory
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets the weight.
		/// </summary>
		/// <value>The weight.</value>
        public decimal Weight
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the type of the package.
		/// </summary>
		/// <value>The type of the package.</value>
        public string PackageType
        {
            get;set;
        }

		/// <summary>
		/// Gets or sets the tax category.
		/// </summary>
		/// <value>The tax category.</value>
        public string TaxCategory
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>The code.</value>
        public string Code
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the parent item identifier.
		/// </summary>
		/// <value>The parent item identifier.</value>
		public string ParentItemId { get; set; }

		/// <summary>
		/// Gets or sets the item assets.
		/// </summary>
		/// <value>The item assets.</value>
        public ItemAsset[] ItemAssets { get; set; }
		/// <summary>
		/// Gets or sets the editorial reviews.
		/// </summary>
		/// <value>The editorial reviews.</value>
        public EditorialReview[] EditorialReviews { get; set; }
		/// <summary>
		/// Gets or sets the association groups.
		/// </summary>
		/// <value>The association groups.</value>
        public AssociationGroup[] AssociationGroups { get; set; }

		/// <summary>
		/// Gets or sets the properties.
		/// </summary>
		/// <value>The properties.</value>
        public PropertiesModel Properties { get; set; }

        public CatalogOutlines CatalogOutlines { get; set; }
    }

	/// <summary>
	/// Class PropertySetModel.
	/// </summary>
    public class PropertySetModel
    {
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }
		/// <summary>
		/// Gets or sets the properties.
		/// </summary>
		/// <value>The properties.</value>
        public PropertyModel[] Properties { get; set; }
    }

	/// <summary>
	/// Class PropertiesModel.
	/// </summary>
    public class PropertiesModel : List<PropertyModel>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="PropertiesModel"/> class.
		/// </summary>
        public PropertiesModel()
        {
            
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertiesModel"/> class.
		/// </summary>
		/// <param name="properties">The properties.</param>
        public PropertiesModel(IEnumerable<PropertyModel> properties)
        {
            AddRange(properties);
        }
    }

	/// <summary>
	/// Class PropertyModel.
	/// </summary>
    public class PropertyModel
    {
		/// <summary>
		/// Gets or sets the priority.
		/// </summary>
		/// <value>The priority.</value>
        public int Priority
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is key.
		/// </summary>
		/// <value><c>true</c> if this instance is key; otherwise, <c>false</c>.</value>
        public bool IsKey
        {
            get; set; 
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is sale.
		/// </summary>
		/// <value><c>true</c> if this instance is sale; otherwise, <c>false</c>.</value>
        public bool IsSale
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is enum.
		/// </summary>
		/// <value><c>true</c> if this instance is enum; otherwise, <c>false</c>.</value>
        public bool IsEnum
        {
            get; set; 
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is input.
		/// </summary>
		/// <value><c>true</c> if this instance is input; otherwise, <c>false</c>.</value>
        public bool IsInput
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is required.
		/// </summary>
		/// <value><c>true</c> if this instance is required; otherwise, <c>false</c>.</value>
        public bool IsRequired
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is multi value.
		/// </summary>
		/// <value><c>true</c> if this instance is multi value; otherwise, <c>false</c>.</value>
        public bool IsMultiValue
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is locale dependant.
		/// </summary>
		/// <value><c>true</c> if this instance is locale dependant; otherwise, <c>false</c>.</value>
        public bool IsLocaleDependant
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether [allow alias].
		/// </summary>
		/// <value><c>true</c> if [allow alias]; otherwise, <c>false</c>.</value>
        public bool AllowAlias
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the attributes.
		/// </summary>
		/// <value>The attributes.</value>
        public PropertyAttributeModel[] Attributes { get; set; }
		/// <summary>
		/// Gets or sets the values.
		/// </summary>
		/// <value>The values.</value>
        public PropertyValueModel[] Values { get; set; }
		/// <summary>
		/// Gets or sets the catalog item.
		/// </summary>
		/// <value>The catalog item.</value>
		public StorageEntity CatalogItem { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
	        var retValue = string.Empty;

            if (Values != null && Values.Length > 0)
            {
				retValue = Values[0].ToString();

				if (IsLocaleDependant)
				{
					var val = Values.FirstOrDefault(
						v =>
						string.IsNullOrEmpty(v.Locale) ||
						CultureInfo.CurrentUICulture.Name.Equals(v.Locale, StringComparison.InvariantCultureIgnoreCase));
					if (val != null)
					{
						retValue = val.ToString();
					}
				}
            }

	        return retValue;
        }
    }

	/// <summary>
	/// Class PropertyAttributeModel.
	/// </summary>
    public class PropertyAttributeModel
    {
    }

	/// <summary>
	/// Class PropertyValueModel.
	/// </summary>
    public class PropertyValueModel
    {
		/// <summary>
		/// Gets or sets the alias.
		/// </summary>
		/// <value>The alias.</value>
        public string Alias
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name
        {
            get; set; 
        }

		/// <summary>
		/// Gets or sets the key value.
		/// </summary>
		/// <value>The key value.</value>
        public string KeyValue
        {
            get;set;
        }

		/// <summary>
		/// Gets or sets the type of the value.
		/// </summary>
		/// <value>The type of the value.</value>
        public int ValueType
        {
            get;set;
        }

		/// <summary>
		/// Gets or sets the short text value.
		/// </summary>
		/// <value>The short text value.</value>
        public string ShortTextValue
        {
            get; set; 
        }

		/// <summary>
		/// Gets or sets the long text value.
		/// </summary>
		/// <value>The long text value.</value>
        public string LongTextValue
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets the decimal value.
		/// </summary>
		/// <value>The decimal value.</value>
        public decimal DecimalValue
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets the integer value.
		/// </summary>
		/// <value>The integer value.</value>
        public int IntegerValue
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether [boolean value].
		/// </summary>
		/// <value><c>true</c> if [boolean value]; otherwise, <c>false</c>.</value>
        public bool BooleanValue
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets the date time value.
		/// </summary>
		/// <value>The date time value.</value>
        public DateTime? DateTimeValue
        {
            get; set; 
        }

		/// <summary>
		/// Gets or sets the locale.
		/// </summary>
		/// <value>The locale.</value>
        public string Locale
        {
            get; set;
        }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            switch (ValueType)
            {
                case (int)PropertyValueType.Boolean:
                    return BooleanValue.ToString();
                case (int)PropertyValueType.DateTime:
                    return DateTimeValue.ToString();
                case (int)PropertyValueType.Decimal:
                    return DecimalValue.ToString(CultureInfo.InvariantCulture);
                case (int)PropertyValueType.Integer:
                    return IntegerValue.ToString(CultureInfo.InvariantCulture);
                case (int)PropertyValueType.LongString:
                    return LongTextValue;
                case (int)PropertyValueType.ShortString:
                    return ShortTextValue;
            }
            return base.ToString();
        }
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

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [KnownType(typeof(CategoryModel))]
    public abstract class CategoryBaseModel
    {
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [DataMember]
        public CategoryBase Category { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        [DataMember]
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is active].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is active]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        [DataMember]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the catalog identifier.
        /// </summary>
        /// <value>
        /// The catalog identifier.
        /// </value>
        [DataMember]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the parent category identifier.
        /// </summary>
        /// <value>
        /// The parent category identifier.
        /// </value>
        [DataMember]
        public string ParentCategoryId { get; set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName
        {
            get
            {
                var retValue = Category is Category ? ((Category)Category).Name : Code;
                var title = CatalogHelper.CatalogClient.GetPropertyValueByName(Category, "Title");
                if (title != null)
                {
                    retValue = title.ToString();
                }
                return retValue;
            }
        }

        public virtual CatalogBase Catalog { get; set; }
        public virtual CategoryBase ParentCategory { get; set; }
        public virtual LinkedCategory[] LinkedCategories { get; set; }
        public CatalogOutline CatalogOutline { get; set; }
    }

    [DataContract]
    public class CategoryModel : CategoryBaseModel
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [DataMember]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the property set identifier.
        /// </summary>
        /// <value>
        /// The property set identifier.
        /// </value>
        [DataMember] 
        public string PropertySetId { get; set; }

        /// <summary>
        /// Gets or sets the property set.
        /// </summary>
        /// <value>
        /// The property set.
        /// </value>
        [DataMember]
        public PropertySet PropertySet { get; set; }

        /// <summary>
        /// Gets or sets the category property values.
        /// </summary>
        /// <value>
        /// The category property values.
        /// </value>
        [DataMember]
        public CategoryPropertyValue[] CategoryPropertyValues { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        [DataMember]
        public PropertiesModel Properties { get; set; } 

    }
}