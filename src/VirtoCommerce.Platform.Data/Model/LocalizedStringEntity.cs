using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.Model;

public abstract class LocalizedStringEntity<T> : Entity, IHasLanguageCode
    where T : Entity
{
    [Required]
    [StringLength(DbContextBase.LanguageCodeLength)]
    public string LanguageCode { get; set; } = string.Empty; // e.g., "en-US"

    [Required]
    public string Value { get; set; } = string.Empty;

    public string ParentEntityId { get; set; } // Foreign key to the parent entity
    public virtual T ParentEntity { get; set; } = null!;
}
