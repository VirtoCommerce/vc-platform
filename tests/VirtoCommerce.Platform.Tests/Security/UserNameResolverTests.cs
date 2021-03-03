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
            var (userFromResolver, userFromThreadLocal) = await async1;

            // Ensure user preserved in resolver
            Assert.Equal(user, userFromResolver);

            // We can lost user in ThreadLocal (in comparison to AsyncLocal inside of HttpContextUserResolver)
            // You can check this assert localy (sometimes you can lost data if context going to switch) Assert.Null(userFromThreadLocal);
        }

        private async Task<(string userFromResolver, string userFromThreadLocal)> EmulateAsync(string user)
        {
            _resolver.SetCurrentUserName(user);
            _threadLocalString.Value = user;
            await Task.Delay(5); // Emulate async call context switching (the call switched to some another thread)
            return (_resolver.GetCurrentUserName(), _threadLocalString.Value);
        }

    }
}
