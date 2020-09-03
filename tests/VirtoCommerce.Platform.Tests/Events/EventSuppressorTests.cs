using System;
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
            Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread before test.");

            using (EventSuppressor.SupressEvents())
            {
                Assert.True(EventSuppressor.EventsSuppressed, "EventSuppressor should be active in this thread.");
            }

            Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread after test.");
        }

        [Fact]
        public void InheritedSuppressInAnotherThread()
        {
            Assert.False(EventSuppressor.EventsSuppressed,
                "EventSuppressor shouldn't be active in this thread before test.");

            using (EventSuppressor.SupressEvents())
            {
                Assert.True(EventSuppressor.EventsSuppressed, "EventSuppressor should be active in this thread.");
                Task.Run(() =>
                {
                    Assert.True(EventSuppressor.EventsSuppressed, "EventSuppressor inherits value in another thread!");
                }).Wait();
            }

            Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread after test.");
        }

        [Fact]
        public async Task NotInheritedSetAfterAsyncMethodStarts()
        {
            Assert.False(EventSuppressor.EventsSuppressed,
                "EventSuppressor shouldn't be active in this thread before test.");

            var taskCompletionSource = new TaskCompletionSource<object>();
            Func<Task> notInteritedAction = async () =>
            {
                await taskCompletionSource.Task;
                Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread.");
            };

            var checkTask = notInteritedAction();

            using (EventSuppressor.SupressEvents())
            {
                Assert.True(EventSuppressor.EventsSuppressed, "EventSuppressor inherits value in another thread!");
                taskCompletionSource.TrySetResult(null);
                await checkTask;
            }

            Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread after test.");
        }

        [Fact]
        public void NotInheritedSetAfterAsyncMethodStartsInAnotherAsyncMethod()
        {
            Assert.False(EventSuppressor.EventsSuppressed,
                "EventSuppressor shouldn't be active in this thread before test.");

            var taskCompletionSource = new TaskCompletionSource<object>();
            Func<Task> notInheritedAction = async () =>
            {
                await taskCompletionSource.Task;
                Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread.");
            };

            var checkTask = notInheritedAction();

            Task.Run(async () =>
            {
                using (EventSuppressor.SupressEvents())
                {
                    Assert.True(EventSuppressor.EventsSuppressed, "EventSuppressor inherits value in another thread!");
                    taskCompletionSource.TrySetResult(null);
                    await checkTask;
                }
                Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread after test.");
            }).Wait();

            Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread after test.");
        }
    }
}
