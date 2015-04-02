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

		public static string Serialize(object instance)
		{
			string result;

			using (var stream = new MemoryStream())
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, instance);
				var buffer = stream.GetBuffer();
				result = Convert.ToBase64String(buffer);
			}

			return result;
		}

		public static T Deserialize<T>(string serializedString) where T : class
		{
			T result;
			using (var stream = new MemoryStream(Convert.FromBase64String(serializedString)))
			{
				IFormatter formatter = new BinaryFormatter();
				result = formatter.Deserialize(stream) as T;
			}

			return result;
		}

	}
}
