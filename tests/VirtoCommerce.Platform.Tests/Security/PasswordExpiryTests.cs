using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Tests.Caching;
using VirtoCommerce.Platform.Web.Security;
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

            var userManager = SecurityMockHelpers.TestCustomUserManager(userStoreMock,
                new UserOptionsExtended
                {
                    MaxPasswordAge = maxPasswordAge
                },
                null, GetPlatformMemoryCache());

            //Act
            user = await userManager.FindByIdAsync(user.Id);

            //Assert
            Assert.Equal(isPasswordExpired, user.PasswordExpired);
        }

        [Theory]
        [ClassData(typeof(CreateExpiryTestDataCalculator))]
        public void TestPasswordExpiryCalculator(bool passwordExpired, int remindPasswordExpiryInDays, TimeSpan? maxPasswordAge, int expectedDays)
        {
            //Arrange
            var user = new ApplicationUser
            {
                LastPasswordChangedDate = DateTime.UtcNow.AddDays(-20),
                PasswordExpired = passwordExpired
            };

            var userOptionsExtended = new UserOptionsExtended
            {
                RemindPasswordExpiryInDays = remindPasswordExpiryInDays,
                MaxPasswordAge = maxPasswordAge
            };

            //Act
            var daysTillPasswordExpiry = PasswordExpiryHelper.ContDaysTillPasswordExpiry(user, userOptionsExtended);


            //Assert
            Assert.Equal(expectedDays, daysTillPasswordExpiry);
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

        class CreateExpiryTestDataCalculator : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // The password was changed 20 days ago. Other params:
                // bool passwordExpired, int remindPasswordExpiryInDays, TimeSpan? maxPasswordAge, int expectedDays
                yield return new object[] { false, 0, null, -1 };
                yield return new object[] { false, 0, TimeSpan.Zero, -1 };
                yield return new object[] { false, 0, TimeSpan.FromDays(25), -1 };
                yield return new object[] { false, 7, null, -1 };
                yield return new object[] { false, 7, TimeSpan.Zero, -1 };
                yield return new object[] { false, 7, new TimeSpan(20, 0, 0, 5), 0 };
                yield return new object[] { false, 7, TimeSpan.FromDays(21), 0 };
                yield return new object[] { false, 7, TimeSpan.FromDays(25), 4 };
                yield return new object[] { false, 7, TimeSpan.FromDays(28), -1 };
                yield return new object[] { true, 7, TimeSpan.FromDays(25), -1 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion TestData
    }
}
