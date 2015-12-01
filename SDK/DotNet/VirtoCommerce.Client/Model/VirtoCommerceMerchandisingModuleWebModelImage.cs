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
    public class VirtoCommerceMerchandisingModuleWebModelImage : IEquatable<VirtoCommerceMerchandisingModuleWebModelImage>
    {
        
        /// <summary>
        /// Gets or sets the image file content as bytes array
        /// </summary>
        /// <value>Gets or sets the image file content as bytes array</value>
        [DataMember(Name="attachement", EmitDefaultValue=false)]
        public string Attachement { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value for image name
        /// </summary>
        /// <value>Gets or sets the value for image name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of image gorup name
        /// </summary>
        /// <value>Gets or sets the value of image gorup name</value>
        [DataMember(Name="group", EmitDefaultValue=false)]
        public string Group { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of image absolute URL
        /// </summary>
        /// <value>Gets or sets the value of image absolute URL</value>
        [DataMember(Name="src", EmitDefaultValue=false)]
        public string Src { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of thumbnail image absolute URL
        /// </summary>
        /// <value>Gets or sets the value of thumbnail image absolute URL</value>
        [DataMember(Name="thumbSrc", EmitDefaultValue=false)]
        public string ThumbSrc { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelImage {\n");
            sb.Append("  Attachement: ").Append(Attachement).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Group: ").Append(Group).Append("\n");
            sb.Append("  Src: ").Append(Src).Append("\n");
            sb.Append("  ThumbSrc: ").Append(ThumbSrc).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelImage);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelImage instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelImage to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelImage other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Attachement == other.Attachement ||
                    this.Attachement != null &&
                    this.Attachement.Equals(other.Attachement)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Group == other.Group ||
                    this.Group != null &&
                    this.Group.Equals(other.Group)
                ) && 
                (
                    this.Src == other.Src ||
                    this.Src != null &&
                    this.Src.Equals(other.Src)
                ) && 
                (
                    this.ThumbSrc == other.ThumbSrc ||
                    this.ThumbSrc != null &&
                    this.ThumbSrc.Equals(other.ThumbSrc)
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
                
                if (this.Attachement != null)
                    hash = hash * 57 + this.Attachement.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 57 + this.Name.GetHashCode();
                
                if (this.Group != null)
                    hash = hash * 57 + this.Group.GetHashCode();
                
                if (this.Src != null)
                    hash = hash * 57 + this.Src.GetHashCode();
                
                if (this.ThumbSrc != null)
                    hash = hash * 57 + this.ThumbSrc.GetHashCode();
                
                return hash;
            }
        }

    }


}
