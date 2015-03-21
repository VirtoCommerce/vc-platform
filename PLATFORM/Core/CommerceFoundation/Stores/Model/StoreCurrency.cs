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
	[EntitySet("StoreCurrencies")]
	[DataServiceKey("StoreCurrencyId")]
    public class StoreCurrency : StorageEntity
    {
		public StoreCurrency()
		{
			_StoreCurrencyId = GenerateNewKey();
		}

		private string _StoreCurrencyId;
        [Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string StoreCurrencyId
        {
			get
			{
				return _StoreCurrencyId;
			}
			set
			{
				SetValue(ref _StoreCurrencyId, () => this.StoreCurrencyId, value);
			}
        }

		private string _CurrencyCode;
        [Required]
        [StringLength(32)]
		[DataMember]
        public string CurrencyCode
        {
			get
			{
				return _CurrencyCode;
			}
			set
			{
				SetValue(ref _CurrencyCode, () => this.CurrencyCode, value);
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
