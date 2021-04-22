using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Model.Security;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Controllers.Api
{
    public class SecurityControllerTests : PlatformWebMockHelper
    {
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

        private readonly Mock<IPermissionsRegistrar> _permissionsProviderMock;
        private readonly Mock<IUserSearchService> _userSearchServiceMock;
        private readonly Mock<IRoleSearchService> _roleSearchServiceMock;
        private readonly Mock<IPasswordValidator<ApplicationUser>> _passwordValidatorMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly Mock<IUserApiKeyService> _userApiKeyServiceMock;

        // Controller
        private readonly SecurityController _controller;

        public SecurityControllerTests()
        {
            _permissionsProviderMock = new Mock<IPermissionsRegistrar>();
            _userSearchServiceMock = new Mock<IUserSearchService>();
            _roleSearchServiceMock = new Mock<IRoleSearchService>();
            _passwordValidatorMock = new Mock<IPasswordValidator<ApplicationUser>>();
            _emailSenderMock = new Mock<IEmailSender>();
            _eventPublisherMock = new Mock<IEventPublisher>();
            _userApiKeyServiceMock = new Mock<IUserApiKeyService>();

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                    /* IUserStore<TUser> store */Mock.Of<IUserStore<ApplicationUser>>(),
                    /* IOptions<IdentityOptions> optionsAccessor */null,
                    /* IPasswordHasher<TUser> passwordHasher */null,
                    /* IEnumerable<IUserValidator<TUser>> userValidators */null,
                    /* IEnumerable<IPasswordValidator<TUser>> passwordValidators */null,
                    /* ILookupNormalizer keyNormalizer */null,
                    /* IdentityErrorDescriber errors */null,
                    /* IServiceProvider services */null,
                    /* ILogger<UserManager<TUser>> logger */null);

            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                _userManagerMock.Object,
                /* IHttpContextAccessor contextAccessor */Mock.Of<IHttpContextAccessor>(),
                /* IUserClaimsPrincipalFactory<TUser> claimsFactory */Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                /* IOptions<IdentityOptions> optionsAccessor */null,
                /* ILogger<SignInManager<TUser>> logger */null,
                /* IAuthenticationSchemeProvider schemes */null,
                /* IUserConfirmation<TUser> confirmation */null);

            _controller = new SecurityController(
                signInManager: _signInManagerMock.Object,
                roleManager: null,
                permissionsProvider: _permissionsProviderMock.Object,
                userSearchService: _userSearchServiceMock.Object,
                roleSearchService: _roleSearchServiceMock.Object,
                securityOptions: Mock.Of<IOptions<AuthorizationOptions>>(),
                userOptionsExtended: Mock.Of<IOptions<UserOptionsExtended>>(),
                passwordValidator: _passwordValidatorMock.Object,
                emailSender: _emailSenderMock.Object,
                eventPublisher: _eventPublisherMock.Object,
                userApiKeyService: _userApiKeyServiceMock.Object
                );
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
    }
}
