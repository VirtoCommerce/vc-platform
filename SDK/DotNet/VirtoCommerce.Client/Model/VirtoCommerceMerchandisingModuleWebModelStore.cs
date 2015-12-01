using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class VirtoCommerceMerchandisingModuleWebModelStore : IEquatable<VirtoCommerceMerchandisingModuleWebModelStore>
    {
        
        /// <summary>
        /// Gets or sets the value of store catalog id
        /// </summary>
        /// <value>Gets or sets the value of store catalog id</value>
        [DataMember(Name="catalog", EmitDefaultValue=false)]
        public string Catalog { get; set; }
  
        
        /// <summary>
        /// Gets or sets the country name where store is located
        /// </summary>
        /// <value>Gets or sets the country name where store is located</value>
        [DataMember(Name="country", EmitDefaultValue=false)]
        public string Country { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of store currencies
        /// </summary>
        /// <value>Gets or sets the collection of store currencies</value>
        [DataMember(Name="currencies", EmitDefaultValue=false)]
        public List<string> Currencies { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of store default currency
        /// </summary>
        /// <value>Gets or sets the value of store default currency</value>
        [DataMember(Name="defaultCurrency", EmitDefaultValue=false)]
        public string DefaultCurrency { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of store default language
        /// </summary>
        /// <value>Gets or sets the value of store default language</value>
        [DataMember(Name="defaultLanguage", EmitDefaultValue=false)]
        public string DefaultLanguage { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of store description
        /// </summary>
        /// <value>Gets or sets the value of store description</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of store id
        /// </summary>
        /// <value>Gets or sets the value of store id</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of store language codes
        /// </summary>
        /// <value>Gets or sets the collection of store language codes</value>
        [DataMember(Name="languages", EmitDefaultValue=false)]
        public List<string> Languages { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of linked stores
        /// </summary>
        /// <value>Gets or sets the collection of linked stores</value>
        [DataMember(Name="linkedStores", EmitDefaultValue=false)]
        public List<string> LinkedStores { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of store name
        /// </summary>
        /// <value>Gets or sets the value of store name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Gets or sets the name of region where store is located
        /// </summary>
        /// <value>Gets or sets the name of region where store is located</value>
        [DataMember(Name="region", EmitDefaultValue=false)]
        public string Region { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of store secure absolute URL
        /// </summary>
        /// <value>Gets or sets the value of store secure absolute URL</value>
        [DataMember(Name="secureUrl", EmitDefaultValue=false)]
        public string SecureUrl { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of store SEO parameters
        /// </summary>
        /// <value>Gets or sets the collection of store SEO parameters</value>
        [DataMember(Name="seo", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelSeoKeyword> Seo { get; set; }
  
        
        /// <summary>
        /// Gets or sets the dictionary of store settings
        /// </summary>
        /// <value>Gets or sets the dictionary of store settings</value>
        [DataMember(Name="settings", EmitDefaultValue=false)]
        public Dictionary<string, Object> Settings { get; set; }
  
        
        /// <summary>
        /// Gets or sets the numeric value of store current state
        /// </summary>
        /// <value>Gets or sets the numeric value of store current state</value>
        [DataMember(Name="storeState", EmitDefaultValue=false)]
        public int? StoreState { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of store time zone
        /// </summary>
        /// <value>Gets or sets the value of store time zone</value>
        [DataMember(Name="timeZone", EmitDefaultValue=false)]
        public string TimeZone { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of store absolute URL (HTTP)
        /// </summary>
        /// <value>Gets or sets the value of store absolute URL (HTTP)</value>
        [DataMember(Name="url", EmitDefaultValue=false)]
        public string Url { get; set; }
  
        
        /// <summary>
        /// Gets or sets the string value of store current state
        /// </summary>
        /// <value>Gets or sets the string value of store current state</value>
        [DataMember(Name="state", EmitDefaultValue=false)]
        public string State { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of payment methods which are available for store
        /// </summary>
        /// <value>Gets or sets the collection of payment methods which are available for store</value>
        [DataMember(Name="paymentMethods", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelPaymentMethod> PaymentMethods { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelStore {\n");
            sb.Append("  Catalog: ").Append(Catalog).Append("\n");
            sb.Append("  Country: ").Append(Country).Append("\n");
            sb.Append("  Currencies: ").Append(Currencies).Append("\n");
            sb.Append("  DefaultCurrency: ").Append(DefaultCurrency).Append("\n");
            sb.Append("  DefaultLanguage: ").Append(DefaultLanguage).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Languages: ").Append(Languages).Append("\n");
            sb.Append("  LinkedStores: ").Append(LinkedStores).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Region: ").Append(Region).Append("\n");
            sb.Append("  SecureUrl: ").Append(SecureUrl).Append("\n");
            sb.Append("  Seo: ").Append(Seo).Append("\n");
            sb.Append("  Settings: ").Append(Settings).Append("\n");
            sb.Append("  StoreState: ").Append(StoreState).Append("\n");
            sb.Append("  TimeZone: ").Append(TimeZone).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("  State: ").Append(State).Append("\n");
            sb.Append("  PaymentMethods: ").Append(PaymentMethods).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelStore);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelStore instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelStore to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelStore other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Catalog == other.Catalog ||
                    this.Catalog != null &&
                    this.Catalog.Equals(other.Catalog)
                ) && 
                (
                    this.Country == other.Country ||
                    this.Country != null &&
                    this.Country.Equals(other.Country)
                ) && 
                (
                    this.Currencies == other.Currencies ||
                    this.Currencies != null &&
                    this.Currencies.SequenceEqual(other.Currencies)
                ) && 
                (
                    this.DefaultCurrency == other.DefaultCurrency ||
                    this.DefaultCurrency != null &&
                    this.DefaultCurrency.Equals(other.DefaultCurrency)
                ) && 
                (
                    this.DefaultLanguage == other.DefaultLanguage ||
                    this.DefaultLanguage != null &&
                    this.DefaultLanguage.Equals(other.DefaultLanguage)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.Languages == other.Languages ||
                    this.Languages != null &&
                    this.Languages.SequenceEqual(other.Languages)
                ) && 
                (
                    this.LinkedStores == other.LinkedStores ||
                    this.LinkedStores != null &&
                    this.LinkedStores.SequenceEqual(other.LinkedStores)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Region == other.Region ||
                    this.Region != null &&
                    this.Region.Equals(other.Region)
                ) && 
                (
                    this.SecureUrl == other.SecureUrl ||
                    this.SecureUrl != null &&
                    this.SecureUrl.Equals(other.SecureUrl)
                ) && 
                (
                    this.Seo == other.Seo ||
                    this.Seo != null &&
                    this.Seo.SequenceEqual(other.Seo)
                ) && 
                (
                    this.Settings == other.Settings ||
                    this.Settings != null &&
                    this.Settings.SequenceEqual(other.Settings)
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
                    this.Url == other.Url ||
                    this.Url != null &&
                    this.Url.Equals(other.Url)
                ) && 
                (
                    this.State == other.State ||
                    this.State != null &&
                    this.State.Equals(other.State)
                ) && 
                (
                    this.PaymentMethods == other.PaymentMethods ||
                    this.PaymentMethods != null &&
                    this.PaymentMethods.SequenceEqual(other.PaymentMethods)
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
                
                if (this.Catalog != null)
                    hash = hash * 57 + this.Catalog.GetHashCode();
                
                if (this.Country != null)
                    hash = hash * 57 + this.Country.GetHashCode();
                
                if (this.Currencies != null)
                    hash = hash * 57 + this.Currencies.GetHashCode();
                
                if (this.DefaultCurrency != null)
                    hash = hash * 57 + this.DefaultCurrency.GetHashCode();
                
                if (this.DefaultLanguage != null)
                    hash = hash * 57 + this.DefaultLanguage.GetHashCode();
                
                if (this.Description != null)
                    hash = hash * 57 + this.Description.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                if (this.Languages != null)
                    hash = hash * 57 + this.Languages.GetHashCode();
                
                if (this.LinkedStores != null)
                    hash = hash * 57 + this.LinkedStores.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 57 + this.Name.GetHashCode();
                
                if (this.Region != null)
                    hash = hash * 57 + this.Region.GetHashCode();
                
                if (this.SecureUrl != null)
                    hash = hash * 57 + this.SecureUrl.GetHashCode();
                
                if (this.Seo != null)
                    hash = hash * 57 + this.Seo.GetHashCode();
                
                if (this.Settings != null)
                    hash = hash * 57 + this.Settings.GetHashCode();
                
                if (this.StoreState != null)
                    hash = hash * 57 + this.StoreState.GetHashCode();
                
                if (this.TimeZone != null)
                    hash = hash * 57 + this.TimeZone.GetHashCode();
                
                if (this.Url != null)
                    hash = hash * 57 + this.Url.GetHashCode();
                
                if (this.State != null)
                    hash = hash * 57 + this.State.GetHashCode();
                
                if (this.PaymentMethods != null)
                    hash = hash * 57 + this.PaymentMethods.GetHashCode();
                
                return hash;
            }
        }

    }


}
