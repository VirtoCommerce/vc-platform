using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MockQueryable;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using VirtoCommerce.Platform.Security.SignInLog;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

// Modules and custom projects compiled against earlier platform versions still call the Func<T> constructors.
// These tests fail to compile if one of them disappears.
[Trait("Category", "Unit")]
public class LegacyConstructorCompatibilityTests
{
    [Fact]
    public async Task UserSignInLogService_FuncConstructor_StillWritesThroughTheRepository()
    {
        var added = new List<UserSignInLogEntity>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(new List<UserSignInLogEntity>().BuildMock());
        repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
        repository.Setup(x => x.Add(It.IsAny<UserSignInLogEntity>())).Callback<UserSignInLogEntity>(added.Add);

#pragma warning disable VC0014 // The legacy constructor is the subject of the test.
        var service = new UserSignInLogService(() => repository.Object);
#pragma warning restore VC0014

        await service.SaveChanges([new UserSignInLog { UserName = "a", SignInType = SignInType.Password, Succeeded = true }], TestContext.Current.CancellationToken);

        added.Should().ContainSingle();
        unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
        repository.Verify(x => x.Dispose(), Times.Once, "the legacy path must still release the repository the Func produced");
    }

    [Fact]
    public void UserApiKeySearchService_FuncConstructor_Compiles()
    {
        var repository = Mock.Of<ISecurityRepository>();

#pragma warning disable VC0014
        var service = new UserApiKeySearchService(() => repository);
#pragma warning restore VC0014

        service.Should().NotBeNull();
    }
}
