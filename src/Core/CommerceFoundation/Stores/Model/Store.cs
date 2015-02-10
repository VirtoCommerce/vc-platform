using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Stores.Model
{
	[DataContract]
	[EntitySet("Stores")]
	[DataServiceKey("StoreId")]
	public class Store : StorageEntity
	{
		public Store()
		{
			_StoreId = GenerateNewKey();
		}

		private string _StoreId;
		[DataMember]
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[CustomValidation(typeof(Store), "ValidateStoreId", ErrorMessage = @"Code is required and can't contain $+;=%{}[]|\/@ ~#!^*&?:'<>, characters")]
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

		private string _Name;
		[DataMember]
		[Required(ErrorMessage = "Field 'Store Name' is required.")]
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

		private string _Url;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
		public string Url
		{
			get
			{
				return _Url;
			}
			set
			{
				SetValue(ref _Url, () => this.Url, value);
			}
		}

		private int _StoreState;
		[DataMember]
		public int StoreState
		{
			get
			{
				return _StoreState;
			}
			set
			{
				SetValue(ref _StoreState, () => this.StoreState, value);
			}
		}

		private string _TimeZone;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string TimeZone
		{
			get
			{
				return _TimeZone;
			}
			set
			{
				SetValue(ref _TimeZone, () => this.TimeZone, value);
			}
		}

		private string _Country;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Country
		{
			get
			{
				return _Country;
			}
			set
			{
				SetValue(ref _Country, () => this.Country, value);
			}
		}

		private string _Region;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Region
		{
			get
			{
				return _Region;
			}
			set
			{
				SetValue(ref _Region, () => this.Region, value);
			}
		}


		private string _DefaultLanguage;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string DefaultLanguage
		{
			get
			{
				return _DefaultLanguage;
			}
			set
			{
				SetValue(ref _DefaultLanguage, () => this.DefaultLanguage, value);
			}
		}

		private string _DefaultCurrency;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string DefaultCurrency
		{
			get
			{
				return _DefaultCurrency;
			}
			set
			{
				SetValue(ref _DefaultCurrency, () => this.DefaultCurrency, value);
			}
		}

		private string _Catalog;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[Required(ErrorMessage = "Field 'Store Catalog' is required.")]
		public string Catalog
		{
			get
			{
				return _Catalog;
			}
			set
			{
				SetValue(ref _Catalog, () => this.Catalog, value);
			}
		}

		private int _CreditCardSavePolicy;
		[DataMember]
		public int CreditCardSavePolicy
		{
			get
			{
				return _CreditCardSavePolicy;
			}
			set
			{
				SetValue(ref _CreditCardSavePolicy, () => this.CreditCardSavePolicy, value);
			}
		}


		private string _SecureUrl;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string SecureUrl
		{
			get
			{
				return _SecureUrl;
			}
			set
			{
				SetValue(ref _SecureUrl, () => this.SecureUrl, value);
			}
		}

		private string _Email;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

		private string _AdminEmail;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string AdminEmail
		{
			get
			{
				return _AdminEmail;
			}
			set
			{
				SetValue(ref _AdminEmail, () => this.AdminEmail, value);
			}
		}

		private bool _DisplayOutOfStock;
		[DataMember]
		public bool DisplayOutOfStock
		{
			get
			{
				return _DisplayOutOfStock;
			}
			set
			{
				SetValue(ref _DisplayOutOfStock, () => this.DisplayOutOfStock, value);
			}
		}

		#region Navigation Properties

		private string _FulfillmentCenterId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string FulfillmentCenterId
		{
			get
			{
				return _FulfillmentCenterId;
			}
			set
			{
				SetValue(ref _FulfillmentCenterId, () => this.FulfillmentCenterId, value);
			}
		}

		[DataMember]
        [ForeignKey("FulfillmentCenterId")]
		public FulfillmentCenter FulfillmentCenter { get; set; }

		private string _ReturnsFulfillmentCenterId;
		[DataMember]
        [ForeignKey("ReturnsFulfillmentCenter")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ReturnsFulfillmentCenterId
		{
			get
			{
				return _ReturnsFulfillmentCenterId;
			}
			set
			{
				SetValue(ref _ReturnsFulfillmentCenterId, () => this.ReturnsFulfillmentCenterId, value);
			}
		}
		[DataMember]
		public FulfillmentCenter ReturnsFulfillmentCenter { get; set; }

		ObservableCollection<StoreLanguage> _Languages = null;
		[DataMember]
		public ObservableCollection<StoreLanguage> Languages
		{
			get
			{
				if (_Languages == null)
					_Languages = new ObservableCollection<StoreLanguage>();

				return _Languages;
			}
			set
			{
				_Languages = value;
			}
		}

		ObservableCollection<StoreCurrency> _Currencies = null;
		[DataMember]
		public ObservableCollection<StoreCurrency> Currencies
		{
			get
			{
				if (_Currencies == null)
					_Currencies = new ObservableCollection<StoreCurrency>();

				return _Currencies;
			}
			set
			{
				_Currencies = value;
			}
		}

		ObservableCollection<StoreSetting> _Settings = null;
		[DataMember]
		public ObservableCollection<StoreSetting> Settings
		{
			get
			{
				if (_Settings == null)
					_Settings = new ObservableCollection<StoreSetting>();

				return _Settings;
			}
			set
			{
				_Settings = value;
			}
		}

		ObservableCollection<StoreLinkedStore> _LinkedStores = null;
		[DataMember]
		public ObservableCollection<StoreLinkedStore> LinkedStores
		{
			get
			{
				if (_LinkedStores == null)
					_LinkedStores = new ObservableCollection<StoreLinkedStore>();

				return _LinkedStores;
			}
			set
			{
				_LinkedStores = value;
			}
		}

		ObservableCollection<StoreCardType> _SupportedCardTypes = null;
		[DataMember]
		public ObservableCollection<StoreCardType> CardTypes
		{
			get
			{
				if (_SupportedCardTypes == null)
					_SupportedCardTypes = new ObservableCollection<StoreCardType>();

				return _SupportedCardTypes;
			}
			set
			{
				_SupportedCardTypes = value;
			}
		}

		ObservableCollection<StorePaymentGateway> _PaymentGateways = null;
		[DataMember]
		public ObservableCollection<StorePaymentGateway> PaymentGateways
		{
			get
			{
				if (_PaymentGateways == null)
					_PaymentGateways = new ObservableCollection<StorePaymentGateway>();

				return _PaymentGateways;
			}
			set
			{
				_PaymentGateways = value;
			}
		}

		ObservableCollection<StoreTaxCode> _TaxCodes = null;
		[DataMember]
		public ObservableCollection<StoreTaxCode> TaxCodes
		{
			get
			{
				if (_TaxCodes == null)
					_TaxCodes = new ObservableCollection<StoreTaxCode>();

				return _TaxCodes;
			}
			set
			{
				_TaxCodes = value;
			}
		}

		ObservableCollection<StoreTaxJurisdiction> _TaxJurisdiction = null;
		[DataMember]
		public ObservableCollection<StoreTaxJurisdiction> TaxJurisdictions
		{
			get
			{
				if (_TaxJurisdiction == null)
					_TaxJurisdiction = new ObservableCollection<StoreTaxJurisdiction>();

				return _TaxJurisdiction;
			}
			set
			{
				_TaxJurisdiction = value;
			}
		}
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
