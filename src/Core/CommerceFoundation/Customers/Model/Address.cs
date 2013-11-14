using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
	[DataContract]
	[EntitySet("Addresses")]
	[DataServiceKey("AddressId")]
	public class Address : StorageEntity
	{

		public Address()
		{
			_AddressId = GenerateNewKey();
		}

		private string _AddressId;
		[Key]
		[DataMember]
		[StringLength(128)]
		public string AddressId
		{
			get
			{
				return _AddressId;
			}
			set
			{
				SetValue(ref _AddressId, () => this.AddressId, value);
			}
		}

		private string _Name;
		[Required(ErrorMessage = "Address Name can't be empty")]
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

		

		private string _FirstName;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

		private string _Line1;
		[Required(ErrorMessage = "Line 1 can't be empty")]
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
		[Required(ErrorMessage = "City can't be empty")]
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


		private string _CountryCode;
		[Required(ErrorMessage = "Country name can't be empty")]
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


		private string _CountryName;
		[Required(ErrorMessage = "Country name can't be empty")]
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
		[Required(ErrorMessage = "Postal code can't be empty")]
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

		private string _RegionId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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


		private string _Type;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string Type
		{
			get
			{
				return _Type;
			}
			set
			{
				SetValue(ref _Type, () => this.Type, value);
			}
		}

		private string _DaytimePhoneNumber;
		//[Required(ErrorMessage = "Phone Number code can't be empty")]
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
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
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
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
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
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
		//[Required(ErrorMessage = "Email is required"), EmailAddress]
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

		private string _Organization;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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
		
		#region Navigation Properties

		private string _MemberId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string MemberId
		{
			get { return _MemberId; }
			set
			{
				SetValue(ref _MemberId, () => this.MemberId, value);
			}
		}

		[DataMember]
        [Parent]
        [ForeignKey("MemberId")]
		public virtual Member Member { get; set; }


		#endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format("{0} {1}, {2} {3}, {4}, {5} {6} {7}", 
                FirstName, LastName, Line1, Line2, City, StateProvince, PostalCode, CountryName);
            
        }
        #endregion
    }
}
