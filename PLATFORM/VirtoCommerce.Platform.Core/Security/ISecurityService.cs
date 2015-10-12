using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface ISecurityService
    {
        Task<ApplicationUserExtended> FindByNameAsync(string userName, UserDetails detailsLevel);
        Task<ApplicationUserExtended> FindByIdAsync(string userId, UserDetails detailsLevel);
        Task<ApplicationUserExtended> FindByEmailAsync(string email, UserDetails detailsLevel);
        Task<ApplicationUserExtended> FindByLoginAsync(string loginProvider, string providerKey, UserDetails detailsLevel);
        Task<SecurityResult> CreateAsync(ApplicationUserExtended user);
        Task<SecurityResult> UpdateAsync(ApplicationUserExtended user);
        Task DeleteAsync(string[] names);
        ApiAccount GenerateNewApiAccount(ApiAccountType type);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<SecurityResult> ChangePasswordAsync(string name, string oldPassword, string newPassword);
        Task<SecurityResult> ResetPasswordAsync(string name, string newPassword);
        Task<SecurityResult> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<UserSearchResponse> SearchUsersAsync(UserSearchRequest request);
        bool UserHasAnyPermission(string userName, string[] scopes, params string[] permissionIds);
        Permission[] GetAllPermissions();
        Permission[] GetUserPermissions(string userName);

    }
}
