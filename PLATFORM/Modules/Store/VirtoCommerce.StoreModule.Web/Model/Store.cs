using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Settings;
using coreModel = VirtoCommerce.Domain.Store.Model;

namespace VirtoCommerce.StoreModule.Web.Model
{
    public class Store : AuditableEntity, IHasDynamicProperties, ISeoSupport
    {
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Url of store storefront, required
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// State of store
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public coreModel.StoreState StoreState { get; set; }

        public string TimeZone { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        /// <summary>
        /// Default locale of store
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Default currency of store. Use ISO 4217 currency codes
        /// </summary>
        public string DefaultCurrency { get; set; }

        /// <summary>
        /// Product catalog id of store
        /// </summary>
        public string Catalog { get; set; }

        public bool CreditCardSavePolicy { get; set; }

        /// <summary>
        /// Secure url of store, must use https protocol, required
        /// </summary>
        public string SecureUrl { get; set; }

        /// <summary>
        /// Contact email of store
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Administrator contact email of store
        /// </summary>
        public string AdminEmail { get; set; }

        /// <summary>
        /// If true - store shows product with status out of stock
        /// </summary>
        public bool DisplayOutOfStock { get; set; }

        public FulfillmentCenter FulfillmentCenter { get; set; }

        public FulfillmentCenter ReturnsFulfillmentCenter { get; set; }

        /// <summary>
        /// All store supported languages
        /// </summary>
        public ICollection<string> Languages { get; set; }

        /// <summary>
        /// All store supported currencies
        /// </summary>
        public ICollection<string> Currencies { get; set; }

        /// <summary>
        /// All linked stores (their accounts can be reused here)
        /// </summary>
        public ICollection<string> TrustedGroups { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; }

        public ICollection<ShippingMethod> ShippingMethods { get; set; }

        public ICollection<TaxProvider> TaxProviders { get; set; }

        public string[] SecurityScopes { get; set; }

        #region ISeoSupport Members
        public string SeoObjectType { get { return GetType().Name; } }
        public ICollection<SeoInfo> SeoInfos { get; set; }
        #endregion

        #region IHasDynamicProperties Members
        public string ObjectType { get; set; }
        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
        #endregion

        public ICollection<Setting> Settings { get; set; }


    }
}
