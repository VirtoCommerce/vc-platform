using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Localizations;

public class LocalizedItem : AuditableEntity, ICloneable
{
    public string Name { get; set; }
    public string Alias { get; set; }
    public string LanguageCode { get; set; }
    public string Value { get; set; }

    public object Clone()
    {
        return (LocalizedItem)MemberwiseClone();
    }
}
