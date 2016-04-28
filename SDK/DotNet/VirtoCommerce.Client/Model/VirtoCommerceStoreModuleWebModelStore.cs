using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Client.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceStoreModuleWebModelStore :  IEquatable<VirtoCommerceStoreModuleWebModelStore>
    {
        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Url of store storefront, required
        /// </summary>
        /// <value>Url of store storefront, required</value>
        [DataMember(Name="url", EmitDefaultValue=false)]
        public string Url { get; set; }

        /// <summary>
        /// State of store
        /// </summary>
        /// <value>State of store</value>
        [DataMember(Name="storeState", EmitDefaultValue=false)]
        public string StoreState { get; set; }

        /// <summary>
        /// Gets or Sets TimeZone
        /// </summary>
        [DataMember(Name="timeZone", EmitDefaultValue=false)]
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or Sets Country
        /// </summary>
        [DataMember(Name="country", EmitDefaultValue=false)]
        public string Country { get; set; }

        /// <summary>
        /// Gets or Sets Region
        /// </summary>
        [DataMember(Name="region", EmitDefaultValue=false)]
        public string Region { get; set; }

        /// <summary>
        /// Default locale of store
        /// </summary>
        /// <value>Default locale of store</value>
        [DataMember(Name="defaultLanguage", EmitDefaultValue=false)]
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Default currency of store. Use ISO 4217 currency codes
        /// </summary>
        /// <value>Default currency of store. Use ISO 4217 currency codes</value>
        [DataMember(Name="defaultCurrency", EmitDefaultValue=false)]
        public string DefaultCurrency { get; set; }

        /// <summary>
        /// Product catalog id of store
        /// </summary>
        /// <value>Product catalog id of store</value>
        [DataMember(Name="catalog", EmitDefaultValue=false)]
        public string Catalog { get; set; }

        /// <summary>
        /// Gets or Sets CreditCardSavePolicy
        /// </summary>
        [DataMember(Name="creditCardSavePolicy", EmitDefaultValue=false)]
        public bool? CreditCardSavePolicy { get; set; }

        /// <summary>
        /// Secure url of store, must use https protocol, required
        /// </summary>
        /// <value>Secure url of store, must use https protocol, required</value>
        [DataMember(Name="secureUrl", EmitDefaultValue=false)]
        public string SecureUrl { get; set; }

        /// <summary>
        /// Contact email of store
        /// </summary>
        /// <value>Contact email of store</value>
        [DataMember(Name="email", EmitDefaultValue=false)]
        public string Email { get; set; }

        /// <summary>
        /// Administrator contact email of store
        /// </summary>
        /// <value>Administrator contact email of store</value>
        [DataMember(Name="adminEmail", EmitDefaultValue=false)]
        public string AdminEmail { get; set; }

        /// <summary>
        /// If true - store shows product with status out of stock
        /// </summary>
        /// <value>If true - store shows product with status out of stock</value>
        [DataMember(Name="displayOutOfStock", EmitDefaultValue=false)]
        public bool? DisplayOutOfStock { get; set; }

        /// <summary>
        /// Gets or Sets FulfillmentCenter
        /// </summary>
        [DataMember(Name="fulfillmentCenter", EmitDefaultValue=false)]
        public VirtoCommerceStoreModuleWebModelFulfillmentCenter FulfillmentCenter { get; set; }

        /// <summary>
        /// Gets or Sets ReturnsFulfillmentCenter
        /// </summary>
        [DataMember(Name="returnsFulfillmentCenter", EmitDefaultValue=false)]
        public VirtoCommerceStoreModuleWebModelFulfillmentCenter ReturnsFulfillmentCenter { get; set; }

        /// <summary>
        /// All store supported languages
        /// </summary>
        /// <value>All store supported languages</value>
        [DataMember(Name="languages", EmitDefaultValue=false)]
        public List<string> Languages { get; set; }

        /// <summary>
        /// All store supported currencies
        /// </summary>
        /// <value>All store supported currencies</value>
        [DataMember(Name="currencies", EmitDefaultValue=false)]
        public List<string> Currencies { get; set; }

        /// <summary>
        /// All linked stores (their accounts can be reused here)
        /// </summary>
        /// <value>All linked stores (their accounts can be reused here)</value>
        [DataMember(Name="trustedGroups", EmitDefaultValue=false)]
        public List<string> TrustedGroups { get; set; }

        /// <summary>
        /// Gets or Sets PaymentMethods
        /// </summary>
        [DataMember(Name="paymentMethods", EmitDefaultValue=false)]
        public List<VirtoCommerceStoreModuleWebModelPaymentMethod> PaymentMethods { get; set; }

        /// <summary>
        /// Gets or Sets ShippingMethods
        /// </summary>
        [DataMember(Name="shippingMethods", EmitDefaultValue=false)]
        public List<VirtoCommerceStoreModuleWebModelShippingMethod> ShippingMethods { get; set; }

        /// <summary>
        /// Gets or Sets TaxProviders
        /// </summary>
        [DataMember(Name="taxProviders", EmitDefaultValue=false)]
        public List<VirtoCommerceStoreModuleWebModelTaxProvider> TaxProviders { get; set; }

        /// <summary>
        /// Gets or Sets SecurityScopes
        /// </summary>
        [DataMember(Name="securityScopes", EmitDefaultValue=false)]
        public List<string> SecurityScopes { get; set; }

        /// <summary>
        /// Gets or Sets SeoObjectType
        /// </summary>
        [DataMember(Name="seoObjectType", EmitDefaultValue=false)]
        public string SeoObjectType { get; private set; }

        /// <summary>
        /// Gets or Sets SeoInfos
        /// </summary>
        [DataMember(Name="seoInfos", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommerceModelSeoInfo> SeoInfos { get; set; }

        /// <summary>
        /// Gets or Sets ObjectType
        /// </summary>
        [DataMember(Name="objectType", EmitDefaultValue=false)]
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or Sets DynamicProperties
        /// </summary>
        [DataMember(Name="dynamicProperties", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty> DynamicProperties { get; set; }

        /// <summary>
        /// Gets or Sets Settings
        /// </summary>
        [DataMember(Name="settings", EmitDefaultValue=false)]
        public List<VirtoCommerceStoreModuleWebModelSetting> Settings { get; set; }

        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        [DataMember(Name="createdDate", EmitDefaultValue=false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="createdBy", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceStoreModuleWebModelStore {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("  StoreState: ").Append(StoreState).Append("\n");
            sb.Append("  TimeZone: ").Append(TimeZone).Append("\n");
            sb.Append("  Country: ").Append(Country).Append("\n");
            sb.Append("  Region: ").Append(Region).Append("\n");
            sb.Append("  DefaultLanguage: ").Append(DefaultLanguage).Append("\n");
            sb.Append("  DefaultCurrency: ").Append(DefaultCurrency).Append("\n");
            sb.Append("  Catalog: ").Append(Catalog).Append("\n");
            sb.Append("  CreditCardSavePolicy: ").Append(CreditCardSavePolicy).Append("\n");
            sb.Append("  SecureUrl: ").Append(SecureUrl).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  AdminEmail: ").Append(AdminEmail).Append("\n");
            sb.Append("  DisplayOutOfStock: ").Append(DisplayOutOfStock).Append("\n");
            sb.Append("  FulfillmentCenter: ").Append(FulfillmentCenter).Append("\n");
            sb.Append("  ReturnsFulfillmentCenter: ").Append(ReturnsFulfillmentCenter).Append("\n");
            sb.Append("  Languages: ").Append(Languages).Append("\n");
            sb.Append("  Currencies: ").Append(Currencies).Append("\n");
            sb.Append("  TrustedGroups: ").Append(TrustedGroups).Append("\n");
            sb.Append("  PaymentMethods: ").Append(PaymentMethods).Append("\n");
            sb.Append("  ShippingMethods: ").Append(ShippingMethods).Append("\n");
            sb.Append("  TaxProviders: ").Append(TaxProviders).Append("\n");
            sb.Append("  SecurityScopes: ").Append(SecurityScopes).Append("\n");
            sb.Append("  SeoObjectType: ").Append(SeoObjectType).Append("\n");
            sb.Append("  SeoInfos: ").Append(SeoInfos).Append("\n");
            sb.Append("  ObjectType: ").Append(ObjectType).Append("\n");
            sb.Append("  DynamicProperties: ").Append(DynamicProperties).Append("\n");
            sb.Append("  Settings: ").Append(Settings).Append("\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
            sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as VirtoCommerceStoreModuleWebModelStore);
        }

        /// <summary>
        /// Returns true if VirtoCommerceStoreModuleWebModelStore instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceStoreModuleWebModelStore to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceStoreModuleWebModelStore other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.Url == other.Url ||
                    this.Url != null &&
                    this.Url.Equals(other.Url)
                ) && 
                (
                    this.StoreState == other.StoreState ||
                    this.StoreState != null &&
                    this.StoreState.Equals(other.StoreState)
                ) && 
                (
                    this.TimeZone == other.TimeZone ||
                    this.TimeZone != null &&
                    this.TimeZone.Equals(other.TimeZone)
                ) && 
                (
                    this.Country == other.Country ||
                    this.Country != null &&
                    this.Country.Equals(other.Country)
                ) && 
                (
                    this.Region == other.Region ||
                    this.Region != null &&
                    this.Region.Equals(other.Region)
                ) && 
                (
                    this.DefaultLanguage == other.DefaultLanguage ||
                    this.DefaultLanguage != null &&
                    this.DefaultLanguage.Equals(other.DefaultLanguage)
                ) && 
                (
                    this.DefaultCurrency == other.DefaultCurrency ||
                    this.DefaultCurrency != null &&
                    this.DefaultCurrency.Equals(other.DefaultCurrency)
                ) && 
                (
                    this.Catalog == other.Catalog ||
                    this.Catalog != null &&
                    this.Catalog.Equals(other.Catalog)
                ) && 
                (
                    this.CreditCardSavePolicy == other.CreditCardSavePolicy ||
                    this.CreditCardSavePolicy != null &&
                    this.CreditCardSavePolicy.Equals(other.CreditCardSavePolicy)
                ) && 
                (
                    this.SecureUrl == other.SecureUrl ||
                    this.SecureUrl != null &&
                    this.SecureUrl.Equals(other.SecureUrl)
                ) && 
                (
                    this.Email == other.Email ||
                    this.Email != null &&
                    this.Email.Equals(other.Email)
                ) && 
                (
                    this.AdminEmail == other.AdminEmail ||
                    this.AdminEmail != null &&
                    this.AdminEmail.Equals(other.AdminEmail)
                ) && 
                (
                    this.DisplayOutOfStock == other.DisplayOutOfStock ||
                    this.DisplayOutOfStock != null &&
                    this.DisplayOutOfStock.Equals(other.DisplayOutOfStock)
                ) && 
                (
                    this.FulfillmentCenter == other.FulfillmentCenter ||
                    this.FulfillmentCenter != null &&
                    this.FulfillmentCenter.Equals(other.FulfillmentCenter)
                ) && 
                (
                    this.ReturnsFulfillmentCenter == other.ReturnsFulfillmentCenter ||
                    this.ReturnsFulfillmentCenter != null &&
                    this.ReturnsFulfillmentCenter.Equals(other.ReturnsFulfillmentCenter)
                ) && 
                (
                    this.Languages == other.Languages ||
                    this.Languages != null &&
                    this.Languages.SequenceEqual(other.Languages)
                ) && 
                (
                    this.Currencies == other.Currencies ||
                    this.Currencies != null &&
                    this.Currencies.SequenceEqual(other.Currencies)
                ) && 
                (
                    this.TrustedGroups == other.TrustedGroups ||
                    this.TrustedGroups != null &&
                    this.TrustedGroups.SequenceEqual(other.TrustedGroups)
                ) && 
                (
                    this.PaymentMethods == other.PaymentMethods ||
                    this.PaymentMethods != null &&
                    this.PaymentMethods.SequenceEqual(other.PaymentMethods)
                ) && 
                (
                    this.ShippingMethods == other.ShippingMethods ||
                    this.ShippingMethods != null &&
                    this.ShippingMethods.SequenceEqual(other.ShippingMethods)
                ) && 
                (
                    this.TaxProviders == other.TaxProviders ||
                    this.TaxProviders != null &&
                    this.TaxProviders.SequenceEqual(other.TaxProviders)
                ) && 
                (
                    this.SecurityScopes == other.SecurityScopes ||
                    this.SecurityScopes != null &&
                    this.SecurityScopes.SequenceEqual(other.SecurityScopes)
                ) && 
                (
                    this.SeoObjectType == other.SeoObjectType ||
                    this.SeoObjectType != null &&
                    this.SeoObjectType.Equals(other.SeoObjectType)
                ) && 
                (
                    this.SeoInfos == other.SeoInfos ||
                    this.SeoInfos != null &&
                    this.SeoInfos.SequenceEqual(other.SeoInfos)
                ) && 
                (
                    this.ObjectType == other.ObjectType ||
                    this.ObjectType != null &&
                    this.ObjectType.Equals(other.ObjectType)
                ) && 
                (
                    this.DynamicProperties == other.DynamicProperties ||
                    this.DynamicProperties != null &&
                    this.DynamicProperties.SequenceEqual(other.DynamicProperties)
                ) && 
                (
                    this.Settings == other.Settings ||
                    this.Settings != null &&
                    this.Settings.SequenceEqual(other.Settings)
                ) && 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.CreatedBy == other.CreatedBy ||
                    this.CreatedBy != null &&
                    this.CreatedBy.Equals(other.CreatedBy)
                ) && 
                (
                    this.ModifiedBy == other.ModifiedBy ||
                    this.ModifiedBy != null &&
                    this.ModifiedBy.Equals(other.ModifiedBy)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.Url != null)
                    hash = hash * 59 + this.Url.GetHashCode();

                if (this.StoreState != null)
                    hash = hash * 59 + this.StoreState.GetHashCode();

                if (this.TimeZone != null)
                    hash = hash * 59 + this.TimeZone.GetHashCode();

                if (this.Country != null)
                    hash = hash * 59 + this.Country.GetHashCode();

                if (this.Region != null)
                    hash = hash * 59 + this.Region.GetHashCode();

                if (this.DefaultLanguage != null)
                    hash = hash * 59 + this.DefaultLanguage.GetHashCode();

                if (this.DefaultCurrency != null)
                    hash = hash * 59 + this.DefaultCurrency.GetHashCode();

                if (this.Catalog != null)
                    hash = hash * 59 + this.Catalog.GetHashCode();

                if (this.CreditCardSavePolicy != null)
                    hash = hash * 59 + this.CreditCardSavePolicy.GetHashCode();

                if (this.SecureUrl != null)
                    hash = hash * 59 + this.SecureUrl.GetHashCode();

                if (this.Email != null)
                    hash = hash * 59 + this.Email.GetHashCode();

                if (this.AdminEmail != null)
                    hash = hash * 59 + this.AdminEmail.GetHashCode();

                if (this.DisplayOutOfStock != null)
                    hash = hash * 59 + this.DisplayOutOfStock.GetHashCode();

                if (this.FulfillmentCenter != null)
                    hash = hash * 59 + this.FulfillmentCenter.GetHashCode();

                if (this.ReturnsFulfillmentCenter != null)
                    hash = hash * 59 + this.ReturnsFulfillmentCenter.GetHashCode();

                if (this.Languages != null)
                    hash = hash * 59 + this.Languages.GetHashCode();

                if (this.Currencies != null)
                    hash = hash * 59 + this.Currencies.GetHashCode();

                if (this.TrustedGroups != null)
                    hash = hash * 59 + this.TrustedGroups.GetHashCode();

                if (this.PaymentMethods != null)
                    hash = hash * 59 + this.PaymentMethods.GetHashCode();

                if (this.ShippingMethods != null)
                    hash = hash * 59 + this.ShippingMethods.GetHashCode();

                if (this.TaxProviders != null)
                    hash = hash * 59 + this.TaxProviders.GetHashCode();

                if (this.SecurityScopes != null)
                    hash = hash * 59 + this.SecurityScopes.GetHashCode();

                if (this.SeoObjectType != null)
                    hash = hash * 59 + this.SeoObjectType.GetHashCode();

                if (this.SeoInfos != null)
                    hash = hash * 59 + this.SeoInfos.GetHashCode();

                if (this.ObjectType != null)
                    hash = hash * 59 + this.ObjectType.GetHashCode();

                if (this.DynamicProperties != null)
                    hash = hash * 59 + this.DynamicProperties.GetHashCode();

                if (this.Settings != null)
                    hash = hash * 59 + this.Settings.GetHashCode();

                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();

                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();

                if (this.CreatedBy != null)
                    hash = hash * 59 + this.CreatedBy.GetHashCode();

                if (this.ModifiedBy != null)
                    hash = hash * 59 + this.ModifiedBy.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
