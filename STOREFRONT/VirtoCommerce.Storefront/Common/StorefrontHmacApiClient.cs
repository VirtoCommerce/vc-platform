using System;
using System.Collections.Generic;
using RestSharp;
using VirtoCommerce.Client;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Common
{
    public class StorefrontHmacApiClient : HmacApiClient
    {
        private readonly Func<WorkContext> _workContextFactory;

        public StorefrontHmacApiClient(string basePath, string appId, string secretKey, Func<WorkContext> workContextFactory) : base(basePath, appId, secretKey)
        {
            _workContextFactory = workContextFactory;
        }


        protected override RestRequest PrepareRequest(string path, Method method, Dictionary<string, string> queryParams, object postBody, Dictionary<string, string> headerParams,
            Dictionary<string, string> formParams, Dictionary<string, FileParameter> fileParams, Dictionary<string, string> pathParams, string contentType)
        {
            var request = base.PrepareRequest(path, method, queryParams, postBody, headerParams, formParams, fileParams, pathParams, contentType);

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

            return request;
        }
    }
}
