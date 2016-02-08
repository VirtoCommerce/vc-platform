using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp;
using VirtoCommerce.Client.Client;
using VirtoCommerce.Client.Model;


namespace VirtoCommerce.Client.Api
{
    
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ICustomerManagementModuleApi
    {
        
        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns></returns>
        void CustomerModuleUpdateContact (VirtoCommerceCustomerModuleWebModelContact contact);
  
        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleUpdateContactWithHttpInfo (VirtoCommerceCustomerModuleWebModelContact contact);

        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleUpdateContactAsync (VirtoCommerceCustomerModuleWebModelContact contact);

        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateContactAsyncWithHttpInfo (VirtoCommerceCustomerModuleWebModelContact contact);
        
        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>VirtoCommerceCustomerModuleWebModelContact</returns>
        VirtoCommerceCustomerModuleWebModelContact CustomerModuleCreateContact (VirtoCommerceCustomerModuleWebModelContact contact);
  
        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>ApiResponse of VirtoCommerceCustomerModuleWebModelContact</returns>
        ApiResponse<VirtoCommerceCustomerModuleWebModelContact> CustomerModuleCreateContactWithHttpInfo (VirtoCommerceCustomerModuleWebModelContact contact);

        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>Task of VirtoCommerceCustomerModuleWebModelContact</returns>
        System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelContact> CustomerModuleCreateContactAsync (VirtoCommerceCustomerModuleWebModelContact contact);

        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCustomerModuleWebModelContact)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCustomerModuleWebModelContact>> CustomerModuleCreateContactAsyncWithHttpInfo (VirtoCommerceCustomerModuleWebModelContact contact);
        
        /// <summary>
        /// Delete contacts
        /// </summary>
        /// <remarks>
        /// Delete contacts by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns></returns>
        void CustomerModuleDeleteContacts (List<string> ids);
  
        /// <summary>
        /// Delete contacts
        /// </summary>
        /// <remarks>
        /// Delete contacts by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleDeleteContactsWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete contacts
        /// </summary>
        /// <remarks>
        /// Delete contacts by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleDeleteContactsAsync (List<string> ids);

