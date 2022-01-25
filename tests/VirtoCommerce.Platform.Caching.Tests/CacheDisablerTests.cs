using System;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests
{
    public class CacheDisablerTests
    {
        [Fact]
        public void DisablesInCurrentThread()
        {
            Assert.False(CacheDisabler.CacheDisabled, "CacheDisabler shouldn't be active in this thread before test.");

            using (CacheDisabler.DisableCache())
            {
                Assert.True(CacheDisabler.CacheDisabled, "CacheDisabler should be active in this thread.");
            }

            Assert.False(CacheDisabler.CacheDisabled, "CacheDisabler shouldn't be active in this thread after test.");
        }

        [Fact]
        public void InheritedSuppressInAnotherThread()
        {
            Assert.False(CacheDisabler.CacheDisabled,
                "CacheDisabler shouldn't be active in this thread before test.");

            using (CacheDisabler.DisableCache())
            {
                Assert.True(CacheDisabler.CacheDisabled, "CacheDisabler should be active in this thread.");
                Task.Run(() =>
                {
                    Assert.True(CacheDisabler.CacheDisabled, "CacheDisabler inherits value in another thread!");
                }).Wait();
            }

            Assert.False(CacheDisabler.CacheDisabled, "CacheDisabler shouldn't be active in this thread after test.");
        }

        [Fact]
        public async Task NotInheritedSetAfterAsyncMethodStarts()
        {
            Assert.False(CacheDisabler.CacheDisabled,
                "CacheDisabler shouldn't be active in this thread before test.");

            var taskCompletionSource = new TaskCompletionSource<object>();
            Func<Task> notInteritedAction = async () =>
            {
                await taskCompletionSource.Task;
                Assert.False(CacheDisabler.CacheDisabled, "CacheDisabler shouldn't be active in this thread.");
            };

            var checkTask = notInteritedAction();

            using (CacheDisabler.DisableCache())
            {
                Assert.True(CacheDisabler.CacheDisabled, "CacheDisabler inherits value in another thread!");
                taskCompletionSource.TrySetResult(null);
                await checkTask;
            }

            Assert.False(CacheDisabler.CacheDisabled, "CacheDisabler shouldn't be active in this thread after test.");
        }

        [Fact]
        public void NotInheritedSetAfterAsyncMethodStartsInAnotherAsyncMethod()
        {
            Assert.False(CacheDisabler.CacheDisabled,
                "CacheDisabler shouldn't be active in this thread before test.");

            var taskCompletionSource = new TaskCompletionSource<object>();
            Func<Task> notInheritedAction = async () =>
            {
                await taskCompletionSource.Task;
                Assert.False(CacheDisabler.CacheDisabled, "CacheDisabler shouldn't be active in this thread.");
            };

            var checkTask = notInheritedAction();

            Task.Run(async () =>
            {
                using (CacheDisabler.DisableCache())
                {
                    Assert.True(CacheDisabler.CacheDisabled, "CacheDisabler inherits value in another thread!");
                    taskCompletionSource.TrySetResult(null);
                    await checkTask;
                }
                Assert.False(CacheDisabler.CacheDisabled, "CacheDisabler shouldn't be active in this thread after test.");
            }).Wait();

            Assert.False(CacheDisabler.CacheDisabled, "CacheDisabler shouldn't be active in this thread after test.");
        }
    }
}
