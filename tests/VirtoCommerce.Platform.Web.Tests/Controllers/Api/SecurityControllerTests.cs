using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.ExternalSignIn;
using VirtoCommerce.Platform.Web.Azure;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Model.Security;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Controllers.Api
{
    public class SecurityControllerTests : PlatformWebMockHelper
    {
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<RoleManager<Role>> _roleManagerMock;

        private readonly Mock<IPermissionsRegistrar> _permissionsProviderMock;
        private readonly Mock<IUserSearchService> _userSearchServiceMock;
        private readonly Mock<IRoleSearchService> _roleSearchServiceMock;
        private readonly Mock<IPasswordValidator<ApplicationUser>> _passwordValidatorMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly Mock<IUserApiKeyService> _userApiKeyServiceMock;
        private readonly Mock<ILogger<SecurityController>> _logger;

        private readonly IEnumerable<ExternalSignInProviderConfiguration> _externalSigninProviderConfigs;

        // Controller
        private SecurityController _controller;

        public SecurityControllerTests()
        {
            _permissionsProviderMock = new Mock<IPermissionsRegistrar>();
            _userSearchServiceMock = new Mock<IUserSearchService>();
            _roleSearchServiceMock = new Mock<IRoleSearchService>();
            _passwordValidatorMock = new Mock<IPasswordValidator<ApplicationUser>>();
            _emailSenderMock = new Mock<IEmailSender>();
            _eventPublisherMock = new Mock<IEventPublisher>();
            _userApiKeyServiceMock = new Mock<IUserApiKeyService>();
            _logger = new Mock<ILogger<SecurityController>>();

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                /* IOptions<IdentityOptions> optionsAccessor */null,
                /* IPasswordHasher<TUser> passwordHasher */null,
                /* IEnumerable<IUserValidator<TUser>> userValidators */null,
                /* IEnumerable<IPasswordValidator<TUser>> passwordValidators */null,
                /* ILookupNormalizer keyNormalizer */null,
                /* IdentityErrorDescriber errors */null,
                /* IServiceProvider services */null,
                /* ILogger<UserManager<TUser>> logger */null);

            _roleManagerMock = new Mock<RoleManager<Role>>(
                Mock.Of<IRoleStore<Role>>(), null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                /* IOptions<IdentityOptions> optionsAccessor */null,
                /* ILogger<SignInManager<TUser>> logger */null,
                /* IAuthenticationSchemeProvider schemes */null,
                /* IUserConfirmation<TUser> confirmation */null);

            _externalSigninProviderConfigs = new List<ExternalSignInProviderConfiguration>();

            _controller = CreateSecurityController();
        }

        private SecurityController CreateSecurityController(
            Mock<IOptions<PasswordOptionsExtended>> passwordOptions = null,
            Mock<IOptions<AuthorizationOptions>> securityOptions = null,
            Mock<IOptions<AzureAdOptions>> azureAdOptions = null,
            Mock<IOptions<PasswordLoginOptions>> passwordLoginOptions = null
            )
        {
            passwordOptions ??= new Mock<IOptions<PasswordOptionsExtended>> { DefaultValue = DefaultValue.Mock };

            securityOptions ??= new Mock<IOptions<AuthorizationOptions>> { DefaultValue = DefaultValue.Mock };

            azureAdOptions ??= new Mock<IOptions<AzureAdOptions>> { DefaultValue = DefaultValue.Mock };

            passwordLoginOptions ??= new Mock<IOptions<PasswordLoginOptions>> { DefaultValue = DefaultValue.Mock };

            return new SecurityController(
                signInManager: _signInManagerMock.Object,
                roleManager: _roleManagerMock.Object,
                permissionsProvider: _permissionsProviderMock.Object,
                userSearchService: _userSearchServiceMock.Object,
                roleSearchService: _roleSearchServiceMock.Object,
                securityOptions: securityOptions.Object,
                userOptionsExtended: Mock.Of<IOptions<UserOptionsExtended>>(),
                passwordOptions: passwordOptions.Object,
                passwordLoginOptions: passwordLoginOptions.Object,
                passwordValidator: _passwordValidatorMock.Object,
                emailSender: _emailSenderMock.Object,
                eventPublisher: _eventPublisherMock.Object,
                userApiKeyService: _userApiKeyServiceMock.Object,
                logger: _logger.Object,
                externalSigninProviderConfigs: _externalSigninProviderConfigs);
        }

        #region Login

        /// <summary>
        /// If sigin in manager returns fail result we should return 200 OK with succeeded = false to the client
        /// </summary>
        [Fact]
        public async Task Login_SignInFailed()
        {
            // Arrange
            var request = _fixture.Create<LoginRequest>();
            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var actual = await _controller.Login(request);
            var result = actual.ExtractFromOkResult();

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
        }

        /// <summary>
        /// If sigin in manager returns success result we should publish user login event
        /// and return 200 OK with succeeded = true to the client
        /// </summary>
        [Fact]
        public async Task Login_SignInSuccess()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            var request = _fixture.Create<LoginRequest>();
            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var actual = await _controller.Login(request);
            var result = actual.ExtractFromOkResult();

            // Assert
            _eventPublisherMock.Verify(x => x.Publish(It.Is<UserLoginEvent>(e => e.User == user), default), Times.Once);
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        /// <summary>
        /// If sigin in manager returns success result, but user role is customer we should publish
        /// user login event and return 200 OK with NotAllowed = true to the client
        /// </summary>
        [Fact]
        public async Task Login_UserRoleIsCustomer()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            var request = _fixture.Create<LoginRequest>();
            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock
                .Setup(x => x.IsInRoleAsync(user, Core.PlatformConstants.Security.SystemRoles.Customer))
                .ReturnsAsync(true);

            // Act
            var actual = await _controller.Login(request);
            var result = actual.ExtractFromOkResult();

            // Assert
            _eventPublisherMock.Verify(x => x.Publish(It.Is<UserLoginEvent>(e => e.User == user), default), Times.Once);
            result.Should().NotBeNull();
            result.IsNotAllowed.Should().BeTrue();
        }

        #endregion Login

        #region Logout

        /// <summary>
        /// If user found we should publish logout event
        /// </summary>
        [Fact]
        public async Task Logout_UserFound()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            await _controller.Logout();

            // Assert
            _signInManagerMock.Verify(x => x.SignOutAsync(), Times.Once);
            _eventPublisherMock.Verify(x => x.Publish(It.Is<UserLogoutEvent>(e => e.User == user), default), Times.Once);
        }

        /// <summary>
        /// If user not found we should do nothing
        /// </summary>
        [Fact]
        public async Task Logout_UserNotFound()
        {
            // Arrange
            ApplicationUser user = null;
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            await _controller.Logout();

            // Assert
            _signInManagerMock.Verify(x => x.SignOutAsync(), Times.Never);
            _eventPublisherMock.Verify(x => x.Publish(It.IsAny<UserLogoutEvent>(), default), Times.Never);
        }

        #endregion Logout

        #region Search roles

        /// <summary>
        /// Should retranslate result from serach service with 200 status code
        /// </summary>
        [Fact]
        public async Task SearchRoles()
        {
            // Arrange
            var request = _fixture.Create<RoleSearchCriteria>();
            var roleSearchResult = _fixture.Create<RoleSearchResult>();
            _roleSearchServiceMock
                .Setup(x => x.SearchRolesAsync(It.IsAny<RoleSearchCriteria>()))
                .ReturnsAsync(roleSearchResult);

            // Act
            var actual = await _controller.SearchRoles(request);
            var result = actual.ExtractFromOkResult();

            // Assert
            result.Should().BeEquivalentTo(roleSearchResult);
        }

        #endregion Search roles

        #region GetRole

        /// <summary>
        /// Should retranslate result from role manager with 200 status code
        /// </summary>
        [Fact]
        public async Task GetRole()
        {
            // Arrange
            var request = _fixture.Create<string>();
            var role = _fixture.Create<Role>();
            _roleManagerMock
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(role);

            // Act
            var actual = await _controller.GetRole(request);
            var result = actual.ExtractFromOkResult();

            // Assert
            result.Should().BeEquivalentTo(role);
        }

        #endregion GetRole

        #region DeleteRoles

        /// <summary>
        /// Should delete only existed roles
        /// </summary>
        [Fact]
        public async Task DeleteRoles()
        {
            // Arrange
            var roleIds = _fixture.CreateMany<string>().ToList();
            var roles = _fixture.CreateMany<Role>(roleIds.Count - 1).ToList();
            // Emulate role with last roleid is not exist
            roles.Add(null);

            var indexer = 0;
            _roleManagerMock
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Callback<string>((id) => indexer = roleIds.IndexOf(id))
                .ReturnsAsync(() => roles[indexer]);

            // Act
            await _controller.DeleteRoles(roleIds.ToArray());

            // Assert
            _roleManagerMock.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Exactly(roleIds.Count));
            _roleManagerMock.Verify(x => x.DeleteAsync(It.IsAny<Role>()), Times.Exactly(roleIds.Count - 1));
        }

        #endregion DeleteRoles

        #region UpdateRole

        /// <summary>
        /// Should find role by id and update role
        /// </summary>
        [Fact]
        public async Task UpdateRole_IdNotNull_RoleExist()
        {
            // Arrange
            var role = _fixture.Create<Role>();
            _roleManagerMock
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(role);
            _roleManagerMock
                .Setup(x => x.UpdateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _controller.UpdateRole(role);

            // Assert
            _roleManagerMock.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once());
            _roleManagerMock.Verify(x => x.UpdateAsync(It.IsAny<Role>()), Times.Once());
        }

        /// <summary>
        /// Should not find role by id and create role
        /// </summary>
        [Fact]
        public async Task UpdateRole_IdNotNull_RoleNotExist()
        {
            // Arrange
            var role = _fixture.Create<Role>();
            _roleManagerMock
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Role)null);
            _roleManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _controller.UpdateRole(role);

            // Assert
            _roleManagerMock.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once());
            _roleManagerMock.Verify(x => x.CreateAsync(It.IsAny<Role>()), Times.Once());
        }

        /// <summary>
        /// Should find role by name and update role
        /// </summary>
        [Fact]
        public async Task UpdateRole_IdNull_RoleExist()
        {
            // Arrange
            var role = _fixture.Create<Role>();
            role.Id = null;

            _roleManagerMock
                .Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _roleManagerMock
                .Setup(x => x.UpdateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _controller.UpdateRole(role);

            // Assert
            _roleManagerMock.Verify(x => x.RoleExistsAsync(It.IsAny<string>()), Times.Once());
            _roleManagerMock.Verify(x => x.UpdateAsync(It.IsAny<Role>()), Times.Once());
        }

        /// <summary>
        /// Should not find role by name and create role
        /// </summary>
        [Fact]
        public async Task UpdateRole_IdNull_RoleNotExist()
        {
            // Arrange
            var role = _fixture.Create<Role>();
            role.Id = null;

            _roleManagerMock
                .Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            _roleManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _controller.UpdateRole(role);

            // Assert
            _roleManagerMock.Verify(x => x.RoleExistsAsync(It.IsAny<string>()), Times.Once());
            _roleManagerMock.Verify(x => x.CreateAsync(It.IsAny<Role>()), Times.Once());
        }

        #endregion UpdateRole

        #region RequestPasswordReset

        /// <summary>
        /// Should return next available pass request date
        /// </summary>
        [Fact]
        public async Task RequestPasswordReset_UserPasswordResetBeforeTimeLimit()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            user.UserName = "test";
            user.LastPasswordChangeRequestDate = DateTime.UtcNow;
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.Is<string>(x => x == user.UserName)))
                .ReturnsAsync(user);

            // Act
            var actual = await _controller.RequestPasswordReset(user.UserName);

            // Assert
            var result = actual.ExtractDynamicPropertyFromOkResult<DateTime>("NextRequestAt");
            result.Should().BeAfter(user.LastPasswordChangeRequestDate.Value);
        }

        #endregion

        #region ResetPassword

        [Fact]
        public async Task ResetPassword_NoUser_UnsuccessfulSecurityResult()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            user.UserName = "test";

            var userName = "test";
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.Is<string>(x => x == user.UserName)))
                .ReturnsAsync(() => { return null; });

            var currentUser = _fixture.Create<ApplicationUser>();
            currentUser.IsAdministrator = false;
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.Is<string>(x => x == null)))
                .ReturnsAsync(currentUser);

            // Act
            var actual = await _controller.ResetPassword(userName, null);

            // Assert
            var result = actual.ExtractFromOkResult();
            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async Task ResetPassword_UserNonEditable_UnsuccessfulSecurityResult()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            user.UserName = "test";
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.Is<string>(x => x == user.UserName)))
                .ReturnsAsync(user);

            var currentUser = _fixture.Create<ApplicationUser>();
            currentUser.IsAdministrator = false;
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.Is<string>(x => x == null)))
                .ReturnsAsync(currentUser);

            var options = new Mock<IOptions<AuthorizationOptions>>();
            options.SetupGet(x => x.Value)
                .Returns(() =>
                {
                    return new AuthorizationOptions
                    {
                        NonEditableUsers = new[] { user.UserName },
                    };
                });

            _controller = CreateSecurityController(securityOptions: options);

            // Act
            var actual = await _controller.ResetPassword(user.UserName, null);

            // Assert
            var result = actual.ExtractFromOkResult();
            result.Succeeded.Should().BeFalse();
        }

        #endregion

        #region ResetPasswordByToken

        [Fact]
        public async Task ResetPasswordByToken_NoUser_UnsuccessfulSecurityResult()
        {
            // Arrange
            var userName = "test";
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(() => { return null; });

            // Act
            var actual = await _controller.ResetPasswordByToken(userName, null);

            // Assert
            var result = actual.ExtractFromOkResult();
            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async Task ResetPasswordByToken_UserNonEditable_UnsuccessfulSecurityResult()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            user.UserName = "test";
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.Is<string>(x => x == user.UserName)))
                .ReturnsAsync(user);

            var options = new Mock<IOptions<AuthorizationOptions>>();
            options.SetupGet(x => x.Value)
                .Returns(() =>
                {
                    return new AuthorizationOptions
                    {
                        NonEditableUsers = new[] { user.UserName },
                    };
                });

            _controller = CreateSecurityController(securityOptions: options);

            // Act
            var actual = await _controller.ResetPasswordByToken(user.UserName, null);

            // Assert
            var result = actual.ExtractFromOkResult();
            result.Succeeded.Should().BeFalse();
        }

        #endregion

        #region ChangePassword

        [Fact]
        public async Task ChangePassword_NoUser_UnsuccessfulSecurityResult()
        {
            // Arrange
            var userName = "test";
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.Is<string>(x => x == userName)))
                .ReturnsAsync(() => { return null; });

            var currentUser = _fixture.Create<ApplicationUser>();
            currentUser.IsAdministrator = false;
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.Is<string>(x => x == null)))
                .ReturnsAsync(currentUser);

            var options = new Mock<IOptions<AuthorizationOptions>>();
            options.SetupGet(x => x.Value)
                .Returns(() =>
                {
                    return new AuthorizationOptions
                    {
                        NonEditableUsers = Array.Empty<string>(),
                    };
                });

            _controller = CreateSecurityController(securityOptions: options);

            // Act
            var actual = await _controller.ChangePassword(userName, new ChangePasswordRequest { NewPassword = "new" });

            // Assert
            var result = actual.ExtractFromOkResult();
            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async Task ChangePassword_UserNonEditable_UnsuccessfulSecurityResult()
        {
            // Arrange
            var userName = "test";

            var currentUser = _fixture.Create<ApplicationUser>();
            currentUser.IsAdministrator = false;
            _userManagerMock
                .Setup(x => x.FindByNameAsync(It.Is<string>(x => x == null)))
                .ReturnsAsync(currentUser);

            var options = new Mock<IOptions<AuthorizationOptions>>();
            options.SetupGet(x => x.Value)
                .Returns(() =>
                {
                    return new AuthorizationOptions
                    {
                        NonEditableUsers = new[] { userName },
                    };
                });

            _controller = CreateSecurityController(securityOptions: options);

            // Act
            var actual = await _controller.ChangePassword(userName, new ChangePasswordRequest { NewPassword = "new" });

            // Assert
            var result = actual.ExtractFromOkResult();
            result.Succeeded.Should().BeFalse();
        }

        #endregion
    }
}
