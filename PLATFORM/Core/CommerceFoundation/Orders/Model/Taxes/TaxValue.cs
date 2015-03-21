using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model.Taxes
{
	[DataContract]
	[EntitySet("TaxValues")]
	[DataServiceKey("TaxValueId")]
	public class TaxValue : StorageEntity
	{
		public TaxValue()
		{
			TaxValueId = GenerateNewKey();
		}

		private string _TaxValueId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string TaxValueId
		{
			get
			{
				return _TaxValueId;
			}
			set
			{
				SetValue(ref _TaxValueId, () => this.TaxValueId, value);
			}
		}

		private string _TaxId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string TaxId
		{
			get
			{
				return _TaxId;
			}
			set
			{
				SetValue(ref _TaxId, () => this.TaxId, value);
			}
		}

		private string _JurisdictionGroupId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [Required]
        [ForeignKey("JurisdictionGroup")]
		public string JurisdictionGroupId
		{
			get
			{
				return _JurisdictionGroupId;
			}
			set
			{
				SetValue(ref _JurisdictionGroupId, () => this.JurisdictionGroupId, value);
			}
		}

		private decimal _Percentage;
		[DataMember]
		public decimal Percentage
		{
			get
			{
				return _Percentage;
			}
			set
			{
				SetValue(ref _Percentage, () => this.Percentage, value);
			}
		}

		private string _TaxCategory;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string TaxCategory
		{
			get
			{
				return _TaxCategory;
			}
			set
			{
				SetValue(ref _TaxCategory, () => this.TaxCategory, value);
			}
		}

		private DateTime? _AffectiveDate;
		[DataMember]
		public DateTime? AffectiveDate
		{
			get
			{
				return _AffectiveDate;
			}
			set
			{
				SetValue(ref _AffectiveDate, () => this.AffectiveDate, value);
			}
		}

		[DataMember]
        [ForeignKey("TaxId")]
        [Parent]
		public Tax Tax { get; set; }

        [DataMember]
        public virtual JurisdictionGroup JurisdictionGroup { get; set; }
	}
}
