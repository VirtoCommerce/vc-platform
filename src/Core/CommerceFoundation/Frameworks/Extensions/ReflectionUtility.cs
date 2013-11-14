using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace VirtoCommerce.Foundation.Frameworks.Extensions
{
	public static class ReflectionUtility
	{
		public static PropertyInfo[] FindPropertiesWithAttribute(this Type type, Type attribute)
		{
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			return properties.Where(x => x.GetCustomAttributes(attribute, true).Any()).ToArray();
		}

		public static bool IsHaveAttribute(this PropertyInfo propertyInfo, Type attribute)
		{
			return propertyInfo.GetCustomAttributes(attribute, true).Any();
		}

	
		public static Type[] GetTypeInheritanceChain(this Type type)
		{
			var retVal = new List<Type>();

			retVal.Add(type);
			var baseType = type.BaseType;
			while (baseType != typeof(StorageEntity))
			{
				retVal.Add(baseType);
				baseType = baseType.BaseType;
			}
			return retVal.ToArray();
		}

		public static bool IsDerivativeOf(this Type type, Type typeToCompare)
		{
			if (type == null)
			{
				throw new NullReferenceException();
			}

			var retVal = type.BaseType != null;
			if (retVal)
			{
				retVal = type.BaseType == typeToCompare;
			}
			if (!retVal && type.BaseType != null)
			{
				retVal = type.BaseType.IsDerivativeOf(typeToCompare);
			}
			return retVal;
		}

	}
}
