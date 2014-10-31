using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("PropertyId")]
	[EntitySet("Properties")]
	public class Property : StorageEntity
	{
		public Property()
		{
			_PropertyId = GenerateNewKey();
		}
		private string _PropertyId;
		[Required(ErrorMessage = "Field 'Property Id' is required.")]
		[Key]
		[StringLength(128)]
		[DataMember]
		public string PropertyId
		{
			get
			{
				return _PropertyId;
			}
			set
			{
				SetValue(ref _PropertyId, () => this.PropertyId, value);
			}
		}

		private string _Name;
		[Required(ErrorMessage = "Field 'Property Name' is required.")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

        private string _TargetType;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string TargetType
        {
            get
            {
                return _TargetType;
            }
            set
            {
                SetValue(ref _TargetType, () => this.TargetType, value);
            }
        }


		private bool _IsKey;
		[DataMember]
		public bool IsKey
		{
			get
			{
				return _IsKey;
			}
			set
			{
				SetValue(ref _IsKey, () => this.IsKey, value);
			}
		}

		private bool _IsSale;
		[DataMember]
		public bool IsSale
		{
			get
			{
				return _IsSale;
			}
			set
			{
				SetValue(ref _IsSale, () => this.IsSale, value);
			}
		}

		private bool _IsEnum;
		[DataMember]
		public bool IsEnum
		{
			get
			{
				return _IsEnum;
			}
			set
			{
				SetValue(ref _IsEnum, () => this.IsEnum, value);
			}
		}

		private bool _IsInput;
		[DataMember]
		public bool IsInput
		{
			get
			{
				return _IsInput;
			}
			set
			{
				SetValue(ref _IsInput, () => this.IsInput, value);
			}
		}

		private bool _IsRequired;
		[DataMember]
		public bool IsRequired
		{
			get
			{
				return _IsRequired;
			}
			set
			{
				SetValue(ref _IsRequired, () => this.IsRequired, value);
			}
		}

		private bool _IsMultiValue;
		[DataMember]
		public bool IsMultiValue
		{
			get
			{
				return _IsMultiValue;
			}
			set
			{
				SetValue(ref _IsMultiValue, () => this.IsMultiValue, value);
			}
		}

		private bool _IsLocaleDependant;
		/// <summary>
		/// Gets or sets a value indicating whether this instance is locale dependant. If true, the locale must be specified for the values.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is locale dependant; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool IsLocaleDependant
		{
			get
			{
				return _IsLocaleDependant;
			}
			set
			{
				SetValue(ref _IsLocaleDependant, () => this.IsLocaleDependant, value);
			}
		}

		private bool _AllowAlias;
		[DataMember]
		public bool AllowAlias
		{
			get
			{
				return _AllowAlias;
			}
			set
			{
				SetValue(ref _AllowAlias, () => this.AllowAlias, value);
			}
		}

		private int _PropertyValueType;
		[Required(ErrorMessage = "Field 'Property Type' is required.")]
		[DataMember]
		public int PropertyValueType
		{
			get
			{
				return _PropertyValueType;
			}
			set
			{
				SetValue(ref _PropertyValueType, () => this.PropertyValueType, value);
			}
		}

		#region Navigation Properties

		private string _ParentPropertyId;
		[StringLength(128)]
		[DataMember]
		[ForeignKey("ParentProperty")]
		public string ParentPropertyId
		{
			get
			{
				return _ParentPropertyId;
			}
			set
			{
				SetValue(ref _ParentPropertyId, () => this.ParentPropertyId, value);
			}
		}

		[DataMember]
		public virtual Property ParentProperty { get; set; }

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
		public virtual Catalog Catalog { get; set; }

		ObservableCollection<PropertyValue> _Values = null;
		[DataMember]
		public virtual ObservableCollection<PropertyValue> PropertyValues
		{
			get
			{
				if (_Values == null)
					_Values = new ObservableCollection<PropertyValue>();

				return _Values;
			}
			set { _Values = value; }
		}

		ObservableCollection<PropertyAttribute> _PropertyAttributes = null;
		[DataMember]
		public virtual ObservableCollection<PropertyAttribute> PropertyAttributes
		{
			get
			{
				if (_PropertyAttributes == null)
					_PropertyAttributes = new ObservableCollection<PropertyAttribute>();

				return _PropertyAttributes;
			}
			set { _PropertyAttributes = value; }
		}

		#endregion
	}
}
