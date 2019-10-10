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
using System.Linq;

namespace VirtoCommerce.Platform.Core.Common
{
	public static class AnonymousComparer
	{
		#region IComparer<T>

		/// <summary>Example:AnonymousComparer.Create&lt;int&gt;((x, y) => y - x)</summary>
		public static IComparer<T> Create<T>(Func<T, T, int> compare)
		{
			if (compare == null)
				throw new ArgumentNullException("compare");

			return new Comparer<T>(compare);
		}

		private class Comparer<T> : IComparer<T>
		{
			private readonly Func<T, T, int> compare;

			public Comparer(Func<T, T, int> compare)
			{
				this.compare = compare;
			}

			public int Compare(T x, T y)
			{
				return compare(x, y);
			}
		}

		#endregion

		#region IEqualityComparer<T>

		/// <summary>Example:AnonymousComparer.Create((MyClass mc) => mc.MyProperty)</summary>
		public static IEqualityComparer<T> Create<T, TKey>(Func<T, TKey> compareKeySelector)
		{
			if (compareKeySelector == null)
				throw new ArgumentNullException("compareKeySelector");

			return new EqualityComparer<T>(
				(x, y) =>
				{
					if (object.ReferenceEquals(x, y))
						return true;
					if (x == null || y == null)
						return false;
					return compareKeySelector(x).Equals(compareKeySelector(y));
				},
				obj =>
				{
					if (obj == null)
						return 0;
					var retVal = compareKeySelector(obj);
					if(retVal == null)
					{
						return 0;
					}
					return retVal.GetHashCode();
				});
		}

		public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
		{
			if (equals == null)
				throw new ArgumentNullException("equals");
			if (getHashCode == null)
				throw new ArgumentNullException("getHashCode");

			return new EqualityComparer<T>(equals, getHashCode);
		}

		private class EqualityComparer<T> : IEqualityComparer<T>
		{
			private readonly Func<T, T, bool> equals;
			private readonly Func<T, int> getHashCode;

			public EqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
			{
				this.equals = equals;
				this.getHashCode = getHashCode;
			}

			public bool Equals(T x, T y)
			{
				return equals(x, y);
			}

			public int GetHashCode(T obj)
			{
				return getHashCode(obj);
			}
		}

		#endregion

		
	}
}