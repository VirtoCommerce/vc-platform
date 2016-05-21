using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using ExpressionSerialization;
using VirtoCommerce.Platform.Core.Serialization;

namespace VirtoCommerce.Platform.Data.Serialization
{
    public class XmlExpressionSerializer : IExpressionSerializer
    {
        public string SerializeExpression(Expression expression)
        {
            var serializer = GetSerializer();
            var result = serializer.Serialize(expression).ToString();
            return result;
        }

        public T DeserializeExpression<T>(string serializedExpression)
        {
            var xElement = XElement.Parse(serializedExpression);
            var serializer = GetSerializer();
            var expression = serializer.Deserialize<T>(xElement);
            var result = expression.Compile();
            return result;
        }


        private static ExpressionSerializer GetSerializer()
        {
            var typeResolver = new TypeResolver(assemblies: AppDomain.CurrentDomain.GetAssemblies(), knownTypes: null);
            var serializer = new ExpressionSerializer(typeResolver);
            return serializer;
        }
    }
}
