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
    public class VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroup : IEquatable<VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroup>
    {
        
        /// <summary>
        /// Gets or sets the value of dynamic content item group name
        /// </summary>
        /// <value>Gets or sets the value of dynamic content item group name</value>
        [DataMember(Name="groupName", EmitDefaultValue=false)]
        public string GroupName { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of dynamic content items for dynamic content item group
        /// </summary>
        /// <value>Gets or sets the collection of dynamic content items for dynamic content item group</value>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelDynamicContentItem> Items { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroup {\n");
            sb.Append("  GroupName: ").Append(GroupName).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroup);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroup instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroup to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroup other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.GroupName == other.GroupName ||
                    this.GroupName != null &&
                    this.GroupName.Equals(other.GroupName)
                ) && 
                (
                    this.Items == other.Items ||
                    this.Items != null &&
                    this.Items.SequenceEqual(other.Items)
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
                
                if (this.GroupName != null)
                    hash = hash * 57 + this.GroupName.GetHashCode();
                
                if (this.Items != null)
                    hash = hash * 57 + this.Items.GetHashCode();
                
                return hash;
            }
        }

    }


}
