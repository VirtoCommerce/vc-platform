using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model
{
    [DataContract] 
	[EntitySet("OrderAddresses")]
	[DataServiceKey("OrderAddressId")]
	public class OrderAddress : StorageEntity
    {
		public OrderAddress()
		{
			OrderAddressId = GenerateNewKey();
		}

		private string _OrderAddressId;
        [Key]
        [StringLength(128)]
		[DataMember]
        public string OrderAddressId
        {
			get
			{
				return _OrderAddressId;
			}
			set
			{
				SetValue(ref _OrderAddressId, () => this.OrderAddressId, value);
			}
        }

		private string _OrderGroupId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string OrderGroupId
		{
			get
			{
				return _OrderGroupId;
			}
			set
			{
				SetValue(ref _OrderGroupId, () => this.OrderGroupId, value);
			}
		}


		private string _Name;
		[Required]
		[DataMember]
		[StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
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

		private string _FirstName;
        [Required(ErrorMessage = "Field 'First Name' is required.")]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string FirstName
        {
			get
           	{
				return _FirstName;
			}
			set
			{
				SetValue(ref _FirstName, () => this.FirstName, value);
            }
        }

		private string _LastName;
        [Required(ErrorMessage = "Field 'Last Name' is required.")]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string LastName
        {
			get
			{
				return _LastName;
			}
			set
			{
				SetValue(ref _LastName, () => this.LastName, value);
			}
        }

		private string _Organization;
        [StringLength(128)]
		[DataMember]
        public string Organization
        {
			get
			{
				return _Organization;
			}
			set
			{
				SetValue(ref _Organization, () => this.Organization, value);
			}
        }

		private string _Line1;
        [Required(ErrorMessage = "Field 'Address Line 1' is required.")]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
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
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
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
        [Required(ErrorMessage = "Field 'City' is required.")]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
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
        [StringLength(128)]
		[DataMember]
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
        [Required(ErrorMessage = "Field 'Country' is required.")]
        [StringLength(128)]
		[DataMember]
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
        [StringLength(64)]
		[DataMember]
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
        [Required(ErrorMessage = "Field 'Postal Code' is required.")]
        [StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
		[DataMember]
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

		private string _RegionId;
        [StringLength(32)]
		[DataMember]
        public string RegionId
        {
			get
			{
				return _RegionId;
			}
			set
			{
				SetValue(ref _RegionId, () => this.RegionId, value);
			}
        }

		private string _RegionName;
        [StringLength(64)]
		[DataMember]
        public string RegionName
        {
			get
			{
				return _RegionName;
			}
			set
			{
				SetValue(ref _RegionName, () => this.RegionName, value);
			}
        }

		private string _DaytimePhoneNumber;
        [Required(ErrorMessage = "Field 'Daytime Phone Number' is required.")]
        [StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
		[DataMember]
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

		private string _EveningPhoneNumber;
        [StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
		[DataMember]
        public string EveningPhoneNumber
        {
			get
			{
				return _EveningPhoneNumber;
			}
			set
			{
				SetValue(ref _EveningPhoneNumber, () => this.EveningPhoneNumber, value);
			}
        }

		private string _FaxNumber;
        [StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
		[DataMember]
        public string FaxNumber
        {
			get
			{
				return _FaxNumber;
			}
			set
			{
				SetValue(ref _FaxNumber, () => this.FaxNumber, value);
			}
        }

		private string _Email;
        [Required, EmailAddress]
		[DataMember]
        [StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string Email
        {
           get
			{
				return _Email;
			}
			set
			{
				SetValue(ref _Email, () => this.Email, value);
			}
        }


		[DataMember]
        [ForeignKey("OrderGroupId")]
        [Parent]
		public OrderGroup OrderGroup { get; set; }

		public override string ToString()
		{
			string retVal = string.Empty;
			retVal = string.Format("{0}{1}{2}{3}{4}{5}{6}", 
				CountryName, 
				Environment.NewLine, 
				StateProvince, 
				Environment.NewLine, 
				Line1, 
				Environment.NewLine, 
				Line2);
			return retVal;
		}
	}
}
