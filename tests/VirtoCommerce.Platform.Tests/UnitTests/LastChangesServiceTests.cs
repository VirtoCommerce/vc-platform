using System.Threading;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.ChangeLog;
using VirtoCommerce.Platform.Tests.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class LastChangesServiceTests : MemoryCacheTestsBase
    {
        private const string _firstEntity = "FirstEntity";
        private const string _secondEntity = "SecondEntity";

        private class BaseEntity : Entity
        {
        }

        private class DerivedEntity : BaseEntity
        {
        }

        [Fact]
        public void RepeatableRead()
        {
            ILastChangesService lastChangesService = new LastChangesService(GetPlatformMemoryCache());
            var firstEntityFirstAttempt = lastChangesService.GetLastModifiedDate(_firstEntity);
            var secondEntityFirstAttempt = lastChangesService.GetLastModifiedDate(_secondEntity);

            Thread.Sleep(10);

            // Next reads should return the same value

            var firstEntitySecondAttempt = lastChangesService.GetLastModifiedDate(_firstEntity);
            var secondEntitySecondAttempt = lastChangesService.GetLastModifiedDate(_secondEntity);

            Assert.Equal(firstEntityFirstAttempt, firstEntitySecondAttempt);
            Assert.Equal(secondEntityFirstAttempt, secondEntitySecondAttempt);
        }

        [Fact]
        public void Reset()
        {
            ILastChangesService lastChangesService = new LastChangesService(GetPlatformMemoryCache());
            var firstEntityFirstAttempt = lastChangesService.GetLastModifiedDate(_firstEntity);
            var secondEntityFirstAttempt = lastChangesService.GetLastModifiedDate(_secondEntity);

            Thread.Sleep(10);
            lastChangesService.Reset(_secondEntity);

            // Next read _firstEntity should have the same value.
            // _secondEntity -- different, because it was reset.

            var firstEntitySecondAttempt = lastChangesService.GetLastModifiedDate(_firstEntity);
            var secondEntitySecondAttempt = lastChangesService.GetLastModifiedDate(_secondEntity);

            Assert.Equal(firstEntityFirstAttempt, firstEntitySecondAttempt);
            Assert.NotEqual(secondEntityFirstAttempt, secondEntitySecondAttempt);
        }

        [Fact]
        public void ResetDatesForBaseEntityTypes()
        {
            // Arrange
            ILastChangesService lastChangesService = new LastChangesService(GetPlatformMemoryCache());
            var initialDateForBaseEntity = lastChangesService.GetLastModifiedDate(typeof(BaseEntity).FullName);
            var initialDateForDerivedEntity = lastChangesService.GetLastModifiedDate(typeof(DerivedEntity).FullName);

            // Act
            Thread.Sleep(10);
            lastChangesService.Reset(new DerivedEntity());

            var dateForBaseEntity = lastChangesService.GetLastModifiedDate(typeof(BaseEntity).FullName);
            var dateForDerivedEntity = lastChangesService.GetLastModifiedDate(typeof(DerivedEntity).FullName);

            // Assert
            Assert.NotEqual(dateForBaseEntity, initialDateForBaseEntity);
            Assert.NotEqual(dateForDerivedEntity, initialDateForDerivedEntity);
        }
    }
}
