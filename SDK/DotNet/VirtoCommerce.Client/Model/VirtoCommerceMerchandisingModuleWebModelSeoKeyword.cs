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
    public class VirtoCommerceMerchandisingModuleWebModelSeoKeyword : IEquatable<VirtoCommerceMerchandisingModuleWebModelSeoKeyword>
    {
        
        /// <summary>
        /// Gets or sets the value of image alternative description
        /// </summary>
        /// <value>Gets or sets the value of image alternative description</value>
        [DataMember(Name="imageAltDescription", EmitDefaultValue=false)]
        public string ImageAltDescription { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of SEO keyword
        /// </summary>
        /// <value>Gets or sets the value of SEO keyword</value>
        [DataMember(Name="keyword", EmitDefaultValue=false)]
        public string Keyword { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of SEO language
        /// </summary>
        /// <value>Gets or sets the value of SEO language</value>
        [DataMember(Name="language", EmitDefaultValue=false)]
        public string Language { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of webpage meta description
        /// </summary>
        /// <value>Gets or sets the value of webpage meta description</value>
        [DataMember(Name="metaDescription", EmitDefaultValue=false)]
        public string MetaDescription { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of webpage meta keywords
        /// </summary>
        /// <value>Gets or sets the value of webpage meta keywords</value>
        [DataMember(Name="metaKeywords", EmitDefaultValue=false)]
        public string MetaKeywords { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of webpage title
        /// </summary>
        /// <value>Gets or sets the value of webpage title</value>
        [DataMember(Name="title", EmitDefaultValue=false)]
        public string Title { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelSeoKeyword {\n");
            sb.Append("  ImageAltDescription: ").Append(ImageAltDescription).Append("\n");
            sb.Append("  Keyword: ").Append(Keyword).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  MetaDescription: ").Append(MetaDescription).Append("\n");
            sb.Append("  MetaKeywords: ").Append(MetaKeywords).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelSeoKeyword);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelSeoKeyword instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelSeoKeyword to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelSeoKeyword other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ImageAltDescription == other.ImageAltDescription ||
                    this.ImageAltDescription != null &&
                    this.ImageAltDescription.Equals(other.ImageAltDescription)
                ) && 
                (
                    this.Keyword == other.Keyword ||
                    this.Keyword != null &&
                    this.Keyword.Equals(other.Keyword)
                ) && 
                (
                    this.Language == other.Language ||
                    this.Language != null &&
                    this.Language.Equals(other.Language)
                ) && 
                (
                    this.MetaDescription == other.MetaDescription ||
                    this.MetaDescription != null &&
                    this.MetaDescription.Equals(other.MetaDescription)
                ) && 
                (
                    this.MetaKeywords == other.MetaKeywords ||
                    this.MetaKeywords != null &&
                    this.MetaKeywords.Equals(other.MetaKeywords)
                ) && 
                (
                    this.Title == other.Title ||
                    this.Title != null &&
                    this.Title.Equals(other.Title)
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
                
                if (this.ImageAltDescription != null)
                    hash = hash * 57 + this.ImageAltDescription.GetHashCode();
                
                if (this.Keyword != null)
                    hash = hash * 57 + this.Keyword.GetHashCode();
                
                if (this.Language != null)
                    hash = hash * 57 + this.Language.GetHashCode();
                
                if (this.MetaDescription != null)
                    hash = hash * 57 + this.MetaDescription.GetHashCode();
                
                if (this.MetaKeywords != null)
                    hash = hash * 57 + this.MetaKeywords.GetHashCode();
                
                if (this.Title != null)
                    hash = hash * 57 + this.Title.GetHashCode();
                
                return hash;
            }
        }

    }


}
