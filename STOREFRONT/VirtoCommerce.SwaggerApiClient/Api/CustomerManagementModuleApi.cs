using System;
using System.IO;
using System.Collections.Generic;
using RestSharp;
using VirtoCommerce.SwaggerApiClient.Client;
using VirtoCommerce.SwaggerApiClient.Model;


namespace VirtoCommerce.SwaggerApiClient.Api
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
        /// <returns></returns>
        System.Threading.Tasks.Task CustomerModuleUpdateContactAsync (VirtoCommerceCustomerModuleWebModelContact contact);
        
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
        /// <returns>VirtoCommerceCustomerModuleWebModelContact</returns>
        System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelContact> CustomerModuleCreateContactAsync (VirtoCommerceCustomerModuleWebModelContact contact);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task CustomerModuleDeleteContactsAsync (List<string> ids);
        
        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Contact id</param>
        /// <returns></returns>
        void CustomerModuleGetContactById (string id);
  
        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Contact id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CustomerModuleGetContactByIdAsync (string id);
        
        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <param name="criteriaKeyword">Word, part of word or phrase to search</param>
        /// <param name="criteriaOrganizationId">It used to limit search within an organization</param>
        /// <param name="criteriaStart">It used to skip some first search results</param>
        /// <param name="criteriaCount">It used to limit the number of search results</param>
        /// <returns>VirtoCommerceCustomerModuleWebModelSearchResult</returns>
        VirtoCommerceCustomerModuleWebModelSearchResult CustomerModuleSearch (string criteriaKeyword, string criteriaOrganizationId, int? criteriaStart, int? criteriaCount);
  
        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <param name="criteriaKeyword">Word, part of word or phrase to search</param>
        /// <param name="criteriaOrganizationId">It used to limit search within an organization</param>
        /// <param name="criteriaStart">It used to skip some first search results</param>
        /// <param name="criteriaCount">It used to limit the number of search results</param>
        /// <returns>VirtoCommerceCustomerModuleWebModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelSearchResult> CustomerModuleSearchAsync (string criteriaKeyword, string criteriaOrganizationId, int? criteriaStart, int? criteriaCount);
        
        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleListOrganizations ();
  
        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCustomerModuleWebModelOrganization>> CustomerModuleListOrganizationsAsync ();
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task CustomerModuleUpdateOrganizationAsync (VirtoCommerceCustomerModuleWebModelOrganization organization);
        
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
        /// <returns>VirtoCommerceCustomerModuleWebModelOrganization</returns>
        System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleCreateOrganizationAsync (VirtoCommerceCustomerModuleWebModelOrganization organization);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task CustomerModuleDeleteOrganizationsAsync (List<string> ids);
        
        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Organization id</param>
        /// <returns></returns>
        void CustomerModuleGetOrganizationById (string id);
  
        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Organization id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CustomerModuleGetOrganizationByIdAsync (string id);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CustomerManagementModuleApi : ICustomerManagementModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerManagementModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public CustomerManagementModuleApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerManagementModuleApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CustomerManagementModuleApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
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
        /// Update contact 
        /// </summary>
        /// <param name="contact"></param> 
        /// <returns></returns>            
        public void CustomerModuleUpdateContact (VirtoCommerceCustomerModuleWebModelContact contact)
        {
            
            // verify the required parameter 'contact' is set
            if (contact == null) throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerModuleUpdateContact");
            
    
            var path = "/api/contacts";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(contact); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleUpdateContact: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleUpdateContact: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update contact 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CustomerModuleUpdateContactAsync (VirtoCommerceCustomerModuleWebModelContact contact)
        {
            // verify the required parameter 'contact' is set
            if (contact == null) throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerModuleUpdateContact");
            
    
            var path = "/api/contacts";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(contact); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleUpdateContact: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Create contact 
        /// </summary>
        /// <param name="contact"></param> 
        /// <returns>VirtoCommerceCustomerModuleWebModelContact</returns>            
        public VirtoCommerceCustomerModuleWebModelContact CustomerModuleCreateContact (VirtoCommerceCustomerModuleWebModelContact contact)
        {
            
            // verify the required parameter 'contact' is set
            if (contact == null) throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerModuleCreateContact");
            
    
            var path = "/api/contacts";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(contact); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleCreateContact: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleCreateContact: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCustomerModuleWebModelContact) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCustomerModuleWebModelContact), response.Headers);
        }
    
        /// <summary>
        /// Create contact 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>VirtoCommerceCustomerModuleWebModelContact</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelContact> CustomerModuleCreateContactAsync (VirtoCommerceCustomerModuleWebModelContact contact)
        {
            // verify the required parameter 'contact' is set
            if (contact == null) throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerModuleCreateContact");
            
    
            var path = "/api/contacts";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(contact); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleCreateContact: " + response.Content, response.Content);

            return (VirtoCommerceCustomerModuleWebModelContact) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCustomerModuleWebModelContact), response.Headers);
        }
        
        /// <summary>
        /// Delete contacts Delete contacts by given array of ids.
        /// </summary>
        /// <param name="ids">An array of contacts ids</param> 
        /// <returns></returns>            
        public void CustomerModuleDeleteContacts (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerModuleDeleteContacts");
            
    
            var path = "/api/contacts";
    
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
            
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleDeleteContacts: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleDeleteContacts: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete contacts Delete contacts by given array of ids.
        /// </summary>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CustomerModuleDeleteContactsAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerModuleDeleteContacts");
            
    
            var path = "/api/contacts";
    
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
            
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleDeleteContacts: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get contact 
        /// </summary>
        /// <param name="id">Contact id</param> 
        /// <returns></returns>            
        public void CustomerModuleGetContactById (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CustomerModuleGetContactById");
            
    
            var path = "/api/contacts/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleGetContactById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleGetContactById: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get contact 
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CustomerModuleGetContactByIdAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CustomerModuleGetContactById");
            
    
            var path = "/api/contacts/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleGetContactById: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <param name="criteriaKeyword">Word, part of word or phrase to search</param> 
        /// <param name="criteriaOrganizationId">It used to limit search within an organization</param> 
        /// <param name="criteriaStart">It used to skip some first search results</param> 
        /// <param name="criteriaCount">It used to limit the number of search results</param> 
        /// <returns>VirtoCommerceCustomerModuleWebModelSearchResult</returns>            
        public VirtoCommerceCustomerModuleWebModelSearchResult CustomerModuleSearch (string criteriaKeyword, string criteriaOrganizationId, int? criteriaStart, int? criteriaCount)
        {
            
    
            var path = "/api/members";
    
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
            
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaOrganizationId != null) queryParams.Add("criteria.organizationId", ApiClient.ParameterToString(criteriaOrganizationId)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleSearch: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCustomerModuleWebModelSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCustomerModuleWebModelSearchResult), response.Headers);
        }
    
        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <param name="criteriaKeyword">Word, part of word or phrase to search</param>
        /// <param name="criteriaOrganizationId">It used to limit search within an organization</param>
        /// <param name="criteriaStart">It used to skip some first search results</param>
        /// <param name="criteriaCount">It used to limit the number of search results</param>
        /// <returns>VirtoCommerceCustomerModuleWebModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelSearchResult> CustomerModuleSearchAsync (string criteriaKeyword, string criteriaOrganizationId, int? criteriaStart, int? criteriaCount)
        {
            
    
            var path = "/api/members";
    
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
            
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaOrganizationId != null) queryParams.Add("criteria.organizationId", ApiClient.ParameterToString(criteriaOrganizationId)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleSearch: " + response.Content, response.Content);

            return (VirtoCommerceCustomerModuleWebModelSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCustomerModuleWebModelSearchResult), response.Headers);
        }
        
        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleListOrganizations ()
        {
            
    
            var path = "/api/organizations";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleListOrganizations: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleListOrganizations: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceCustomerModuleWebModelOrganization>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCustomerModuleWebModelOrganization>), response.Headers);
        }
    
        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCustomerModuleWebModelOrganization>> CustomerModuleListOrganizationsAsync ()
        {
            
    
            var path = "/api/organizations";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleListOrganizations: " + response.Content, response.Content);

            return (List<VirtoCommerceCustomerModuleWebModelOrganization>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCustomerModuleWebModelOrganization>), response.Headers);
        }
        
        /// <summary>
        /// Update organization 
        /// </summary>
        /// <param name="organization"></param> 
        /// <returns></returns>            
        public void CustomerModuleUpdateOrganization (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
            
            // verify the required parameter 'organization' is set
            if (organization == null) throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerModuleUpdateOrganization");
            
    
            var path = "/api/organizations";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(organization); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleUpdateOrganization: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleUpdateOrganization: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update organization 
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CustomerModuleUpdateOrganizationAsync (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
            // verify the required parameter 'organization' is set
            if (organization == null) throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerModuleUpdateOrganization");
            
    
            var path = "/api/organizations";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(organization); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleUpdateOrganization: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Create organization 
        /// </summary>
        /// <param name="organization"></param> 
        /// <returns>VirtoCommerceCustomerModuleWebModelOrganization</returns>            
        public VirtoCommerceCustomerModuleWebModelOrganization CustomerModuleCreateOrganization (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
            
            // verify the required parameter 'organization' is set
            if (organization == null) throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerModuleCreateOrganization");
            
    
            var path = "/api/organizations";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(organization); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleCreateOrganization: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleCreateOrganization: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCustomerModuleWebModelOrganization) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCustomerModuleWebModelOrganization), response.Headers);
        }
    
        /// <summary>
        /// Create organization 
        /// </summary>
        /// <param name="organization"></param>
        /// <returns>VirtoCommerceCustomerModuleWebModelOrganization</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCustomerModuleWebModelOrganization> CustomerModuleCreateOrganizationAsync (VirtoCommerceCustomerModuleWebModelOrganization organization)
        {
            // verify the required parameter 'organization' is set
            if (organization == null) throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerModuleCreateOrganization");
            
    
            var path = "/api/organizations";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(organization); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleCreateOrganization: " + response.Content, response.Content);

            return (VirtoCommerceCustomerModuleWebModelOrganization) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCustomerModuleWebModelOrganization), response.Headers);
        }
        
        /// <summary>
        /// Delete organizations Delete organizations by given array of ids.
        /// </summary>
        /// <param name="ids">An array of organizations ids</param> 
        /// <returns></returns>            
        public void CustomerModuleDeleteOrganizations (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerModuleDeleteOrganizations");
            
    
            var path = "/api/organizations";
    
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
            
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleDeleteOrganizations: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleDeleteOrganizations: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete organizations Delete organizations by given array of ids.
        /// </summary>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CustomerModuleDeleteOrganizationsAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerModuleDeleteOrganizations");
            
    
            var path = "/api/organizations";
    
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
            
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleDeleteOrganizations: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get organization 
        /// </summary>
        /// <param name="id">Organization id</param> 
        /// <returns></returns>            
        public void CustomerModuleGetOrganizationById (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CustomerModuleGetOrganizationById");
            
    
            var path = "/api/organizations/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleGetOrganizationById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleGetOrganizationById: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get organization 
        /// </summary>
        /// <param name="id">Organization id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CustomerModuleGetOrganizationByIdAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CustomerModuleGetOrganizationById");
            
    
            var path = "/api/organizations/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CustomerModuleGetOrganizationById: " + response.Content, response.Content);

            
            return;
        }
        
    }
    
}
