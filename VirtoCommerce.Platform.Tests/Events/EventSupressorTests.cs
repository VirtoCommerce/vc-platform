using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Events;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Events
{
    [Trait("Category", "Unit")]
    public class EventSupressorTests
    {
        [Fact]
        public void SuppressInCurrentThread()
        {
            Assert.False(EventSupressor.EventsSuppressed, "EventSupressor shouldn't be active in this thread before test.");

            using (var guard = EventSupressor.SupressEvents())
            {
                Assert.True(EventSupressor.EventsSuppressed, "EventSupressor should be active in this thread.");
            }

            Assert.False(EventSupressor.EventsSuppressed, "EventSupressor shouldn't be active in this thread after test.");
        }

        [Fact]
        public void InheritedSuppressInAnotherThread()
        {
            Assert.False(EventSupressor.EventsSuppressed,
                "EventSupressor shouldn't be active in this thread before test.");

            using (var guard = EventSupressor.SupressEvents())
            {
                Assert.True(EventSupressor.EventsSuppressed, "EventSupressor should be active in this thread.");
                Task.Run(() =>
                {
                    Assert.True(EventSupressor.EventsSuppressed, "EventSupressor inherits value in another thread!");
                }).Wait();
            }

            Assert.False(EventSupressor.EventsSuppressed, "EventSupressor shouldn't be active in this thread after test.");
        }

        [Fact]
        public async Task NotInheritedSetAfterAsyncMethodStarts()
        {
            Assert.False(EventSupressor.EventsSuppressed,
                "EventSupressor shouldn't be active in this thread before test.");

            var taskCompletionSource = new TaskCompletionSource<object>();
            Func<Task> notInteritedAction = async () =>
            {
                await taskCompletionSource.Task;
                Assert.False(EventSupressor.EventsSuppressed, "EventSupressor shouldn't be active in this thread.");
            };

            var checkTask = notInteritedAction();

            using (var guard = EventSupressor.SupressEvents())
            {
                Assert.True(EventSupressor.EventsSuppressed, "EventSupressor inherits value in another thread!");
                taskCompletionSource.TrySetResult(null);
                await checkTask;
            }

            Assert.False(EventSupressor.EventsSuppressed, "EventSupressor shouldn't be active in this thread after test.");
        }

        [Fact]
        public void NotInheritedSetAfterAsyncMethodStartsInAnotherAsyncMethod()
        {
            Assert.False(EventSupressor.EventsSuppressed,
                "EventSupressor shouldn't be active in this thread before test.");

            var taskCompletionSource = new TaskCompletionSource<object>();
            Func<Task> notInheritedAction = async () =>
            {
                await taskCompletionSource.Task;
                Assert.False(EventSupressor.EventsSuppressed, "EventSupressor shouldn't be active in this thread.");
            };

            var checkTask = notInheritedAction();

            Task.Run(async () =>
            {
                using (var guard = EventSupressor.SupressEvents())
                {
                    Assert.True(EventSupressor.EventsSuppressed, "EventSupressor inherits value in another thread!");
                    taskCompletionSource.TrySetResult(null);
                    await checkTask;
                }
                Assert.False(EventSupressor.EventsSuppressed, "EventSupressor shouldn't be active in this thread after test.");
            }).Wait();

            Assert.False(EventSupressor.EventsSuppressed, "EventSupressor shouldn't be active in this thread after test.");
        }
    }
}
