using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerModule.Data.Model
{
		
	public class Email : Entity
	{
		[CustomValidation(typeof(Email), "ValidateEmailContent", ErrorMessage = "Email has error")]
		public string Address { get; set; }

		public bool IsValidated { get; set; }

		[StringLength(64)]
		public string Type { get; set; }


		#region Navigation Properties

		[StringLength(128)]
		public string MemberId { get; set; }

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
