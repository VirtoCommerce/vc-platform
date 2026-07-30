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
        public static bool IsMatch(string pattern, string value)
        {
            if (string.IsNullOrEmpty(pattern) && string.IsNullOrEmpty(value))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(pattern))
            {
                return false;
            }

            return FileSystemName.MatchesSimpleExpression(pattern, value, ignoreCase: true);
        }
    }
}
