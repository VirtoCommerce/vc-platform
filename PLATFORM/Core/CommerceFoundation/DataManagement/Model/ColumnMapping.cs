using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
namespace VirtoCommerce.Foundation.Importing.Model
{
	[DataContract]
	[EntitySet("ColumnMappings")]
	[DataServiceKey("ColumnMappingId")]
	public class ColumnMapping: StorageEntity
	{
		public ColumnMapping()
        {
			_ColumnMappingId = GenerateNewKey();
        }

		private string _ColumnMappingId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string ColumnMappingId
		{
			get
			{
				return _ColumnMappingId;
			}
			set
			{
				SetValue(ref _ColumnMappingId, () => this.ColumnMappingId, value);
			}
		}

		private string _Name;
		[Required(ErrorMessage = "Field 'Name' is required.")]
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

		private string _EntityImporter;
		[Required]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		[DataMember]
		public string EntityImporter // product, sku, bundle, package, dynamickit, category, association, price, customer, inventory, itemrelation
		{
			get
			{
				return _EntityImporter;
			}
			set
			{
				SetValue(ref _EntityImporter, () => this.EntityImporter, value);
			}
		}

		#region Navigation Properties
		
		ObservableCollection<MappingItem> _PropertiesMap = null;
		[DataMember]
		public virtual ObservableCollection<MappingItem> PropertiesMap
		{
			get
			{
				if (_PropertiesMap == null)
					_PropertiesMap = new ObservableCollection<MappingItem>();

				return _PropertiesMap;
			}
			set
			{
				_PropertiesMap = value;
			}
		}

		#endregion
	}
}
