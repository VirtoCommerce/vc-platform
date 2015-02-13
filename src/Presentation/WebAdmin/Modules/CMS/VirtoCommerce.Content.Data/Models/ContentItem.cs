namespace VirtoCommerce.Content.Data.Models
{
    #region

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using VirtoCommerce.Foundation.Frameworks;

    #endregion

    public class ContentItem : Entity
    {
        #region Public Properties

        [Required]
        public string Content { get; set; }

        [NotMapped]
        public ContentType ContentType
        {
            get
            {
                return (ContentType)Enum.Parse(typeof(ContentType), this.Type);
            }
            set
            {
                this.Type = value.ToString();
            }
        }

        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [StringLength(64)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ContentItem ParentContentItem { get; set; }

        [StringLength(128)]
        public string ParentContentItemId { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Type { get; set; }

        #endregion
    }
}