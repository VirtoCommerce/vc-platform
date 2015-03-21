using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Stores.Model
{
	[DataContract]
	[EntitySet("StoreLinkedStores")]
	[DataServiceKey("StoreLinkedStoreId")]
    public class StoreLinkedStore : StorageEntity
    {
		public StoreLinkedStore()
		{
			_StoreLinkedStoreId = GenerateNewKey();
		}

		private string _StoreLinkedStoreId;
        [Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string StoreLinkedStoreId
        {
			get
			{
				return _StoreLinkedStoreId;
			}
			set
			{
				SetValue(ref _StoreLinkedStoreId, () => this.StoreLinkedStoreId, value);
			}
        }

		#region Navigation Properties

		private string _StoreId;
		[Required]
		[ForeignKey("Store")]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string StoreId
		{
			get
			{
				return _StoreId;
			}
			set
			{
				SetValue(ref _StoreId, () => this.StoreId, value);
			}
		}

		[DataMember]
		public Store Store { get; set; }

		private string _LinkedStoreId;
		[Required]
		[ForeignKey("LinkedStore")]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string LinkedStoreId
		{
			get
			{
				return _LinkedStoreId;
			}
			set
			{
				SetValue(ref _LinkedStoreId, () => this.LinkedStoreId, value);
			}
		}

		[DataMember]
		public Store LinkedStore { get; set; }

		#endregion
    }
}
