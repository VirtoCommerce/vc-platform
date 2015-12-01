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
    public class VirtoCommerceMerchandisingModuleWebModelReview : IEquatable<VirtoCommerceMerchandisingModuleWebModelReview>
    {
        
        /// <summary>
        /// Gets or sets the value of review abuse reports count
        /// </summary>
        /// <value>Gets or sets the value of review abuse reports count</value>
        [DataMember(Name="abuseCount", EmitDefaultValue=false)]
        public int? AbuseCount { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review author id
        /// </summary>
        /// <value>Gets or sets the value of review author id</value>
        [DataMember(Name="authorId", EmitDefaultValue=false)]
        public string AuthorId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review author location
        /// </summary>
        /// <value>Gets or sets the value of review author location</value>
        [DataMember(Name="authorLocation", EmitDefaultValue=false)]
        public string AuthorLocation { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of Review author name
        /// </summary>
        /// <value>Gets or sets the value of Review author name</value>
        [DataMember(Name="authorName", EmitDefaultValue=false)]
        public string AuthorName { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of review comments
        /// </summary>
        /// <value>Gets or sets the collection of review comments</value>
        [DataMember(Name="comments", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelReviewComment> Comments { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review created date/time
        /// </summary>
        /// <value>Gets or sets the value of review created date/time</value>
        [DataMember(Name="created", EmitDefaultValue=false)]
        public DateTime? Created { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review id
        /// </summary>
        /// <value>Gets or sets the value of review id</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review negative feedback count
        /// </summary>
        /// <value>Gets or sets the value of review negative feedback count</value>
        [DataMember(Name="negativeFeedbackCount", EmitDefaultValue=false)]
        public int? NegativeFeedbackCount { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review positive feedback count
        /// </summary>
        /// <value>Gets or sets the value of review positive feedback count</value>
        [DataMember(Name="positiveFeedbackCount", EmitDefaultValue=false)]
        public int? PositiveFeedbackCount { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review rating
        /// </summary>
        /// <value>Gets or sets the value of review rating</value>
        [DataMember(Name="rating", EmitDefaultValue=false)]
        public int? Rating { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review rating comment
        /// </summary>
        /// <value>Gets or sets the value of review rating comment</value>
        [DataMember(Name="ratingComment", EmitDefaultValue=false)]
        public string RatingComment { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review text content
        /// </summary>
        /// <value>Gets or sets the value of review text content</value>
        [DataMember(Name="reviewText", EmitDefaultValue=false)]
        public string ReviewText { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of review total comments count
        /// </summary>
        /// <value>Gets or sets the value of review total comments count</value>
        [DataMember(Name="totalComments", EmitDefaultValue=false)]
        public int? TotalComments { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelReview {\n");
            sb.Append("  AbuseCount: ").Append(AbuseCount).Append("\n");
            sb.Append("  AuthorId: ").Append(AuthorId).Append("\n");
            sb.Append("  AuthorLocation: ").Append(AuthorLocation).Append("\n");
            sb.Append("  AuthorName: ").Append(AuthorName).Append("\n");
            sb.Append("  Comments: ").Append(Comments).Append("\n");
            sb.Append("  Created: ").Append(Created).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  NegativeFeedbackCount: ").Append(NegativeFeedbackCount).Append("\n");
            sb.Append("  PositiveFeedbackCount: ").Append(PositiveFeedbackCount).Append("\n");
            sb.Append("  Rating: ").Append(Rating).Append("\n");
            sb.Append("  RatingComment: ").Append(RatingComment).Append("\n");
            sb.Append("  ReviewText: ").Append(ReviewText).Append("\n");
            sb.Append("  TotalComments: ").Append(TotalComments).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelReview);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelReview instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelReview to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelReview other)
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
                    this.AuthorId == other.AuthorId ||
                    this.AuthorId != null &&
                    this.AuthorId.Equals(other.AuthorId)
                ) && 
                (
                    this.AuthorLocation == other.AuthorLocation ||
                    this.AuthorLocation != null &&
                    this.AuthorLocation.Equals(other.AuthorLocation)
                ) && 
                (
                    this.AuthorName == other.AuthorName ||
                    this.AuthorName != null &&
                    this.AuthorName.Equals(other.AuthorName)
                ) && 
                (
                    this.Comments == other.Comments ||
                    this.Comments != null &&
                    this.Comments.SequenceEqual(other.Comments)
                ) && 
                (
                    this.Created == other.Created ||
                    this.Created != null &&
                    this.Created.Equals(other.Created)
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
                ) && 
                (
                    this.Rating == other.Rating ||
                    this.Rating != null &&
                    this.Rating.Equals(other.Rating)
                ) && 
                (
                    this.RatingComment == other.RatingComment ||
                    this.RatingComment != null &&
                    this.RatingComment.Equals(other.RatingComment)
                ) && 
                (
                    this.ReviewText == other.ReviewText ||
                    this.ReviewText != null &&
                    this.ReviewText.Equals(other.ReviewText)
                ) && 
                (
                    this.TotalComments == other.TotalComments ||
                    this.TotalComments != null &&
                    this.TotalComments.Equals(other.TotalComments)
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
                
                if (this.AuthorId != null)
                    hash = hash * 57 + this.AuthorId.GetHashCode();
                
                if (this.AuthorLocation != null)
                    hash = hash * 57 + this.AuthorLocation.GetHashCode();
                
                if (this.AuthorName != null)
                    hash = hash * 57 + this.AuthorName.GetHashCode();
                
                if (this.Comments != null)
                    hash = hash * 57 + this.Comments.GetHashCode();
                
                if (this.Created != null)
                    hash = hash * 57 + this.Created.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                if (this.NegativeFeedbackCount != null)
                    hash = hash * 57 + this.NegativeFeedbackCount.GetHashCode();
                
                if (this.PositiveFeedbackCount != null)
                    hash = hash * 57 + this.PositiveFeedbackCount.GetHashCode();
                
                if (this.Rating != null)
                    hash = hash * 57 + this.Rating.GetHashCode();
                
                if (this.RatingComment != null)
                    hash = hash * 57 + this.RatingComment.GetHashCode();
                
                if (this.ReviewText != null)
                    hash = hash * 57 + this.ReviewText.GetHashCode();
                
                if (this.TotalComments != null)
                    hash = hash * 57 + this.TotalComments.GetHashCode();
                
                return hash;
            }
        }

    }


}
