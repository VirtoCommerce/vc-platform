using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Security
{
    public interface IApiAccountProvider
    {
        ApiAccountEntity GetAccountByAppId(string appId);
        ApiAccountEntity GenerateApiCredentials();
    }
}
