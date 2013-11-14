namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public interface IAuthenticationContext
    {
        bool IsUserAuthenticated { get; }
        bool IsAdminUser { get; }
        string CurrentUserName { get; }
        string CurrentUserId { get; }
        string Token { get; }

        void UpdateToken();
        bool Login(string userName, string password, string baseUrl);
        bool CheckPermission(string permission);
    }
}
