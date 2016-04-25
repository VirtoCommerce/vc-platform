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
    public interface IQuoteModuleApi
    {
        #region Synchronous Operations
        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleCalculateTotals (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCalculateTotalsWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleCreate (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCreateWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns></returns>
        void QuoteModuleDelete (List<string> ids);

        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> QuoteModuleDeleteWithHttpInfo (List<string> ids);
        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleGetById (string id);

        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleGetByIdWithHttpInfo (string id);
        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        List<VirtoCommerceQuoteModuleWebModelShipmentMethod> QuoteModuleGetShipmentMethods (string id);

        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> QuoteModuleGetShipmentMethodsWithHttpInfo (string id);
        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult</returns>
        VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult QuoteModuleSearch (VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria criteria);

        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult</returns>
        ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult> QuoteModuleSearchWithHttpInfo (VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria criteria);
        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns></returns>
        void QuoteModuleUpdate (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> QuoteModuleUpdateWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCalculateTotalsAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleCalculateTotalsAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCreateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleCreateAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task QuoteModuleDeleteAsync (List<string> ids);

        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> QuoteModuleDeleteAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleGetByIdAsync (string id);

        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleGetByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> QuoteModuleGetShipmentMethodsAsync (string id);

        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>>> QuoteModuleGetShipmentMethodsAsyncWithHttpInfo (string id);
        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult> QuoteModuleSearchAsync (VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria criteria);

        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult>> QuoteModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria criteria);
        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task QuoteModuleUpdateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> QuoteModuleUpdateAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class QuoteModuleApi : IQuoteModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuoteModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public QuoteModuleApi(Configuration configuration)
        {
            if (configuration == null) // use the default one in Configuration
                this.Configuration = Configuration.Default;
            else
                this.Configuration = configuration;

            // ensure API client has configuration ready
            if (Configuration.ApiClient.Configuration == null)
            {
                this.Configuration.ApiClient.Configuration = this.Configuration;
            }
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
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleCalculateTotals (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> localVarResponse = QuoteModuleCalculateTotalsWithHttpInfo(quoteRequest);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public ApiResponse< VirtoCommerceQuoteModuleWebModelQuoteRequest > QuoteModuleCalculateTotalsWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleApi->QuoteModuleCalculateTotals");

            var localVarPath = "/api/quote/requests/recalculate";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleCalculateTotals: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleCalculateTotals: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }

        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCalculateTotalsAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> localVarResponse = await QuoteModuleCalculateTotalsAsyncWithHttpInfo(quoteRequest);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleCalculateTotalsAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleApi->QuoteModuleCalculateTotals");

            var localVarPath = "/api/quote/requests/recalculate";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleCalculateTotals: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleCalculateTotals: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }

        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleCreate (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> localVarResponse = QuoteModuleCreateWithHttpInfo(quoteRequest);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public ApiResponse< VirtoCommerceQuoteModuleWebModelQuoteRequest > QuoteModuleCreateWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleApi->QuoteModuleCreate");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }

        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCreateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> localVarResponse = await QuoteModuleCreateAsyncWithHttpInfo(quoteRequest);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleCreateAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleApi->QuoteModuleCreate");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }

        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns></returns>
        public void QuoteModuleDelete (List<string> ids)
        {
             QuoteModuleDeleteWithHttpInfo(ids);
        }

        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> QuoteModuleDeleteWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling QuoteModuleApi->QuoteModuleDelete");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task QuoteModuleDeleteAsync (List<string> ids)
        {
             await QuoteModuleDeleteAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> QuoteModuleDeleteAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling QuoteModuleApi->QuoteModuleDelete");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleGetById (string id)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> localVarResponse = QuoteModuleGetByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public ApiResponse< VirtoCommerceQuoteModuleWebModelQuoteRequest > QuoteModuleGetByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleApi->QuoteModuleGetById");

            var localVarPath = "/api/quote/requests/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleGetById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleGetById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }

        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleGetByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> localVarResponse = await QuoteModuleGetByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleGetByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleApi->QuoteModuleGetById");

            var localVarPath = "/api/quote/requests/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleGetById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleGetById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }

        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        public List<VirtoCommerceQuoteModuleWebModelShipmentMethod> QuoteModuleGetShipmentMethods (string id)
        {
             ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> localVarResponse = QuoteModuleGetShipmentMethodsWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        public ApiResponse< List<VirtoCommerceQuoteModuleWebModelShipmentMethod> > QuoteModuleGetShipmentMethodsWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleApi->QuoteModuleGetShipmentMethods");

            var localVarPath = "/api/quote/requests/{id}/shipmentmethods";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleGetShipmentMethods: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleGetShipmentMethods: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceQuoteModuleWebModelShipmentMethod>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceQuoteModuleWebModelShipmentMethod>)));
            
        }

        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> QuoteModuleGetShipmentMethodsAsync (string id)
        {
             ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> localVarResponse = await QuoteModuleGetShipmentMethodsAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>>> QuoteModuleGetShipmentMethodsAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleApi->QuoteModuleGetShipmentMethods");

            var localVarPath = "/api/quote/requests/{id}/shipmentmethods";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleGetShipmentMethods: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleGetShipmentMethods: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceQuoteModuleWebModelShipmentMethod>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceQuoteModuleWebModelShipmentMethod>)));
            
        }

        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult</returns>
        public VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult QuoteModuleSearch (VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult> localVarResponse = QuoteModuleSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult</returns>
        public ApiResponse< VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult > QuoteModuleSearchWithHttpInfo (VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling QuoteModuleApi->QuoteModuleSearch");

            var localVarPath = "/api/quote/requests/search";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult)));
            
        }

        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult> QuoteModuleSearchAsync (VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult> localVarResponse = await QuoteModuleSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult>> QuoteModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling QuoteModuleApi->QuoteModuleSearch");

            var localVarPath = "/api/quote/requests/search";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequestSearchResult)));
            
        }

        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns></returns>
        public void QuoteModuleUpdate (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             QuoteModuleUpdateWithHttpInfo(quoteRequest);
        }

        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> QuoteModuleUpdateWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleApi->QuoteModuleUpdate");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task QuoteModuleUpdateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             await QuoteModuleUpdateAsyncWithHttpInfo(quoteRequest);

        }

        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> QuoteModuleUpdateAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null)
                throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleApi->QuoteModuleUpdate");

            var localVarPath = "/api/quote/requests";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (quoteRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = quoteRequest; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling QuoteModuleUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

    }
}
