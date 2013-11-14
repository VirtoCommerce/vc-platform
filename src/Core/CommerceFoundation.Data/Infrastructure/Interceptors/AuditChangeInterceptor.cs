using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Infrastructure.Interceptors
{
    public class AuditChangeInterceptor : ChangeInterceptor<IModifiedDateTimeFields>
    {
        public override void OnBeforeInsert(DbEntityEntry entry, IModifiedDateTimeFields item)
        {
            base.OnBeforeInsert(entry, item);

            var currentTime = DateTime.UtcNow;

            item.Created = currentTime;
            item.LastModified = currentTime;
        }

        public override void OnBeforeUpdate(DbEntityEntry entry, IModifiedDateTimeFields item)
        {
            base.OnBeforeUpdate(entry, item);
            var currentTime = DateTime.UtcNow;
            item.LastModified = currentTime;
        }

        public override void OnAfterInsert(DbEntityEntry entry, IModifiedDateTimeFields item)
        {
            base.OnAfterInsert(entry, item);
        }
    }
}
