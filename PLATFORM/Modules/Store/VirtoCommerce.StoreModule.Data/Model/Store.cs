using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.StoreModule.Data.Model
{
	public class Store : AuditableEntity
	{
		public Store()
		{
			Languages = new NullCollection<StoreLanguage>();
			Currencies = new NullCollection<StoreCurrency>();
			PaymentMethods = new NullCollection<StorePaymentMethod>();
			ShippingMethods = new NullCollection<StoreShippingMethod>();
		}

		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(256)]
		public string Description { get; set; }

		[StringLength(256)]
		public string Url { get; set; }

		public int StoreState { get; set; }

		[StringLength(128)]
		public string TimeZone { get; set; }

		[StringLength(128)]
		public string Country { get; set; }

		[StringLength(128)]
		public string Region { get; set; }

		[StringLength(128)]
		public string DefaultLanguage { get; set; }

		[StringLength(64)]
		public string DefaultCurrency { get; set; }

		[StringLength(128)]
		[Required]
		public string Catalog { get; set; }

		public int CreditCardSavePolicy { get; set; }

		[StringLength(128)]
		public string SecureUrl { get; set; }

		[StringLength(128)]
		public string Email { get; set; }

		[StringLength(128)]
		public string AdminEmail { get; set; }

		public bool DisplayOutOfStock { get; set; }

		[StringLength(128)]
		public string FulfillmentCenterId { get; set; }
		[StringLength(128)]
		public string ReturnsFulfillmentCenterId { get; set; }

		#region Navigation Properties

		public virtual ObservableCollection<StoreLanguage> Languages { get; set; }

		public virtual ObservableCollection<StoreCurrency> Currencies { get; set; }

		public virtual ObservableCollection<StorePaymentMethod> PaymentMethods { get; set; }
		public virtual ObservableCollection<StoreShippingMethod> ShippingMethods { get; set; }
		#endregion

		public static ValidationResult ValidateStoreId(string value, ValidationContext context)
		{
			if (string.IsNullOrEmpty(value))
			{
				return new ValidationResult("Code can't be empty");
			}

			const string invalidKeywordCharacters = @"$+;=%{}[]|\/@ ~#!^*&?:'<>,";

			if (value.IndexOfAny(invalidKeywordCharacters.ToCharArray()) > -1)
			{
				return new ValidationResult(@"Code must be valid");
			}
			else
			{
				return ValidationResult.Success;
			}
		}
	}
}
