using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using VirtoCommerce.Platform.Security.SignInLog;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

// Modules and custom projects compiled against earlier platform versions derive from these services and forward a
// Func<T> to base(...). The legacy constructors are protected, so that is the only way to reach them; these tests
// fail to compile if one of them disappears.
[Trait("Category", "Unit")]
public class LegacyConstructorCompatibilityTests
{
    [Fact]
    public async Task DerivedService_ForwardingFuncToBase_StillWritesThroughTheRepository()
    {
        var added = new List<UserSignInLogEntity>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(new List<UserSignInLogEntity>().BuildMock());
        repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
        repository.Setup(x => x.Add(It.IsAny<UserSignInLogEntity>())).Callback<UserSignInLogEntity>(added.Add);

        var service = new LegacyUserSignInLogService(() => repository.Object);

        await service.SaveChanges([new UserSignInLog { UserName = "a", SignInType = SignInType.Password, Succeeded = true }], TestContext.Current.CancellationToken);

        added.Should().ContainSingle();
        unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
        repository.Verify(x => x.Dispose(), Times.Once, "the legacy path must still release the repository the Func produced");
    }

    [Fact]
    public void DerivedService_ForwardingFuncToBase_Compiles()
    {
        var repository = Mock.Of<ISecurityRepository>();

        var service = new LegacyUserApiKeySearchService(() => repository);

        service.Should().NotBeNull();
    }

    [Fact]
    public void PlainRegistration_WithLegacyAndNewFactoriesRegistered_Activates()
    {
        // The legacy constructor is protected, so the container sees a single public constructor and never
        // reports an ambiguity, even though both Func<T> and IScopedServiceFactory<T> are resolvable.
        var services = new ServiceCollection();
        services.AddSingleton(typeof(IScopedServiceFactory<>), typeof(ScopedServiceFactory<>));
        services.AddTransient<ISecurityRepository>(_ => Mock.Of<ISecurityRepository>());
        services.AddTransient<Func<ISecurityRepository>>(provider => () => provider.GetRequiredService<ISecurityRepository>());
        services.AddSingleton<IUserApiKeySearchService, UserApiKeySearchService>();
        services.AddSingleton<IUserSignInLogService, UserSignInLogService>();
        using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IUserApiKeySearchService>().Should().BeOfType<UserApiKeySearchService>();
        provider.GetRequiredService<IUserSignInLogService>().Should().BeOfType<UserSignInLogService>();
    }

#pragma warning disable VC0016 // The legacy constructors are the subject of these tests.
    private sealed class LegacyUserSignInLogService(Func<ISecurityRepository> repositoryFactory) : UserSignInLogService(repositoryFactory);

    private sealed class LegacyUserApiKeySearchService(Func<ISecurityRepository> repositoryFactory) : UserApiKeySearchService(repositoryFactory);
#pragma warning restore VC0016
}
