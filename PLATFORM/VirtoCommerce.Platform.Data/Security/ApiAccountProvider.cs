using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Security
{
    public class ApiAccountProvider : IApiAccountProvider
    {
        private readonly Func<IPlatformRepository> _platformRepository;
        private readonly CacheManager _cacheManager;

        public ApiAccountProvider(Func<IPlatformRepository> platformRepository, CacheManager cacheManager)
        {
            _platformRepository = platformRepository;
            _cacheManager = cacheManager;
        }

        #region IApiAccountProvider Members

        public ApiAccountEntity GetAccountByAppId(string appId)
        {
            return _cacheManager.Get(
                CacheKey.Create(CacheGroups.Security, "ApiAccount", appId),
                () => LoadApiAccount(appId));
        }

        public ApiAccountEntity GenerateApiCredentials()
        {
            return new ApiAccountEntity
            {
                AppId = Guid.NewGuid().ToString("N"),
                SecretKey = ConvertBytesToHexString(GetRandomBytes(64))
            };
        }

        #endregion


        private ApiAccountEntity LoadApiAccount(string appId)
        {
            using (var repository = _platformRepository())
            {
                var apiAccount = repository.ApiAccounts.FirstOrDefault(c => c.AppId == appId && c.IsActive && c.Account.AccountState == AccountState.Approved);
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
