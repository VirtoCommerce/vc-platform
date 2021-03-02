using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using VirtoCommerce.Platform.Security;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security
{
    /// <summary>
    /// This tests user name preserved storage in HttpContextUserResolver
    /// </summary>
    public class UserNameResolverTests
    {

        private HttpContextUserResolver _resolver = new HttpContextUserResolver(Mock.Of<IHttpContextAccessor>());
        private ThreadLocal<string> _threadLocalString = new ThreadLocal<string>();

        [Fact]
        public async Task TestUserPreserveForAsyncCalls()
        {
            const string user = "User1";

            var async1 = EmulateAsync(user);
            var (UserFromResolver, UserFromThreadLocal) = await async1;

            // Ensure user preserved in resolver
            Assert.Equal(user, UserFromResolver);

            // We are sure user was lost in ThreadLocal (in comparison to AsyncLocal inside of HttpContextUserResolver)
            Assert.Null(UserFromThreadLocal);
        }

        private async Task<(string UserFromResolver, string UserFromThreadLocal)> EmulateAsync(string user)
        {
            _resolver.SetCurrentUserName(user);
            _threadLocalString.Value = user;
            await Task.Delay(5); // Emulate async call context switching (the call switched to some another thread)
            return (_resolver.GetCurrentUserName(), _threadLocalString.Value);
        }

    }
}
