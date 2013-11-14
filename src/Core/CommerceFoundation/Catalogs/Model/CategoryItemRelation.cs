using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[EntitySet("CategoryItemRelations")]
	[DataServiceKey("CategoryItemRelationId")]
	public class CategoryItemRelation : StorageEntity
	{
		public CategoryItemRelation()
		{
			_CategoryItemRelationId = GenerateNewKey();
		}

		private string _CategoryItemRelationId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string CategoryItemRelationId
		{
			get
			{
				return _CategoryItemRelationId;
			}
			set
			{
				SetValue(ref _CategoryItemRelationId, () => this.CategoryItemRelationId, value);
			}
		}

		private int _Priority;
		[DataMember]
		public int Priority
		{
			get
			{
				return _Priority;
			}
			set
			{
				SetValue(ref _Priority, () => this.Priority, value);
			}
		}

		#region Navigation Properties

		private string _ItemId;
		[DataMember]
		[StringLength(128)]
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

		[DataMember]
		[Parent]
		[ForeignKey("ItemId")]
		public virtual Item CatalogItem { get; set; }

		private string _CategoryId;
		[DataMember]
		[StringLength(128)]
		[Required]
		public string CategoryId
		{
			get
			{
				return _CategoryId;
			}
			set
			{
				SetValue(ref _CategoryId, () => this.CategoryId, value);
			}
		}

		[DataMember]
		public virtual CategoryBase Category { get; set; }

		private string _CatalogId;
		[DataMember]
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
		public virtual CatalogBase Catalog { get; set; }

		#endregion
	}
}
