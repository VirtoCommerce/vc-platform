using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Settings;

public class DictionaryItem
{
    public string Alias { get; set; }
    public IList<LocalizedValue> LocalizedValues { get; set; } = new List<LocalizedValue>();
}
