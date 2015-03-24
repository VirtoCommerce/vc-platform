using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Framework.Web.Settings;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public class ApiAccountProvider : CachingSecurityService, IApiAccountProvider
    {
        private readonly Func<ISecurityRepository> _securityRepository;

        public ApiAccountProvider(Func<ISecurityRepository> securityRepository, ICacheRepository cacheRepository, ISettingsManager settingsManager)
            : base(cacheRepository, settingsManager)
        {
            _securityRepository = securityRepository;
        }

        #region IApiAccountProvider Members

        public ApiAccount GetAccountByAppId(string appId)
        {
            return Cache.Get(
                Cache.CreateKey("ApiAccount", appId),
                () => LoadApiAccount(appId),
                GetCacheTimeout());
        }

        public ApiAccount GenerateApiCredentials()
        {
            return new ApiAccount
            {
                AppId = Guid.NewGuid().ToString("N"),
                SecretKey = ConvertBytesToHexString(GetRandomBytes(64))
            };
        }

        #endregion


        private ApiAccount LoadApiAccount(string appId)
        {
            using (var repository = _securityRepository())
            {
                var apiAccount = repository.ApiAccounts.FirstOrDefault(c => c.IsActive && c.AppId == appId);
                return apiAccount;
            }
        }

        private static byte[] GetRandomBytes(int count)
        {
            var bytes = new byte[count];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return bytes;
        }

        private static string ConvertBytesToHexString(byte[] bytes)
        {
            var builder = new StringBuilder();

            foreach (var b in bytes)
                builder.Append(b.ToString("x2", CultureInfo.InvariantCulture));

            return builder.ToString();
        }
    }
}
