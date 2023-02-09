/*--------------------------------------------------------------------------
* AnonymousComparer - lambda compare selector for Linq
* ver 1.3.0.0 (Oct. 18th, 2010)
*
* created and maintained by neuecc <ils@neue.cc>
* licensed under Microsoft Public License(Ms-PL)
* http://neue.cc/
* http://linqcomparer.codeplex.com/
*--------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class AnonymousComparer
    {
        #region IComparer<T>

        /// <summary>Example:AnonymousComparer.Create&lt;int&gt;((x, y) => y - x)</summary>
        public static IComparer<T> Create<T>(Func<T, T, int> compare) =>
            compare == null ? throw new ArgumentNullException(nameof(compare)) : new Comparer<T>(compare);

        private sealed class Comparer<T> : IComparer<T>
        {
            private readonly Func<T, T, int> _compare;

            public Comparer(Func<T, T, int> compare)
            {
                _compare = compare;
            }

            public int Compare(T x, T y) => _compare(x, y);
        }

        #endregion

        #region IEqualityComparer<T>

        /// <summary>
        /// Examples:
        /// AnonymousComparer.Create((MyClass mc) => mc.MyProperty)
        /// </summary>
        public static IEqualityComparer<T> Create<T, TKey>(Func<T, TKey> compareKeySelector) =>
            compareKeySelector == null
                ? throw new ArgumentNullException(nameof(compareKeySelector))
                : new EqualityComparer<T>(
                    (x, y) =>
                    {
                        if (ReferenceEquals(null, x) || ReferenceEquals(null, y))
                        {
                            return false;
                        }

                        if (ReferenceEquals(x, y))
                        {
                            return true;
                        }

                        return compareKeySelector(x).Equals(compareKeySelector(y));
                    },
                    obj =>
                    {
                        if (obj == null)
                        {
                            return 0;
                        }

                        var compareKey = compareKeySelector(obj);
                        if (compareKey == null)
                        {
                            return 0;
                        }

                        return compareKey.GetHashCode();
                    });

        /// <summary>
        /// Examples:
        /// AnonymousComparer.Create((MyClass mc) => mc.MyStringProperty, StringComparer.OrdinalIgnoreCase)
        /// </summary>
        public static IEqualityComparer<T> Create<T, TKey>(Func<T, TKey> compareKeySelector, IEqualityComparer<TKey> keyEqualityComparer) =>
            compareKeySelector == null
                ? throw new ArgumentNullException(nameof(compareKeySelector))
                : new EqualityComparer<T>(
                    (x, y) =>
                    {
                        if (ReferenceEquals(null, x) || ReferenceEquals(null, y))
                        {
                            return false;
                        }

                        if (ReferenceEquals(x, y))
                        {
                            return true;
                        }

                        return keyEqualityComparer.Equals(compareKeySelector(x), compareKeySelector(y));
                    },
                    obj =>
                    {
                        if (obj == null)
                        {
                            return 0;
                        }

                        var compareKey = compareKeySelector(obj);
                        if (compareKey == null)
                        {
                            return 0;
                        }

                        return keyEqualityComparer.GetHashCode(compareKey);
                    });

        public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            if (equals == null)
            {
                throw new ArgumentNullException(nameof(equals));
            }

            if (getHashCode == null)
            {
                throw new ArgumentNullException(nameof(getHashCode));
            }

            return new EqualityComparer<T>(equals, getHashCode);
        }

        private sealed class EqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equals;
            private readonly Func<T, int> _getHashCode;

            public EqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
            {
                _equals = equals;
                _getHashCode = getHashCode;
            }

            public bool Equals(T x, T y) => _equals(x, y);

            public int GetHashCode(T obj) => _getHashCode(obj);
        }

        #endregion
    }
}
