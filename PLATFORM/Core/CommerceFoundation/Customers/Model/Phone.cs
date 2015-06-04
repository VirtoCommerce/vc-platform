using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	[EntitySet("Phones")]
	[DataServiceKey("PhoneId")]
	public class Phone : StorageEntity
	{

		public Phone()
		{
			_PhoneId = GenerateNewKey();
		}


		private string _PhoneId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string PhoneId
		{
			get
			{
				return _PhoneId;
			}
			set
			{
				SetValue(ref _PhoneId, () => this.PhoneId, value);
			}
		}


		private string _Number;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string Number
		{
			get
			{
				return _Number;
			}
			set
			{
				SetValue(ref _Number, () => this.Number, value);
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
        [ForeignKey("MemberId")]
        [Parent]
		public virtual Member Member { get; set; }


		#endregion

		#region Validation

		public static ValidationResult PhoneNumberValidate(string value, ValidationContext context)
		{
			if (string.IsNullOrEmpty(value))
			{
				return new ValidationResult("Number can't be empty");
			}
            /*
			//TODO сделать нормальную проверку на телефон
			Regex regex = new Regex(@"^\d{11}$");
			if (value.Length != 11)
			{
				return new ValidationResult("phone number must contain 11 digits");
			}
			else
			{
				return ValidationResult.Success;
			}
             * */
            return ValidationResult.Success;
		}

		#endregion

	}
}
