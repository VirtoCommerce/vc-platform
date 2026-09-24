using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Platform.DistributedLock.Redis
{
    /// <summary>
    /// <see cref="IDistributedLock"/> on Redis (RedLock.net). Held locks are extended automatically until the handle is disposed.
    /// </summary>
    public sealed class RedisDistributedLock : DistributedLockBase
    {
        private readonly IDistributedLockFactory _lockFactory;

        public RedisDistributedLock(IDistributedLockFactory lockFactory, IOptions<DistributedLockOptions> options, ILogger<RedisDistributedLock> logger)
            : base(options)
        {
            _lockFactory = lockFactory;
            logger.LogInformation("Distributed lock: using Redis.");
        }

        protected override async Task<IDistributedLockHandle> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var key = string.IsNullOrEmpty(LockOptions.KeyPrefix) ? resource : $"{LockOptions.KeyPrefix}:{resource}";

            var redLock = timeout == TimeSpan.Zero
                ? await _lockFactory.CreateLockAsync(key, LockOptions.Expiry)
                : await _lockFactory.CreateLockAsync(key, LockOptions.Expiry, timeout, LockOptions.RetryInterval, cancellationToken);

            if (redLock.IsAcquired)
            {
                return new Handle(resource, redLock);
            }

            await redLock.DisposeAsync();
            return null;
        }

        private sealed class Handle : IDistributedLockHandle
        {
            private readonly IRedLock _redLock;

            public Handle(string resource, IRedLock redLock)
            {
                Resource = resource;
                _redLock = redLock;
            }

            public string Resource { get; }

            public void Dispose()
            {
                _redLock.Dispose();
            }

            public ValueTask DisposeAsync()
            {
                return _redLock.DisposeAsync();
            }
        }
    }
}
