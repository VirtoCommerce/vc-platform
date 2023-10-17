using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Localizations;

namespace VirtoCommerce.Platform.Data.Localizations;

public class LocalizedItemEntity : AuditableEntity, IDataEntity<LocalizedItemEntity, LocalizedItem>
{
    [StringLength(128)]
    public string Name { get; set; }

    [StringLength(512)]
    public string Alias { get; set; }

    [StringLength(512)]
    public string LanguageCode { get; set; }

    [StringLength(512)]
    public string Value { get; set; }

    public virtual LocalizedItem ToModel(LocalizedItem model)
    {
        model.Id = Id;

        model.CreatedDate = CreatedDate;
        model.ModifiedDate = ModifiedDate;
        model.CreatedBy = CreatedBy;
        model.ModifiedBy = ModifiedBy;

        model.Name = Name;
        model.Alias = Alias;
        model.LanguageCode = LanguageCode;
        model.Value = Value;

        return model;
    }

    public virtual LocalizedItemEntity FromModel(LocalizedItem model, PrimaryKeyResolvingMap pkMap)
    {
        pkMap.AddPair(model, this);

        Id = model.Id;

        CreatedDate = model.CreatedDate;
        ModifiedDate = model.ModifiedDate;
        CreatedBy = model.CreatedBy;
        ModifiedBy = model.ModifiedBy;

        Name = model.Name;
        Alias = model.Alias;
        LanguageCode = model.LanguageCode;
        Value = model.Value;

        return this;
    }

    public virtual void Patch(LocalizedItemEntity target)
    {
        target.Name = Name;
        target.Alias = Alias;
        target.LanguageCode = LanguageCode;
        target.Value = Value;
    }
}
