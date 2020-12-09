using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Data.ChangeLog;
using VirtoCommerce.Platform.Tests.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class LastChangesServiceTests : MemoryCacheTestsBase
    {
        private const string entity1 = "Entity1";
        private const string entity2 = "Entity2";

        [Fact]
        public void RepeatableRead()
        {
            ILastChangesService lastChangesService = new LastChangesService(GetPlatformMemoryCache());
            var entity1_1stAttempt = lastChangesService.GetLastModified(entity1);
            var entity2_1stAttempt = lastChangesService.GetLastModified(entity2);
            System.Threading.Thread.Sleep(10);
            // Next reads should return the same value
            var entity1_2stAttempt = lastChangesService.GetLastModified(entity1);
            var entity2_2stAttempt = lastChangesService.GetLastModified(entity2);
            Assert.Equal(entity1_1stAttempt, entity1_2stAttempt);
            Assert.Equal(entity2_1stAttempt, entity2_2stAttempt);
        }

        [Fact]
        public void Reset()
        {
            ILastChangesService lastChangesService = new LastChangesService(GetPlatformMemoryCache());
            var entity1_1stAttempt = lastChangesService.GetLastModified(entity1);
            var entity2_1stAttempt = lastChangesService.GetLastModified(entity2);
            System.Threading.Thread.Sleep(10);
            lastChangesService.Reset(entity2);
            // Next read entity1 should have the same value.
            // entity2 -- different, because it was reset.
            var entity1_2stAttempt = lastChangesService.GetLastModified(entity1);
            var entity2_2stAttempt = lastChangesService.GetLastModified(entity2);
            Assert.Equal(entity1_1stAttempt, entity1_2stAttempt);
            Assert.NotEqual(entity2_1stAttempt, entity2_2stAttempt);
        }
    }
}
