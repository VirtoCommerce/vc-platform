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
			Languages = new ObservableCollection<StoreLanguage>();
			Currencies = new ObservableCollection<StoreCurrency>();
			Settings = new ObservableCollection<StoreSetting>();
			PaymentGateways = new ObservableCollection<StorePaymentGateway>();
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

		#region Navigation Properties

		[StringLength(128)]
		public string FulfillmentCenterId { get; set; }
        [ForeignKey("FulfillmentCenterId")]
		public FulfillmentCenter FulfillmentCenter { get; set; }

        [ForeignKey("ReturnsFulfillmentCenter")]
		[StringLength(128)]
		public string ReturnsFulfillmentCenterId { get; set; }
		public FulfillmentCenter ReturnsFulfillmentCenter { get; set; }

		public ObservableCollection<StoreLanguage> Languages { get; set; }

		public ObservableCollection<StoreCurrency> Currencies { get; set; }

		public ObservableCollection<StoreSetting> Settings { get; set; }

		public ObservableCollection<StorePaymentGateway> PaymentGateways { get; set; }
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
