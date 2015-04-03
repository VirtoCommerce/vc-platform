using System;
using linq = System.Linq.Expressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using ExpressionSerialization;
using System.Xml.Linq;

namespace VirtoCommerce.MarketingModule.Data.Common
{
	public static class SerializationUtil
	{
		public static string SerializeExpression(linq.Expression expr)
		{
			var typeResolver = new TypeResolver(assemblies: AppDomain.CurrentDomain.GetAssemblies(), knownTypes: null);
			var serializer = new ExpressionSerializer(typeResolver);
			var retVal = serializer.Serialize(expr).ToString();

			return retVal;
		}

		public static T DeserializeExpression<T>(string expr)
		{
			var xElement = XElement.Parse(expr);
			var typeResolver = new TypeResolver(assemblies: AppDomain.CurrentDomain.GetAssemblies(), knownTypes: null);
			var serializer = new ExpressionSerializer(typeResolver);
			var conditionExpression = serializer.Deserialize<T>(xElement);
			var retVal = conditionExpression.Compile();
			return retVal;
		}
	}
}
