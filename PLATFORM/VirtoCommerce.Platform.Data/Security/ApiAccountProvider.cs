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

        public ApiAccountEntity GetAccountByAppId(ApiAccountType type, string appId)
        {
            return _cacheManager.Get(
                CacheKey.Create(CacheGroups.Security, "ApiAccount", type.ToString(), appId),
                () => LoadApiAccount(type, appId));
        }

        public ApiAccountEntity GenerateApiCredentials(ApiAccountType type, string name)
        {
            var result = new ApiAccountEntity
            {
                Name = name,
                ApiAccountType = type,
                AppId = Guid.NewGuid().ToString("N"),
                IsActive = true
            };

            if (type == ApiAccountType.Hmac)
            {
                result.SecretKey = ConvertBytesToHexString(GetRandomBytes(64));
            }

            return result;
        }

        #endregion


        private ApiAccountEntity LoadApiAccount(ApiAccountType type, string appId)
        {
            using (var repository = _platformRepository())
            {
                var apiAccount = repository.ApiAccounts.FirstOrDefault(c => c.ApiAccountType == type && c.AppId == appId &&
                    c.IsActive && c.Account.AccountState == AccountState.Approved);
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
