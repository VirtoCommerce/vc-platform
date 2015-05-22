using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Store.Model;

namespace VirtoCommerce.StoreModule.Web.Model
{
	public class Store : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Url { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public coreModel.StoreState StoreState { get; set; }

		public string TimeZone { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }
		public string DefaultLanguage { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes? DefaultCurrency { get; set; }
		public string Catalog { get; set; }
		public bool CreditCardSavePolicy { get; set; }
		public string SecureUrl { get; set; }
		public string Email { get; set; }
		public string AdminEmail { get; set; }
		public bool DisplayOutOfStock { get; set; }
	
		public FulfillmentCenter FulfillmentCenter { get; set; }
		public FulfillmentCenter ReturnsFulfillmentCenter { get; set; }
		public ICollection<string> Languages { get; set; }
		[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
		public ICollection<CurrencyCodes> Currencies { get; set; }
		public ICollection<Setting> Settings { get; set; }
		public ICollection<PaymentMethod> PaymentMethods { get; set; }
		public ICollection<ShippingMethod> ShippingMethods { get; set; }
		public ICollection<SeoInfo> SeoInfos { get; set; }
	}
}
