using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security.Converters;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly Func<IPlatformRepository> _platformRepository;
		private readonly Func<ApplicationUserManager> _userManagerFactory;
        private readonly IApiAccountProvider _apiAccountProvider;
        private readonly ISecurityOptions _securityOptions;

        public SecurityService(Func<IPlatformRepository> platformRepository, Func<ApplicationUserManager> userManagerFactory, IApiAccountProvider apiAccountProvider, ISecurityOptions securityOptions)
        {
            _platformRepository = platformRepository;
			_userManagerFactory = userManagerFactory;
            _apiAccountProvider = apiAccountProvider;
            _securityOptions = securityOptions;
        }

        public async Task<ApplicationUserExtended> FindByNameAsync(string userName, UserDetails detailsLevel)
        {
			using (var userManager = _userManagerFactory())
			{
				var user = await userManager.FindByNameAsync(userName);
				return GetUserExtended(user, detailsLevel);
			}
        }

        public async Task<ApplicationUserExtended> FindByIdAsync(string userId, UserDetails detailsLevel)
        {
			using (var userManager = _userManagerFactory())
			{
				var user = await userManager.FindByIdAsync(userId);
				return GetUserExtended(user, detailsLevel);
			}
        }

        public async Task<ApplicationUserExtended> FindByEmailAsync(string email, UserDetails detailsLevel)
        {
			using (var userManager = _userManagerFactory())
			{
				var user = await userManager.FindByEmailAsync(email);
				return GetUserExtended(user, detailsLevel);
			}
        }

        public async Task<ApplicationUserExtended> FindByLoginAsync(string loginProvider, string providerKey, UserDetails detailsLevel)
        {
			using (var userManager = _userManagerFactory())
			{
				var user = await userManager.FindAsync(new UserLoginInfo(loginProvider, providerKey));
				return GetUserExtended(user, detailsLevel);
			}
        }

        public async Task<SecurityResult> CreateAsync(ApplicationUserExtended user)
        {
            IdentityResult result = null;

            if (user != null)
            {
                var dbUser = user.ToDataModel();

				using (var userManager = _userManagerFactory())
				{
					if (string.IsNullOrEmpty(user.Password))
					{
						result = await userManager.CreateAsync(dbUser);
					}
					else
					{
						result = await userManager.CreateAsync(dbUser, user.Password);
					}

				}
                if (result.Succeeded)
                {
                    using (var repository = _platformRepository())
                    {
                        var account = new AccountEntity
                        {
                            Id = dbUser.Id,
                            UserName = user.UserName,
                            MemberId = user.MemberId,
                            AccountState = AccountState.Approved,
                            RegisterType = (RegisterType)user.UserType,
                            StoreId = user.StoreId
                        };

                        if (user.Roles != null)
                        {
                            foreach (var role in user.Roles)
                            {
                                account.RoleAssignments.Add(new RoleAssignmentEntity { RoleId = role.Id, AccountId = account.Id });
                            }
                        }

                        repository.Add(account);
                        repository.UnitOfWork.Commit();
                    }
                }
            }

            return result == null ? null : result.ToCoreModel();
        }

        public async Task<SecurityResult> UpdateAsync(ApplicationUserExtended user)
        {
            SecurityResult result = null;

            if (user != null)
            {
				ApplicationUser dbUser;
				using (var userManager = _userManagerFactory())
				{
					dbUser = await userManager.FindByIdAsync(user.Id);

					result = ValidateUser(dbUser);

					if (result.Succeeded)
					{
						dbUser.InjectFrom(user);

						if (user.Logins != null)
						{
							foreach (var login in user.Logins)
							{
								var userLogin = dbUser.Logins.FirstOrDefault(l => l.LoginProvider == login.LoginProvider);
								if (userLogin != null)
								{
									userLogin.ProviderKey = login.ProviderKey;
								}
								else
								{
									dbUser.Logins.Add(new IdentityUserLogin
									{
										LoginProvider = login.LoginProvider,
										ProviderKey = login.ProviderKey,
										UserId = dbUser.Id
									});
								}
							}
						}

						IdentityResult identityResult;

						identityResult = await userManager.UpdateAsync(dbUser);

						result = identityResult.ToCoreModel();

						if (result.Succeeded)
						{
							using (var repository = _platformRepository())
							{
								var account = repository.GetAccountByName(user.UserName, UserDetails.Full);

								if (account == null)
								{
									result = new SecurityResult { Errors = new[] { "Account not found." } };
								}
								else
								{
									account.RegisterType = (RegisterType)user.UserType;
									account.AccountState = (AccountState)user.UserState;
									account.MemberId = user.MemberId;
									account.StoreId = user.StoreId;

									if (user.ApiAccounts != null)
									{
										var sourceCollection = new ObservableCollection<ApiAccountEntity>(user.ApiAccounts.Select(x => x.ToEntity()));
										var comparer = AnonymousComparer.Create((ApiAccountEntity x) => x.Id);
										account.ApiAccounts.ObserveCollection(x => repository.Add(x), x => repository.Remove(x));
										sourceCollection.Patch(account.ApiAccounts, comparer, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
									}

									if (user.Roles != null)
									{
										var sourceCollection = new ObservableCollection<RoleAssignmentEntity>(user.Roles.Select(r => new RoleAssignmentEntity { RoleId = r.Id }));
										var comparer = AnonymousComparer.Create((RoleAssignmentEntity x) => x.RoleId);
										account.RoleAssignments.ObserveCollection(x => repository.Add(x), ra => repository.Remove(ra));
										sourceCollection.Patch(account.RoleAssignments, comparer, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
									}

									repository.UnitOfWork.Commit();
								}
							}
						}
					}
				}
            }

            return result;
        }

        public async Task DeleteAsync(string[] names)
        {
			using (var userManager = _userManagerFactory())
			{
				foreach (var name in names.Where(IsEditableUser))
				{

					var dbUser = await userManager.FindByNameAsync(name);

					if (dbUser != null)
					{
						await userManager.DeleteAsync(dbUser);

						using (var repository = _platformRepository())
						{
							var account = repository.GetAccountByName(name, UserDetails.Reduced);
							if (account != null)
							{
								repository.Remove(account);
								repository.UnitOfWork.Commit();
							}
						}
					}
				}
			}
        }

        public ApiAccount GenerateNewApiAccount(ApiAccountType type)
        {
            var apiAccount = _apiAccountProvider.GenerateApiCredentials(type);
            var result = apiAccount.ToCoreModel();
            return result;
        }

        public async Task<SecurityResult> ChangePasswordAsync(string name, string oldPassword, string newPassword)
        {
			using (var userManager = _userManagerFactory())
			{
				var dbUser = await userManager.FindByNameAsync(name);
				var result = ValidateUser(dbUser);

				if (result.Succeeded)
				{
					var identityResult = await userManager.ChangePasswordAsync(dbUser.Id, oldPassword, newPassword);
					result = identityResult.ToCoreModel();
				}

				return result;
			}
        }

        public async Task<SecurityResult> ResetPasswordAsync(string name, string newPassword)
        {
			using (var userManager = _userManagerFactory())
			{
				var dbUser = await userManager.FindByNameAsync(name);
				var result = ValidateUser(dbUser);

				if (result.Succeeded)
				{
					var token = await userManager.GeneratePasswordResetTokenAsync(dbUser.Id);
					var identityResult = await userManager.ResetPasswordAsync(dbUser.Id, token, newPassword);
					result = identityResult.ToCoreModel();
				}

				return result;
			}
        }

        public async Task<SecurityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
			using (var userManager = _userManagerFactory())
			{
				var dbUser = await userManager.FindByIdAsync(userId);
				var result = ValidateUser(dbUser);

				if (result.Succeeded)
				{
					var identityResult = await userManager.ResetPasswordAsync(userId, token, newPassword);
					result = identityResult.ToCoreModel();
				}

				return result;
			}
        }

        public async Task<UserSearchResponse> SearchUsersAsync(UserSearchRequest request)
        {
            request = request ?? new UserSearchRequest();
            var result = new UserSearchResponse();

			using (var userManager = _userManagerFactory())
			{
				var query = userManager.Users;

				if (request.Keyword != null)
				{
					query = query.Where(u => u.UserName.Contains(request.Keyword));
				}

				result.TotalCount = query.Count();

				var users = query.OrderBy(x => x.UserName)
								 .Skip(request.SkipCount)
								 .Take(request.TakeCount)
								 .ToArray();

				var extendedUsers = new List<ApplicationUserExtended>();

				foreach (var user in users)
				{
					var extendedUser = await FindByNameAsync(user.UserName, UserDetails.Reduced);
					extendedUsers.Add(extendedUser);
				}

				result.Users = extendedUsers.ToArray();

				return result;
			}
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
			using (var userManager = _userManagerFactory())
			{
				return await userManager.GeneratePasswordResetTokenAsync(userId);
			}
        }


        private SecurityResult ValidateUser(ApplicationUser dbUser)
        {
            SecurityResult result;

            if (dbUser == null)
            {
                result = new SecurityResult { Errors = new[] { "User not found." } };
            }
            else
            {
                if (!IsEditableUser(dbUser.UserName))
                {
                    result = new SecurityResult { Errors = new[] { "It is forbidden to edit this user." } };
                }
                else
                {
                    result = new SecurityResult { Succeeded = true };
                }
            }

            return result;
        }

        private bool IsEditableUser(string userName)
        {
            var result = true;

            if (_securityOptions != null && _securityOptions.NonEditableUsers != null)
                result = !_securityOptions.NonEditableUsers.Contains(userName);

            return result;
        }

        private ApplicationUserExtended GetUserExtended(ApplicationUser applicationUser, UserDetails detailsLevel)
        {
            ApplicationUserExtended result = null;

            if (applicationUser != null)
            {
                result = new ApplicationUserExtended();
                result.InjectFrom(applicationUser);

                using (var repository = _platformRepository())
                {
                    var user = repository.GetAccountByName(applicationUser.UserName, detailsLevel);

                    if (user != null)
                    {
                        result.InjectFrom(user);

                        result.UserState = (UserState)user.AccountState;
                        result.UserType = (UserType)user.RegisterType;

                        if (detailsLevel == UserDetails.Full)
                        {
                            var roles = user.RoleAssignments.Select(x => x.Role).ToArray();
                            result.Roles = roles.Select(r => r.ToCoreModel()).ToArray();

                            var permissionIds = roles
                                    .SelectMany(x => x.RolePermissions)
                                    .Select(x => x.PermissionId)
                                    .Distinct()
                                    .ToArray();

                            result.Permissions = permissionIds;
                            result.ApiAccounts = user.ApiAccounts.Select(x => x.ToCoreModel()).ToArray();
                        }
                    }
                }
            }

            return result;
        }
    }
}
