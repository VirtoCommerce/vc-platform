using System;
using System.Collections.Generic;
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
        private readonly RoleManager<Role> _roleManager;

        public UserSearchService(Func<UserManager<ApplicationUser>> userManager, RoleManager<Role> roleManager)
        {
            _userManagerFactory = userManager;
            _roleManager = roleManager;
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

                result.TotalCount = await query.CountAsync();

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<ApplicationUser>(x => x.UserName), SortDirection = SortDirection.Descending } };
                }
                result.Results = await query.OrderBySortInfos(sortInfos).Skip(criteria.Skip).Take(criteria.Take).ToArrayAsync();

                foreach (var user in result.Results)
                {
                    user.Roles = new List<Role>();
                    foreach (var roleName in await userManager.GetRolesAsync(user))
                        {
                            var role = await _roleManager.FindByNameAsync(roleName);
                            if (role != null)
                                {
                                    user.Roles.Add(role);
                                }
                        }
                }
                return result;
            }
        }
    }
}
