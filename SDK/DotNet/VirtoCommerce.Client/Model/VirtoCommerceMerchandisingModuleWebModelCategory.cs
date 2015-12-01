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
    public class VirtoCommerceMerchandisingModuleWebModelCategory : IEquatable<VirtoCommerceMerchandisingModuleWebModelCategory>
    {
        
        /// <summary>
        /// Gets or sets the value of category code
        /// </summary>
        /// <value>Gets or sets the value of category code</value>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of category id
        /// </summary>
        /// <value>Gets or sets the value of category id</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of category name
        /// </summary>
        /// <value>Gets or sets the value of category name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of category parents categories
        /// </summary>
        /// <value>Gets or sets the collection of category parents categories</value>
        [DataMember(Name="parents", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelCategory> Parents { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of category SEO parameters
        /// </summary>
        /// <value>Gets or sets the collection of category SEO parameters</value>
        [DataMember(Name="seo", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelSeoKeyword> Seo { get; set; }
  
        
        /// <summary>
        /// Gets or sets the category image
        /// </summary>
        /// <value>Gets or sets the category image</value>
        [DataMember(Name="image", EmitDefaultValue=false)]
        public VirtoCommerceMerchandisingModuleWebModelImage Image { get; set; }
  
        
        /// <summary>
        /// Gets or sets the flag of virtual category
        /// </summary>
        /// <value>Gets or sets the flag of virtual category</value>
        [DataMember(Name="virtual", EmitDefaultValue=false)]
        public bool? Virtual { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelCategory {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Parents: ").Append(Parents).Append("\n");
            sb.Append("  Seo: ").Append(Seo).Append("\n");
            sb.Append("  Image: ").Append(Image).Append("\n");
            sb.Append("  Virtual: ").Append(Virtual).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelCategory);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelCategory instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelCategory to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelCategory other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Parents == other.Parents ||
                    this.Parents != null &&
                    this.Parents.SequenceEqual(other.Parents)
                ) && 
                (
                    this.Seo == other.Seo ||
                    this.Seo != null &&
                    this.Seo.SequenceEqual(other.Seo)
                ) && 
                (
                    this.Image == other.Image ||
                    this.Image != null &&
                    this.Image.Equals(other.Image)
                ) && 
                (
                    this.Virtual == other.Virtual ||
                    this.Virtual != null &&
                    this.Virtual.Equals(other.Virtual)
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
                
                if (this.Code != null)
                    hash = hash * 57 + this.Code.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 57 + this.Name.GetHashCode();
                
                if (this.Parents != null)
                    hash = hash * 57 + this.Parents.GetHashCode();
                
                if (this.Seo != null)
                    hash = hash * 57 + this.Seo.GetHashCode();
                
                if (this.Image != null)
                    hash = hash * 57 + this.Image.GetHashCode();
                
                if (this.Virtual != null)
                    hash = hash * 57 + this.Virtual.GetHashCode();
                
                return hash;
            }
        }

    }


}
