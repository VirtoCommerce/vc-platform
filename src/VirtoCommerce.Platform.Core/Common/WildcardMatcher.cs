using System.IO.Enumeration;

namespace VirtoCommerce.Platform.Core.Common
{
    /// <summary>
    /// Matches a value against a mask, where '*' stands for any sequence of characters
    /// and '?' for exactly one. The whole value must match the mask:
    /// 'catalog:*' matches 'catalog:read' but not 'catalog'.
    /// </summary>
    public static class WildcardMatcher
    {
        /// <summary>
        /// Returns true when the value matches the mask. Matching is case-insensitive.
        /// A null or blank mask, or a null value, never matches.
        /// </summary>
        public static bool IsMatch(string pattern, string value)
        {
            if (string.IsNullOrWhiteSpace(pattern) || value is null)
            {
                return false;
            }

            // The BCL matcher is span-based and allocation-free, and needs no pattern cache.
            // Preferred over a regex built from configuration: there is no expression to compile
            // and no backtracking to guard against.
            return FileSystemName.MatchesSimpleExpression(pattern, value, ignoreCase: true);
        }
    }
}
