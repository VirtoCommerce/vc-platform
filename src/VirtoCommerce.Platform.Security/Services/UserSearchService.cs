using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;

namespace VirtoCommerce.Platform.Security.Services
{
    public class UserSearchService : IUserSearchService
    {
        private readonly Func<UserManager<ApplicationUser>> _userManagerFactory;
        private readonly Func<RoleManager<Role>> _roleManagerFactory;

        public UserSearchService(Func<UserManager<ApplicationUser>> userManager, Func<RoleManager<Role>> roleManagerFactory)
        {
            _userManagerFactory = userManager;
            _roleManagerFactory = roleManagerFactory;
        }

        public async Task<UserSearchResult> SearchUsersAsync(UserSearchCriteria criteria)
        {
            using (var userManager = _userManagerFactory())
            {
                if (criteria == null)
                {
                    throw new ArgumentNullException(nameof(criteria));
                }
                if (!userManager.SupportsQueryableUsers)
                {
                    throw new NotSupportedException();
                }

                var result = AbstractTypeFactory<UserSearchResult>.TryCreateInstance();
                var query = userManager.Users;
                if (criteria.Keyword != null)
                {
                    query = query.Where(x => x.UserName.Contains(criteria.Keyword));
                }

                if (!string.IsNullOrEmpty(criteria.MemberId))
                {
                    query = query.Where(x => x.MemberId == criteria.MemberId);
                }

                if (!criteria.MemberIds.IsNullOrEmpty())
                {
                    query = query.Where(x => criteria.MemberIds.Contains(x.MemberId));
                }

                if (criteria.ModifiedSinceDate != null && criteria.ModifiedSinceDate != default(DateTime))
                {
                    query = query.Where(x => x.ModifiedDate > criteria.ModifiedSinceDate);
                }

                if (!criteria.Roles.IsNullOrEmpty())
                {
                    var roleManager = _roleManagerFactory();
                    if (!roleManager.SupportsQueryableRoles)
                    {
                        throw new NotSupportedException();
                    }
                    var rolesIds = await roleManager.Roles.Where(x => criteria.Roles.Contains(x.Name)).Select(x => x.Id).Distinct().ToArrayAsync();

                    query = query.Where(x => x.UserRoles.Any(r => rolesIds.Contains(r.RoleId)));

                }

                result.TotalCount = await query.CountAsync();

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<ApplicationUser>(x => x.UserName), SortDirection = SortDirection.Descending } };
                }
                result.Results = await query.OrderBySortInfos(sortInfos).Skip(criteria.Skip).Take(criteria.Take).ToArrayAsync();

                return result;
            }
        }
    }
}
