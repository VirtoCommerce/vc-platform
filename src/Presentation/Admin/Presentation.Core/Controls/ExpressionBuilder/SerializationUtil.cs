using System;
using linq = System.Linq.Expressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using ExpressionSerialization;

namespace VirtoCommerce.ManagementClient.Core.Controls
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
