using System;
using System.Data.Entity.Infrastructure;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
    public class EntityPrimaryKeyGeneratorInterceptor : ChangeInterceptor<Entity>
    {
        public override void OnBeforeInsert(DbEntityEntry entry, Entity entity)
        {
            base.OnBeforeInsert(entry, entity);

            if (entity.IsTransient())
            {
                entity.Id = Guid.NewGuid().ToString("N");
            }
        }

    }
}
