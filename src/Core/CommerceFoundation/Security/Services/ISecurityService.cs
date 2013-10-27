using System.ServiceModel;
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
        [OperationContract]
        string CreateUser(string memberId, string userName, string password, string storeId, bool requireConfirmationToken = false);
        [OperationContract]
        string CreateAdminUser(string memberId, string userName, string password, bool requireConfirmationToken = false);
        [OperationContract]
        bool DeleteUser(string userName);
        [OperationContract]
        bool ChangePassword(string userName, string currentPassword, string newPassword);

        /// <summary>
        /// Resets the password. Should only be used by the super admin user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        [OperationContract]
        bool ResetPassword(string userName, string newPassword);

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
