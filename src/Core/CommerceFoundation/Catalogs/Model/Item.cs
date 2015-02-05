using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
    [DataContract]
    [EntitySet("Items")]
    [DataServiceKey("ItemId")]
    [KnownType(typeof(Sku))]
    [KnownType(typeof(Bundle))]
    [KnownType(typeof(Product))]
    [KnownType(typeof(Package))]
    [KnownType(typeof(DynamicKit))]
    public abstract class Item : StorageEntity
    {
        public Item()
        {
            _ItemId = GenerateNewKey();
        }

        private string _ItemId;
        [Key]
		[StringLength(128)]
        [DataMember]
        public string ItemId
        {
            get
            {
                return _ItemId;
            }
            set
            {
                SetValue(ref _ItemId, () => this.ItemId, value);
            }
        }

        private string _Name;
		[StringLength(1024)]
        [Required(ErrorMessage = "Field 'Name' is required.")]
        [DataMember]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetValue(ref _Name, () => this.Name, value);
            }
        }

        private DateTime _StartDate;
        [DataMember]
        public DateTime StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                SetValue(ref _StartDate, () => this.StartDate, value);
            }
        }

        private DateTime? _EndDate;
        [DataMember]
        public DateTime? EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                SetValue(ref _EndDate, () => this.EndDate, value);
            }
        }

        private bool _IsActive;
        [DataMember]
        [Required]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                SetValue(ref _IsActive, () => this.IsActive, value);
            }
        }

        private bool _IsBuyable;
        [DataMember]
        public bool IsBuyable
        {
            get
            {
                return _IsBuyable;
            }
            set
            {
                SetValue(ref _IsBuyable, () => this.IsBuyable, value);
            }
        }

        private int _AvailabilityRule;
        [DataMember]
        public int AvailabilityRule
        {
            get
            {
                return _AvailabilityRule;
            }
            set
            {
                SetValue(ref _AvailabilityRule, () => this.AvailabilityRule, value);
            }
        }

        private decimal _MinQuantity;
        [DataMember]
        public decimal MinQuantity
        {
            get
            {
                return _MinQuantity;
            }
            set
            {
                SetValue(ref _MinQuantity, () => this.MinQuantity, value);
            }
        }

        private decimal _MaxQuantity;
        [DataMember]
        public decimal MaxQuantity
        {
            get
            {
                return _MaxQuantity;
            }
            set
            {
                SetValue(ref _MaxQuantity, () => this.MaxQuantity, value);
            }
        }

        private bool _TrackInventory;
        [DataMember]
        public bool TrackInventory
        {
            get
            {
                return _TrackInventory;
            }
            set
            {
                SetValue(ref _TrackInventory, () => this.TrackInventory, value);
            }
        }

        private decimal _Weight;
        [DataMember]
        public decimal Weight
        {
            get
            {
                return _Weight;
            }
            set
            {
                SetValue(ref _Weight, () => this.Weight, value);
            }
        }

        private string _PackageType;
        [DataMember]
		[StringLength(128)]
        public string PackageType
        {
            get
            {
                return _PackageType;
            }
            set
            {
                SetValue(ref _PackageType, () => this.PackageType, value);
            }
        }

        private string _TaxCategory;
        [DataMember]
        [StringLength(128)]
        public string TaxCategory
        {
            get
            {
                return _TaxCategory;
            }
            set
            {
                SetValue(ref _TaxCategory, () => this.TaxCategory, value);
            }
        }

        private string _Code;
        [DataMember]
        [StringLength(64)]
        [Required]
		[CustomValidation(typeof(Item), "ValidateItemCode", ErrorMessage = @"Code can't contain $+;=%{}[]|\/@ ~#!^*&()?:'<>, characters")]
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetValue(ref _Code, () => this.Code, value);
            }
        }

        #region Navigation Properties

        ObservableCollection<CategoryItemRelation> _Categories = null;
        [DataMember]
        public virtual ObservableCollection<CategoryItemRelation> CategoryItemRelations
        {
	        get
            {
                if (_Categories == null)
                    _Categories = new ObservableCollection<CategoryItemRelation>();

                return _Categories;
            }
			set { _Categories = value; }
        }

	    ObservableCollection<ItemAsset> _Assets = null;
        [DataMember]
        public virtual ObservableCollection<ItemAsset> ItemAssets
        {
	        get
            {
                if (_Assets == null)
                    _Assets = new ObservableCollection<ItemAsset>();

                return _Assets;
            }
			set { _Assets = value; }
        }

	    ObservableCollection<AssociationGroup> _AssociationGroups = null;
        [DataMember]
        public virtual ObservableCollection<AssociationGroup> AssociationGroups
        {
	        get
            {
                if (_AssociationGroups == null)
                    _AssociationGroups = new ObservableCollection<AssociationGroup>();

                return _AssociationGroups;
            }
			set { _AssociationGroups = value; }
        }

	    ObservableCollection<EditorialReview> _EditorialReviews = null;
        [DataMember]
        public virtual ObservableCollection<EditorialReview> EditorialReviews
        {
	        get
            {
                if (_EditorialReviews == null)
                    _EditorialReviews = new ObservableCollection<EditorialReview>();

                return _EditorialReviews;
            }
			set { _EditorialReviews = value; }
        }

	    ObservableCollection<ItemPropertyValue> _PropertyValues = null;
        [DataMember]
        public virtual ObservableCollection<ItemPropertyValue> ItemPropertyValues
        {
	        get
            {
                if (_PropertyValues == null)
                    _PropertyValues = new ObservableCollection<ItemPropertyValue>();

                return _PropertyValues;
            }
			set { _PropertyValues = value; }
        }

	    private string _PropertySetId;
        [DataMember]
		[StringLength(128)]
        public string PropertySetId
        {
            get
            {
                return _PropertySetId;
            }
            set
            {
                SetValue(ref _PropertySetId, () => this.PropertySetId, value);
            }
        }

        [DataMember]
        public virtual PropertySet PropertySet { get; set; }


        private string _CatalogId;
        [DataMember]
		[StringLength(128)]
        public string CatalogId
        {
            get
            {
                return _CatalogId;
            }
            set
            {
                SetValue(ref _CatalogId, () => this.CatalogId, value);
            }
        }

        [DataMember]
        public virtual CatalogBase Catalog { get; set; }

        #endregion

		public static ValidationResult ValidateItemCode(string value, ValidationContext context)
		{
			if (value == null || string.IsNullOrEmpty(value))
			{
				return new ValidationResult("Code can't be empty");
			}

			const string invalidCodeCharacters = @"$+;=%{}[]|\/@ ~#!^*&()?:'<>,";

			if (value.IndexOfAny(invalidCodeCharacters.ToCharArray()) > -1)
			{
				return new ValidationResult((@"Code must be valid"));
			}
			else
			{
				return ValidationResult.Success;
			}
		}
    }
}
