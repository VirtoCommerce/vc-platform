using System;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiWebClient.Customer.Services;
using VirtoCommerce.Web.Core.DataContracts.Security;

namespace VirtoCommerce.ApiWebClient.Clients
{
    public class SecurityClient
    {
        private ICustomerSessionService _customerSession;

        public SecurityClient(ICustomerSessionService customerSession)
        {
            _customerSession = customerSession;
        }

        public ApiClient.SecurityClient SecurityApiClient
        {
            get { return ClientContext.Clients.CreateDefaultSecurityClient(); }
        }

        #region IUserStore<ApplicationUser> Members wrapper

        public async Task CreateAsync(ApplicationUser user)
        {
            await SecurityApiClient.CreateAsync(user);
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await SecurityApiClient.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await SecurityApiClient.FindByNameAsync(userName);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        #endregion
    }
}
