using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections;

namespace VirtoCommerce.Platform.Core.Common
{
	public static class ReflectionUtility
	{
		public static IEnumerable<string> GetPropertyNames<T>(params Expression<Func<T, object>>[] propertyExpressions)
		{
			var retVal = new List<string>();
			foreach(var propertyExpression in propertyExpressions)
			{
				retVal.Add(GetPropertyName(propertyExpression));
			}
			return retVal;
		}

		public static string GetPropertyName<T>(Expression<Func<T, object>> propertyExpression)
		{
			string retVal = null;
			if (propertyExpression != null)
			{
				var lambda = (LambdaExpression)propertyExpression;
				MemberExpression memberExpression;
				if (lambda.Body is UnaryExpression)
				{
					var unaryExpression = (UnaryExpression)lambda.Body;
					memberExpression = (MemberExpression)unaryExpression.Operand;
				}
				else
				{
					memberExpression = (MemberExpression)lambda.Body;
				}
				retVal = memberExpression.Member.Name;

			}
			return retVal;
		}

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
			while (baseType != typeof(Entity))
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

		public static T[] GetFlatListObjectsWithInterface<T>(this object obj)
		{
			var retVal = new List<T>();

			var objectType = obj.GetType();

			if(objectType.GetInterface(typeof(T).Name) != null)
			{
				retVal.Add((T)obj);
			}

			var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var objects = properties.Where(x => x.PropertyType.GetInterface(typeof(T).Name) != null)
									.Select(x =>(T)x.GetValue(obj)).ToList();

			retVal.AddRange(objects.SelectMany(x => x.GetFlatListObjectsWithInterface<T>()));

			var collections = properties.Select(x => x.GetValue(obj, null))
										.Where(x => x is IEnumerable && !(x is String))
										.Cast<IEnumerable>();

			foreach(var collection in collections)
			{
				foreach(var collectionObject in collection)
				{
					if (collectionObject is T)
					{
						retVal.AddRange(collectionObject.GetFlatListObjectsWithInterface<T>());
					}
				}
			}

			return retVal.ToArray();
		
			
		}

	}
}
