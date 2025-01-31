using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Common;

public class LocalizedString : ValueObject
{
    public Dictionary<string, string> Values { get; } = [];

    public void SetValue(string languageCode, string value)
    {
        ArgumentNullException.ThrowIfNull(languageCode);
        Values[languageCode] = value;
    }

    public string GetValue(string languageCode)
    {
        ArgumentNullException.ThrowIfNull(languageCode);
        return Values.GetValueOrDefault(languageCode);
    }

    public bool TryGetValue(string languageCode, out string value)
    {
        ArgumentNullException.ThrowIfNull(languageCode);
        return Values.TryGetValue(languageCode, out value);
    }

    public void RemoveValue(string languageCode)
    {
        ArgumentNullException.ThrowIfNull(languageCode);
        Values.Remove(languageCode);
    }

    public bool Validate(IList<string> allowedLanguages, out IList<string> invalidLanguages)
    {
        invalidLanguages = Values.Keys.Where(key => !allowedLanguages.Contains(key)).ToList();
        return invalidLanguages.Count == 0;
    }

    public void Clean(IList<string> allowedLanguages)
    {
        var invalidKeys = Values.Keys.Where(key => !allowedLanguages.Contains(key));
        foreach (var key in invalidKeys)
        {
            Values.Remove(key);
        }
    }

    public override object Clone()
    {
        var clone = new LocalizedString();
        foreach (var kvp in Values)
        {
            clone.SetValue(kvp.Key, kvp.Value);
        }
        return clone;
    }

    public virtual object GetCopy() => Clone();
}
