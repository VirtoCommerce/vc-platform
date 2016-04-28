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
    public interface ICommerceCoreModuleApi
    {
        #region Synchronous Operations
        /// <summary>
        /// Batch create or update seo infos
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="seoInfos"></param>
        /// <returns></returns>
        void CommerceBatchUpdateSeoInfos (List<VirtoCommerceDomainCommerceModelSeoInfo> seoInfos);

        /// <summary>
        /// Batch create or update seo infos
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="seoInfos"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CommerceBatchUpdateSeoInfosWithHttpInfo (List<VirtoCommerceDomainCommerceModelSeoInfo> seoInfos);
        /// <summary>
        /// Create new currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        void CommerceCreateCurrency (VirtoCommerceDomainCommerceModelCurrency currency);

        /// <summary>
        /// Create new currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CommerceCreateCurrencyWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency);
        /// <summary>
        /// Delete currencies
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codes">currency codes</param>
        /// <returns></returns>
        void CommerceDeleteCurrencies (List<string> codes);

        /// <summary>
        /// Delete currencies
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codes">currency codes</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CommerceDeleteCurrenciesWithHttpInfo (List<string> codes);
        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;</returns>
        List<VirtoCommerceDomainTaxModelTaxRate> CommerceEvaluateTaxes (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext);

        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;</returns>
        ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>> CommerceEvaluateTaxesWithHttpInfo (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext);
        /// <summary>
        /// Return all currencies registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        List<VirtoCommerceDomainCommerceModelCurrency> CommerceGetAllCurrencies ();

        /// <summary>
        /// Return all currencies registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>> CommerceGetAllCurrenciesWithHttpInfo ();
        /// <summary>
        /// Find fulfillment center by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">fulfillment center id</param>
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        VirtoCommerceCoreModuleWebModelFulfillmentCenter CommerceGetFulfillmentCenter (string id);

        /// <summary>
        /// Find fulfillment center by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">fulfillment center id</param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenterWithHttpInfo (string id);
        /// <summary>
        /// Return all fulfillment centers registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        List<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenters ();

        /// <summary>
        /// Return all fulfillment centers registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCentersWithHttpInfo ();
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <returns>List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        List<VirtoCommerceDomainCommerceModelSeoInfo> CommerceGetSeoDuplicates (string objectId, string objectType);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoDuplicatesWithHttpInfo (string objectId, string objectType);
        /// <summary>
        /// Find all SEO records for object by slug
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="slug">slug</param>
        /// <returns>List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        List<VirtoCommerceDomainCommerceModelSeoInfo> CommerceGetSeoInfoBySlug (string slug);

        /// <summary>
        /// Find all SEO records for object by slug
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="slug">slug</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoInfoBySlugWithHttpInfo (string slug);
        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        VirtoCommerceDomainPaymentModelPostProcessPaymentResult CommercePostProcessPayment (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback);

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>ApiResponse of VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> CommercePostProcessPaymentWithHttpInfo (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback);
        /// <summary>
        /// Update a existing currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        void CommerceUpdateCurrency (VirtoCommerceDomainCommerceModelCurrency currency);

        /// <summary>
        /// Update a existing currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CommerceUpdateCurrencyWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency);
        /// <summary>
        /// Update a existing fulfillment center
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="center">fulfillment center</param>
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        VirtoCommerceCoreModuleWebModelFulfillmentCenter CommerceUpdateFulfillmentCenter (VirtoCommerceCoreModuleWebModelFulfillmentCenter center);

        /// <summary>
        /// Update a existing fulfillment center
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="center">fulfillment center</param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceUpdateFulfillmentCenterWithHttpInfo (VirtoCommerceCoreModuleWebModelFulfillmentCenter center);
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult StorefrontSecurityCreate (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityCreateWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        /// <summary>
        /// Generate a password reset token
        /// </summary>
        /// <remarks>
        /// Generates a password reset token and sends a password reset link to the user via email.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> StorefrontSecurityGenerateResetPasswordTokenWithHttpInfo (string userId, string storeName, string language, string callbackUrl);
        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        VirtoCommerceCoreModuleWebModelStorefrontUser StorefrontSecurityGetUserById (string userId);

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser> StorefrontSecurityGetUserByIdWithHttpInfo (string userId);
        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        VirtoCommerceCoreModuleWebModelStorefrontUser StorefrontSecurityGetUserByLogin (string loginProvider, string providerKey);

        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser> StorefrontSecurityGetUserByLoginWithHttpInfo (string loginProvider, string providerKey);
        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        VirtoCommerceCoreModuleWebModelStorefrontUser StorefrontSecurityGetUserByName (string userName);

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser> StorefrontSecurityGetUserByNameWithHttpInfo (string userName);
        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelSignInResult</returns>
        ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult> StorefrontSecurityPasswordSignInWithHttpInfo (string userName, string password);
        /// <summary>
        /// Reset a password for the user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityResetPasswordWithHttpInfo (string userId, string token, string newPassword);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Batch create or update seo infos
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="seoInfos"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CommerceBatchUpdateSeoInfosAsync (List<VirtoCommerceDomainCommerceModelSeoInfo> seoInfos);

        /// <summary>
        /// Batch create or update seo infos
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="seoInfos"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CommerceBatchUpdateSeoInfosAsyncWithHttpInfo (List<VirtoCommerceDomainCommerceModelSeoInfo> seoInfos);
        /// <summary>
        /// Create new currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CommerceCreateCurrencyAsync (VirtoCommerceDomainCommerceModelCurrency currency);

        /// <summary>
        /// Create new currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CommerceCreateCurrencyAsyncWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency);
        /// <summary>
        /// Delete currencies
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codes">currency codes</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CommerceDeleteCurrenciesAsync (List<string> codes);

        /// <summary>
        /// Delete currencies
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codes">currency codes</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CommerceDeleteCurrenciesAsyncWithHttpInfo (List<string> codes);
        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>Task of List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceDomainTaxModelTaxRate>> CommerceEvaluateTaxesAsync (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext);

        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>>> CommerceEvaluateTaxesAsyncWithHttpInfo (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext);
        /// <summary>
        /// Return all currencies registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelCurrency>> CommerceGetAllCurrenciesAsync ();

        /// <summary>
        /// Return all currencies registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>>> CommerceGetAllCurrenciesAsyncWithHttpInfo ();
        /// <summary>
        /// Find fulfillment center by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">fulfillment center id</param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenterAsync (string id);

        /// <summary>
        /// Find fulfillment center by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">fulfillment center id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelFulfillmentCenter)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCenterAsyncWithHttpInfo (string id);
        /// <summary>
        /// Return all fulfillment centers registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCentersAsync ();

        /// <summary>
        /// Return all fulfillment centers registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>>> CommerceGetFulfillmentCentersAsyncWithHttpInfo ();
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <returns>Task of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoDuplicatesAsync (string objectId, string objectType);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>> CommerceGetSeoDuplicatesAsyncWithHttpInfo (string objectId, string objectType);
        /// <summary>
        /// Find all SEO records for object by slug
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="slug">slug</param>
        /// <returns>Task of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoInfoBySlugAsync (string slug);

        /// <summary>
        /// Find all SEO records for object by slug
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="slug">slug</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>> CommerceGetSeoInfoBySlugAsyncWithHttpInfo (string slug);
        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>Task of VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> CommercePostProcessPaymentAsync (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback);

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainPaymentModelPostProcessPaymentResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult>> CommercePostProcessPaymentAsyncWithHttpInfo (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback);
        /// <summary>
        /// Update a existing currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CommerceUpdateCurrencyAsync (VirtoCommerceDomainCommerceModelCurrency currency);

        /// <summary>
        /// Update a existing currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CommerceUpdateCurrencyAsyncWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency);
        /// <summary>
        /// Update a existing fulfillment center
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="center">fulfillment center</param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceUpdateFulfillmentCenterAsync (VirtoCommerceCoreModuleWebModelFulfillmentCenter center);

        /// <summary>
        /// Update a existing fulfillment center
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="center">fulfillment center</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelFulfillmentCenter)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceUpdateFulfillmentCenterAsyncWithHttpInfo (VirtoCommerceCoreModuleWebModelFulfillmentCenter center);
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> StorefrontSecurityCreateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        /// <summary>
        /// Generate a password reset token
        /// </summary>
        /// <remarks>
        /// Generates a password reset token and sends a password reset link to the user via email.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task StorefrontSecurityGenerateResetPasswordTokenAsync (string userId, string storeName, string language, string callbackUrl);

        /// <summary>
        /// Generate a password reset token
        /// </summary>
        /// <remarks>
        /// Generates a password reset token and sends a password reset link to the user via email.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> StorefrontSecurityGenerateResetPasswordTokenAsyncWithHttpInfo (string userId, string storeName, string language, string callbackUrl);
        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelStorefrontUser> StorefrontSecurityGetUserByIdAsync (string userId);

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelStorefrontUser)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>> StorefrontSecurityGetUserByIdAsyncWithHttpInfo (string userId);
        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelStorefrontUser> StorefrontSecurityGetUserByLoginAsync (string loginProvider, string providerKey);

        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelStorefrontUser)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>> StorefrontSecurityGetUserByLoginAsyncWithHttpInfo (string loginProvider, string providerKey);
        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelStorefrontUser> StorefrontSecurityGetUserByNameAsync (string userName);

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelStorefrontUser)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>> StorefrontSecurityGetUserByNameAsyncWithHttpInfo (string userName);
        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelSignInResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelSignInResult> StorefrontSecurityPasswordSignInAsync (string userName, string password);

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelSignInResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult>> StorefrontSecurityPasswordSignInAsyncWithHttpInfo (string userName, string password);
        /// <summary>
        /// Reset a password for the user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityResetPasswordAsync (string userId, string token, string newPassword);

        /// <summary>
        /// Reset a password for the user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> StorefrontSecurityResetPasswordAsyncWithHttpInfo (string userId, string token, string newPassword);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CommerceCoreModuleApi : ICommerceCoreModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommerceCoreModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public CommerceCoreModuleApi(Configuration configuration)
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
        /// Batch create or update seo infos 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="seoInfos"></param>
        /// <returns></returns>
        public void CommerceBatchUpdateSeoInfos (List<VirtoCommerceDomainCommerceModelSeoInfo> seoInfos)
        {
             CommerceBatchUpdateSeoInfosWithHttpInfo(seoInfos);
        }

        /// <summary>
        /// Batch create or update seo infos 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="seoInfos"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CommerceBatchUpdateSeoInfosWithHttpInfo (List<VirtoCommerceDomainCommerceModelSeoInfo> seoInfos)
        {
            // verify the required parameter 'seoInfos' is set
            if (seoInfos == null)
                throw new ApiException(400, "Missing required parameter 'seoInfos' when calling CommerceCoreModuleApi->CommerceBatchUpdateSeoInfos");

            var localVarPath = "/api/seoinfos/batchupdate";
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
            if (seoInfos.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(seoInfos); // http body (model) parameter
            }
            else
            {
                localVarPostBody = seoInfos; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceBatchUpdateSeoInfos: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceBatchUpdateSeoInfos: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Batch create or update seo infos 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="seoInfos"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CommerceBatchUpdateSeoInfosAsync (List<VirtoCommerceDomainCommerceModelSeoInfo> seoInfos)
        {
             await CommerceBatchUpdateSeoInfosAsyncWithHttpInfo(seoInfos);

        }

        /// <summary>
        /// Batch create or update seo infos 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="seoInfos"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CommerceBatchUpdateSeoInfosAsyncWithHttpInfo (List<VirtoCommerceDomainCommerceModelSeoInfo> seoInfos)
        {
            // verify the required parameter 'seoInfos' is set
            if (seoInfos == null)
                throw new ApiException(400, "Missing required parameter 'seoInfos' when calling CommerceCoreModuleApi->CommerceBatchUpdateSeoInfos");

            var localVarPath = "/api/seoinfos/batchupdate";
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
            if (seoInfos.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(seoInfos); // http body (model) parameter
            }
            else
            {
                localVarPostBody = seoInfos; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceBatchUpdateSeoInfos: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceBatchUpdateSeoInfos: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Create new currency 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        public void CommerceCreateCurrency (VirtoCommerceDomainCommerceModelCurrency currency)
        {
             CommerceCreateCurrencyWithHttpInfo(currency);
        }

        /// <summary>
        /// Create new currency 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CommerceCreateCurrencyWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency)
        {
            // verify the required parameter 'currency' is set
            if (currency == null)
                throw new ApiException(400, "Missing required parameter 'currency' when calling CommerceCoreModuleApi->CommerceCreateCurrency");

            var localVarPath = "/api/currencies";
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
            if (currency.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(currency); // http body (model) parameter
            }
            else
            {
                localVarPostBody = currency; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceCreateCurrency: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceCreateCurrency: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Create new currency 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CommerceCreateCurrencyAsync (VirtoCommerceDomainCommerceModelCurrency currency)
        {
             await CommerceCreateCurrencyAsyncWithHttpInfo(currency);

        }

        /// <summary>
        /// Create new currency 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CommerceCreateCurrencyAsyncWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency)
        {
            // verify the required parameter 'currency' is set
            if (currency == null)
                throw new ApiException(400, "Missing required parameter 'currency' when calling CommerceCoreModuleApi->CommerceCreateCurrency");

            var localVarPath = "/api/currencies";
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
            if (currency.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(currency); // http body (model) parameter
            }
            else
            {
                localVarPostBody = currency; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceCreateCurrency: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceCreateCurrency: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete currencies 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codes">currency codes</param>
        /// <returns></returns>
        public void CommerceDeleteCurrencies (List<string> codes)
        {
             CommerceDeleteCurrenciesWithHttpInfo(codes);
        }

        /// <summary>
        /// Delete currencies 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codes">currency codes</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CommerceDeleteCurrenciesWithHttpInfo (List<string> codes)
        {
            // verify the required parameter 'codes' is set
            if (codes == null)
                throw new ApiException(400, "Missing required parameter 'codes' when calling CommerceCoreModuleApi->CommerceDeleteCurrencies");

            var localVarPath = "/api/currencies";
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
            if (codes != null) localVarQueryParams.Add("codes", Configuration.ApiClient.ParameterToString(codes)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceDeleteCurrencies: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceDeleteCurrencies: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete currencies 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codes">currency codes</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CommerceDeleteCurrenciesAsync (List<string> codes)
        {
             await CommerceDeleteCurrenciesAsyncWithHttpInfo(codes);

        }

        /// <summary>
        /// Delete currencies 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="codes">currency codes</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CommerceDeleteCurrenciesAsyncWithHttpInfo (List<string> codes)
        {
            // verify the required parameter 'codes' is set
            if (codes == null)
                throw new ApiException(400, "Missing required parameter 'codes' when calling CommerceCoreModuleApi->CommerceDeleteCurrencies");

            var localVarPath = "/api/currencies";
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
            if (codes != null) localVarQueryParams.Add("codes", Configuration.ApiClient.ParameterToString(codes)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceDeleteCurrencies: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceDeleteCurrencies: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;</returns>
        public List<VirtoCommerceDomainTaxModelTaxRate> CommerceEvaluateTaxes (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>> localVarResponse = CommerceEvaluateTaxesWithHttpInfo(storeId, evalContext);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;</returns>
        public ApiResponse< List<VirtoCommerceDomainTaxModelTaxRate> > CommerceEvaluateTaxesWithHttpInfo (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CommerceCoreModuleApi->CommerceEvaluateTaxes");
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling CommerceCoreModuleApi->CommerceEvaluateTaxes");

            var localVarPath = "/api/taxes/{storeId}/evaluate";
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (evalContext.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(evalContext); // http body (model) parameter
            }
            else
            {
                localVarPostBody = evalContext; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceEvaluateTaxes: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceEvaluateTaxes: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainTaxModelTaxRate>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceDomainTaxModelTaxRate>)));
            
        }

        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>Task of List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceDomainTaxModelTaxRate>> CommerceEvaluateTaxesAsync (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>> localVarResponse = await CommerceEvaluateTaxesAsyncWithHttpInfo(storeId, evalContext);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>>> CommerceEvaluateTaxesAsyncWithHttpInfo (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CommerceCoreModuleApi->CommerceEvaluateTaxes");
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling CommerceCoreModuleApi->CommerceEvaluateTaxes");

            var localVarPath = "/api/taxes/{storeId}/evaluate";
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (evalContext.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(evalContext); // http body (model) parameter
            }
            else
            {
                localVarPostBody = evalContext; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceEvaluateTaxes: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceEvaluateTaxes: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainTaxModelTaxRate>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceDomainTaxModelTaxRate>)));
            
        }

        /// <summary>
        /// Return all currencies registered in the system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        public List<VirtoCommerceDomainCommerceModelCurrency> CommerceGetAllCurrencies ()
        {
             ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>> localVarResponse = CommerceGetAllCurrenciesWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Return all currencies registered in the system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        public ApiResponse< List<VirtoCommerceDomainCommerceModelCurrency> > CommerceGetAllCurrenciesWithHttpInfo ()
        {

            var localVarPath = "/api/currencies";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetAllCurrencies: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetAllCurrencies: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCommerceModelCurrency>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceDomainCommerceModelCurrency>)));
            
        }

        /// <summary>
        /// Return all currencies registered in the system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelCurrency>> CommerceGetAllCurrenciesAsync ()
        {
             ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>> localVarResponse = await CommerceGetAllCurrenciesAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Return all currencies registered in the system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>>> CommerceGetAllCurrenciesAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/currencies";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetAllCurrencies: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetAllCurrencies: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCommerceModelCurrency>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceDomainCommerceModelCurrency>)));
            
        }

        /// <summary>
        /// Find fulfillment center by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">fulfillment center id</param>
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public VirtoCommerceCoreModuleWebModelFulfillmentCenter CommerceGetFulfillmentCenter (string id)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> localVarResponse = CommerceGetFulfillmentCenterWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find fulfillment center by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">fulfillment center id</param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public ApiResponse< VirtoCommerceCoreModuleWebModelFulfillmentCenter > CommerceGetFulfillmentCenterWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CommerceCoreModuleApi->CommerceGetFulfillmentCenter");

            var localVarPath = "/api/fulfillment/centers/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetFulfillmentCenter: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetFulfillmentCenter: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelFulfillmentCenter) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter)));
            
        }

        /// <summary>
        /// Find fulfillment center by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">fulfillment center id</param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenterAsync (string id)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> localVarResponse = await CommerceGetFulfillmentCenterAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find fulfillment center by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">fulfillment center id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelFulfillmentCenter)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCenterAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CommerceCoreModuleApi->CommerceGetFulfillmentCenter");

            var localVarPath = "/api/fulfillment/centers/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetFulfillmentCenter: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetFulfillmentCenter: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelFulfillmentCenter) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter)));
            
        }

        /// <summary>
        /// Return all fulfillment centers registered in the system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        public List<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenters ()
        {
             ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> localVarResponse = CommerceGetFulfillmentCentersWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Return all fulfillment centers registered in the system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        public ApiResponse< List<VirtoCommerceCoreModuleWebModelFulfillmentCenter> > CommerceGetFulfillmentCentersWithHttpInfo ()
        {

            var localVarPath = "/api/fulfillment/centers";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetFulfillmentCenters: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetFulfillmentCenters: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>)));
            
        }

        /// <summary>
        /// Return all fulfillment centers registered in the system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCentersAsync ()
        {
             ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> localVarResponse = await CommerceGetFulfillmentCentersAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Return all fulfillment centers registered in the system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>>> CommerceGetFulfillmentCentersAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/fulfillment/centers";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetFulfillmentCenters: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetFulfillmentCenters: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>)));
            
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <returns>List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        public List<VirtoCommerceDomainCommerceModelSeoInfo> CommerceGetSeoDuplicates (string objectId, string objectType)
        {
             ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>> localVarResponse = CommerceGetSeoDuplicatesWithHttpInfo(objectId, objectType);
             return localVarResponse.Data;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        public ApiResponse< List<VirtoCommerceDomainCommerceModelSeoInfo> > CommerceGetSeoDuplicatesWithHttpInfo (string objectId, string objectType)
        {
            // verify the required parameter 'objectId' is set
            if (objectId == null)
                throw new ApiException(400, "Missing required parameter 'objectId' when calling CommerceCoreModuleApi->CommerceGetSeoDuplicates");
            // verify the required parameter 'objectType' is set
            if (objectType == null)
                throw new ApiException(400, "Missing required parameter 'objectType' when calling CommerceCoreModuleApi->CommerceGetSeoDuplicates");

            var localVarPath = "/api/seoinfos/duplicates";
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
            if (objectId != null) localVarQueryParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // query parameter
            if (objectType != null) localVarQueryParams.Add("objectType", Configuration.ApiClient.ParameterToString(objectType)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetSeoDuplicates: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetSeoDuplicates: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCommerceModelSeoInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceDomainCommerceModelSeoInfo>)));
            
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <returns>Task of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoDuplicatesAsync (string objectId, string objectType)
        {
             ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>> localVarResponse = await CommerceGetSeoDuplicatesAsyncWithHttpInfo(objectId, objectType);
             return localVarResponse.Data;

        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>> CommerceGetSeoDuplicatesAsyncWithHttpInfo (string objectId, string objectType)
        {
            // verify the required parameter 'objectId' is set
            if (objectId == null)
                throw new ApiException(400, "Missing required parameter 'objectId' when calling CommerceCoreModuleApi->CommerceGetSeoDuplicates");
            // verify the required parameter 'objectType' is set
            if (objectType == null)
                throw new ApiException(400, "Missing required parameter 'objectType' when calling CommerceCoreModuleApi->CommerceGetSeoDuplicates");

            var localVarPath = "/api/seoinfos/duplicates";
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
            if (objectId != null) localVarQueryParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // query parameter
            if (objectType != null) localVarQueryParams.Add("objectType", Configuration.ApiClient.ParameterToString(objectType)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetSeoDuplicates: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetSeoDuplicates: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCommerceModelSeoInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceDomainCommerceModelSeoInfo>)));
            
        }

        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="slug">slug</param>
        /// <returns>List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        public List<VirtoCommerceDomainCommerceModelSeoInfo> CommerceGetSeoInfoBySlug (string slug)
        {
             ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>> localVarResponse = CommerceGetSeoInfoBySlugWithHttpInfo(slug);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="slug">slug</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        public ApiResponse< List<VirtoCommerceDomainCommerceModelSeoInfo> > CommerceGetSeoInfoBySlugWithHttpInfo (string slug)
        {
            // verify the required parameter 'slug' is set
            if (slug == null)
                throw new ApiException(400, "Missing required parameter 'slug' when calling CommerceCoreModuleApi->CommerceGetSeoInfoBySlug");

            var localVarPath = "/api/seoinfos/{slug}";
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
            if (slug != null) localVarPathParams.Add("slug", Configuration.ApiClient.ParameterToString(slug)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetSeoInfoBySlug: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetSeoInfoBySlug: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCommerceModelSeoInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceDomainCommerceModelSeoInfo>)));
            
        }

        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="slug">slug</param>
        /// <returns>Task of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoInfoBySlugAsync (string slug)
        {
             ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>> localVarResponse = await CommerceGetSeoInfoBySlugAsyncWithHttpInfo(slug);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="slug">slug</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>> CommerceGetSeoInfoBySlugAsyncWithHttpInfo (string slug)
        {
            // verify the required parameter 'slug' is set
            if (slug == null)
                throw new ApiException(400, "Missing required parameter 'slug' when calling CommerceCoreModuleApi->CommerceGetSeoInfoBySlug");

            var localVarPath = "/api/seoinfos/{slug}";
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
            if (slug != null) localVarPathParams.Add("slug", Configuration.ApiClient.ParameterToString(slug)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetSeoInfoBySlug: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceGetSeoInfoBySlug: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCommerceModelSeoInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceDomainCommerceModelSeoInfo>)));
            
        }

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        public VirtoCommerceDomainPaymentModelPostProcessPaymentResult CommercePostProcessPayment (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback)
        {
             ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> localVarResponse = CommercePostProcessPaymentWithHttpInfo(callback);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>ApiResponse of VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        public ApiResponse< VirtoCommerceDomainPaymentModelPostProcessPaymentResult > CommercePostProcessPaymentWithHttpInfo (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback)
        {
            // verify the required parameter 'callback' is set
            if (callback == null)
                throw new ApiException(400, "Missing required parameter 'callback' when calling CommerceCoreModuleApi->CommercePostProcessPayment");

            var localVarPath = "/api/paymentcallback";
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
            if (callback.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(callback); // http body (model) parameter
            }
            else
            {
                localVarPostBody = callback; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommercePostProcessPayment: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommercePostProcessPayment: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainPaymentModelPostProcessPaymentResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainPaymentModelPostProcessPaymentResult)));
            
        }

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>Task of VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> CommercePostProcessPaymentAsync (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback)
        {
             ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> localVarResponse = await CommercePostProcessPaymentAsyncWithHttpInfo(callback);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainPaymentModelPostProcessPaymentResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult>> CommercePostProcessPaymentAsyncWithHttpInfo (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback)
        {
            // verify the required parameter 'callback' is set
            if (callback == null)
                throw new ApiException(400, "Missing required parameter 'callback' when calling CommerceCoreModuleApi->CommercePostProcessPayment");

            var localVarPath = "/api/paymentcallback";
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
            if (callback.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(callback); // http body (model) parameter
            }
            else
            {
                localVarPostBody = callback; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommercePostProcessPayment: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommercePostProcessPayment: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainPaymentModelPostProcessPaymentResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainPaymentModelPostProcessPaymentResult)));
            
        }

        /// <summary>
        /// Update a existing currency 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        public void CommerceUpdateCurrency (VirtoCommerceDomainCommerceModelCurrency currency)
        {
             CommerceUpdateCurrencyWithHttpInfo(currency);
        }

        /// <summary>
        /// Update a existing currency 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CommerceUpdateCurrencyWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency)
        {
            // verify the required parameter 'currency' is set
            if (currency == null)
                throw new ApiException(400, "Missing required parameter 'currency' when calling CommerceCoreModuleApi->CommerceUpdateCurrency");

            var localVarPath = "/api/currencies";
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
            if (currency.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(currency); // http body (model) parameter
            }
            else
            {
                localVarPostBody = currency; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceUpdateCurrency: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceUpdateCurrency: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing currency 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CommerceUpdateCurrencyAsync (VirtoCommerceDomainCommerceModelCurrency currency)
        {
             await CommerceUpdateCurrencyAsyncWithHttpInfo(currency);

        }

        /// <summary>
        /// Update a existing currency 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currency">currency</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CommerceUpdateCurrencyAsyncWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency)
        {
            // verify the required parameter 'currency' is set
            if (currency == null)
                throw new ApiException(400, "Missing required parameter 'currency' when calling CommerceCoreModuleApi->CommerceUpdateCurrency");

            var localVarPath = "/api/currencies";
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
            if (currency.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(currency); // http body (model) parameter
            }
            else
            {
                localVarPostBody = currency; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceUpdateCurrency: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceUpdateCurrency: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing fulfillment center 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="center">fulfillment center</param>
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public VirtoCommerceCoreModuleWebModelFulfillmentCenter CommerceUpdateFulfillmentCenter (VirtoCommerceCoreModuleWebModelFulfillmentCenter center)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> localVarResponse = CommerceUpdateFulfillmentCenterWithHttpInfo(center);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update a existing fulfillment center 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="center">fulfillment center</param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public ApiResponse< VirtoCommerceCoreModuleWebModelFulfillmentCenter > CommerceUpdateFulfillmentCenterWithHttpInfo (VirtoCommerceCoreModuleWebModelFulfillmentCenter center)
        {
            // verify the required parameter 'center' is set
            if (center == null)
                throw new ApiException(400, "Missing required parameter 'center' when calling CommerceCoreModuleApi->CommerceUpdateFulfillmentCenter");

            var localVarPath = "/api/fulfillment/centers";
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
            if (center.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(center); // http body (model) parameter
            }
            else
            {
                localVarPostBody = center; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceUpdateFulfillmentCenter: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceUpdateFulfillmentCenter: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelFulfillmentCenter) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter)));
            
        }

        /// <summary>
        /// Update a existing fulfillment center 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="center">fulfillment center</param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceUpdateFulfillmentCenterAsync (VirtoCommerceCoreModuleWebModelFulfillmentCenter center)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> localVarResponse = await CommerceUpdateFulfillmentCenterAsyncWithHttpInfo(center);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update a existing fulfillment center 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="center">fulfillment center</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelFulfillmentCenter)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceUpdateFulfillmentCenterAsyncWithHttpInfo (VirtoCommerceCoreModuleWebModelFulfillmentCenter center)
        {
            // verify the required parameter 'center' is set
            if (center == null)
                throw new ApiException(400, "Missing required parameter 'center' when calling CommerceCoreModuleApi->CommerceUpdateFulfillmentCenter");

            var localVarPath = "/api/fulfillment/centers";
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
            if (center.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(center); // http body (model) parameter
            }
            else
            {
                localVarPostBody = center; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CommerceUpdateFulfillmentCenter: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CommerceUpdateFulfillmentCenter: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelFulfillmentCenter) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter)));
            
        }

        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult StorefrontSecurityCreate (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = StorefrontSecurityCreateWithHttpInfo(user);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecuritySecurityResult > StorefrontSecurityCreateWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling CommerceCoreModuleApi->StorefrontSecurityCreate");

            var localVarPath = "/api/storefront/security/user";
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
            if (user.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(user); // http body (model) parameter
            }
            else
            {
                localVarPostBody = user; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = await StorefrontSecurityCreateAsyncWithHttpInfo(user);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> StorefrontSecurityCreateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling CommerceCoreModuleApi->StorefrontSecurityCreate");

            var localVarPath = "/api/storefront/security/user";
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
            if (user.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(user); // http body (model) parameter
            }
            else
            {
                localVarPostBody = user; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Generate a password reset token Generates a password reset token and sends a password reset link to the user via email.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        public void StorefrontSecurityGenerateResetPasswordToken (string userId, string storeName, string language, string callbackUrl)
        {
             StorefrontSecurityGenerateResetPasswordTokenWithHttpInfo(userId, storeName, language, callbackUrl);
        }

        /// <summary>
        /// Generate a password reset token Generates a password reset token and sends a password reset link to the user via email.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> StorefrontSecurityGenerateResetPasswordTokenWithHttpInfo (string userId, string storeName, string language, string callbackUrl)
        {
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling CommerceCoreModuleApi->StorefrontSecurityGenerateResetPasswordToken");
            // verify the required parameter 'storeName' is set
            if (storeName == null)
                throw new ApiException(400, "Missing required parameter 'storeName' when calling CommerceCoreModuleApi->StorefrontSecurityGenerateResetPasswordToken");
            // verify the required parameter 'language' is set
            if (language == null)
                throw new ApiException(400, "Missing required parameter 'language' when calling CommerceCoreModuleApi->StorefrontSecurityGenerateResetPasswordToken");
            // verify the required parameter 'callbackUrl' is set
            if (callbackUrl == null)
                throw new ApiException(400, "Missing required parameter 'callbackUrl' when calling CommerceCoreModuleApi->StorefrontSecurityGenerateResetPasswordToken");

            var localVarPath = "/api/storefront/security/user/password/resettoken";
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
            if (userId != null) localVarQueryParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // query parameter
            if (storeName != null) localVarQueryParams.Add("storeName", Configuration.ApiClient.ParameterToString(storeName)); // query parameter
            if (language != null) localVarQueryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            if (callbackUrl != null) localVarQueryParams.Add("callbackUrl", Configuration.ApiClient.ParameterToString(callbackUrl)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Generate a password reset token Generates a password reset token and sends a password reset link to the user via email.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task StorefrontSecurityGenerateResetPasswordTokenAsync (string userId, string storeName, string language, string callbackUrl)
        {
             await StorefrontSecurityGenerateResetPasswordTokenAsyncWithHttpInfo(userId, storeName, language, callbackUrl);

        }

        /// <summary>
        /// Generate a password reset token Generates a password reset token and sends a password reset link to the user via email.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> StorefrontSecurityGenerateResetPasswordTokenAsyncWithHttpInfo (string userId, string storeName, string language, string callbackUrl)
        {
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling CommerceCoreModuleApi->StorefrontSecurityGenerateResetPasswordToken");
            // verify the required parameter 'storeName' is set
            if (storeName == null)
                throw new ApiException(400, "Missing required parameter 'storeName' when calling CommerceCoreModuleApi->StorefrontSecurityGenerateResetPasswordToken");
            // verify the required parameter 'language' is set
            if (language == null)
                throw new ApiException(400, "Missing required parameter 'language' when calling CommerceCoreModuleApi->StorefrontSecurityGenerateResetPasswordToken");
            // verify the required parameter 'callbackUrl' is set
            if (callbackUrl == null)
                throw new ApiException(400, "Missing required parameter 'callbackUrl' when calling CommerceCoreModuleApi->StorefrontSecurityGenerateResetPasswordToken");

            var localVarPath = "/api/storefront/security/user/password/resettoken";
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
            if (userId != null) localVarQueryParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // query parameter
            if (storeName != null) localVarQueryParams.Add("storeName", Configuration.ApiClient.ParameterToString(storeName)); // query parameter
            if (language != null) localVarQueryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            if (callbackUrl != null) localVarQueryParams.Add("callbackUrl", Configuration.ApiClient.ParameterToString(callbackUrl)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        public VirtoCommerceCoreModuleWebModelStorefrontUser StorefrontSecurityGetUserById (string userId)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser> localVarResponse = StorefrontSecurityGetUserByIdWithHttpInfo(userId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        public ApiResponse< VirtoCommerceCoreModuleWebModelStorefrontUser > StorefrontSecurityGetUserByIdWithHttpInfo (string userId)
        {
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserById");

            var localVarPath = "/api/storefront/security/user/id/{userId}";
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
            if (userId != null) localVarPathParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelStorefrontUser) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelStorefrontUser)));
            
        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelStorefrontUser> StorefrontSecurityGetUserByIdAsync (string userId)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser> localVarResponse = await StorefrontSecurityGetUserByIdAsyncWithHttpInfo(userId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelStorefrontUser)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>> StorefrontSecurityGetUserByIdAsyncWithHttpInfo (string userId)
        {
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserById");

            var localVarPath = "/api/storefront/security/user/id/{userId}";
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
            if (userId != null) localVarPathParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelStorefrontUser) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelStorefrontUser)));
            
        }

        /// <summary>
        /// Get user details by external login provider 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        public VirtoCommerceCoreModuleWebModelStorefrontUser StorefrontSecurityGetUserByLogin (string loginProvider, string providerKey)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser> localVarResponse = StorefrontSecurityGetUserByLoginWithHttpInfo(loginProvider, providerKey);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get user details by external login provider 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        public ApiResponse< VirtoCommerceCoreModuleWebModelStorefrontUser > StorefrontSecurityGetUserByLoginWithHttpInfo (string loginProvider, string providerKey)
        {
            // verify the required parameter 'loginProvider' is set
            if (loginProvider == null)
                throw new ApiException(400, "Missing required parameter 'loginProvider' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserByLogin");
            // verify the required parameter 'providerKey' is set
            if (providerKey == null)
                throw new ApiException(400, "Missing required parameter 'providerKey' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserByLogin");

            var localVarPath = "/api/storefront/security/user/external";
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
            if (loginProvider != null) localVarQueryParams.Add("loginProvider", Configuration.ApiClient.ParameterToString(loginProvider)); // query parameter
            if (providerKey != null) localVarQueryParams.Add("providerKey", Configuration.ApiClient.ParameterToString(providerKey)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserByLogin: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserByLogin: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelStorefrontUser) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelStorefrontUser)));
            
        }

        /// <summary>
        /// Get user details by external login provider 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelStorefrontUser> StorefrontSecurityGetUserByLoginAsync (string loginProvider, string providerKey)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser> localVarResponse = await StorefrontSecurityGetUserByLoginAsyncWithHttpInfo(loginProvider, providerKey);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get user details by external login provider 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelStorefrontUser)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>> StorefrontSecurityGetUserByLoginAsyncWithHttpInfo (string loginProvider, string providerKey)
        {
            // verify the required parameter 'loginProvider' is set
            if (loginProvider == null)
                throw new ApiException(400, "Missing required parameter 'loginProvider' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserByLogin");
            // verify the required parameter 'providerKey' is set
            if (providerKey == null)
                throw new ApiException(400, "Missing required parameter 'providerKey' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserByLogin");

            var localVarPath = "/api/storefront/security/user/external";
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
            if (loginProvider != null) localVarQueryParams.Add("loginProvider", Configuration.ApiClient.ParameterToString(loginProvider)); // query parameter
            if (providerKey != null) localVarQueryParams.Add("providerKey", Configuration.ApiClient.ParameterToString(providerKey)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserByLogin: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserByLogin: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelStorefrontUser) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelStorefrontUser)));
            
        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        public VirtoCommerceCoreModuleWebModelStorefrontUser StorefrontSecurityGetUserByName (string userName)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser> localVarResponse = StorefrontSecurityGetUserByNameWithHttpInfo(userName);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        public ApiResponse< VirtoCommerceCoreModuleWebModelStorefrontUser > StorefrontSecurityGetUserByNameWithHttpInfo (string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserByName");

            var localVarPath = "/api/storefront/security/user/name/{userName}";
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
            if (userName != null) localVarPathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserByName: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserByName: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelStorefrontUser) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelStorefrontUser)));
            
        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelStorefrontUser</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelStorefrontUser> StorefrontSecurityGetUserByNameAsync (string userName)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser> localVarResponse = await StorefrontSecurityGetUserByNameAsyncWithHttpInfo(userName);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelStorefrontUser)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>> StorefrontSecurityGetUserByNameAsyncWithHttpInfo (string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserByName");

            var localVarPath = "/api/storefront/security/user/name/{userName}";
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
            if (userName != null) localVarPathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserByName: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityGetUserByName: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelStorefrontUser>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelStorefrontUser) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelStorefrontUser)));
            
        }

        /// <summary>
        /// Sign in with user name and password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>VirtoCommerceCoreModuleWebModelSignInResult</returns>
        public VirtoCommerceCoreModuleWebModelSignInResult StorefrontSecurityPasswordSignIn (string userName, string password)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult> localVarResponse = StorefrontSecurityPasswordSignInWithHttpInfo(userName, password);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Sign in with user name and password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelSignInResult</returns>
        public ApiResponse< VirtoCommerceCoreModuleWebModelSignInResult > StorefrontSecurityPasswordSignInWithHttpInfo (string userName, string password)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling CommerceCoreModuleApi->StorefrontSecurityPasswordSignIn");
            // verify the required parameter 'password' is set
            if (password == null)
                throw new ApiException(400, "Missing required parameter 'password' when calling CommerceCoreModuleApi->StorefrontSecurityPasswordSignIn");

            var localVarPath = "/api/storefront/security/user/signin";
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
            if (userName != null) localVarQueryParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // query parameter
            if (password != null) localVarQueryParams.Add("password", Configuration.ApiClient.ParameterToString(password)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityPasswordSignIn: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityPasswordSignIn: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelSignInResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelSignInResult)));
            
        }

        /// <summary>
        /// Sign in with user name and password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelSignInResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelSignInResult> StorefrontSecurityPasswordSignInAsync (string userName, string password)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult> localVarResponse = await StorefrontSecurityPasswordSignInAsyncWithHttpInfo(userName, password);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Sign in with user name and password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelSignInResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult>> StorefrontSecurityPasswordSignInAsyncWithHttpInfo (string userName, string password)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling CommerceCoreModuleApi->StorefrontSecurityPasswordSignIn");
            // verify the required parameter 'password' is set
            if (password == null)
                throw new ApiException(400, "Missing required parameter 'password' when calling CommerceCoreModuleApi->StorefrontSecurityPasswordSignIn");

            var localVarPath = "/api/storefront/security/user/signin";
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
            if (userName != null) localVarQueryParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // query parameter
            if (password != null) localVarQueryParams.Add("password", Configuration.ApiClient.ParameterToString(password)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityPasswordSignIn: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityPasswordSignIn: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelSignInResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCoreModuleWebModelSignInResult)));
            
        }

        /// <summary>
        /// Reset a password for the user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult StorefrontSecurityResetPassword (string userId, string token, string newPassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = StorefrontSecurityResetPasswordWithHttpInfo(userId, token, newPassword);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Reset a password for the user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecuritySecurityResult > StorefrontSecurityResetPasswordWithHttpInfo (string userId, string token, string newPassword)
        {
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling CommerceCoreModuleApi->StorefrontSecurityResetPassword");
            // verify the required parameter 'token' is set
            if (token == null)
                throw new ApiException(400, "Missing required parameter 'token' when calling CommerceCoreModuleApi->StorefrontSecurityResetPassword");
            // verify the required parameter 'newPassword' is set
            if (newPassword == null)
                throw new ApiException(400, "Missing required parameter 'newPassword' when calling CommerceCoreModuleApi->StorefrontSecurityResetPassword");

            var localVarPath = "/api/storefront/security/user/password/reset";
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
            if (userId != null) localVarQueryParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // query parameter
            if (token != null) localVarQueryParams.Add("token", Configuration.ApiClient.ParameterToString(token)); // query parameter
            if (newPassword != null) localVarQueryParams.Add("newPassword", Configuration.ApiClient.ParameterToString(newPassword)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityResetPassword: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityResetPassword: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Reset a password for the user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityResetPasswordAsync (string userId, string token, string newPassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = await StorefrontSecurityResetPasswordAsyncWithHttpInfo(userId, token, newPassword);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Reset a password for the user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> StorefrontSecurityResetPasswordAsyncWithHttpInfo (string userId, string token, string newPassword)
        {
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling CommerceCoreModuleApi->StorefrontSecurityResetPassword");
            // verify the required parameter 'token' is set
            if (token == null)
                throw new ApiException(400, "Missing required parameter 'token' when calling CommerceCoreModuleApi->StorefrontSecurityResetPassword");
            // verify the required parameter 'newPassword' is set
            if (newPassword == null)
                throw new ApiException(400, "Missing required parameter 'newPassword' when calling CommerceCoreModuleApi->StorefrontSecurityResetPassword");

            var localVarPath = "/api/storefront/security/user/password/reset";
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
            if (userId != null) localVarQueryParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // query parameter
            if (token != null) localVarQueryParams.Add("token", Configuration.ApiClient.ParameterToString(token)); // query parameter
            if (newPassword != null) localVarQueryParams.Add("newPassword", Configuration.ApiClient.ParameterToString(newPassword)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityResetPassword: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StorefrontSecurityResetPassword: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

    }
}
