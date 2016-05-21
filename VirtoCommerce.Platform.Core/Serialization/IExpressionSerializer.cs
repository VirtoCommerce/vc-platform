using System.Linq.Expressions;

namespace VirtoCommerce.Platform.Core.Serialization
{
    public interface IExpressionSerializer
    {
        string SerializeExpression(Expression expression);
        T DeserializeExpression<T>(string serializedExpression);
    }
}
