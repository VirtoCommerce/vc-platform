using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[EntitySet("PropertySets")]
	[DataServiceKey("PropertySetId")]
	public class PropertySet : StorageEntity
	{
		public PropertySet()
		{
			_PropertySetId = GenerateNewKey();
		}

		private string _PropertySetId;
		[DataMember]
		[StringLength(128)]
		[Key, Required(ErrorMessage = "Field 'PropertySetId' is required.")]
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

		private string _Name;
		[DataMember]
		[Required(ErrorMessage = "Field 'Name' is required."),
		StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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
		[Required(ErrorMessage = "Field 'Target' is required."),
		StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
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

		#region NavigationProperties

		private string _CatalogId;
		[DataMember]
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
		[Parent]
		[ForeignKey("CatalogId")]
		public virtual Catalog Catalog { get; set; }

		ObservableCollection<PropertySetProperty> _Properties = null;
		[DataMember]
		public virtual ObservableCollection<PropertySetProperty> PropertySetProperties
		{
			get
			{
				if (_Properties == null)
					_Properties = new ObservableCollection<PropertySetProperty>();

				return _Properties;
			}
			set { _Properties = value; }
		}

		#endregion
	}
}
