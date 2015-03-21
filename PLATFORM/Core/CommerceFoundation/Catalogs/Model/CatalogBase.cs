using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[EntitySet("CatalogBases")]
	[DataServiceKey("CatalogId")]
	public abstract class CatalogBase : StorageEntity
	{
		private string _CatalogId;
		[Key]
		[StringLength(128)]
		[Required]
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

		private string _Name;
		[Required(ErrorMessage = "Field 'Catalog Name' is required.")]
		[StringLength(128)]
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

		private string _DefaultLanguage;
		//[Required(ErrorMessage = "Field 'Default Language' is required.")]
		[StringLength(64)]
		[Required]
		[DataMember]
		public string DefaultLanguage
		{
			get
			{
				return _DefaultLanguage;
			}
			set
			{
				SetValue(ref _DefaultLanguage, () => this.DefaultLanguage, value);
			}
		}

		private string _OwnerId;
		[StringLength(128)]
		[DataMember]
		public string OwnerId
		{
			get
			{
				return _OwnerId;
			}
			set
			{
				SetValue(ref _OwnerId, () => this.OwnerId, value);
			}
		}

		#region Navigation Properties

		ObservableCollection<CategoryBase> _Categories = null;
		[DataMember]
		public virtual ObservableCollection<CategoryBase> CategoryBases
		{
			get
			{
				if (_Categories == null)
					_Categories = new ObservableCollection<CategoryBase>();

				return _Categories;
			}
			set { _Categories = value; }
		}

		#endregion

	}
}
