using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public interface IApiAccountProvider
    {
        ApiAccount GetAccountByAppId(string appId);
        ApiAccount GenerateApiCredentials();
    }
}
