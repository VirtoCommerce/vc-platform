using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.Platform.Core.Extensions;

public static class IDataEntityExtensions
{
    public static TModel ToModel<TEntity, TModel>(this IDataEntity<TEntity, TModel> entity)
    {
        return entity.ToModel(AbstractTypeFactory<TModel>.TryCreateInstance());
    }
}
