using System;
using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Tax.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Domain.Store.Model
{
    public class Store : AuditableEntity, IHasDynamicProperties, IHaveSettings, ISeoSupport
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Store current state (Open, Closed, RestrictedAccess)
        /// </summary>
        public StoreState StoreState { get; set; }

        public string TimeZone { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string DefaultLanguage { get; set; }

        public string DefaultCurrency { get; set; }
        /// <summary>
        /// Catalog id used as primary store catalog
        /// </summary>
        public string Catalog { get; set; }
        public bool CreditCardSavePolicy { get; set; }
        /// <summary>
        /// Store storefront url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Store storefront https url
        /// </summary>
        public string SecureUrl { get; set; }
        /// <summary>
        /// Primary store contact email can be used for store event notifications and for feed back
        /// </summary>
        public string Email { get; set; }
        public string AdminEmail { get; set; }
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
        /// All store trusted groups (group of stores that shared the user logins)
        /// </summary>
        public ICollection<string> TrustedGroups { get; set; }

        public ICollection<TaxProvider> TaxProviders { get; set; }
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
        public ICollection<ShippingMethod> ShippingMethods { get; set; }

        #region ISeoSupport Members
        public string SeoObjectType { get { return GetType().Name; } }
        public ICollection<SeoInfo> SeoInfos { get; set; }
        #endregion

        #region IHasDynamicProperties Members
        public string ObjectType { get; set; }
        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
        #endregion

        #region IHaveSettings Members
        public ICollection<SettingEntry> Settings { get; set; }
        #endregion
    }
}
