using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security
{
    public class CustomUserStore : UserStore<ApplicationUser, Role, SecurityDbContext>
    {
        public CustomUserStore(SecurityDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        public override async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken = new())
        {
            var result = await base.FindByIdAsync(userId, cancellationToken);

            if (result != null)
            {
                await Context.Entry(result).ReloadAsync(cancellationToken);
            }

            return result;
        }

    }
}
