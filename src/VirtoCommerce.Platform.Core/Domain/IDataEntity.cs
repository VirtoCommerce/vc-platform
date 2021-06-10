using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Domain
{
    /// <summary>
    /// An interface for basic entity (data-layer) implementation.
    /// Entity-model conversion and vice-versa 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public interface IDataEntity<TEntity, TModel>
    {
        public TModel ToModel(TModel model);

        public TEntity FromModel(TModel model, PrimaryKeyResolvingMap pkMap);

        public void Patch(TEntity target);
    }
}
