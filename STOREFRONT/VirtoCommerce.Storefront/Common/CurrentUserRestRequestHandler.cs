using System;
using RestSharp;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Common
{
    public class CurrentUserRestRequestHandler
    {
        private readonly Func<WorkContext> _workContextFactory;

        public CurrentUserRestRequestHandler(Func<WorkContext> workContextFactory)
        {
            _workContextFactory = workContextFactory;
        }

        public void PrepareRequest(IRestRequest request)
        {
            var workContext = _workContextFactory();
            var currentUser = workContext.CurrentCustomer;

            //Add special header with user name to each API request for future audit and logging
            if (currentUser != null && currentUser.IsRegisteredUser)
            {
                var userName = currentUser.OperatorUserName;

                if (string.IsNullOrEmpty(userName))
                {
                    userName = currentUser.UserName;
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    request.AddHeader("VirtoCommerce-User-Name", userName);
                }
            }
        }
    }
}
