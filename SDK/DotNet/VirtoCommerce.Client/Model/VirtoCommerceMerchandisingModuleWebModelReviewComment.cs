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
    public class VirtoCommerceMerchandisingModuleWebModelReviewComment : IEquatable<VirtoCommerceMerchandisingModuleWebModelReviewComment>
    {
        
        /// <summary>
        /// Gets or sets the value of review comment abuse reports count
        /// </summary>
        /// <value>Gets or sets the value of review comment abuse reports count</value>
        [DataMember(Name="abuseCount", EmitDefaultValue=false)]
        public int? AbuseCount { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review comment author name
        /// </summary>
        /// <value>Gets or sets the value of review comment author name</value>
        [DataMember(Name="author", EmitDefaultValue=false)]
        public string Author { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review comment text content
        /// </summary>
        /// <value>Gets or sets the value of review comment text content</value>
        [DataMember(Name="comment", EmitDefaultValue=false)]
        public string Comment { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review comment created date/time
        /// </summary>
        /// <value>Gets or sets the value of review comment created date/time</value>
        [DataMember(Name="createdDateTime", EmitDefaultValue=false)]
        public DateTime? CreatedDateTime { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review comment id
        /// </summary>
        /// <value>Gets or sets the value of review comment id</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review comment negative feedback count
        /// </summary>
        /// <value>Gets or sets the value of review comment negative feedback count</value>
        [DataMember(Name="negativeFeedbackCount", EmitDefaultValue=false)]
        public int? NegativeFeedbackCount { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review comment positive feedback count
        /// </summary>
        /// <value>Gets or sets the value of review comment positive feedback count</value>
        [DataMember(Name="positiveFeedbackCount", EmitDefaultValue=false)]
        public int? PositiveFeedbackCount { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelReviewComment {\n");
            sb.Append("  AbuseCount: ").Append(AbuseCount).Append("\n");
            sb.Append("  Author: ").Append(Author).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  CreatedDateTime: ").Append(CreatedDateTime).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  NegativeFeedbackCount: ").Append(NegativeFeedbackCount).Append("\n");
            sb.Append("  PositiveFeedbackCount: ").Append(PositiveFeedbackCount).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelReviewComment);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelReviewComment instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelReviewComment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelReviewComment other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.AbuseCount == other.AbuseCount ||
                    this.AbuseCount != null &&
                    this.AbuseCount.Equals(other.AbuseCount)
                ) && 
                (
                    this.Author == other.Author ||
                    this.Author != null &&
                    this.Author.Equals(other.Author)
                ) && 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
                ) && 
                (
                    this.CreatedDateTime == other.CreatedDateTime ||
                    this.CreatedDateTime != null &&
                    this.CreatedDateTime.Equals(other.CreatedDateTime)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.NegativeFeedbackCount == other.NegativeFeedbackCount ||
                    this.NegativeFeedbackCount != null &&
                    this.NegativeFeedbackCount.Equals(other.NegativeFeedbackCount)
                ) && 
                (
                    this.PositiveFeedbackCount == other.PositiveFeedbackCount ||
                    this.PositiveFeedbackCount != null &&
                    this.PositiveFeedbackCount.Equals(other.PositiveFeedbackCount)
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
                
                if (this.AbuseCount != null)
                    hash = hash * 57 + this.AbuseCount.GetHashCode();
                
                if (this.Author != null)
                    hash = hash * 57 + this.Author.GetHashCode();
                
                if (this.Comment != null)
                    hash = hash * 57 + this.Comment.GetHashCode();
                
                if (this.CreatedDateTime != null)
                    hash = hash * 57 + this.CreatedDateTime.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                if (this.NegativeFeedbackCount != null)
                    hash = hash * 57 + this.NegativeFeedbackCount.GetHashCode();
                
                if (this.PositiveFeedbackCount != null)
                    hash = hash * 57 + this.PositiveFeedbackCount.GetHashCode();
                
                return hash;
            }
        }

    }


}
