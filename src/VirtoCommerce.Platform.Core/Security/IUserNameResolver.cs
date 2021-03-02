namespace VirtoCommerce.Platform.Core.Security
{
    public interface IUserNameResolver
    {
        string GetCurrentUserName();

        void SetCurrentUserName(string userName);
    }
}
