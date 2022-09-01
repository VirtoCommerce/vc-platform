using System;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests
{
    [Trait("Category", "Unit")]
    [Collection(nameof(NotThreadSafeCollection))]
    public class PlatformMemoryCacheTests : MemoryCacheTestsBase
    {
        [Fact]
        public void SetWithTokenRegistersForNotification()
        {
            var cache = GetPlatformMemoryCache();
            var key = "myKey";
            var value = new object();
            var expirationToken = new TestExpirationToken() { ActiveChangeCallbacks = true };
            cache.Set(key, value, expirationToken);

            Assert.True(expirationToken.HasChangedWasCalled);
            Assert.True(expirationToken.ActiveChangeCallbacksWasCalled);
            Assert.NotNull(expirationToken.Registration);
            Assert.NotNull(expirationToken.Registration.RegisteredCallback);
            Assert.NotNull(expirationToken.Registration.RegisteredState);
            Assert.False(expirationToken.Registration.Disposed);
        }

        [Fact]
        public void FireTokenRemovesItem()
        {
            var cache = GetPlatformMemoryCache();
            var key = "myKey";
            var value = new object();
            var callbackInvoked = new ManualResetEvent(false);
            var expirationToken = new TestExpirationToken() { ActiveChangeCallbacks = true };
            cache.Set(key, value, new MemoryCacheEntryOptions()
                .AddExpirationToken(expirationToken)
                .RegisterPostEvictionCallback((subkey, subValue, reason, state) =>
                {
                    // TODO: Verify params
                    var localCallbackInvoked = (ManualResetEvent)state;
                    localCallbackInvoked.Set();
                }, state: callbackInvoked));

            expirationToken.Fire();

            var found = cache.TryGetValue(key, out value);
            Assert.False(found);

            Assert.True(callbackInvoked.WaitOne(TimeSpan.FromSeconds(30)), "Callback");
        }

        [Fact]
        public void RemoveItemDisposesTokenRegistration()
        {
            var cache = GetPlatformMemoryCache();
            var key = "myKey";
            var value = new object();
            var callbackInvoked = new ManualResetEvent(false);
            var expirationToken = new TestExpirationToken() { ActiveChangeCallbacks = true };
            cache.Set(key, value, new MemoryCacheEntryOptions()
                .AddExpirationToken(expirationToken)
                .RegisterPostEvictionCallback((subkey, subValue, reason, state) =>
                {
                    // TODO: Verify params
                    var localCallbackInvoked = (ManualResetEvent)state;
                    localCallbackInvoked.Set();
                }, state: callbackInvoked));
            cache.Remove(key);

            Assert.NotNull(expirationToken.Registration);
            Assert.True(expirationToken.Registration.Disposed);
            Assert.True(callbackInvoked.WaitOne(TimeSpan.FromSeconds(30)), "Callback");
        }
    }

    internal class TestExpirationToken : IChangeToken
    {
        private bool _hasChanged;
        private bool _activeChangeCallbacks;

        public bool HasChanged
        {
            get
            {
                HasChangedWasCalled = true;
                return _hasChanged;
            }
            set
            {
                _hasChanged = value;
            }
        }

        public bool HasChangedWasCalled { get; set; }

        public bool ActiveChangeCallbacks
        {
            get
            {
                ActiveChangeCallbacksWasCalled = true;
                return _activeChangeCallbacks;
            }
            set
            {
                _activeChangeCallbacks = value;
            }
        }

        public bool ActiveChangeCallbacksWasCalled { get; set; }

        public TokenCallbackRegistration Registration { get; set; }

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            Registration = new TokenCallbackRegistration()
            {
                RegisteredCallback = callback,
                RegisteredState = state,
            };
            return Registration;
        }

        public void Fire()
        {
            HasChanged = true;
            if (Registration != null && !Registration.Disposed)
            {
                Registration.RegisteredCallback(Registration.RegisteredState);
            }
        }
    }

    public class TokenCallbackRegistration : IDisposable
    {
        public Action<object> RegisteredCallback { get; set; }

        public object RegisteredState { get; set; }

        public bool Disposed { get; set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }
}
