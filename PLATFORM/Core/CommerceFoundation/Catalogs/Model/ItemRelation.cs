using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("ItemRelationId")]
	[EntitySet("ItemRelations")]
	public class ItemRelation : StorageEntity
	{
		public ItemRelation()
		{
			_ItemRelationId = GenerateNewKey();
		}

		private string _ItemRelationId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string ItemRelationId
		{
			get
			{
				return _ItemRelationId;
			}
			set
			{
				SetValue(ref _ItemRelationId, () => this.ItemRelationId, value);
			}
		}

		private string _RelationTypeId;
		[DataMember]
		[StringLength(64)]
		public string RelationTypeId
		{
			get
			{
				return _RelationTypeId;
			}
			set
			{
				SetValue(ref _RelationTypeId, () => this.RelationTypeId, value);
			}
		}

		private decimal _Quantity;
		[DataMember]
		public decimal Quantity
		{
			get
			{
				return _Quantity;
			}
			set
			{
				SetValue(ref _Quantity, () => this.Quantity, value);
			}
		}

		private string _GroupName;
		[DataMember]
		[StringLength(64)]
		[Required(ErrorMessage = "Field 'Group Name' is required.")]
		public string GroupName
		{
			get
			{
				return _GroupName;
			}
			set
			{
				SetValue(ref _GroupName, () => this.GroupName, value);
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

		private string _ChildItemId;
		[DataMember]
		[StringLength(128)]
		[ForeignKey("ChildItem")]
		[Required]
		public string ChildItemId
		{
			get
			{
				return _ChildItemId;
			}
			set
			{
				SetValue(ref _ChildItemId, () => this.ChildItemId, value);
			}
		}

		[DataMember]
		public virtual Item ChildItem { get; set; }

		private string _ParentItemId;
		[DataMember]
		[StringLength(128)]
		[ForeignKey("ParentItem")]
		[Required]
		public string ParentItemId
		{
			get
			{
				return _ParentItemId;
			}
			set
			{
				SetValue(ref _ParentItemId, () => this.ParentItemId, value);
			}
		}

		[DataMember]
		public virtual Item ParentItem { get; set; }
		#endregion
	}
}
