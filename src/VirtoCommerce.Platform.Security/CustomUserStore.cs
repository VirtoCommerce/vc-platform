using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
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

        public override async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.FindByIdAsync(userId, cancellationToken);
            await Context.Entry(result).ReloadAsync(cancellationToken);
            return result;
        }

        public override async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd,
            CancellationToken cancellationToken = new())
        {
            await base.SetLockoutEndDateAsync(user, lockoutEnd, cancellationToken);
        }
    }
}