        /// <summary>
        /// Delete contacts
        /// </summary>
        /// <remarks>
        /// Delete contacts by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteContactsAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Contact id</param>
        /// <returns>VirtoCommerceCustomerModuleWebModelContact</returns>
        VirtoCommerceCustomerModuleWebModelContact CustomerModuleGetContactById (string id);
  
        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Contact id</param>
        /// <returns>ApiResponse of VirtoCommerceCustomerModuleWebModelContact</returns>
        ApiResponse<VirtoCommerceCustomerModuleWebModelContact> CustomerModuleGetContactByIdWithHttpInfo (string id);

        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Contact id</param>
        /// <returns>Task of VirtoCommerceCustomerModuleWebModelContact</returns>
        System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelContact> CustomerModuleGetContactByIdAsync (string id);

        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Contact id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCustomerModuleWebModelContact)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCustomerModuleWebModelContact>> CustomerModuleGetContactByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>VirtoCommerceCustomerModuleWebModelSearchResult</returns>
        VirtoCommerceCustomerModuleWebModelSearchResult CustomerModuleSearch (VirtoCommerceDomainCustomerModelSearchCriteria criteria);
  
        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>ApiResponse of VirtoCommerceCustomerModuleWebModelSearchResult</returns>
        ApiResponse<VirtoCommerceCustomerModuleWebModelSearchResult> CustomerModuleSearchWithHttpInfo (VirtoCommerceDomainCustomerModelSearchCriteria criteria);

        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of VirtoCommerceCustomerModuleWebModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelSearchResult> CustomerModuleSearchAsync (VirtoCommerceDomainCustomerModelSearchCriteria criteria);

        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCustomerModuleWebModelSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCustomerModuleWebModelSearchResult>> CustomerModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelSearchCriteria criteria);
        
        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <returns>List&lt;VirtoCommerceCustomerModuleWebModelOrganization&gt;</returns>
        List<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleListOrganizations ();
  
        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCustomerModuleWebModelOrganization&gt;</returns>
        ApiResponse<List<VirtoCommerceCustomerModuleWebModelOrganization>> CustomerModuleListOrganizationsWithHttpInfo ();

        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommerceCustomerModuleWebModelOrganization&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCustomerModuleWebModelOrganization>> CustomerModuleListOrganizationsAsync ();

        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCustomerModuleWebModelOrganization&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCustomerModuleWebModelOrganization>>> CustomerModuleListOrganizationsAsyncWithHttpInfo ();
        
        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns></returns>
        void CustomerModuleUpdateOrganization (VirtoCommerceCustomerModuleWebModelOrganization organization);
  
        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleUpdateOrganizationWithHttpInfo (VirtoCommerceCustomerModuleWebModelOrganization organization);

        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleUpdateOrganizationAsync (VirtoCommerceCustomerModuleWebModelOrganization organization);

        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateOrganizationAsyncWithHttpInfo (VirtoCommerceCustomerModuleWebModelOrganization organization);
        
        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>VirtoCommerceCustomerModuleWebModelOrganization</returns>
        VirtoCommerceCustomerModuleWebModelOrganization CustomerModuleCreateOrganization (VirtoCommerceCustomerModuleWebModelOrganization organization);
  
        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>ApiResponse of VirtoCommerceCustomerModuleWebModelOrganization</returns>
        ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleCreateOrganizationWithHttpInfo (VirtoCommerceCustomerModuleWebModelOrganization organization);

        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>Task of VirtoCommerceCustomerModuleWebModelOrganization</returns>
        System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleCreateOrganizationAsync (VirtoCommerceCustomerModuleWebModelOrganization organization);

        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCustomerModuleWebModelOrganization)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization>> CustomerModuleCreateOrganizationAsyncWithHttpInfo (VirtoCommerceCustomerModuleWebModelOrganization organization);
        
        /// <summary>
        /// Delete organizations
        /// </summary>
        /// <remarks>
        /// Delete organizations by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns></returns>
        void CustomerModuleDeleteOrganizations (List<string> ids);
  
        /// <summary>
        /// Delete organizations
        /// </summary>
        /// <remarks>
        /// Delete organizations by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleDeleteOrganizationsWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete organizations
        /// </summary>
        /// <remarks>
        /// Delete organizations by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleDeleteOrganizationsAsync (List<string> ids);

        /// <summary>
        /// Delete organizations
        /// </summary>
        /// <remarks>
        /// Delete organizations by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteOrganizationsAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Organization id</param>
        /// <returns>VirtoCommerceCustomerModuleWebModelOrganization</returns>
        VirtoCommerceCustomerModuleWebModelOrganization CustomerModuleGetOrganizationById (string id);
  
        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Organization id</param>
        /// <returns>ApiResponse of VirtoCommerceCustomerModuleWebModelOrganization</returns>
        ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleGetOrganizationByIdWithHttpInfo (string id);

        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Organization id</param>
        /// <returns>Task of VirtoCommerceCustomerModuleWebModelOrganization</returns>
        System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleGetOrganizationByIdAsync (string id);

        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Organization id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCustomerModuleWebModelOrganization)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization>> CustomerModuleGetOrganizationByIdAsyncWithHttpInfo (string id);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CustomerManagementModuleApi : ICustomerManagementModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerManagementModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public CustomerManagementModuleApi(Configuration configuration)
        {
            if (configuration == null) // use the default one in Configuration
                this.Configuration = Configuration.Default; 
            else
                this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public String GetBasePath()
        {
            return this.Configuration.ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        [Obsolete("SetBasePath is deprecated, please do 'Configuraiton.ApiClient = new ApiClient(\"http://new-path\")' instead.")]
        public void SetBasePath(String basePath)
        {
            // do nothing
        }
    
        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Configuration Configuration {get; set;}

        /// <summary>
        /// Gets the default header.
        /// </summary>
        /// <returns>Dictionary of HTTP header</returns>
        [Obsolete("DefaultHeader is deprecated, please use Configuration.DefaultHeader instead.")]
        public Dictionary<String, String> DefaultHeader()
        {
            return this.Configuration.DefaultHeader;
        }

        /// <summary>
        /// Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns>
        [Obsolete("AddDefaultHeader is deprecated, please use Configuration.AddDefaultHeader instead.")]
        public void AddDefaultHeader(string key, string value)
        {
            this.Configuration.AddDefaultHeader(key, value);
        }
   
        
        /// <summary>
        /// Update contact 
        /// </summary>
        /// <param name="contact"></param> 
        /// <returns></returns>
        public void CustomerModuleUpdateContact (VirtoCommerceCustomerModuleWebModelContact contact)
        {
             CustomerModuleUpdateContactWithHttpInfo(contact);
        }

        /// <summary>
        /// Update contact 
        /// </summary>
        /// <param name="contact"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleUpdateContactWithHttpInfo (VirtoCommerceCustomerModuleWebModelContact contact)
        {
            
            // verify the required parameter 'contact' is set
            if (contact == null)
                throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerManagementModuleApi->CustomerModuleUpdateContact");
            
    
            var path_ = "/api/contacts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (contact.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(contact); // http body (model) parameter
            }
            else
            {
                postBody = contact; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateContact: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateContact: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update contact 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleUpdateContactAsync (VirtoCommerceCustomerModuleWebModelContact contact)
        {
             await CustomerModuleUpdateContactAsyncWithHttpInfo(contact);

        }

        /// <summary>
        /// Update contact 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateContactAsyncWithHttpInfo (VirtoCommerceCustomerModuleWebModelContact contact)
        {
            // verify the required parameter 'contact' is set
            if (contact == null) throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerModuleUpdateContact");
            
    
            var path_ = "/api/contacts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(contact); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateContact: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateContact: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create contact 
        /// </summary>
        /// <param name="contact"></param> 
        /// <returns>VirtoCommerceCustomerModuleWebModelContact</returns>
        public VirtoCommerceCustomerModuleWebModelContact CustomerModuleCreateContact (VirtoCommerceCustomerModuleWebModelContact contact)
        {
             ApiResponse<VirtoCommerceCustomerModuleWebModelContact> response = CustomerModuleCreateContactWithHttpInfo(contact);
             return response.Data;
        }

        /// <summary>
        /// Create contact 
        /// </summary>
        /// <param name="contact"></param> 
        /// <returns>ApiResponse of VirtoCommerceCustomerModuleWebModelContact</returns>
        public ApiResponse< VirtoCommerceCustomerModuleWebModelContact > CustomerModuleCreateContactWithHttpInfo (VirtoCommerceCustomerModuleWebModelContact contact)
        {
            
            // verify the required parameter 'contact' is set
            if (contact == null)
                throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerManagementModuleApi->CustomerModuleCreateContact");
            
    
            var path_ = "/api/contacts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (contact.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(contact); // http body (model) parameter
            }
            else
            {
                postBody = contact; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateContact: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateContact: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCustomerModuleWebModelContact>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCustomerModuleWebModelContact) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCustomerModuleWebModelContact)));
            
        }
    
        /// <summary>
        /// Create contact 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Task of VirtoCommerceCustomerModuleWebModelContact</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelContact> CustomerModuleCreateContactAsync (VirtoCommerceCustomerModuleWebModelContact contact)
        {
             ApiResponse<VirtoCommerceCustomerModuleWebModelContact> response = await CustomerModuleCreateContactAsyncWithHttpInfo(contact);
             return response.Data;

        }

        /// <summary>
        /// Create contact 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCustomerModuleWebModelContact)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCustomerModuleWebModelContact>> CustomerModuleCreateContactAsyncWithHttpInfo (VirtoCommerceCustomerModuleWebModelContact contact)
        {
            // verify the required parameter 'contact' is set
            if (contact == null) throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerModuleCreateContact");
            
    
            var path_ = "/api/contacts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(contact); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateContact: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateContact: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCustomerModuleWebModelContact>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCustomerModuleWebModelContact) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCustomerModuleWebModelContact)));
            
        }
        
        /// <summary>
        /// Delete contacts Delete contacts by given array of ids.
        /// </summary>
        /// <param name="ids">An array of contacts ids</param> 
        /// <returns></returns>
        public void CustomerModuleDeleteContacts (List<string> ids)
        {
             CustomerModuleDeleteContactsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete contacts Delete contacts by given array of ids.
        /// </summary>
        /// <param name="ids">An array of contacts ids</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleDeleteContactsWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerManagementModuleApi->CustomerModuleDeleteContacts");
            
    
            var path_ = "/api/contacts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteContacts: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteContacts: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete contacts Delete contacts by given array of ids.
        /// </summary>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleDeleteContactsAsync (List<string> ids)
        {
             await CustomerModuleDeleteContactsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete contacts Delete contacts by given array of ids.
        /// </summary>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteContactsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerModuleDeleteContacts");
            
    
            var path_ = "/api/contacts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteContacts: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteContacts: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get contact 
        /// </summary>
        /// <param name="id">Contact id</param> 
        /// <returns>VirtoCommerceCustomerModuleWebModelContact</returns>
        public VirtoCommerceCustomerModuleWebModelContact CustomerModuleGetContactById (string id)
        {
             ApiResponse<VirtoCommerceCustomerModuleWebModelContact> response = CustomerModuleGetContactByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get contact 
        /// </summary>
        /// <param name="id">Contact id</param> 
        /// <returns>ApiResponse of VirtoCommerceCustomerModuleWebModelContact</returns>
        public ApiResponse< VirtoCommerceCustomerModuleWebModelContact > CustomerModuleGetContactByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CustomerManagementModuleApi->CustomerModuleGetContactById");
            
    
            var path_ = "/api/contacts/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetContactById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetContactById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCustomerModuleWebModelContact>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCustomerModuleWebModelContact) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCustomerModuleWebModelContact)));
            
        }
    
        /// <summary>
        /// Get contact 
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <returns>Task of VirtoCommerceCustomerModuleWebModelContact</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelContact> CustomerModuleGetContactByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceCustomerModuleWebModelContact> response = await CustomerModuleGetContactByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get contact 
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCustomerModuleWebModelContact)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCustomerModuleWebModelContact>> CustomerModuleGetContactByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CustomerModuleGetContactById");
            
    
            var path_ = "/api/contacts/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetContactById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetContactById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCustomerModuleWebModelContact>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCustomerModuleWebModelContact) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCustomerModuleWebModelContact)));
            
        }
        
        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <param name="criteria">Search criteria</param> 
        /// <returns>VirtoCommerceCustomerModuleWebModelSearchResult</returns>
        public VirtoCommerceCustomerModuleWebModelSearchResult CustomerModuleSearch (VirtoCommerceDomainCustomerModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCustomerModuleWebModelSearchResult> response = CustomerModuleSearchWithHttpInfo(criteria);
             return response.Data;
        }

        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <param name="criteria">Search criteria</param> 
        /// <returns>ApiResponse of VirtoCommerceCustomerModuleWebModelSearchResult</returns>
        public ApiResponse< VirtoCommerceCustomerModuleWebModelSearchResult > CustomerModuleSearchWithHttpInfo (VirtoCommerceDomainCustomerModelSearchCriteria criteria)
        {
            
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling CustomerManagementModuleApi->CustomerModuleSearch");
            
    
            var path_ = "/api/members";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (criteria.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                postBody = criteria; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCustomerModuleWebModelSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCustomerModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCustomerModuleWebModelSearchResult)));
            
        }
    
        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of VirtoCommerceCustomerModuleWebModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelSearchResult> CustomerModuleSearchAsync (VirtoCommerceDomainCustomerModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCustomerModuleWebModelSearchResult> response = await CustomerModuleSearchAsyncWithHttpInfo(criteria);
             return response.Data;

        }

        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCustomerModuleWebModelSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCustomerModuleWebModelSearchResult>> CustomerModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null) throw new ApiException(400, "Missing required parameter 'criteria' when calling CustomerModuleSearch");
            
    
            var path_ = "/api/members";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCustomerModuleWebModelSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCustomerModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCustomerModuleWebModelSearchResult)));
            
        }
        
        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <returns>List&lt;VirtoCommerceCustomerModuleWebModelOrganization&gt;</returns>
        public List<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleListOrganizations ()
        {
             ApiResponse<List<VirtoCommerceCustomerModuleWebModelOrganization>> response = CustomerModuleListOrganizationsWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCustomerModuleWebModelOrganization&gt;</returns>
        public ApiResponse< List<VirtoCommerceCustomerModuleWebModelOrganization> > CustomerModuleListOrganizationsWithHttpInfo ()
        {
            
    
            var path_ = "/api/organizations";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleListOrganizations: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleListOrganizations: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceCustomerModuleWebModelOrganization>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCustomerModuleWebModelOrganization>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCustomerModuleWebModelOrganization>)));
            
        }
    
        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommerceCustomerModuleWebModelOrganization&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCustomerModuleWebModelOrganization>> CustomerModuleListOrganizationsAsync ()
        {
             ApiResponse<List<VirtoCommerceCustomerModuleWebModelOrganization>> response = await CustomerModuleListOrganizationsAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCustomerModuleWebModelOrganization&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCustomerModuleWebModelOrganization>>> CustomerModuleListOrganizationsAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/organizations";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleListOrganizations: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleListOrganizations: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCustomerModuleWebModelOrganization>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCustomerModuleWebModelOrganization>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCustomerModuleWebModelOrganization>)));
            
        }
        
        /// <summary>
        /// Update organization 
        /// </summary>
        /// <param name="organization"></param> 
        /// <returns></returns>
        public void CustomerModuleUpdateOrganization (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
             CustomerModuleUpdateOrganizationWithHttpInfo(organization);
        }

        /// <summary>
        /// Update organization 
        /// </summary>
        /// <param name="organization"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleUpdateOrganizationWithHttpInfo (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
            
            // verify the required parameter 'organization' is set
            if (organization == null)
                throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerManagementModuleApi->CustomerModuleUpdateOrganization");
            
    
            var path_ = "/api/organizations";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (organization.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(organization); // http body (model) parameter
            }
            else
            {
                postBody = organization; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateOrganization: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateOrganization: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update organization 
        /// </summary>
        /// <param name="organization"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleUpdateOrganizationAsync (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
             await CustomerModuleUpdateOrganizationAsyncWithHttpInfo(organization);

        }

        /// <summary>
        /// Update organization 
        /// </summary>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateOrganizationAsyncWithHttpInfo (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
            // verify the required parameter 'organization' is set
            if (organization == null) throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerModuleUpdateOrganization");
            
    
            var path_ = "/api/organizations";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(organization); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateOrganization: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateOrganization: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create organization 
        /// </summary>
        /// <param name="organization"></param> 
        /// <returns>VirtoCommerceCustomerModuleWebModelOrganization</returns>
        public VirtoCommerceCustomerModuleWebModelOrganization CustomerModuleCreateOrganization (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
             ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization> response = CustomerModuleCreateOrganizationWithHttpInfo(organization);
             return response.Data;
        }

        /// <summary>
        /// Create organization 
        /// </summary>
        /// <param name="organization"></param> 
        /// <returns>ApiResponse of VirtoCommerceCustomerModuleWebModelOrganization</returns>
        public ApiResponse< VirtoCommerceCustomerModuleWebModelOrganization > CustomerModuleCreateOrganizationWithHttpInfo (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
            
            // verify the required parameter 'organization' is set
            if (organization == null)
                throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerManagementModuleApi->CustomerModuleCreateOrganization");
            
    
            var path_ = "/api/organizations";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (organization.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(organization); // http body (model) parameter
            }
            else
            {
                postBody = organization; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateOrganization: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateOrganization: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCustomerModuleWebModelOrganization) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCustomerModuleWebModelOrganization)));
            
        }
    
        /// <summary>
        /// Create organization 
        /// </summary>
        /// <param name="organization"></param>
        /// <returns>Task of VirtoCommerceCustomerModuleWebModelOrganization</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleCreateOrganizationAsync (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
             ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization> response = await CustomerModuleCreateOrganizationAsyncWithHttpInfo(organization);
             return response.Data;

        }

        /// <summary>
        /// Create organization 
        /// </summary>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCustomerModuleWebModelOrganization)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization>> CustomerModuleCreateOrganizationAsyncWithHttpInfo (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
            // verify the required parameter 'organization' is set
            if (organization == null) throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerModuleCreateOrganization");
            
    
            var path_ = "/api/organizations";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(organization); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateOrganization: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateOrganization: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCustomerModuleWebModelOrganization) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCustomerModuleWebModelOrganization)));
            
        }
        
        /// <summary>
        /// Delete organizations Delete organizations by given array of ids.
        /// </summary>
        /// <param name="ids">An array of organizations ids</param> 
        /// <returns></returns>
        public void CustomerModuleDeleteOrganizations (List<string> ids)
        {
             CustomerModuleDeleteOrganizationsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete organizations Delete organizations by given array of ids.
        /// </summary>
        /// <param name="ids">An array of organizations ids</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleDeleteOrganizationsWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerManagementModuleApi->CustomerModuleDeleteOrganizations");
            
    
            var path_ = "/api/organizations";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteOrganizations: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteOrganizations: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete organizations Delete organizations by given array of ids.
        /// </summary>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleDeleteOrganizationsAsync (List<string> ids)
        {
             await CustomerModuleDeleteOrganizationsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete organizations Delete organizations by given array of ids.
        /// </summary>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteOrganizationsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerModuleDeleteOrganizations");
            
    
            var path_ = "/api/organizations";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteOrganizations: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteOrganizations: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get organization 
        /// </summary>
        /// <param name="id">Organization id</param> 
        /// <returns>VirtoCommerceCustomerModuleWebModelOrganization</returns>
        public VirtoCommerceCustomerModuleWebModelOrganization CustomerModuleGetOrganizationById (string id)
        {
             ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization> response = CustomerModuleGetOrganizationByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get organization 
        /// </summary>
        /// <param name="id">Organization id</param> 
        /// <returns>ApiResponse of VirtoCommerceCustomerModuleWebModelOrganization</returns>
        public ApiResponse< VirtoCommerceCustomerModuleWebModelOrganization > CustomerModuleGetOrganizationByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CustomerManagementModuleApi->CustomerModuleGetOrganizationById");
            
    
            var path_ = "/api/organizations/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetOrganizationById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetOrganizationById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCustomerModuleWebModelOrganization) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCustomerModuleWebModelOrganization)));
            
        }
    
        /// <summary>
        /// Get organization 
        /// </summary>
        /// <param name="id">Organization id</param>
        /// <returns>Task of VirtoCommerceCustomerModuleWebModelOrganization</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleGetOrganizationByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization> response = await CustomerModuleGetOrganizationByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get organization 
        /// </summary>
        /// <param name="id">Organization id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCustomerModuleWebModelOrganization)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization>> CustomerModuleGetOrganizationByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CustomerModuleGetOrganizationById");
            
    
            var path_ = "/api/organizations/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetOrganizationById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetOrganizationById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCustomerModuleWebModelOrganization>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCustomerModuleWebModelOrganization) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCustomerModuleWebModelOrganization)));
            
        }
        
    }
    
}
