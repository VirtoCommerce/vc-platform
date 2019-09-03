using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using ExpressionSerialization;
using VirtoCommerce.Platform.Core.Serialization;

namespace VirtoCommerce.Platform.Data.Serialization
{
    public class XmlExpressionSerializer : IExpressionSerializer
    {
        private static Lazy<ExpressionSerializer> _expressionSerializer;
        private static readonly object _expressionSerializerLock = new object();
        static XmlExpressionSerializer()
        {
            //TypeResolver creates a dynamic assembly each time it's constructed, which is awfully wasteful. You should create only one AssemblyBuilder and reuse it, and you should create it lazily - anonymous types are not commonly used, especially in where serialization is required.
            _expressionSerializer = new Lazy<ExpressionSerializer>(() =>
            {
                var typeResolver = new TypeResolver(assemblies: AppDomain.CurrentDomain.GetAssemblies(), knownTypes: null);
                return new ExpressionSerializer(typeResolver);
            });
        }
        public string SerializeExpression(Expression expression)
        {

            var result = _expressionSerializer.Value.Serialize(expression).ToString();
            return result;

        }

        public T DeserializeExpression<T>(string serializedExpression)
        {
            var xElement = XElement.Parse(serializedExpression);

            Expression<T> expression = null;

            lock (_expressionSerializerLock)
            {
                expression = _expressionSerializer.Value.Deserialize<T>(xElement);
            }
            
            var result = expression.Compile();
            return result;
        }



    }
}
