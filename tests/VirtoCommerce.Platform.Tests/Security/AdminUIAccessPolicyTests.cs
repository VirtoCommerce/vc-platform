using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Services;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security
{
    public class AdminUIAccessPolicyTests
    {
        private static ConfigurationAdminUIAccessPolicy CreatePolicy(AdminUIAccessOptions access = null)
        {
            var options = new PlatformUIOptions();
            if (access != null)
            {
                options.Access = access;
            }

            var monitor = new Mock<IOptionsMonitor<PlatformUIOptions>>();
            monitor.SetupGet(x => x.CurrentValue).Returns(options);

            return new ConfigurationAdminUIAccessPolicy(monitor.Object);
        }

        private static AdminUIAccessContext CreateContext(
            bool isAdministrator = false,
            string userType = nameof(UserType.Manager),
            params string[] permissions) =>
            new()
            {
                User = new ApplicationUser
                {
                    UserName = "test",
                    IsAdministrator = isAdministrator,
                    UserType = userType,
                },
                Permissions = permissions,
            };

        #region Rule 1 - administrator bypass

        [Fact]
        public async Task Administrator_IsAllowed_EvenWithNoPermissionsAndDisallowedAccountType()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                AllowedAccountTypes = [nameof(UserType.Manager)],
            });
            var context = CreateContext(isAdministrator: true, userType: nameof(UserType.Customer));

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.True(result.IsAllowed);
        }

        [Fact]
        public async Task Administrator_IsSubjectToRules_WhenAllowAdministratorsIsFalse()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                AllowAdministrators = false,
                AllowedAccountTypes = [nameof(UserType.Manager)],
            });
            var context = CreateContext(true, nameof(UserType.Customer), "platform:setting:access");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.False(result.IsAllowed);
        }

        #endregion

        #region Rule 2 - allowed account types

        [Fact]
        public async Task AccountTypeNotInAllowedList_IsDenied()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                AllowedAccountTypes = [nameof(UserType.Manager), nameof(UserType.Administrator)],
            });
            var context = CreateContext(false, nameof(UserType.Customer), "platform:setting:access");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.False(result.IsAllowed);
        }

        [Fact]
        public async Task AccountTypeInAllowedList_IsAllowed()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                AllowedAccountTypes = [nameof(UserType.Manager), nameof(UserType.Administrator)],
            });
            var context = CreateContext(false, nameof(UserType.Manager), "platform:setting:access");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.True(result.IsAllowed);
        }

        [Fact]
        public async Task AccountTypeComparison_IsCaseInsensitive()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                AllowedAccountTypes = ["manager"],
            });
            var context = CreateContext(false, "Manager", "platform:setting:access");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.True(result.IsAllowed);
        }

        [Fact]
        public async Task EmptyUserType_IsDenied_WhenAllowedAccountTypesConfigured()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                AllowedAccountTypes = [nameof(UserType.Manager)],
            });
            var context = CreateContext(false, null, "platform:setting:access");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.False(result.IsAllowed);
        }

        [Fact]
        public async Task EmptyAllowedAccountTypes_DoesNotConstrainAccountType()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions());
            var context = CreateContext(false, nameof(UserType.Customer), "platform:setting:access");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.True(result.IsAllowed);
        }

        #endregion

        #region Rules 3 and 4 - permission allow / deny masks

        [Fact]
        public async Task PermissionMatchingDeniedMask_IsDenied()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                DeniedPermissions = ["mymodule:customer-api:*"],
            });
            var context = CreateContext(false, nameof(UserType.Manager), "mymodule:customer-api:read");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.False(result.IsAllowed);
        }

        [Fact]
        public async Task DeniedMask_TakesPrecedenceOverBroaderAllowedMask()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                AllowedPermissions = ["platform:*"],
                DeniedPermissions = ["platform:setting:*"],
            });
            var context = CreateContext(false, nameof(UserType.Manager), "platform:setting:access");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.False(result.IsAllowed);
        }

        [Fact]
        public async Task AnyDeniedPermission_DeniesAccess_EvenWhenOtherPermissionsAreAllowed()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                DeniedPermissions = ["mymodule:customer-api:*"],
            });
            var context = CreateContext(false, nameof(UserType.Manager), "catalog:read", "mymodule:customer-api:read");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.False(result.IsAllowed);
        }

        [Fact]
        public async Task NoPermissionMatchingAllowedMask_IsDenied()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                AllowedPermissions = ["platform:*"],
            });
            var context = CreateContext(false, nameof(UserType.Manager), "catalog:read");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.False(result.IsAllowed);
        }

        [Fact]
        public async Task PermissionMatchingAllowedMask_IsAllowed()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                AllowedPermissions = ["platform:*"],
            });
            var context = CreateContext(false, nameof(UserType.Manager), "platform:setting:access");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.True(result.IsAllowed);
        }

        #endregion

        #region Rule 5 - RequireAnyPermission preserves the existing behaviour

        [Fact]
        public async Task NonAdministratorWithNoPermissions_IsDenied_ByDefault()
        {
            //Arrange
            var policy = CreatePolicy();
            var context = CreateContext();

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.False(result.IsAllowed);
        }

        [Fact]
        public async Task NonAdministratorWithOnePermission_IsAllowed_ByDefault()
        {
            //Arrange
            var policy = CreatePolicy();
            var context = CreateContext(false, nameof(UserType.Manager), "catalog:read");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.True(result.IsAllowed);
        }

        [Fact]
        public async Task NonAdministratorWithNoPermissions_IsAllowed_WhenRequireAnyPermissionIsFalse()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions { RequireAnyPermission = false });
            var context = CreateContext();

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.True(result.IsAllowed);
        }

        #endregion

        #region The reported scenario

        [Fact]
        public async Task CustomerWithApiPermission_IsDeniedAdminUI_ButKeepsThePermission()
        {
            //Arrange
            var policy = CreatePolicy(new AdminUIAccessOptions
            {
                AllowedAccountTypes = [nameof(UserType.Manager), nameof(UserType.Administrator)],
            });
            var context = CreateContext(false, nameof(UserType.Customer), "mymodule:custom-endpoint:access");

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.False(result.IsAllowed);
            Assert.False(string.IsNullOrEmpty(result.DenyReason));
            // The policy only gates the admin UI - it must not strip the user's permissions.
            Assert.Contains("mymodule:custom-endpoint:access", context.Permissions);
        }

        #endregion

        #region Context validation

        [Fact]
        public async Task NullPermissions_AreTreatedAsEmpty()
        {
            //Arrange
            var policy = CreatePolicy();
            var context = new AdminUIAccessContext
            {
                User = new ApplicationUser { UserName = "test" },
                Permissions = null,
            };

            //Act
            var result = await policy.EvaluateAsync(context, TestContext.Current.CancellationToken);

            //Assert
            Assert.False(result.IsAllowed);
        }

        [Fact]
        public async Task NullContext_Throws()
        {
            //Arrange
            var policy = CreatePolicy();

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => policy.EvaluateAsync(null, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task NullUser_Throws()
        {
            //Arrange
            var policy = CreatePolicy();
            var context = new AdminUIAccessContext { User = null };

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => policy.EvaluateAsync(context, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task CancelledToken_Throws()
        {
            //Arrange
            var policy = CreatePolicy();
            var context = CreateContext(false, nameof(UserType.Manager), "catalog:read");
            using var cancellationTokenSource = new CancellationTokenSource();
            await cancellationTokenSource.CancelAsync();

            //Act & Assert
            // This test is about the policy honoring an already-cancelled token, so it must pass
            // its own instead of TestContext.Current.CancellationToken.
#pragma warning disable xUnit1051
            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                () => policy.EvaluateAsync(context, cancellationTokenSource.Token));
#pragma warning restore xUnit1051
        }

        #endregion
    }
}
