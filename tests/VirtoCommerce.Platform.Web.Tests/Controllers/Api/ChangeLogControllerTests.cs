using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Model;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Controllers.Api
{
    [Trait("Category", "Unit")]
    public class ChangeLogControllerTests
    {
        private readonly Mock<ILastChangesService> _lastChangesServiceMock = new();

        [Theory]
        [InlineData(nameof(ChangeLogController.GetChangedEntities))]
        [InlineData(nameof(ChangeLogController.GetLastModifiedDate))]
        public void Action_RequiresChangesReadPermission(string actionName)
        {
            var method = typeof(ChangeLogController).GetMethod(actionName);

            Assert.Empty(method!.GetCustomAttributes(typeof(AllowAnonymousAttribute), inherit: true));
            var policies = method.GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true).Cast<AuthorizeAttribute>().Select(x => x.Policy);
            Assert.Contains(PlatformConstants.Security.Permissions.ChangesRead, policies);
            Assert.Contains(PlatformConstants.Security.Permissions.ChangesRead, PlatformConstants.Security.Permissions.AllPermissions);
        }

        [Fact]
        public void GetChangedEntities_ReturnsNamesModifiedSinceTheGivenDate()
        {
            // Arrange
            var since = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            _lastChangesServiceMock.Setup(x => x.GetLastModifiedDate("Old")).Returns(since.AddDays(-1));
            _lastChangesServiceMock.Setup(x => x.GetLastModifiedDate("New")).Returns(since.AddDays(1));
            var controller = CreateController();

            // Act
            var result = controller.GetChangedEntities(new ChangedEntitiesRequest { EntityNames = ["Old", "New", "new"], ModifiedSince = since });

            // Assert
            var response = Assert.IsType<ChangedEntitiesResponse>(result.Value);
            Assert.Equal(["New"], response.Entities.Select(x => x.Name));
        }

        [Fact]
        public void GetChangedEntities_NoRequest_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.GetChangedEntities(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void GetChangedEntities_NoNamesOrNullNames_ReturnsEmptyList()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var noNames = controller.GetChangedEntities(new ChangedEntitiesRequest());
            var nullNames = controller.GetChangedEntities(new ChangedEntitiesRequest { EntityNames = [null, null] });

            // Assert
            Assert.Empty(Assert.IsType<ChangedEntitiesResponse>(noNames.Value).Entities);
            Assert.Empty(Assert.IsType<ChangedEntitiesResponse>(nullNames.Value).Entities);
            _lastChangesServiceMock.Verify(x => x.GetLastModifiedDate(It.IsAny<string>()), Times.Never);
        }

        private ChangeLogController CreateController()
        {
            return new ChangeLogController(Mock.Of<IChangeLogSearchService>(), Mock.Of<ILastModifiedDateTime>(), _lastChangesServiceMock.Object);
        }
    }
}
