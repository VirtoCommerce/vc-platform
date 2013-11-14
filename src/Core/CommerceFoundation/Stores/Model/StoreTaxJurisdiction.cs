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
	[EntitySet("StoreTaxJurisdictions")]
	[DataServiceKey("StoreTaxJurisdictionId")]
    public class StoreTaxJurisdiction : StorageEntity
    {
		public StoreTaxJurisdiction()
		{
			_StoreTaxJurisdictionId = GenerateNewKey();
		}

		private string _StoreTaxJurisdictionId;
        [Key]
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string StoreTaxJurisdictionId
        {
			get
			{
				return _StoreTaxJurisdictionId;
			}
			set
			{
				SetValue(ref _StoreTaxJurisdictionId, () => this.StoreTaxJurisdictionId, value);
			}
        }

		private string _TaxJurisdiction;
        [Required]
        [StringLength(128)]
        [DataMember]
        public string TaxJurisdiction
        {
			get
			{
				return _TaxJurisdiction;
			}
			set
			{
				SetValue(ref _TaxJurisdiction, () => this.TaxJurisdiction, value);
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
