using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Web.Security.BackgroundJobs;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security
{
    public class AutoAccountLockoutJobTests
    {
        private static (Mock<IUserSearchService> SearchService, Mock<UserManager<ApplicationUser>> UserManager, AutoAccountLockoutJob Job)
            BuildJob(LockoutOptionsExtended options)
        {
            var searchService = new Mock<IUserSearchService>();

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                userStore.Object, null, null, null, null, null, null, null, null);

            var optionsAccessor = new Mock<IOptions<LockoutOptionsExtended>>();
            optionsAccessor.Setup(x => x.Value).Returns(options);

            var logger = new Mock<ILogger<AutoAccountLockoutJob>>();

            var job = new AutoAccountLockoutJob(searchService.Object, userManager.Object, optionsAccessor.Object, logger.Object);
            return (searchService, userManager, job);
        }

        private static ApplicationUser User(string id) => new ApplicationUser { Id = id, UserName = "user-" + id };

        private static UserSearchResult PageOf(IEnumerable<ApplicationUser> users)
        {
            var list = users.ToList();
            return new UserSearchResult { Results = list, TotalCount = list.Count };
        }

        [Fact]
        public async Task Process_PassesBatchSizeAsTake()
        {
            // Arrange
            var (search, userManager, job) = BuildJob(new LockoutOptionsExtended { AutoAccountsLockoutJobBatchSize = 5 });

            UserSearchCriteria captured = null;
            search.Setup(s => s.SearchUsersAsync(It.IsAny<UserSearchCriteria>()))
                .Callback<UserSearchCriteria>(c => captured = c)
                .ReturnsAsync(PageOf(Enumerable.Range(1, 5).Select(i => User(i.ToString()))));
            userManager.Setup(u => u.SetLockoutEndDateAsync(It.IsAny<ApplicationUser>(), It.IsAny<DateTimeOffset?>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await job.Process();

            // Assert
            Assert.Equal(5, captured.Take);
            Assert.True(captured.OnlyUnlocked);
            userManager.Verify(u => u.SetLockoutEndDateAsync(It.IsAny<ApplicationUser>(), It.IsAny<DateTimeOffset?>()), Times.Exactly(5));
        }

        [Fact]
        public async Task Process_BatchSizeZero_PassesIntMaxAsTake()
        {
            // Arrange
            var (search, userManager, job) = BuildJob(new LockoutOptionsExtended { AutoAccountsLockoutJobBatchSize = 0 });

            UserSearchCriteria captured = null;
            search.Setup(s => s.SearchUsersAsync(It.IsAny<UserSearchCriteria>()))
                .Callback<UserSearchCriteria>(c => captured = c)
                .ReturnsAsync(PageOf(Enumerable.Range(1, 47).Select(i => User(i.ToString()))));
            userManager.Setup(u => u.SetLockoutEndDateAsync(It.IsAny<ApplicationUser>(), It.IsAny<DateTimeOffset?>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await job.Process();

            // Assert
            Assert.Equal(int.MaxValue, captured.Take);
            userManager.Verify(u => u.SetLockoutEndDateAsync(It.IsAny<ApplicationUser>(), It.IsAny<DateTimeOffset?>()), Times.Exactly(47));
            search.Verify(s => s.SearchUsersAsync(It.IsAny<UserSearchCriteria>()), Times.Once);
        }

        [Fact]
        public async Task Process_PerUserExceptionDoesNotAbortRun()
        {
            // Arrange
            var (search, userManager, job) = BuildJob(new LockoutOptionsExtended { AutoAccountsLockoutJobBatchSize = 10 });

            var users = new[] { User("a"), User("b"), User("c") };
            search.Setup(s => s.SearchUsersAsync(It.IsAny<UserSearchCriteria>()))
                .ReturnsAsync(PageOf(users));
            userManager.Setup(u => u.SetLockoutEndDateAsync(It.Is<ApplicationUser>(x => x.Id == "b"), It.IsAny<DateTimeOffset?>()))
                .ThrowsAsync(new InvalidOperationException("boom"));
            userManager.Setup(u => u.SetLockoutEndDateAsync(It.Is<ApplicationUser>(x => x.Id != "b"), It.IsAny<DateTimeOffset?>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await job.Process();

            // Assert
            userManager.Verify(u => u.SetLockoutEndDateAsync(It.Is<ApplicationUser>(x => x.Id == "a"), It.IsAny<DateTimeOffset?>()), Times.Once);
            userManager.Verify(u => u.SetLockoutEndDateAsync(It.Is<ApplicationUser>(x => x.Id == "b"), It.IsAny<DateTimeOffset?>()), Times.Once);
            userManager.Verify(u => u.SetLockoutEndDateAsync(It.Is<ApplicationUser>(x => x.Id == "c"), It.IsAny<DateTimeOffset?>()), Times.Once);
        }

        [Fact]
        public async Task Process_IdentityResultFailed_DoesNotAbortRun()
        {
            // Arrange
            var (search, userManager, job) = BuildJob(new LockoutOptionsExtended { AutoAccountsLockoutJobBatchSize = 10 });

            var users = new[] { User("a"), User("rejected"), User("c") };
            search.Setup(s => s.SearchUsersAsync(It.IsAny<UserSearchCriteria>()))
                .ReturnsAsync(PageOf(users));
            userManager.Setup(u => u.SetLockoutEndDateAsync(It.Is<ApplicationUser>(x => x.Id == "rejected"), It.IsAny<DateTimeOffset?>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "X", Description = "nope" }));
            userManager.Setup(u => u.SetLockoutEndDateAsync(It.Is<ApplicationUser>(x => x.Id != "rejected"), It.IsAny<DateTimeOffset?>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await job.Process();

            // Assert
            userManager.Verify(u => u.SetLockoutEndDateAsync(It.IsAny<ApplicationUser>(), It.IsAny<DateTimeOffset?>()), Times.Exactly(3));
        }
    }
}
