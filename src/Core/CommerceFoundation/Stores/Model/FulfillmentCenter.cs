using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;

namespace VirtoCommerce.Foundation.Stores.Model
{
	[DataContract]
	[EntitySet("FulfillmentCenters")]
	[DataServiceKey("FulfillmentCenterId")]
	public class FulfillmentCenter : StorageEntity
	{
		public FulfillmentCenter()
		{
			_fulfillmentCenterId = GenerateNewKey();
		}

		private string _fulfillmentCenterId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string FulfillmentCenterId
		{
			get
			{
				return _fulfillmentCenterId;
			}
			set
			{
				SetValue(ref _fulfillmentCenterId, () => this.FulfillmentCenterId, value);
			}
		}

		private string _Name;
		[Required]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				SetValue(ref _Name, () => this.Name, value);
			}
		}

		private string _Description;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
		public string Description
		{
			get
			{
				return _Description;
			}
			set
			{
				SetValue(ref _Description, () => this.Description, value);
			}
		}

		private int _MaxReleasesPerPickBatch;
		[DataMember]
		public int MaxReleasesPerPickBatch
		{
			get
			{
				return _MaxReleasesPerPickBatch;
			}
			set
			{
				SetValue(ref _MaxReleasesPerPickBatch, () => this.MaxReleasesPerPickBatch, value);
			}
		}

		private int _PickDelay;
		[DataMember]
		public int PickDelay
		{
			get
			{
				return _PickDelay;
			}
			set
			{
				SetValue(ref _PickDelay, () => this.PickDelay, value);
			}
		}

		private string _DaytimePhoneNumber;
		[Required]
		[DataMember]
		[StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
		public string DaytimePhoneNumber
		{
			get
			{
				return _DaytimePhoneNumber;
			}
			set
			{
				SetValue(ref _DaytimePhoneNumber, () => this.DaytimePhoneNumber, value);
			}
		}

		private string _Line1;
		[Required]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Line1
		{
			get
			{
				return _Line1;
			}
			set
			{
				SetValue(ref _Line1, () => this.Line1, value);
			}
		}

		private string _Line2;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Line2
		{
			get
			{
				return _Line2;
			}
			set
			{
				SetValue(ref _Line2, () => this.Line2, value);
			}
		}

		private string _City;
		[Required]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string City
		{
			get
			{
				return _City;
			}
			set
			{
				SetValue(ref _City, () => this.City, value);
			}
		}

		private string _StateProvince;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string StateProvince
		{
			get
			{
				return _StateProvince;
			}
			set
			{
				SetValue(ref _StateProvince, () => this.StateProvince, value);
			}
		}

		private string _CountryCode;
		[Required]
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string CountryCode
		{
			get
			{
				return _CountryCode;
			}
			set
			{
				SetValue(ref _CountryCode, () => this.CountryCode, value);
			}
		}

		private string _CountryName;
		[Required]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string CountryName
		{
			get
			{
				return _CountryName;
			}
			set
			{
				SetValue(ref _CountryName, () => this.CountryName, value);
			}
		}

		private string _PostalCode;
		[Required]
		[DataMember]
		[StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
		public string PostalCode
		{
			get
			{
				return _PostalCode;
			}
			set
			{
				SetValue(ref _PostalCode, () => this.PostalCode, value);
			}
		}
	}
}
