using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Security
{
    public interface IApiAccountProvider
    {
        ApiAccountEntity GetAccountByAppId(ApiAccountType type, string appId);
        ApiAccountEntity GenerateApiCredentials(ApiAccountType type, string name);
    }
}
