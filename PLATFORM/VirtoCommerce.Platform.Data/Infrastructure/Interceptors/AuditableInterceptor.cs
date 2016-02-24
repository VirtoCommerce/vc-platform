using System;
using System.Data.Entity.Infrastructure;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
    public class AuditableInterceptor : ChangeInterceptor<IAuditable>
    {
        private readonly Func<IUserNameResolver> _userNameResolverFactory;

        public AuditableInterceptor(Func<IUserNameResolver> userNameResolverFactory)
        {
            _userNameResolverFactory = userNameResolverFactory;
        }

        public override void OnBeforeInsert(DbEntityEntry entry, IAuditable item)
        {
            base.OnBeforeInsert(entry, item);

            var currentTime = DateTime.UtcNow;
            var currentUser = GetCurrentUserName();
            item.CreatedDate = item.CreatedDate == default(DateTime) ? currentTime : item.CreatedDate;
            item.ModifiedDate = item.ModifiedDate ?? currentTime;
            item.CreatedBy = item.CreatedBy ?? currentUser;
            item.ModifiedBy = item.ModifiedBy ?? currentUser;
        }

        public override void OnBeforeUpdate(DbEntityEntry entry, IAuditable item)
        {
            base.OnBeforeUpdate(entry, item);
            var currentTime = DateTime.UtcNow;
            item.ModifiedDate = currentTime;
            item.ModifiedBy = GetCurrentUserName();
        }

        private string GetCurrentUserName()
        {
            var resolver = _userNameResolverFactory != null ? _userNameResolverFactory() : null;
            var result = resolver != null ? resolver.GetCurrentUserName() : "unknown";
            return result;
        }
    }
}
