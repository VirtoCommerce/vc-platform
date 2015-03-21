using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
namespace VirtoCommerce.Foundation.Importing.Model
{
	[DataContract]
	[EntitySet("ImportProperties")]
	[DataServiceKey("ImportPropertyId")]
	public class ImportProperty: StorageEntity
	{
		public ImportProperty()
		{
			_ImportPropertyId = GenerateNewKey();
		}

		private string _ImportPropertyId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string ImportPropertyId
		{
			get
			{
				return _ImportPropertyId;
			}
			set
			{
				SetValue(ref _ImportPropertyId, () => this.ImportPropertyId, value);
			}
		}

		private string _Name;
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

		private string _DisplayName;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string DisplayName
		{
			get
			{
				return _DisplayName;
			}
			set
			{
				SetValue(ref _DisplayName, () => this.DisplayName, value);
			}
		}

		private string _DefaultValue;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string DefaultValue
		{
			get
			{
				return _DefaultValue;
			}
			set
			{
				SetValue(ref _DefaultValue, () => this.DefaultValue, value);
			}
		}
		
		private bool _IsEntityProperty;
		[DataMember]
		[Required]
		public bool IsEntityProperty
		{
			get
			{
				return _IsEntityProperty;
			}
			set
			{
				SetValue(ref _IsEntityProperty, () => this.IsEntityProperty, value);
			}
		}

		private bool _hasDefaultValue;
		[DataMember]
		[Required]
		public bool HasDefaultValue
		{
			get
			{
				return _hasDefaultValue;
			}
			set
			{
				SetValue(ref _hasDefaultValue, () => this.HasDefaultValue, value);
			}
		}

		private bool _IsRequieredProperty;
		[DataMember]
		[Required]
		public bool IsRequiredProperty
		{
			get
			{
				return _IsRequieredProperty;
			}
			set
			{
				SetValue(ref _IsRequieredProperty, () => this.IsRequiredProperty, value);
			}
		}

		private bool _IsEnumValuesProperty;
		[DataMember]
		public bool IsEnumValuesProperty
		{
			get
			{
				return _IsEnumValuesProperty;
			}
			set
			{
				SetValue(ref _IsEnumValuesProperty, () => IsEnumValuesProperty, value);
			}
		}

		private ObservableCollection<string> _EnumValues;
		public ObservableCollection<string> EnumValues
		{
			get { return _EnumValues ?? (_EnumValues = new ObservableCollection<string>()); }
			set
			{
				SetValue(ref _EnumValues, () => this.EnumValues, value);
			}
		}

		#region Navigation Properties

		private string _EntityImporterId;
		[Required]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string EntityImporterId // product, sku, bundle, package, dynamickit, category, association, price, customer
		{
			get
			{
				return _EntityImporterId;
			}
			set
			{
				SetValue(ref _EntityImporterId, () => this.EntityImporterId, value);
			}
		}
		
		#endregion
	}
}
