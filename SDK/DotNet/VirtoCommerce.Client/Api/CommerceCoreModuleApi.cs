using System;
using System.IO;
using System.Collections.Generic;
using RestSharp;
using VirtoCommerce.Client.Client;
using VirtoCommerce.Client.Model;


namespace VirtoCommerce.Client.Api
{
    
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ICommerceCoreModuleApi
    {
        
        /// <summary>
        /// Return all fulfillment centers registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenters ();
  
        /// <summary>
        /// Return all fulfillment centers registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCentersAsync ();
        
        /// <summary>
        /// Update a existing fulfillment center
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="center">fulfillment center</param>
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        VirtoCommerceCoreModuleWebModelFulfillmentCenter CommerceUpdateFulfillmentCenter (VirtoCommerceCoreModuleWebModelFulfillmentCenter center);
  
        /// <summary>
        /// Update a existing fulfillment center
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="center">fulfillment center</param>
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceUpdateFulfillmentCenterAsync (VirtoCommerceCoreModuleWebModelFulfillmentCenter center);
        
        /// <summary>
        /// Find fulfillment center by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">fulfillment center id</param>
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        VirtoCommerceCoreModuleWebModelFulfillmentCenter CommerceGetFulfillmentCenter (string id);
  
        /// <summary>
        /// Find fulfillment center by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">fulfillment center id</param>
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenterAsync (string id);
        
        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="orderId">customer order id</param>
        /// <returns>VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        VirtoCommerceDomainPaymentModelPostProcessPaymentResult CommercePostProcessPayment (string orderId);
  
        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="orderId">customer order id</param>
        /// <returns>VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> CommercePostProcessPaymentAsync (string orderId);
        
        /// <summary>
        /// Find all SEO records for object by slug
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="slug">slug</param>
        /// <returns></returns>
        List<VirtoCommerceDomainCommerceModelSeoInfo> CommerceGetSeoInfoBySlug (string slug);
  
        /// <summary>
        /// Find all SEO records for object by slug
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="slug">slug</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoInfoBySlugAsync (string slug);
        
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult StorefrontSecurityCreate (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
  
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        
        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended StorefrontSecurityGetUserByLogin (string loginProvider, string providerKey);
  
        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByLoginAsync (string loginProvider, string providerKey);
        
        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended StorefrontSecurityGetUserById (string userId);
  
        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByIdAsync (string userId);
        
        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended StorefrontSecurityGetUserByName (string userName);
  
        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByNameAsync (string userName);
        
        /// <summary>
        /// Reset a password for the user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult StorefrontSecurityResetPassword (string userId, string token, string newPassword);
  
        /// <summary>
        /// Reset a password for the user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityResetPasswordAsync (string userId, string token, string newPassword);
        
        /// <summary>
        /// Generate a password reset token
        /// </summary>
        /// <remarks>
        /// Generates a password reset token and sends a password reset link to the user via email.
        /// </remarks>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        void StorefrontSecurityGenerateResetPasswordToken (string userId, string storeName, string language, string callbackUrl);
  
        /// <summary>
        /// Generate a password reset token
        /// </summary>
        /// <remarks>
        /// Generates a password reset token and sends a password reset link to the user via email.
        /// </remarks>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task StorefrontSecurityGenerateResetPasswordTokenAsync (string userId, string storeName, string language, string callbackUrl);
        
        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>VirtoCommerceCoreModuleWebModelSignInResult</returns>
        VirtoCommerceCoreModuleWebModelSignInResult StorefrontSecurityPasswordSignIn (string userName, string password);
  
        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>VirtoCommerceCoreModuleWebModelSignInResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelSignInResult> StorefrontSecurityPasswordSignInAsync (string userName, string password);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CommerceCoreModuleApi : ICommerceCoreModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommerceCoreModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient</param>
        /// <returns></returns>
        public CommerceCoreModuleApi(ApiClient apiClient)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }
    
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public String GetBasePath()
        {
            return this.ApiClient.BasePath;
        }
    
        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient {get; set;}
    
        
        /// <summary>
        /// Return all fulfillment centers registered in the system 
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenters ()
        {
            
    
            var path_ = "/api/fulfillment/centers";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceGetFulfillmentCenters: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceGetFulfillmentCenters: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>) ApiClient.Deserialize(response, typeof(List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>));
        }
    
