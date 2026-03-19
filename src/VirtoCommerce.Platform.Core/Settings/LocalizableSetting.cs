using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Settings;

public class LocalizableSetting
{
    public string Name { get; set; }
    public bool IsLocalizable { get; set; }
    public IList<DictionaryItem> Items { get; set; } = new List<DictionaryItem>();
}
