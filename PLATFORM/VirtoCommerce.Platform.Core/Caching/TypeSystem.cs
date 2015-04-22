using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Platform.Core.Caching
{
	internal static class TypeSystem
	{
		/// <summary>
		/// Gets the elementtype for enumerable
		/// </summary>
		/// <param name="type">The type to inspect.</param>
		/// <returns>If the <paramref name="type"/> was IEnumerable then it returns the type of a single element
		/// otherwise it returns null.</returns>
		public static Type GetIEnumerableElementType(Type type)
		{
			Type ienum = FindIEnumerable(type);
			if (ienum == null)
			{
				return null;
			}

			return ienum.GetGenericArguments()[0];
		}

		/// <summary>
		/// Finds type that implements IEnumerable so can get elemtent type
		/// </summary>
		/// <param name="seqType">The Type to check</param>
		/// <returns>returns the type which implements IEnumerable</returns>
		public static Type FindIEnumerable(Type seqType)
		{
			if (seqType == null || seqType == typeof(string))
			{
				return null;
			}

			if (seqType.IsArray)
			{
				return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
			}

			if (seqType.IsGenericType)
			{
				foreach (Type arg in seqType.GetGenericArguments())
				{
					Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
					if (ienum.IsAssignableFrom(seqType))
					{
						return ienum;
					}
				}
			}

			Type[] ifaces = seqType.GetInterfaces();
			if (ifaces != null && ifaces.Length > 0)
			{
				foreach (Type iface in ifaces)
				{
					Type ienum = FindIEnumerable(iface);
					if (ienum != null)
					{
						return ienum;
					}
				}
			}

			if (seqType.BaseType != null && seqType.BaseType != typeof(object))
			{
				return FindIEnumerable(seqType.BaseType);
			}

			return null;
		}
	}
}
