using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class TreeExtension
    {
        public static IEnumerable<T> Traverse<T>(this T node, Func<T, IEnumerable<T>> childrenFor)
        {
            yield return node;

            var childNodes = childrenFor(node);
            if (childNodes != null)
            {
                foreach (var childNode in childNodes.SelectMany(n => n.Traverse(childrenFor)))
                {
                    yield return childNode;
                }
            }
        }

        public static IEnumerable<TItem> GetAncestors<TItem>(TItem item, Func<TItem, TItem> getParentFunc)
        {
            if (getParentFunc == null)
            {
                throw new ArgumentNullException(nameof(getParentFunc));
            }
            if (ReferenceEquals(item, null)) yield break;
            for (TItem curItem = getParentFunc(item); !ReferenceEquals(curItem, null); curItem = getParentFunc(curItem))
            {
                yield return curItem;
            }
        }

    }
}
