using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Domain
{
    public interface IDataEntity<TEntity, TModel>
    {
        public TModel ToModel(TModel model);

        public TEntity FromModel(TModel model, PrimaryKeyResolvingMap pkMap);

        public void Patch(TEntity target);
    }
}
