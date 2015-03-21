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
	[EntitySet("StoreTaxCodes")]
	[DataServiceKey("StoreTaxCodeId")]
    public class StoreTaxCode : StorageEntity
    {
		public StoreTaxCode()
		{
			_StoreTaxCodeId = GenerateNewKey();
		}

		private string _StoreTaxCodeId;
        [Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string StoreTaxCodeId
        {
			get
			{
				return _StoreTaxCodeId;
			}
			set
			{
				SetValue(ref _StoreTaxCodeId, () => this.StoreTaxCodeId, value);
			}
        }

		private string _TaxCode;
        [Required]
		[DataMember]
		[StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
        public string TaxCode
        {
			get
			{
				return _TaxCode;
			}
			set
			{
				SetValue(ref _TaxCode, () => this.TaxCode, value);
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
