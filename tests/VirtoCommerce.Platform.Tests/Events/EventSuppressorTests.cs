using System;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Events;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Events
{
    [Trait("Category", "Unit")]
    public class EventSuppressorTests
    {
        [Fact]
        public void SuppressInCurrentThread()
        {
            Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread before test.");

            using (EventSuppressor.SuppressEvents())
            {
                Assert.True(EventSuppressor.EventsSuppressed, "EventSuppressor should be active in this thread.");
            }

            Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread after test.");
        }

        [Fact]
        public async Task InheritedSuppressInAnotherThread()
        {
            Assert.False(EventSuppressor.EventsSuppressed,
                "EventSuppressor shouldn't be active in this thread before test.");

            using (EventSuppressor.SuppressEvents())
            {
                Assert.True(EventSuppressor.EventsSuppressed, "EventSuppressor should be active in this thread.");
                await Task.Run(() =>
                {
                    Assert.True(EventSuppressor.EventsSuppressed, "EventSuppressor inherits value in another thread!");
                });
            }

            Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread after test.");
        }

        [Fact]
        public async Task NotInheritedSetAfterAsyncMethodStarts()
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

            using (EventSuppressor.SuppressEvents())
            {
                Assert.True(EventSuppressor.EventsSuppressed, "EventSuppressor inherits value in another thread!");
                taskCompletionSource.TrySetResult(null);
                await checkTask;
            }

            Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread after test.");
        }

        [Fact]
        public async Task NotInheritedSetAfterAsyncMethodStartsInAnotherAsyncMethod()
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

            await Task.Run(async () =>
            {
                using (EventSuppressor.SuppressEvents())
                {
                    Assert.True(EventSuppressor.EventsSuppressed, "EventSuppressor inherits value in another thread!");
                    taskCompletionSource.TrySetResult(null);
                    await checkTask;
                }
                Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread after test.");
            });

            Assert.False(EventSuppressor.EventsSuppressed, "EventSuppressor shouldn't be active in this thread after test.");
        }
    }
}
