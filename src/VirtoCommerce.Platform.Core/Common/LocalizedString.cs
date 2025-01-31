using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Common;

public class LocalizedString : ValueObject
{
    private readonly Dictionary<string, string> _values = [];

    public Dictionary<string, string> Values => _values;

    public void Set(string languageCode, string value)
    {
        ArgumentNullException.ThrowIfNull(languageCode);
        _values[languageCode] = value;
    }

    public string Get(string languageCode)
    {
        ArgumentNullException.ThrowIfNull(languageCode);
        return _values.TryGetValue(languageCode, out var value) ? value : null;
    }

    public bool TryGet(string languageCode, out string value)
    {
        ArgumentNullException.ThrowIfNull(languageCode);
        return _values.TryGetValue(languageCode, out value);
    }

    public void Remove(string languageCode)
    {
        ArgumentNullException.ThrowIfNull(languageCode);
        _values.Remove(languageCode);
    }

    public bool Validate(IList<string> allowedLanguages, out IList<string> invalidLanguages)
    {
        invalidLanguages = _values.Keys.Where(key => !allowedLanguages.Contains(key)).ToList();
        if (invalidLanguages.Count > 0)
        {
            return false;
        }
        return true;
    }

    public void Clean(IList<string> allowedLanguages)
    {
        var invalidKeys = _values.Keys.Where(key => !allowedLanguages.Contains(key));
        foreach (var key in invalidKeys)
        {
            _values.Remove(key);
        }
    }

    public override object Clone()
    {
        var clone = new LocalizedString();
        foreach (var kvp in _values)
        {
            clone.Set(kvp.Key, kvp.Value);
        }
        return clone;
    }

    public virtual object GetCopy() => Clone();
}
