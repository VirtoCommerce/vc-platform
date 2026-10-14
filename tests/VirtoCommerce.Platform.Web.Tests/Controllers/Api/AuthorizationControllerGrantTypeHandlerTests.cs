using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Web.Controllers.Api;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Controllers.Api;

public class AuthorizationControllerGrantTypeHandlerTests
{
    private const string _grantType = "custom_grant";

    [Fact]
    public async Task Exchange_Should_UseTheLastRegisteredHandler_When_TwoHandlersShareGrantType()
    {
        var firstResult = new OkObjectResult("first");
        var lastResult = new OkObjectResult("last");
        var firstHandler = CreateHandler(firstResult);
        var lastHandler = CreateHandler(lastResult);
        var controller = CreateController(firstHandler.Object, lastHandler.Object);

        var actionResult = await controller.Exchange();

        actionResult.Should().BeSameAs(lastResult);
        firstHandler.Verify(x => x.HandleAsync(It.IsAny<TokenRequestContext>()), Times.Never);
    }

    private static Mock<IGrantTypeHandler> CreateHandler(ActionResult result)
    {
        var handler = new Mock<IGrantTypeHandler>();
        handler.SetupGet(x => x.GrantType).Returns(_grantType);
        handler.Setup(x => x.HandleAsync(It.IsAny<TokenRequestContext>())).ReturnsAsync(result);

        return handler;
    }

    private static AuthorizationController CreateController(params IGrantTypeHandler[] grantTypeHandlers)
    {
        var userManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        var signInManager = new Mock<SignInManager<ApplicationUser>>(
            userManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null, null, null, null);

        var controller = new AuthorizationController(
            applicationManager: null,
            identityOptions: Options.Create(new IdentityOptions()),
            signInManager: signInManager.Object,
            passwordLoginOptions: Options.Create(new PasswordLoginOptions()),
            eventPublisher: null,
            requestValidators: [],
            claimProviders: [],
            requestHandlers: [],
            grantTypeHandlers: grantTypeHandlers,
            tokenManager: null,
            authorizationService: null,
            externalSignInService: null,
            authorizationManager: null,
            scopeManager: null,
            authorizationOptions: Options.Create(new AuthorizationOptions()));

        var httpContext = new DefaultHttpContext();

        httpContext.Features.Set(new OpenIddictServerAspNetCoreFeature
        {
            Transaction = new OpenIddictServerTransaction { Request = new OpenIddictRequest { GrantType = _grantType } },
        });

        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

        return controller;
    }
}
