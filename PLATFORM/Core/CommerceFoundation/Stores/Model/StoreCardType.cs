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
	[EntitySet("StoreCardTypes")]
	[DataServiceKey("StoreCardTypeId")]
    public class StoreCardType : StorageEntity
    {
		public StoreCardType()
		{
			_StoreCardTypeId = GenerateNewKey();
		}

		private string _StoreCardTypeId;
        [Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string StoreCardTypeId
        {
			get
			{
				return _StoreCardTypeId;
			}
			set
			{
				SetValue(ref _StoreCardTypeId, () => this.StoreCardTypeId, value);
			}
        }

		private string _CardType;
        [Required]
        [StringLength(64)]
		[DataMember]
        public string CardType
        {
			get
			{
				return _CardType;
			}
			set
			{
				SetValue(ref _CardType, () => this.CardType, value);
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
		#endregion
    }
}
