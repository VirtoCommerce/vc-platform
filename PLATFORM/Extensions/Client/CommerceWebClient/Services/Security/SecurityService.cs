using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Foundation.Security.Services;

namespace VirtoCommerce.Web.Client.Services.Security
{
    /// <summary>
    /// Class SecurityService.
    /// </summary>
	[UnityInstanceProviderServiceBehavior]
	public class SecurityService : ISecurityService
	{
		#region Private Methods
        /// <summary>
        /// The security repository
        /// </summary>
		protected ISecurityRepository SecurityRepository;
        /// <summary>
        /// The membership provider
        /// </summary>
		protected IUserIdentitySecurity MembershipProvider;
		#endregion

		#region ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="membershipProvider">The membership provider.</param>
        public SecurityService(ISecurityRepository repository, IUserIdentitySecurity membershipProvider)
		{
			SecurityRepository = repository;
			MembershipProvider = membershipProvider;
		} 
		#endregion

		#region ISecurityService Members
		#region User methods
        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="requireConfirmationToken">if set to <c>true</c> [require confirmation token].</param>
        /// <returns>System.String.</returns>
		public virtual async Task<string> CreateUserAsync(string memberId, string userName, string password, string storeId, bool requireConfirmationToken = false)
		{
			var ret = await MembershipProvider.CreateUserAndAccountAsync(userName, password,
				new { Discriminator="Account", 
					MemberId = memberId, 
					RegisterType = RegisterType.GuestUser.GetHashCode(), 
					AccountState = requireConfirmationToken ? AccountState.PendingApproval.GetHashCode() : AccountState.Approved.GetHashCode(),
					storeId
				}, requireConfirmationToken);

			return ret;
		}

        /// <summary>
        /// Creates the admin user.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
		public virtual async Task<string> CreateAdminUserAsync(string memberId, string userName, string password)
		{
			var ret = await MembershipProvider.CreateUserAndAccountAsync(userName, password, 
				new { Discriminator="Account", 
					MemberId = memberId, 
					RegisterType = RegisterType.SiteAdministrator.GetHashCode(), 
					AccountState = AccountState.Approved.GetHashCode() });

			return ret;
		}

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public virtual async Task<bool> DeleteUserAsync(string userName)
		{
			var ret = await MembershipProvider.DeleteUserAsync(userName);
			return ret;
		}

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public virtual async Task<bool> ChangePasswordAsync(string userName, string currentPassword, string newPassword)
		{
			var ret = await MembershipProvider.ChangePasswordAsync(userName, currentPassword, newPassword);
			return ret;
		}

        /// <summary>
        /// Resets the password. Should only be used by the super admin user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public virtual async Task<bool> ResetPasswordAsync(string userName, string newPassword)
		{
			var ret = await MembershipProvider.ResetPasswordAsync(userName, newPassword);
			return ret;
		}

        /// <summary>
        /// Locks the specified member by id.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        /// <returns>true whenn user is succesfully locked</returns>
		public bool Lock(string memberId)
		{
			var account = SecurityRepository.Accounts.FirstOrDefault(a => a.MemberId == memberId);

			if (account != null)
			{
				account.AccountState = AccountState.Rejected.GetHashCode();
				SecurityRepository.UnitOfWork.Commit();
				return true;
			}
			return false;
		}

        /// <summary>
        /// Unlocks the specified member.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool Unlock(string memberId)
		{
			var account = SecurityRepository.Accounts.FirstOrDefault(a => a.MemberId == memberId);

			if (account != null)
			{
				account.AccountState = AccountState.Approved.GetHashCode();
				SecurityRepository.UnitOfWork.Commit();
				return true;
			}
			return false;
		}

		#endregion


        /// <summary>
        /// Checks the member permission.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="permission">The permission.</param>
        /// <returns><c>true</c> if has permission, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// memberId
        /// or
        /// permission
        /// </exception>
		public bool CheckMemberPermission(string memberId, Permission permission)
		{
			if (memberId == null)
			{
				throw new ArgumentNullException("memberId");
			}
			if (permission == null)
			{
				throw new ArgumentNullException("permission");
			}

			var account = SecurityRepository.Accounts.FirstOrDefault(a => a.MemberId == memberId);

			if (account == null)
			{
				return false;
			}

			var retVal = false;
			//TODO: member organization check
			foreach (var assignment in SecurityRepository.RoleAssignments.Where(x => x.AccountId == account.AccountId).Expand("Role/RolePermissions"))
			{
				retVal = assignment.Role != null && assignment.Role.RolePermissions
					.Any(p=>p.PermissionId.Equals(permission.PermissionId, StringComparison.OrdinalIgnoreCase));
				if (retVal)
				{
					break;
				}
			}
			return retVal;
		}

        /// <summary>
        /// Checks the member in role.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="role">The role.</param>
        /// <returns><c>true</c> if member is in role, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// memberId
        /// or
        /// role
        /// </exception>
		public bool CheckMemberInRole(string memberId, Role role)
		{
			if (memberId == null)
			{
				throw new ArgumentNullException("memberId");
			}
			if (role == null)
			{
				throw new ArgumentNullException("role");
			}

			var account = SecurityRepository.Accounts.FirstOrDefault(a => a.MemberId == memberId);

			if (account == null)
			{
				return false;
			}

			var retVal = false;
			foreach (var assignment in SecurityRepository.RoleAssignments.Where(x => x.AccountId == account.AccountId).Expand("Role"))
			{
				var examineRole = assignment.Role;
				retVal = examineRole != null && examineRole.RoleId.Equals(role.RoleId);
				if (retVal)
				{
					break;
				}
			}
			return retVal;
		}
		#endregion
	
	}
}
