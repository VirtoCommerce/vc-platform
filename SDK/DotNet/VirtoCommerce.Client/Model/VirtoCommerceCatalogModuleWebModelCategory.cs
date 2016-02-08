using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model
{

    /// <summary>
    /// Merchandising Category
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCatalogModuleWebModelCategory :  IEquatable<VirtoCommerceCatalogModuleWebModelCategory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceCatalogModuleWebModelCategory" /> class.
        /// </summary>
        public VirtoCommerceCatalogModuleWebModelCategory()
        {
            
        }

        
        /// <summary>
        /// Gets or sets the parent category id.
        /// </summary>
        /// <value>Gets or sets the parent category id.</value>
        [DataMember(Name="parentId", EmitDefaultValue=false)]
        public string ParentId { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }
  
        
        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Category} is virtual or common.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Category} is virtual or common.</value>
        [DataMember(Name="virtual", EmitDefaultValue=false)]
        public bool? Virtual { get; set; }
  
        
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>Gets or sets the code.</value>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }
  
        
        /// <summary>
        /// Gets or sets the type of the tax.
        /// </summary>
        /// <value>Gets or sets the type of the tax.</value>
        [DataMember(Name="taxType", EmitDefaultValue=false)]
        public string TaxType { get; set; }
  
        
        /// <summary>
        /// Gets or sets the catalog that this category belongs to.
        /// </summary>
        /// <value>Gets or sets the catalog that this category belongs to.</value>
        [DataMember(Name="catalog", EmitDefaultValue=false)]
        public VirtoCommerceCatalogModuleWebModelCatalog Catalog { get; set; }
  
        
        /// <summary>
        /// Gets or sets the catalog id that this category belongs to.
        /// </summary>
        /// <value>Gets or sets the catalog id that this category belongs to.</value>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>Gets or sets the name.</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Path
        /// </summary>
        [DataMember(Name="path", EmitDefaultValue=false)]
        public string Path { get; set; }
  
        
        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Category} is active.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Category} is active.</value>
        [DataMember(Name="isActive", EmitDefaultValue=false)]
        public bool? IsActive { get; set; }
  
        
        /// <summary>
        /// All parents categories
        /// </summary>
        /// <value>All parents categories</value>
        [DataMember(Name="parents", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelCategory> Parents { get; set; }
  
        
        /// <summary>
        /// Gets or sets the children categories.
        /// </summary>
        /// <value>Gets or sets the children categories.</value>
        [DataMember(Name="children", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelCategory> Children { get; set; }
  
        
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>Gets or sets the properties.</value>
        [DataMember(Name="properties", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelProperty> Properties { get; set; }
  
        
        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>Gets or sets the links.</value>
        [DataMember(Name="links", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelCategoryLink> Links { get; set; }
  
        
        /// <summary>
        /// Gets or sets the list of SEO information records.
        /// </summary>
        /// <value>Gets or sets the list of SEO information records.</value>
        [DataMember(Name="seoInfos", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommerceModelSeoInfo> SeoInfos { get; set; }
  
        
        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        /// <value>Gets or sets the images.</value>
        [DataMember(Name="images", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelImage> Images { get; set; }
  
        
        /// <summary>
        /// Gets or Sets SecurityScopes
        /// </summary>
        [DataMember(Name="securityScopes", EmitDefaultValue=false)]
        public List<string> SecurityScopes { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelCategory {\n");
            sb.Append("  ParentId: ").Append(ParentId).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Virtual: ").Append(Virtual).Append("\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  TaxType: ").Append(TaxType).Append("\n");
            sb.Append("  Catalog: ").Append(Catalog).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Path: ").Append(Path).Append("\n");
            sb.Append("  IsActive: ").Append(IsActive).Append("\n");
            sb.Append("  Parents: ").Append(Parents).Append("\n");
            sb.Append("  Children: ").Append(Children).Append("\n");
            sb.Append("  Properties: ").Append(Properties).Append("\n");
            sb.Append("  Links: ").Append(Links).Append("\n");
            sb.Append("  SeoInfos: ").Append(SeoInfos).Append("\n");
            sb.Append("  Images: ").Append(Images).Append("\n");
            sb.Append("  SecurityScopes: ").Append(SecurityScopes).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelCategory);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelCategory instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelCategory to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelCategory other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ParentId == other.ParentId ||
                    this.ParentId != null &&
                    this.ParentId.Equals(other.ParentId)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.Virtual == other.Virtual ||
                    this.Virtual != null &&
                    this.Virtual.Equals(other.Virtual)
                ) && 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
                (
                    this.TaxType == other.TaxType ||
                    this.TaxType != null &&
                    this.TaxType.Equals(other.TaxType)
                ) && 
                (
                    this.Catalog == other.Catalog ||
                    this.Catalog != null &&
                    this.Catalog.Equals(other.Catalog)
                ) && 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Path == other.Path ||
                    this.Path != null &&
                    this.Path.Equals(other.Path)
                ) && 
                (
                    this.IsActive == other.IsActive ||
                    this.IsActive != null &&
                    this.IsActive.Equals(other.IsActive)
                ) && 
                (
                    this.Parents == other.Parents ||
                    this.Parents != null &&
                    this.Parents.SequenceEqual(other.Parents)
                ) && 
                (
                    this.Children == other.Children ||
                    this.Children != null &&
                    this.Children.SequenceEqual(other.Children)
                ) && 
                (
                    this.Properties == other.Properties ||
                    this.Properties != null &&
                    this.Properties.SequenceEqual(other.Properties)
                ) && 
                (
                    this.Links == other.Links ||
                    this.Links != null &&
                    this.Links.SequenceEqual(other.Links)
                ) && 
                (
                    this.SeoInfos == other.SeoInfos ||
                    this.SeoInfos != null &&
                    this.SeoInfos.SequenceEqual(other.SeoInfos)
                ) && 
                (
                    this.Images == other.Images ||
                    this.Images != null &&
                    this.Images.SequenceEqual(other.Images)
                ) && 
                (
                    this.SecurityScopes == other.SecurityScopes ||
                    this.SecurityScopes != null &&
                    this.SecurityScopes.SequenceEqual(other.SecurityScopes)
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
                
                if (this.ParentId != null)
                    hash = hash * 59 + this.ParentId.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();
                
                if (this.Virtual != null)
                    hash = hash * 59 + this.Virtual.GetHashCode();
                
                if (this.Code != null)
                    hash = hash * 59 + this.Code.GetHashCode();
                
                if (this.TaxType != null)
                    hash = hash * 59 + this.TaxType.GetHashCode();
                
                if (this.Catalog != null)
                    hash = hash * 59 + this.Catalog.GetHashCode();
                
                if (this.CatalogId != null)
                    hash = hash * 59 + this.CatalogId.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();
                
                if (this.Path != null)
                    hash = hash * 59 + this.Path.GetHashCode();
                
                if (this.IsActive != null)
                    hash = hash * 59 + this.IsActive.GetHashCode();
                
                if (this.Parents != null)
                    hash = hash * 59 + this.Parents.GetHashCode();
                
                if (this.Children != null)
                    hash = hash * 59 + this.Children.GetHashCode();
                
                if (this.Properties != null)
                    hash = hash * 59 + this.Properties.GetHashCode();
                
                if (this.Links != null)
                    hash = hash * 59 + this.Links.GetHashCode();
                
                if (this.SeoInfos != null)
                    hash = hash * 59 + this.SeoInfos.GetHashCode();
                
                if (this.Images != null)
                    hash = hash * 59 + this.Images.GetHashCode();
                
                if (this.SecurityScopes != null)
                    hash = hash * 59 + this.SecurityScopes.GetHashCode();
                
                return hash;
            }
        }

    }


}
