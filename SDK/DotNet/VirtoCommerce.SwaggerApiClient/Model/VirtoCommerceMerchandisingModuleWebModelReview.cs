using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class VirtoCommerceMerchandisingModuleWebModelReview {
    
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
