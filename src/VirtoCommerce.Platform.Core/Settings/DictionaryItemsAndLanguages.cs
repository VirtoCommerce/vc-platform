using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Settings;

public class DictionaryItemsAndLanguages
{
    public IList<DictionaryItem> Items { get; set; } = new List<DictionaryItem>();
    public IList<string> Languages { get; set; } = new List<string>();
}
