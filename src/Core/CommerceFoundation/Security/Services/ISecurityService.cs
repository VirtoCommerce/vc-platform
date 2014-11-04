using System.ServiceModel;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.Foundation.Security.Services
{
    [ServiceContract(Namespace = "http://schemas.virtocommerce.com/1.0/security/")]
	public interface ISecurityService 
	{
		[OperationContract]
		bool CheckMemberPermission(string memberId, Permission permission);
		[OperationContract]
		bool CheckMemberInRole(string memberId, Role role);

        #region User methods
        /// <summary>
        /// Creates the user asynchronous.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="requireConfirmationToken">if set to <c>true</c> [require confirmation token].</param>
        /// <returns></returns>
        [OperationContract]
        Task<string> CreateUserAsync(string memberId, string userName, string password, string storeId, bool requireConfirmationToken = false);


        /// <summary>
        /// Creates the admin user asynchronous.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [OperationContract]
        Task<string> CreateAdminUserAsync(string memberId, string userName, string passwor);

        /// <summary>a
        /// Deletes the user asynchronous.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [OperationContract]
        Task<bool> DeleteUserAsync(string userName);
        /// <summary>
        /// Changes the password asynchronous.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        [OperationContract]
        Task<bool> ChangePasswordAsync(string userName, string currentPassword, string newPassword);

        /// <summary>
        /// Resets the password. Should only be used by the super admin user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        [OperationContract]
        Task<bool> ResetPasswordAsync(string userName, string newPassword);

        /// <summary>
        /// Locks the specified member by id.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        /// <returns>true whenn user is succesfully locked</returns>
        [OperationContract]
        bool Lock(string memberId);

        /// <summary>
        /// Unlocks the specified memeber.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        /// <returns></returns>
        [OperationContract]
        bool Unlock(string memberId);

        #endregion
	}
}
