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
        void CustomerModuleUpdateContact (VirtoCommerceDomainCustomerModelContact contact);
  
        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleUpdateContactWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact);

        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleUpdateContactAsync (VirtoCommerceDomainCustomerModelContact contact);

        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateContactAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact);
        
        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>VirtoCommerceDomainCustomerModelContact</returns>
        VirtoCommerceDomainCustomerModelContact CustomerModuleCreateContact (VirtoCommerceDomainCustomerModelContact contact);
  
        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelContact</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelContact> CustomerModuleCreateContactWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact);

        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelContact</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelContact> CustomerModuleCreateContactAsync (VirtoCommerceDomainCustomerModelContact contact);

        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelContact)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelContact>> CustomerModuleCreateContactAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact);
        
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
        /// <param name="id">Contact ID</param>
        /// <returns>VirtoCommerceDomainCustomerModelContact</returns>
        VirtoCommerceDomainCustomerModelContact CustomerModuleGetContactById (string id);
  
        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Contact ID</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelContact</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelContact> CustomerModuleGetContactByIdWithHttpInfo (string id);

        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelContact</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelContact> CustomerModuleGetContactByIdAsync (string id);

        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelContact)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelContact>> CustomerModuleGetContactByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Update member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="member"></param>
        /// <returns></returns>
        void CustomerModuleUpdateMember (VirtoCommerceDomainCustomerModelMember member);
  
        /// <summary>
        /// Update member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="member"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleUpdateMemberWithHttpInfo (VirtoCommerceDomainCustomerModelMember member);

        /// <summary>
        /// Update member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="member"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleUpdateMemberAsync (VirtoCommerceDomainCustomerModelMember member);

        /// <summary>
        /// Update member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="member"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateMemberAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelMember member);
        
        /// <summary>
        /// Create member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="member"></param>
        /// <returns>VirtoCommerceDomainCustomerModelMember</returns>
        VirtoCommerceDomainCustomerModelMember CustomerModuleCreateMember (VirtoCommerceDomainCustomerModelMember member);
  
        /// <summary>
        /// Create member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="member"></param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelMember</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelMember> CustomerModuleCreateMemberWithHttpInfo (VirtoCommerceDomainCustomerModelMember member);

        /// <summary>
        /// Create member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="member"></param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelMember</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelMember> CustomerModuleCreateMemberAsync (VirtoCommerceDomainCustomerModelMember member);

        /// <summary>
        /// Create member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="member"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelMember)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelMember>> CustomerModuleCreateMemberAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelMember member);
        
        /// <summary>
        /// Delete members
        /// </summary>
        /// <remarks>
        /// Delete members by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of members ids</param>
        /// <returns></returns>
        void CustomerModuleDeleteMembers (List<string> ids);
  
        /// <summary>
        /// Delete members
        /// </summary>
        /// <remarks>
        /// Delete members by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of members ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleDeleteMembersWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete members
        /// </summary>
        /// <remarks>
        /// Delete members by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of members ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleDeleteMembersAsync (List<string> ids);

        /// <summary>
        /// Delete members
        /// </summary>
        /// <remarks>
        /// Delete members by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of members ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteMembersAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>VirtoCommerceDomainCustomerModelSearchResult</returns>
        VirtoCommerceDomainCustomerModelSearchResult CustomerModuleSearch (VirtoCommerceDomainCustomerModelSearchCriteria criteria);
  
        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelSearchResult</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelSearchResult> CustomerModuleSearchWithHttpInfo (VirtoCommerceDomainCustomerModelSearchCriteria criteria);

        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelSearchResult> CustomerModuleSearchAsync (VirtoCommerceDomainCustomerModelSearchCriteria criteria);

        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelSearchResult>> CustomerModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelSearchCriteria criteria);
        
        /// <summary>
        /// Get member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">member id</param>
        /// <returns>VirtoCommerceDomainCustomerModelMember</returns>
        VirtoCommerceDomainCustomerModelMember CustomerModuleGetMemberById (string id);
  
        /// <summary>
        /// Get member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">member id</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelMember</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelMember> CustomerModuleGetMemberByIdWithHttpInfo (string id);

        /// <summary>
        /// Get member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">member id</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelMember</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelMember> CustomerModuleGetMemberByIdAsync (string id);

        /// <summary>
        /// Get member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">member id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelMember)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelMember>> CustomerModuleGetMemberByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <returns>List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        List<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleListOrganizations ();
  
        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleListOrganizationsWithHttpInfo ();

        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleListOrganizationsAsync ();

        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>>> CustomerModuleListOrganizationsAsyncWithHttpInfo ();
        
        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns></returns>
        void CustomerModuleUpdateOrganization (VirtoCommerceDomainCustomerModelOrganization organization);
  
        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleUpdateOrganizationWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization);

        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleUpdateOrganizationAsync (VirtoCommerceDomainCustomerModelOrganization organization);

        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateOrganizationAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization);
        
        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>VirtoCommerceDomainCustomerModelOrganization</returns>
        VirtoCommerceDomainCustomerModelOrganization CustomerModuleCreateOrganization (VirtoCommerceDomainCustomerModelOrganization organization);
  
        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelOrganization</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleCreateOrganizationWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization);

        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelOrganization</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleCreateOrganizationAsync (VirtoCommerceDomainCustomerModelOrganization organization);

        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelOrganization)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleCreateOrganizationAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization);
        
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
        /// <returns>VirtoCommerceDomainCustomerModelOrganization</returns>
        VirtoCommerceDomainCustomerModelOrganization CustomerModuleGetOrganizationById (string id);
  
        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Organization id</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelOrganization</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleGetOrganizationByIdWithHttpInfo (string id);

        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Organization id</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelOrganization</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleGetOrganizationByIdAsync (string id);

        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Organization id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelOrganization)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleGetOrganizationByIdAsyncWithHttpInfo (string id);
        
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
        public void CustomerModuleUpdateContact (VirtoCommerceDomainCustomerModelContact contact)
        {
             CustomerModuleUpdateContactWithHttpInfo(contact);
        }

        /// <summary>
        /// Update contact 
        /// </summary>
        /// <param name="contact"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleUpdateContactWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact)
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
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
        public async System.Threading.Tasks.Task CustomerModuleUpdateContactAsync (VirtoCommerceDomainCustomerModelContact contact)
        {
             await CustomerModuleUpdateContactAsyncWithHttpInfo(contact);

        }

        /// <summary>
        /// Update contact 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateContactAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact)
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
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
        /// <returns>VirtoCommerceDomainCustomerModelContact</returns>
        public VirtoCommerceDomainCustomerModelContact CustomerModuleCreateContact (VirtoCommerceDomainCustomerModelContact contact)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelContact> response = CustomerModuleCreateContactWithHttpInfo(contact);
             return response.Data;
        }

        /// <summary>
        /// Create contact 
        /// </summary>
        /// <param name="contact"></param> 
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelContact</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelContact > CustomerModuleCreateContactWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact)
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateContact: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateContact: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceDomainCustomerModelContact>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelContact) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelContact)));
            
        }
    
        /// <summary>
        /// Create contact 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelContact</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelContact> CustomerModuleCreateContactAsync (VirtoCommerceDomainCustomerModelContact contact)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelContact> response = await CustomerModuleCreateContactAsyncWithHttpInfo(contact);
             return response.Data;

        }

        /// <summary>
        /// Create contact 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelContact)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelContact>> CustomerModuleCreateContactAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact)
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateContact: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateContact: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelContact>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelContact) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelContact)));
            
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
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
        /// <param name="id">Contact ID</param> 
        /// <returns>VirtoCommerceDomainCustomerModelContact</returns>
        public VirtoCommerceDomainCustomerModelContact CustomerModuleGetContactById (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelContact> response = CustomerModuleGetContactByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get contact 
        /// </summary>
        /// <param name="id">Contact ID</param> 
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelContact</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelContact > CustomerModuleGetContactByIdWithHttpInfo (string id)
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleGetContactById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetContactById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceDomainCustomerModelContact>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelContact) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelContact)));
            
        }
    
        /// <summary>
        /// Get contact 
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelContact</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelContact> CustomerModuleGetContactByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelContact> response = await CustomerModuleGetContactByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get contact 
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelContact)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelContact>> CustomerModuleGetContactByIdAsyncWithHttpInfo (string id)
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleGetContactById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetContactById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelContact>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelContact) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelContact)));
            
        }
        
        /// <summary>
        /// Update member 
        /// </summary>
        /// <param name="member"></param> 
        /// <returns></returns>
        public void CustomerModuleUpdateMember (VirtoCommerceDomainCustomerModelMember member)
        {
             CustomerModuleUpdateMemberWithHttpInfo(member);
        }

        /// <summary>
        /// Update member 
        /// </summary>
        /// <param name="member"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleUpdateMemberWithHttpInfo (VirtoCommerceDomainCustomerModelMember member)
        {
            
            // verify the required parameter 'member' is set
            if (member == null)
                throw new ApiException(400, "Missing required parameter 'member' when calling CustomerManagementModuleApi->CustomerModuleUpdateMember");
            
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (member.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(member); // http body (model) parameter
            }
            else
            {
                postBody = member; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateMember: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateMember: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update member 
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleUpdateMemberAsync (VirtoCommerceDomainCustomerModelMember member)
        {
             await CustomerModuleUpdateMemberAsyncWithHttpInfo(member);

        }

        /// <summary>
        /// Update member 
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateMemberAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelMember member)
        {
            // verify the required parameter 'member' is set
            if (member == null) throw new ApiException(400, "Missing required parameter 'member' when calling CustomerModuleUpdateMember");
            
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(member); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateMember: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleUpdateMember: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create member 
        /// </summary>
        /// <param name="member"></param> 
        /// <returns>VirtoCommerceDomainCustomerModelMember</returns>
        public VirtoCommerceDomainCustomerModelMember CustomerModuleCreateMember (VirtoCommerceDomainCustomerModelMember member)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelMember> response = CustomerModuleCreateMemberWithHttpInfo(member);
             return response.Data;
        }

        /// <summary>
        /// Create member 
        /// </summary>
        /// <param name="member"></param> 
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelMember</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelMember > CustomerModuleCreateMemberWithHttpInfo (VirtoCommerceDomainCustomerModelMember member)
        {
            
            // verify the required parameter 'member' is set
            if (member == null)
                throw new ApiException(400, "Missing required parameter 'member' when calling CustomerManagementModuleApi->CustomerModuleCreateMember");
            
    
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
            
            
            
            
            if (member.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(member); // http body (model) parameter
            }
            else
            {
                postBody = member; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateMember: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateMember: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceDomainCustomerModelMember>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelMember) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelMember)));
            
        }
    
        /// <summary>
        /// Create member 
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelMember</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelMember> CustomerModuleCreateMemberAsync (VirtoCommerceDomainCustomerModelMember member)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelMember> response = await CustomerModuleCreateMemberAsyncWithHttpInfo(member);
             return response.Data;

        }

        /// <summary>
        /// Create member 
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelMember)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelMember>> CustomerModuleCreateMemberAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelMember member)
        {
            // verify the required parameter 'member' is set
            if (member == null) throw new ApiException(400, "Missing required parameter 'member' when calling CustomerModuleCreateMember");
            
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(member); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateMember: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateMember: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelMember>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelMember) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelMember)));
            
        }
        
        /// <summary>
        /// Delete members Delete members by given array of ids.
        /// </summary>
        /// <param name="ids">An array of members ids</param> 
        /// <returns></returns>
        public void CustomerModuleDeleteMembers (List<string> ids)
        {
             CustomerModuleDeleteMembersWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete members Delete members by given array of ids.
        /// </summary>
        /// <param name="ids">An array of members ids</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleDeleteMembersWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerManagementModuleApi->CustomerModuleDeleteMembers");
            
    
            var path_ = "/api/members";
    
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteMembers: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteMembers: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete members Delete members by given array of ids.
        /// </summary>
        /// <param name="ids">An array of members ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleDeleteMembersAsync (List<string> ids)
        {
             await CustomerModuleDeleteMembersAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete members Delete members by given array of ids.
        /// </summary>
        /// <param name="ids">An array of members ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteMembersAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerModuleDeleteMembers");
            
    
            var path_ = "/api/members";
    
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteMembers: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleDeleteMembers: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <param name="criteria">Search criteria</param> 
        /// <returns>VirtoCommerceDomainCustomerModelSearchResult</returns>
        public VirtoCommerceDomainCustomerModelSearchResult CustomerModuleSearch (VirtoCommerceDomainCustomerModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelSearchResult> response = CustomerModuleSearchWithHttpInfo(criteria);
             return response.Data;
        }

        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <param name="criteria">Search criteria</param> 
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelSearchResult</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelSearchResult > CustomerModuleSearchWithHttpInfo (VirtoCommerceDomainCustomerModelSearchCriteria criteria)
        {
            
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling CustomerManagementModuleApi->CustomerModuleSearch");
            
    
            var path_ = "/api/members/search";
    
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceDomainCustomerModelSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelSearchResult)));
            
        }
    
        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelSearchResult> CustomerModuleSearchAsync (VirtoCommerceDomainCustomerModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelSearchResult> response = await CustomerModuleSearchAsyncWithHttpInfo(criteria);
             return response.Data;

        }

        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelSearchResult>> CustomerModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null) throw new ApiException(400, "Missing required parameter 'criteria' when calling CustomerModuleSearch");
            
    
            var path_ = "/api/members/search";
    
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelSearchResult)));
            
        }
        
        /// <summary>
        /// Get member 
        /// </summary>
        /// <param name="id">member id</param> 
        /// <returns>VirtoCommerceDomainCustomerModelMember</returns>
        public VirtoCommerceDomainCustomerModelMember CustomerModuleGetMemberById (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelMember> response = CustomerModuleGetMemberByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get member 
        /// </summary>
        /// <param name="id">member id</param> 
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelMember</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelMember > CustomerModuleGetMemberByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CustomerManagementModuleApi->CustomerModuleGetMemberById");
            
    
            var path_ = "/api/members/{id}";
    
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleGetMemberById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetMemberById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceDomainCustomerModelMember>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelMember) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelMember)));
            
        }
    
        /// <summary>
        /// Get member 
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelMember</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelMember> CustomerModuleGetMemberByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelMember> response = await CustomerModuleGetMemberByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get member 
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelMember)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelMember>> CustomerModuleGetMemberByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CustomerModuleGetMemberById");
            
    
            var path_ = "/api/members/{id}";
    
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleGetMemberById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetMemberById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelMember>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelMember) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelMember)));
            
        }
        
        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <returns>List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        public List<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleListOrganizations ()
        {
             ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>> response = CustomerModuleListOrganizationsWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        public ApiResponse< List<VirtoCommerceDomainCustomerModelOrganization> > CustomerModuleListOrganizationsWithHttpInfo ()
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleListOrganizations: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleListOrganizations: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCustomerModelOrganization>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceDomainCustomerModelOrganization>)));
            
        }
    
        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleListOrganizationsAsync ()
        {
             ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>> response = await CustomerModuleListOrganizationsAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>>> CustomerModuleListOrganizationsAsyncWithHttpInfo ()
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleListOrganizations: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleListOrganizations: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCustomerModelOrganization>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceDomainCustomerModelOrganization>)));
            
        }
        
        /// <summary>
        /// Update organization 
        /// </summary>
        /// <param name="organization"></param> 
        /// <returns></returns>
        public void CustomerModuleUpdateOrganization (VirtoCommerceDomainCustomerModelOrganization organization)
        {
             CustomerModuleUpdateOrganizationWithHttpInfo(organization);
        }

        /// <summary>
        /// Update organization 
        /// </summary>
        /// <param name="organization"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleUpdateOrganizationWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization)
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
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
        public async System.Threading.Tasks.Task CustomerModuleUpdateOrganizationAsync (VirtoCommerceDomainCustomerModelOrganization organization)
        {
             await CustomerModuleUpdateOrganizationAsyncWithHttpInfo(organization);

        }

        /// <summary>
        /// Update organization 
        /// </summary>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateOrganizationAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization)
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
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
        /// <returns>VirtoCommerceDomainCustomerModelOrganization</returns>
        public VirtoCommerceDomainCustomerModelOrganization CustomerModuleCreateOrganization (VirtoCommerceDomainCustomerModelOrganization organization)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelOrganization> response = CustomerModuleCreateOrganizationWithHttpInfo(organization);
             return response.Data;
        }

        /// <summary>
        /// Create organization 
        /// </summary>
        /// <param name="organization"></param> 
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelOrganization</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelOrganization > CustomerModuleCreateOrganizationWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization)
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateOrganization: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateOrganization: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceDomainCustomerModelOrganization>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelOrganization) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelOrganization)));
            
        }
    
        /// <summary>
        /// Create organization 
        /// </summary>
        /// <param name="organization"></param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelOrganization</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleCreateOrganizationAsync (VirtoCommerceDomainCustomerModelOrganization organization)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelOrganization> response = await CustomerModuleCreateOrganizationAsyncWithHttpInfo(organization);
             return response.Data;

        }

        /// <summary>
        /// Create organization 
        /// </summary>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelOrganization)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleCreateOrganizationAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization)
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateOrganization: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleCreateOrganization: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelOrganization>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelOrganization) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelOrganization)));
            
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
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
        /// <returns>VirtoCommerceDomainCustomerModelOrganization</returns>
        public VirtoCommerceDomainCustomerModelOrganization CustomerModuleGetOrganizationById (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelOrganization> response = CustomerModuleGetOrganizationByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get organization 
        /// </summary>
        /// <param name="id">Organization id</param> 
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelOrganization</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelOrganization > CustomerModuleGetOrganizationByIdWithHttpInfo (string id)
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleGetOrganizationById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetOrganizationById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceDomainCustomerModelOrganization>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelOrganization) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelOrganization)));
            
        }
    
        /// <summary>
        /// Get organization 
        /// </summary>
        /// <param name="id">Organization id</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelOrganization</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleGetOrganizationByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelOrganization> response = await CustomerModuleGetOrganizationByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get organization 
        /// </summary>
        /// <param name="id">Organization id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelOrganization)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleGetOrganizationByIdAsyncWithHttpInfo (string id)
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
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CustomerModuleGetOrganizationById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CustomerModuleGetOrganizationById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelOrganization>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelOrganization) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainCustomerModelOrganization)));
            
        }
        
    }
    
}