        /// <summary>
        /// Return all fulfillment centers registered in the system 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCentersAsync ()
        {
            
    
            var path_ = "/api/fulfillment/centers";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceGetFulfillmentCenters: " + response.Content, response.Content);

            return (List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>) ApiClient.Deserialize(response, typeof(List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>));
        }
        
        /// <summary>
        /// Update a existing fulfillment center 
        /// </summary>
        /// <param name="center">fulfillment center</param> 
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>            
        public VirtoCommerceCoreModuleWebModelFulfillmentCenter CommerceUpdateFulfillmentCenter (VirtoCommerceCoreModuleWebModelFulfillmentCenter center)
        {
            
            // verify the required parameter 'center' is set
            if (center == null) throw new ApiException(400, "Missing required parameter 'center' when calling CommerceUpdateFulfillmentCenter");
            
    
            var path_ = "/api/fulfillment/centers";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = ApiClient.Serialize(center); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceUpdateFulfillmentCenter: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceUpdateFulfillmentCenter: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCoreModuleWebModelFulfillmentCenter) ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter));
        }
    
        /// <summary>
        /// Update a existing fulfillment center 
        /// </summary>
        /// <param name="center">fulfillment center</param>
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceUpdateFulfillmentCenterAsync (VirtoCommerceCoreModuleWebModelFulfillmentCenter center)
        {
            // verify the required parameter 'center' is set
            if (center == null) throw new ApiException(400, "Missing required parameter 'center' when calling CommerceUpdateFulfillmentCenter");
            
    
            var path_ = "/api/fulfillment/centers";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = ApiClient.Serialize(center); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceUpdateFulfillmentCenter: " + response.Content, response.Content);

            return (VirtoCommerceCoreModuleWebModelFulfillmentCenter) ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter));
        }
        
        /// <summary>
        /// Find fulfillment center by id 
        /// </summary>
        /// <param name="id">fulfillment center id</param> 
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>            
        public VirtoCommerceCoreModuleWebModelFulfillmentCenter CommerceGetFulfillmentCenter (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CommerceGetFulfillmentCenter");
            
    
            var path_ = "/api/fulfillment/centers/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceGetFulfillmentCenter: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceGetFulfillmentCenter: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCoreModuleWebModelFulfillmentCenter) ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter));
        }
    
        /// <summary>
        /// Find fulfillment center by id 
        /// </summary>
        /// <param name="id">fulfillment center id</param>
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenterAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CommerceGetFulfillmentCenter");
            
    
            var path_ = "/api/fulfillment/centers/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceGetFulfillmentCenter: " + response.Content, response.Content);

            return (VirtoCommerceCoreModuleWebModelFulfillmentCenter) ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter));
        }
        
        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system 
        /// </summary>
        /// <param name="orderId">customer order id</param> 
        /// <returns>VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>            
        public VirtoCommerceDomainPaymentModelPostProcessPaymentResult CommercePostProcessPayment (string orderId)
        {
            
            // verify the required parameter 'orderId' is set
            if (orderId == null) throw new ApiException(400, "Missing required parameter 'orderId' when calling CommercePostProcessPayment");
            
    
            var path_ = "/api/paymentcallback";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (orderId != null) queryParams.Add("orderId", ApiClient.ParameterToString(orderId)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommercePostProcessPayment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CommercePostProcessPayment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceDomainPaymentModelPostProcessPaymentResult) ApiClient.Deserialize(response, typeof(VirtoCommerceDomainPaymentModelPostProcessPaymentResult));
        }
    
        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system 
        /// </summary>
        /// <param name="orderId">customer order id</param>
        /// <returns>VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> CommercePostProcessPaymentAsync (string orderId)
        {
            // verify the required parameter 'orderId' is set
            if (orderId == null) throw new ApiException(400, "Missing required parameter 'orderId' when calling CommercePostProcessPayment");
            
    
            var path_ = "/api/paymentcallback";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (orderId != null) queryParams.Add("orderId", ApiClient.ParameterToString(orderId)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommercePostProcessPayment: " + response.Content, response.Content);

            return (VirtoCommerceDomainPaymentModelPostProcessPaymentResult) ApiClient.Deserialize(response, typeof(VirtoCommerceDomainPaymentModelPostProcessPaymentResult));
        }
        
        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <param name="slug">slug</param> 
        /// <returns></returns>            
        public List<VirtoCommerceDomainCommerceModelSeoInfo> CommerceGetSeoInfoBySlug (string slug)
        {
            
            // verify the required parameter 'slug' is set
            if (slug == null) throw new ApiException(400, "Missing required parameter 'slug' when calling CommerceGetSeoInfoBySlug");
            
    
            var path_ = "/api/seoinfos/{slug}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (slug != null) pathParams.Add("slug", ApiClient.ParameterToString(slug)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceGetSeoInfoBySlug: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceGetSeoInfoBySlug: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceDomainCommerceModelSeoInfo>) ApiClient.Deserialize(response, typeof(List<VirtoCommerceDomainCommerceModelSeoInfo>));
        }
    
        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <param name="slug">slug</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoInfoBySlugAsync (string slug)
        {
            // verify the required parameter 'slug' is set
            if (slug == null) throw new ApiException(400, "Missing required parameter 'slug' when calling CommerceGetSeoInfoBySlug");
            
    
            var path_ = "/api/seoinfos/{slug}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (slug != null) pathParams.Add("slug", ApiClient.ParameterToString(slug)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommerceGetSeoInfoBySlug: " + response.Content, response.Content);

            return (List<VirtoCommerceDomainCommerceModelSeoInfo>) ApiClient.Deserialize(response, typeof(List<VirtoCommerceDomainCommerceModelSeoInfo>));
        }
        
        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <param name="user"></param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>            
        public VirtoCommercePlatformCoreSecuritySecurityResult StorefrontSecurityCreate (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling StorefrontSecurityCreate");
            
    
            var path_ = "/api/storefront/security/user";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = ApiClient.Serialize(user); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityCreate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
    
        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling StorefrontSecurityCreate");
            
    
            var path_ = "/api/storefront/security/user";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = ApiClient.Serialize(user); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityCreate: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
        
        /// <summary>
        /// Get user details by external login provider 
        /// </summary>
        /// <param name="loginProvider"></param> 
        /// <param name="providerKey"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>            
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended StorefrontSecurityGetUserByLogin (string loginProvider, string providerKey)
        {
            
            // verify the required parameter 'loginProvider' is set
            if (loginProvider == null) throw new ApiException(400, "Missing required parameter 'loginProvider' when calling StorefrontSecurityGetUserByLogin");
            
            // verify the required parameter 'providerKey' is set
            if (providerKey == null) throw new ApiException(400, "Missing required parameter 'providerKey' when calling StorefrontSecurityGetUserByLogin");
            
    
            var path_ = "/api/storefront/security/user/external";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (loginProvider != null) queryParams.Add("loginProvider", ApiClient.ParameterToString(loginProvider)); // query parameter
            if (providerKey != null) queryParams.Add("providerKey", ApiClient.ParameterToString(providerKey)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGetUserByLogin: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGetUserByLogin: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
    
        /// <summary>
        /// Get user details by external login provider 
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByLoginAsync (string loginProvider, string providerKey)
        {
            // verify the required parameter 'loginProvider' is set
            if (loginProvider == null) throw new ApiException(400, "Missing required parameter 'loginProvider' when calling StorefrontSecurityGetUserByLogin");
            // verify the required parameter 'providerKey' is set
            if (providerKey == null) throw new ApiException(400, "Missing required parameter 'providerKey' when calling StorefrontSecurityGetUserByLogin");
            
    
            var path_ = "/api/storefront/security/user/external";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (loginProvider != null) queryParams.Add("loginProvider", ApiClient.ParameterToString(loginProvider)); // query parameter
            if (providerKey != null) queryParams.Add("providerKey", ApiClient.ParameterToString(providerKey)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGetUserByLogin: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
        
        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>            
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended StorefrontSecurityGetUserById (string userId)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling StorefrontSecurityGetUserById");
            
    
            var path_ = "/api/storefront/security/user/id/{userId}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (userId != null) pathParams.Add("userId", ApiClient.ParameterToString(userId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGetUserById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGetUserById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
    
        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByIdAsync (string userId)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling StorefrontSecurityGetUserById");
            
    
            var path_ = "/api/storefront/security/user/id/{userId}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (userId != null) pathParams.Add("userId", ApiClient.ParameterToString(userId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGetUserById: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
        
        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>            
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended StorefrontSecurityGetUserByName (string userName)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling StorefrontSecurityGetUserByName");
            
    
            var path_ = "/api/storefront/security/user/name/{userName}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (userName != null) pathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGetUserByName: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGetUserByName: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
    
        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByNameAsync (string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling StorefrontSecurityGetUserByName");
            
    
            var path_ = "/api/storefront/security/user/name/{userName}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (userName != null) pathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGetUserByName: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
        
        /// <summary>
        /// Reset a password for the user 
        /// </summary>
        /// <param name="userId"></param> 
        /// <param name="token"></param> 
        /// <param name="newPassword"></param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>            
        public VirtoCommercePlatformCoreSecuritySecurityResult StorefrontSecurityResetPassword (string userId, string token, string newPassword)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling StorefrontSecurityResetPassword");
            
            // verify the required parameter 'token' is set
            if (token == null) throw new ApiException(400, "Missing required parameter 'token' when calling StorefrontSecurityResetPassword");
            
            // verify the required parameter 'newPassword' is set
            if (newPassword == null) throw new ApiException(400, "Missing required parameter 'newPassword' when calling StorefrontSecurityResetPassword");
            
    
            var path_ = "/api/storefront/security/user/password/reset";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (userId != null) queryParams.Add("userId", ApiClient.ParameterToString(userId)); // query parameter
            if (token != null) queryParams.Add("token", ApiClient.ParameterToString(token)); // query parameter
            if (newPassword != null) queryParams.Add("newPassword", ApiClient.ParameterToString(newPassword)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityResetPassword: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityResetPassword: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
    
        /// <summary>
        /// Reset a password for the user 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityResetPasswordAsync (string userId, string token, string newPassword)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling StorefrontSecurityResetPassword");
            // verify the required parameter 'token' is set
            if (token == null) throw new ApiException(400, "Missing required parameter 'token' when calling StorefrontSecurityResetPassword");
            // verify the required parameter 'newPassword' is set
            if (newPassword == null) throw new ApiException(400, "Missing required parameter 'newPassword' when calling StorefrontSecurityResetPassword");
            
    
            var path_ = "/api/storefront/security/user/password/reset";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (userId != null) queryParams.Add("userId", ApiClient.ParameterToString(userId)); // query parameter
            if (token != null) queryParams.Add("token", ApiClient.ParameterToString(token)); // query parameter
            if (newPassword != null) queryParams.Add("newPassword", ApiClient.ParameterToString(newPassword)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityResetPassword: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
        
        /// <summary>
        /// Generate a password reset token Generates a password reset token and sends a password reset link to the user via email.
        /// </summary>
        /// <param name="userId"></param> 
        /// <param name="storeName"></param> 
        /// <param name="language"></param> 
        /// <param name="callbackUrl"></param> 
        /// <returns></returns>            
        public void StorefrontSecurityGenerateResetPasswordToken (string userId, string storeName, string language, string callbackUrl)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling StorefrontSecurityGenerateResetPasswordToken");
            
            // verify the required parameter 'storeName' is set
            if (storeName == null) throw new ApiException(400, "Missing required parameter 'storeName' when calling StorefrontSecurityGenerateResetPasswordToken");
            
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling StorefrontSecurityGenerateResetPasswordToken");
            
            // verify the required parameter 'callbackUrl' is set
            if (callbackUrl == null) throw new ApiException(400, "Missing required parameter 'callbackUrl' when calling StorefrontSecurityGenerateResetPasswordToken");
            
    
            var path_ = "/api/storefront/security/user/password/resettoken";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (userId != null) queryParams.Add("userId", ApiClient.ParameterToString(userId)); // query parameter
            if (storeName != null) queryParams.Add("storeName", ApiClient.ParameterToString(storeName)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            if (callbackUrl != null) queryParams.Add("callbackUrl", ApiClient.ParameterToString(callbackUrl)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Generate a password reset token Generates a password reset token and sends a password reset link to the user via email.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task StorefrontSecurityGenerateResetPasswordTokenAsync (string userId, string storeName, string language, string callbackUrl)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling StorefrontSecurityGenerateResetPasswordToken");
            // verify the required parameter 'storeName' is set
            if (storeName == null) throw new ApiException(400, "Missing required parameter 'storeName' when calling StorefrontSecurityGenerateResetPasswordToken");
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling StorefrontSecurityGenerateResetPasswordToken");
            // verify the required parameter 'callbackUrl' is set
            if (callbackUrl == null) throw new ApiException(400, "Missing required parameter 'callbackUrl' when calling StorefrontSecurityGenerateResetPasswordToken");
            
    
            var path_ = "/api/storefront/security/user/password/resettoken";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (userId != null) queryParams.Add("userId", ApiClient.ParameterToString(userId)); // query parameter
            if (storeName != null) queryParams.Add("storeName", ApiClient.ParameterToString(storeName)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            if (callbackUrl != null) queryParams.Add("callbackUrl", ApiClient.ParameterToString(callbackUrl)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Sign in with user name and password 
        /// </summary>
        /// <param name="userName"></param> 
        /// <param name="password"></param> 
        /// <returns>VirtoCommerceCoreModuleWebModelSignInResult</returns>            
        public VirtoCommerceCoreModuleWebModelSignInResult StorefrontSecurityPasswordSignIn (string userName, string password)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling StorefrontSecurityPasswordSignIn");
            
            // verify the required parameter 'password' is set
            if (password == null) throw new ApiException(400, "Missing required parameter 'password' when calling StorefrontSecurityPasswordSignIn");
            
    
            var path_ = "/api/storefront/security/user/signin";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (userName != null) queryParams.Add("userName", ApiClient.ParameterToString(userName)); // query parameter
            if (password != null) queryParams.Add("password", ApiClient.ParameterToString(password)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityPasswordSignIn: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityPasswordSignIn: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCoreModuleWebModelSignInResult) ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelSignInResult));
        }
    
        /// <summary>
        /// Sign in with user name and password 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>VirtoCommerceCoreModuleWebModelSignInResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelSignInResult> StorefrontSecurityPasswordSignInAsync (string userName, string password)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling StorefrontSecurityPasswordSignIn");
            // verify the required parameter 'password' is set
            if (password == null) throw new ApiException(400, "Missing required parameter 'password' when calling StorefrontSecurityPasswordSignIn");
            
    
            var path_ = "/api/storefront/security/user/signin";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (userName != null) queryParams.Add("userName", ApiClient.ParameterToString(userName)); // query parameter
            if (password != null) queryParams.Add("password", ApiClient.ParameterToString(password)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StorefrontSecurityPasswordSignIn: " + response.Content, response.Content);

            return (VirtoCommerceCoreModuleWebModelSignInResult) ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelSignInResult));
        }
        
    }
    
}
