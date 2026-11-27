using System.Text.RegularExpressions;

namespace VirtoCommerce.Platform.Data.Localizations;

internal static partial class TranslationLanguage
{
    /// <summary>
    /// A language is the first part of a translation file name ("en", "zh-CN", "sr-Latn-RS").
    /// It must never hold path separators, dots or wildcards, because it becomes part of a file search pattern.
    /// </summary>
    public static bool IsValid(string lang)
    {
        return !string.IsNullOrEmpty(lang) && LanguageRegex().IsMatch(lang);
    }

    [GeneratedRegex(@"^[A-Za-z]{2,8}(?:[-_][A-Za-z0-9]{1,8})*$", RegexOptions.CultureInvariant)]
    private static partial Regex LanguageRegex();
}
