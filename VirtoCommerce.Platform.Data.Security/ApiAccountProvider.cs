using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CacheManager.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security.Resources;

namespace VirtoCommerce.Platform.Data.Security
{
    public class ApiAccountProvider : IApiAccountProvider
    {
        private readonly Func<IPlatformRepository> _platformRepository;
        private readonly ICacheManager<object> _cacheManager;

        [CLSCompliant(false)]
        public ApiAccountProvider(Func<IPlatformRepository> platformRepository, ICacheManager<object> cacheManager)
        {
            _platformRepository = platformRepository;
            _cacheManager = cacheManager;
        }

        #region IApiAccountProvider Members

        public ApiAccountEntity GetAccountByAppId(ApiAccountType type, string appId)
        {
            var cacheKey = string.Join(":", "ApiAccount", type.ToString(), appId);
            return _cacheManager.Get(cacheKey, SecurityConstants.CacheRegion, () => LoadApiAccount(type, appId));
        }

        public ApiAccountEntity GenerateApiCredentials(ApiAccountType type)
        {
            var result = new ApiAccountEntity
            {
                ApiAccountType = type,
                AppId = Guid.NewGuid().ToString("N")
            };

            if (type == ApiAccountType.Hmac)
            {
                result = GenerateApiKey(result);
            }

            return result;
        }

        public ApiAccountEntity GenerateApiKey(ApiAccountEntity account)
        {
            if (account.ApiAccountType != ApiAccountType.Hmac)
            {
                throw new InvalidOperationException(SecurityAccountExceptions.NonHmacKeyGenerationException);
            }
            account.SecretKey = ConvertBytesToHexString(GetRandomBytes(64));
            return account;
        }

        #endregion


        private ApiAccountEntity LoadApiAccount(ApiAccountType type, string appId)
        {
            using (var repository = _platformRepository())
            {
                //Do not return EF DynamicProxies types, it can leads to strange behavior when caching
                repository.DisableChangesTracking();
                var apiAccount = repository.ApiAccounts.FirstOrDefault(c => c.ApiAccountType == type && c.AppId == appId &&
                    c.IsActive && c.Account.AccountState == AccountState.Approved.ToString());
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
