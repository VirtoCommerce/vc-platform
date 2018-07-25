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
        ApiAccount GenerateNewApiKey(ApiAccount account);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<bool> ValidatePasswordResetTokenAsync(string userId, string token);
        Task<SecurityResult> ChangePasswordAsync(string name, string oldPassword, string newPassword);
        Task<SecurityResult> ResetPasswordAsync(string name, string newPassword);
        Task<SecurityResult> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<UserSearchResponse> SearchUsersAsync(UserSearchRequest request);
        bool UserHasAnyPermission(string userName, string[] scopes, params string[] permissionIds);
        Permission[] GetAllPermissions();
        Permission[] GetUserPermissions(string userName);
        Task<bool> IsUserLockedAsync(string userId);
        Task<SecurityResult> UnlockUserAsync(string userId);
    }
}
