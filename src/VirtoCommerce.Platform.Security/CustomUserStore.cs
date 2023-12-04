using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Security.Caching;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security
{
    public class CustomUserStore : UserStore<ApplicationUser, Role, SecurityDbContext>
    {
        private readonly IEventPublisher _eventPublisher;

        public CustomUserStore(SecurityDbContext context, IEventPublisher eventPublisher, IdentityErrorDescriber describer = null) : base(context, describer)
        {
            _eventPublisher = eventPublisher;
        }

        public override async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd,
            CancellationToken cancellationToken = new())
        {
            var oldUser = (ApplicationUser)user.Clone();
            await base.SetLockoutEndDateAsync(user, lockoutEnd, cancellationToken);

            var changedEntries = new List<GenericChangedEntry<ApplicationUser>>
            {
                new GenericChangedEntry<ApplicationUser>(user, oldUser, EntryState.Modified)
            };

            SecurityCacheRegion.ExpireUser(user);
            await _eventPublisher.Publish(new UserChangedEvent(changedEntries), cancellationToken);
        }
    }
}
