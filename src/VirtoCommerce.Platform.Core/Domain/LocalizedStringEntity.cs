using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Core.Common;

public abstract class LocalizedStringEntity<T> : Entity
    where T : Entity
{
    [Required]
    [StringLength(16)]
    public string LanguageCode { get; set; } = string.Empty; // e.g., "en-US"

    [Required]
    public string Value { get; set; } = string.Empty;

    public string ParentEntityId { get; set; } // Foreign key to the parent entity
    public virtual T ParentEntity { get; set; } = null!;
}
