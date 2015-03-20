
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace CommerceFoundation.UI.FunctionalTests.TestHelpers
{
	class TestAuthenticationContext : IAuthenticationContext
	{
		public bool IsUserAuthenticated { get { return true; }}
		public bool IsAdminUser { get { return true; } }
		public string CurrentUserName { get { return "Test user"; } }
		public string CurrentUserId { get { return "test"; } }
		public string Token { get { return ""; } }

		public void UpdateToken()
		{
		}

		public bool Login(string userName, string password, string baseUrl)
		{
			return true;
		}

		public bool CheckPermission(string permission)
		{
			return true;
		}
	}
}
