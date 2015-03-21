using System;
using System.Linq;

namespace VirtoCommerce.Framework.Web.Common
{
    public static class AttributeExtensions
    {
        private static readonly string[] _emptyArray = new string[0];

        public static string[] SplitString(this Attribute attribute, string original, char separator)
        {
            var result = _emptyArray;

            if (!string.IsNullOrEmpty(original))
            {
                result = original.Split(separator)
                    .Select(part => part.Trim())
                    .Where(part => !string.IsNullOrEmpty(part))
                    .ToArray();
            }

            return result;
        }
    }
}
