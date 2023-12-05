using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Settings;

public class LocalizableSettingsAndLanguages
{
    public IList<LocalizableSetting> Settings { get; set; } = new List<LocalizableSetting>();
    public IList<string> Languages { get; set; } = new List<string>();
}
