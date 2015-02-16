using System;
using System.Linq;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.CoreModule.Web.Security
{
	public class ApiAccountProvider : IApiAccountProvider
	{
		private readonly Func<ISecurityRepository> _securityRepository;

		public ApiAccountProvider(Func<ISecurityRepository> securityRepository)
		{
			_securityRepository = securityRepository;
		}

		#region IApiAccountProvider Members

		public ApiAccount GetAccountByAppId(string appId)
		{
			using (var repository = _securityRepository())
			{
				var apiAccount = repository.ApiAccounts.FirstOrDefault(c => c.IsActive && c.AppId == appId);
				return apiAccount;
			}
		}

		#endregion
	}
}
