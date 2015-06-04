using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("ItemAssetId")]
	[EntitySet("ItemAssets")]
	public class ItemAsset : StorageEntity
    {
		public ItemAsset()
		{
			_ItemAssetId = GenerateNewKey();
		}

		private string _ItemAssetId;
        [Key]
		[StringLength(128)]
		[DataMember]
        public string ItemAssetId
        {
			get
			{
				return _ItemAssetId;
			}
			set
			{
				SetValue(ref _ItemAssetId, () => this.ItemAssetId, value);
			}
        }

		private string _AssetId;
		[DataMember]
        [StringLength(128)]
        [Required]
        public string AssetId
        {
			get
			{
				return _AssetId;
			}
			set
			{
				SetValue(ref _AssetId, () => this.AssetId, value);
			}
        }

		private string _AssetType;
		[DataMember]
        [StringLength(64)]
        [Required]
        public string AssetType
        {
			get
			{
				return _AssetType;
			}
			set
			{
				SetValue(ref _AssetType, () => this.AssetType, value);
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

		private int _SortOrder;
		[DataMember]
        public int SortOrder
        {
			get
			{
				return _SortOrder;
			}
			set
			{
				SetValue(ref _SortOrder, () => this.SortOrder, value);
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
		[ForeignKey("ItemId")]
		[Parent]
		public virtual Item CatalogItem { get; set; }
		#endregion
    }
}
