using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{

	[DataContract]
	[EntitySet("Emails")]
	[DataServiceKey("EmailId")]
	public class Email : StorageEntity
	{

		public Email()
		{
			_EmailId = GenerateNewKey();
		}


		private string _EmailId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string EmailId
		{
			get
			{
				return _EmailId;
			}
			set
			{
				SetValue(ref _EmailId, () => this.EmailId, value);
			}
		}


		private string _Address;
		[DataMember]
		[CustomValidation(typeof(Email), "ValidateEmailContent", ErrorMessage = "Email has error")]
        public string Address
		{
			get
			{
                return _Address;
			}
			set
			{
                SetValue(ref _Address, () => this.Address, value);
			}
		}


        private bool _isValidated;
        [DataMember]
        public bool IsValidated
        {
            get { return _isValidated; }
            set
            {
                SetValue(ref _isValidated, () => IsValidated, value);
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

		public static ValidationResult ValidateEmailContent(string value, ValidationContext context)
		{
			if (value == null || string.IsNullOrEmpty(value))
			{
				return new ValidationResult("Email can't be empty");
			}

			Regex regex = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
			if (!regex.IsMatch(value))
			{
				return new ValidationResult((@"Email must be ""email@server.[domain 2].domain"));
			}
			else
			{
				return ValidationResult.Success;
			}

		}

		#endregion

	}


}
