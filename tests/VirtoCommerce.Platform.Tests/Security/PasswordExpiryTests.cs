using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Tests.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security
{
    public class PasswordExpiryTests : MemoryCacheTestsBase
    {
        [Theory]
        [ClassData(typeof(CreateExpiryTestData))]
        public async Task TestUserPasswordExpiry(DateTime? lastPasswordChangedDate, TimeSpan? maxPasswordAge, bool isPasswordExpired)
        {
            //Arrange
            var user = new ApplicationUser { Id = "id1", LastPasswordChangedDate = lastPasswordChangedDate };
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.Setup(x => x.FindByIdAsync(user.Id, CancellationToken.None)).ReturnsAsync(user);

            var userManager = SecurityMockHelpers.TestCustomUserManager(userStoreMock, new UserOptionsExtended
            {
                MaxPasswordAge = maxPasswordAge
            },
            GetPlatformMemoryCache());

            //Act
            user = await userManager.FindByIdAsync(user.Id);

            //Assert
            Assert.Equal(isPasswordExpired, user.PasswordExpired);
        }

        #region TestData

        class CreateExpiryTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // DateTime? lastPasswordChangedDate, TimeSpan? maxPasswordAge, bool isPasswordExpired
                yield return new object[] { null, null, false };
                yield return new object[] { null, TimeSpan.Zero, false };
                yield return new object[] { null, TimeSpan.FromDays(7), true };
                yield return new object[] { DateTime.UtcNow.AddDays(-9), null, false };
                yield return new object[] { DateTime.UtcNow.AddDays(-9), TimeSpan.Zero, false };
                yield return new object[] { DateTime.UtcNow.AddDays(-9), TimeSpan.FromDays(7), true };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion TestData
    }
}
