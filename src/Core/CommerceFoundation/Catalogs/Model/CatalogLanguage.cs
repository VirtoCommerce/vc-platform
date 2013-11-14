using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("CatalogLanguageId")]
	[EntitySet("CatalogLanguages")]
	public class CatalogLanguage : StorageEntity
	{
		public CatalogLanguage()
		{
			_CatalogLanguageId = GenerateNewKey();
		}

		private string _CatalogLanguageId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string CatalogLanguageId
		{
			get
			{
				return _CatalogLanguageId;
			}
			set
			{
				SetValue(ref _CatalogLanguageId, () => this.CatalogLanguageId, value);
			}
		}

		private string _Language;
		[DataMember]
		[StringLength(64)]
		public string Language
		{
			get
			{
				return _Language;
			}
			set
			{
				SetValue(ref _Language, () => this.Language, value);
			}
		}

		#region Navigation Properties

		private string _CatalogId;
		[DataMember]
		[StringLength(128)]
		[Required]
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
		#endregion
	}
}
