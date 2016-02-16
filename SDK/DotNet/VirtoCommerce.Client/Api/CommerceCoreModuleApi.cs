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
        
        /// <summary>
        /// Return all currencies registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        List<VirtoCommerceDomainCommerceModelCurrency> CommerceGetAllCurrencies ();
  
        /// <summary>
        /// Return all currencies registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>> CommerceGetAllCurrenciesWithHttpInfo ();

        /// <summary>
        /// Return all currencies registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelCurrency>> CommerceGetAllCurrenciesAsync ();

        /// <summary>
        /// Return all currencies registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>>> CommerceGetAllCurrenciesAsyncWithHttpInfo ();
        
        /// <summary>
        /// Update a existing currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        void CommerceUpdateCurrency (VirtoCommerceDomainCommerceModelCurrency currency);
  
        /// <summary>
        /// Update a existing currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="currency">currency</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CommerceUpdateCurrencyWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency);

        /// <summary>
        /// Update a existing currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="currency">currency</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CommerceUpdateCurrencyAsync (VirtoCommerceDomainCommerceModelCurrency currency);

        /// <summary>
        /// Update a existing currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="currency">currency</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CommerceUpdateCurrencyAsyncWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency);
        
        /// <summary>
        /// Create new currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        void CommerceCreateCurrency (VirtoCommerceDomainCommerceModelCurrency currency);
  
        /// <summary>
        /// Create new currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="currency">currency</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CommerceCreateCurrencyWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency);

        /// <summary>
        /// Create new currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="currency">currency</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CommerceCreateCurrencyAsync (VirtoCommerceDomainCommerceModelCurrency currency);

        /// <summary>
        /// Create new currency
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="currency">currency</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CommerceCreateCurrencyAsyncWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency);
        
        /// <summary>
        /// Delete currencies
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="codes">currency codes</param>
        /// <returns></returns>
        void CommerceDeleteCurrencies (List<string> codes);
  
        /// <summary>
        /// Delete currencies
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="codes">currency codes</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CommerceDeleteCurrenciesWithHttpInfo (List<string> codes);

        /// <summary>
        /// Delete currencies
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="codes">currency codes</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CommerceDeleteCurrenciesAsync (List<string> codes);

        /// <summary>
        /// Delete currencies
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="codes">currency codes</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CommerceDeleteCurrenciesAsyncWithHttpInfo (List<string> codes);
        
        /// <summary>
        /// Return all fulfillment centers registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        List<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenters ();
  
        /// <summary>
        /// Return all fulfillment centers registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCentersWithHttpInfo ();

        /// <summary>
        /// Return all fulfillment centers registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCentersAsync ();

        /// <summary>
        /// Return all fulfillment centers registered in the system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>>> CommerceGetFulfillmentCentersAsyncWithHttpInfo ();
        
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
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceUpdateFulfillmentCenterWithHttpInfo (VirtoCommerceCoreModuleWebModelFulfillmentCenter center);

        /// <summary>
        /// Update a existing fulfillment center
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="center">fulfillment center</param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceUpdateFulfillmentCenterAsync (VirtoCommerceCoreModuleWebModelFulfillmentCenter center);

        /// <summary>
        /// Update a existing fulfillment center
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="center">fulfillment center</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelFulfillmentCenter)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceUpdateFulfillmentCenterAsyncWithHttpInfo (VirtoCommerceCoreModuleWebModelFulfillmentCenter center);
        
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
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenterWithHttpInfo (string id);

        /// <summary>
        /// Find fulfillment center by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">fulfillment center id</param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenterAsync (string id);

        /// <summary>
        /// Find fulfillment center by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">fulfillment center id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelFulfillmentCenter)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCenterAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        VirtoCommerceDomainPaymentModelPostProcessPaymentResult CommercePostProcessPayment (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback);
  
        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>ApiResponse of VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> CommercePostProcessPaymentWithHttpInfo (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback);

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>Task of VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> CommercePostProcessPaymentAsync (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback);

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainPaymentModelPostProcessPaymentResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult>> CommercePostProcessPaymentAsyncWithHttpInfo (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback);
        
        /// <summary>
        /// Find all SEO records for object by slug
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="slug">slug</param>
        /// <returns>List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        List<VirtoCommerceDomainCommerceModelSeoInfo> CommerceGetSeoInfoBySlug (string slug);
  
        /// <summary>
        /// Find all SEO records for object by slug
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="slug">slug</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoInfoBySlugWithHttpInfo (string slug);

        /// <summary>
        /// Find all SEO records for object by slug
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="slug">slug</param>
        /// <returns>Task of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoInfoBySlugAsync (string slug);

        /// <summary>
        /// Find all SEO records for object by slug
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="slug">slug</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>> CommerceGetSeoInfoBySlugAsyncWithHttpInfo (string slug);
        
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
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityCreateWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> StorefrontSecurityCreateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        
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
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByLoginWithHttpInfo (string loginProvider, string providerKey);

        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByLoginAsync (string loginProvider, string providerKey);

        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> StorefrontSecurityGetUserByLoginAsyncWithHttpInfo (string loginProvider, string providerKey);
        
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
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByIdWithHttpInfo (string userId);

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByIdAsync (string userId);

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> StorefrontSecurityGetUserByIdAsyncWithHttpInfo (string userId);
        
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
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByNameWithHttpInfo (string userName);

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByNameAsync (string userName);

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> StorefrontSecurityGetUserByNameAsyncWithHttpInfo (string userName);
        
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
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityResetPasswordWithHttpInfo (string userId, string token, string newPassword);

        /// <summary>
        /// Reset a password for the user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> StorefrontSecurityResetPasswordAsyncWithHttpInfo (string userId, string token, string newPassword);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> StorefrontSecurityGenerateResetPasswordTokenWithHttpInfo (string userId, string storeName, string language, string callbackUrl);

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
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task StorefrontSecurityGenerateResetPasswordTokenAsync (string userId, string storeName, string language, string callbackUrl);

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
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> StorefrontSecurityGenerateResetPasswordTokenAsyncWithHttpInfo (string userId, string storeName, string language, string callbackUrl);
        
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
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelSignInResult</returns>
        ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult> StorefrontSecurityPasswordSignInWithHttpInfo (string userName, string password);

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelSignInResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult>> StorefrontSecurityPasswordSignInAsyncWithHttpInfo (string userName, string password);
        
        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;</returns>
        ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>> CommerceEvaluateTaxesWithHttpInfo (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext);

        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>>> CommerceEvaluateTaxesAsyncWithHttpInfo (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext);
        
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
        /// Return all currencies registered in the system 
        /// </summary>
        /// <returns>List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        public List<VirtoCommerceDomainCommerceModelCurrency> CommerceGetAllCurrencies ()
        {
             ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>> response = CommerceGetAllCurrenciesWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Return all currencies registered in the system 
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        public ApiResponse< List<VirtoCommerceDomainCommerceModelCurrency> > CommerceGetAllCurrenciesWithHttpInfo ()
        {
            
    
            var path_ = "/api/currencies";
    
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
                throw new ApiException (statusCode, "Error calling CommerceGetAllCurrencies: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceGetAllCurrencies: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCommerceModelCurrency>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceDomainCommerceModelCurrency>)));
            
        }
    
        /// <summary>
        /// Return all currencies registered in the system 
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelCurrency>> CommerceGetAllCurrenciesAsync ()
        {
             ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>> response = await CommerceGetAllCurrenciesAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Return all currencies registered in the system 
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCommerceModelCurrency&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>>> CommerceGetAllCurrenciesAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/currencies";
    
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
                throw new ApiException (statusCode, "Error calling CommerceGetAllCurrencies: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceGetAllCurrencies: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCommerceModelCurrency>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCommerceModelCurrency>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceDomainCommerceModelCurrency>)));
            
        }
        
        /// <summary>
        /// Update a existing currency 
        /// </summary>
        /// <param name="currency">currency</param> 
        /// <returns></returns>
        public void CommerceUpdateCurrency (VirtoCommerceDomainCommerceModelCurrency currency)
        {
             CommerceUpdateCurrencyWithHttpInfo(currency);
        }

        /// <summary>
        /// Update a existing currency 
        /// </summary>
        /// <param name="currency">currency</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CommerceUpdateCurrencyWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency)
        {
            
            // verify the required parameter 'currency' is set
            if (currency == null)
                throw new ApiException(400, "Missing required parameter 'currency' when calling CommerceCoreModuleApi->CommerceUpdateCurrency");
            
    
            var path_ = "/api/currencies";
    
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
            
            
            
            
            if (currency.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(currency); // http body (model) parameter
            }
            else
            {
                postBody = currency; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceUpdateCurrency: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceUpdateCurrency: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update a existing currency 
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CommerceUpdateCurrencyAsync (VirtoCommerceDomainCommerceModelCurrency currency)
        {
             await CommerceUpdateCurrencyAsyncWithHttpInfo(currency);

        }

        /// <summary>
        /// Update a existing currency 
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CommerceUpdateCurrencyAsyncWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency)
        {
            // verify the required parameter 'currency' is set
            if (currency == null) throw new ApiException(400, "Missing required parameter 'currency' when calling CommerceUpdateCurrency");
            
    
            var path_ = "/api/currencies";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(currency); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceUpdateCurrency: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceUpdateCurrency: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create new currency 
        /// </summary>
        /// <param name="currency">currency</param> 
        /// <returns></returns>
        public void CommerceCreateCurrency (VirtoCommerceDomainCommerceModelCurrency currency)
        {
             CommerceCreateCurrencyWithHttpInfo(currency);
        }

        /// <summary>
        /// Create new currency 
        /// </summary>
        /// <param name="currency">currency</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CommerceCreateCurrencyWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency)
        {
            
            // verify the required parameter 'currency' is set
            if (currency == null)
                throw new ApiException(400, "Missing required parameter 'currency' when calling CommerceCoreModuleApi->CommerceCreateCurrency");
            
    
            var path_ = "/api/currencies";
    
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
            
            
            
            
            if (currency.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(currency); // http body (model) parameter
            }
            else
            {
                postBody = currency; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceCreateCurrency: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceCreateCurrency: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Create new currency 
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CommerceCreateCurrencyAsync (VirtoCommerceDomainCommerceModelCurrency currency)
        {
             await CommerceCreateCurrencyAsyncWithHttpInfo(currency);

        }

        /// <summary>
        /// Create new currency 
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CommerceCreateCurrencyAsyncWithHttpInfo (VirtoCommerceDomainCommerceModelCurrency currency)
        {
            // verify the required parameter 'currency' is set
            if (currency == null) throw new ApiException(400, "Missing required parameter 'currency' when calling CommerceCreateCurrency");
            
    
            var path_ = "/api/currencies";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(currency); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceCreateCurrency: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceCreateCurrency: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Delete currencies 
        /// </summary>
        /// <param name="codes">currency codes</param> 
        /// <returns></returns>
        public void CommerceDeleteCurrencies (List<string> codes)
        {
             CommerceDeleteCurrenciesWithHttpInfo(codes);
        }

        /// <summary>
        /// Delete currencies 
        /// </summary>
        /// <param name="codes">currency codes</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CommerceDeleteCurrenciesWithHttpInfo (List<string> codes)
        {
            
            // verify the required parameter 'codes' is set
            if (codes == null)
                throw new ApiException(400, "Missing required parameter 'codes' when calling CommerceCoreModuleApi->CommerceDeleteCurrencies");
            
    
            var path_ = "/api/currencies";
    
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
            
            if (codes != null) queryParams.Add("codes", Configuration.ApiClient.ParameterToString(codes)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceDeleteCurrencies: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceDeleteCurrencies: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete currencies 
        /// </summary>
        /// <param name="codes">currency codes</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CommerceDeleteCurrenciesAsync (List<string> codes)
        {
             await CommerceDeleteCurrenciesAsyncWithHttpInfo(codes);

        }

        /// <summary>
        /// Delete currencies 
        /// </summary>
        /// <param name="codes">currency codes</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CommerceDeleteCurrenciesAsyncWithHttpInfo (List<string> codes)
        {
            // verify the required parameter 'codes' is set
            if (codes == null) throw new ApiException(400, "Missing required parameter 'codes' when calling CommerceDeleteCurrencies");
            
    
            var path_ = "/api/currencies";
    
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
            
            if (codes != null) queryParams.Add("codes", Configuration.ApiClient.ParameterToString(codes)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceDeleteCurrencies: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceDeleteCurrencies: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Return all fulfillment centers registered in the system 
        /// </summary>
        /// <returns>List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        public List<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenters ()
        {
             ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> response = CommerceGetFulfillmentCentersWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Return all fulfillment centers registered in the system 
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        public ApiResponse< List<VirtoCommerceCoreModuleWebModelFulfillmentCenter> > CommerceGetFulfillmentCentersWithHttpInfo ()
        {
            
    
            var path_ = "/api/fulfillment/centers";
    
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
                throw new ApiException (statusCode, "Error calling CommerceGetFulfillmentCenters: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceGetFulfillmentCenters: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>)));
            
        }
    
        /// <summary>
        /// Return all fulfillment centers registered in the system 
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCentersAsync ()
        {
             ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> response = await CommerceGetFulfillmentCentersAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Return all fulfillment centers registered in the system 
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCoreModuleWebModelFulfillmentCenter&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>>> CommerceGetFulfillmentCentersAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/fulfillment/centers";
    
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
                throw new ApiException (statusCode, "Error calling CommerceGetFulfillmentCenters: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceGetFulfillmentCenters: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCoreModuleWebModelFulfillmentCenter>)));
            
        }
        
        /// <summary>
        /// Update a existing fulfillment center 
        /// </summary>
        /// <param name="center">fulfillment center</param> 
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public VirtoCommerceCoreModuleWebModelFulfillmentCenter CommerceUpdateFulfillmentCenter (VirtoCommerceCoreModuleWebModelFulfillmentCenter center)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> response = CommerceUpdateFulfillmentCenterWithHttpInfo(center);
             return response.Data;
        }

        /// <summary>
        /// Update a existing fulfillment center 
        /// </summary>
        /// <param name="center">fulfillment center</param> 
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public ApiResponse< VirtoCommerceCoreModuleWebModelFulfillmentCenter > CommerceUpdateFulfillmentCenterWithHttpInfo (VirtoCommerceCoreModuleWebModelFulfillmentCenter center)
        {
            
            // verify the required parameter 'center' is set
            if (center == null)
                throw new ApiException(400, "Missing required parameter 'center' when calling CommerceCoreModuleApi->CommerceUpdateFulfillmentCenter");
            
    
            var path_ = "/api/fulfillment/centers";
    
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
            
            
            
            
            if (center.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(center); // http body (model) parameter
            }
            else
            {
                postBody = center; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceUpdateFulfillmentCenter: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceUpdateFulfillmentCenter: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelFulfillmentCenter) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter)));
            
        }
    
        /// <summary>
        /// Update a existing fulfillment center 
        /// </summary>
        /// <param name="center">fulfillment center</param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceUpdateFulfillmentCenterAsync (VirtoCommerceCoreModuleWebModelFulfillmentCenter center)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> response = await CommerceUpdateFulfillmentCenterAsyncWithHttpInfo(center);
             return response.Data;

        }

        /// <summary>
        /// Update a existing fulfillment center 
        /// </summary>
        /// <param name="center">fulfillment center</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelFulfillmentCenter)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceUpdateFulfillmentCenterAsyncWithHttpInfo (VirtoCommerceCoreModuleWebModelFulfillmentCenter center)
        {
            // verify the required parameter 'center' is set
            if (center == null) throw new ApiException(400, "Missing required parameter 'center' when calling CommerceUpdateFulfillmentCenter");
            
    
            var path_ = "/api/fulfillment/centers";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(center); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceUpdateFulfillmentCenter: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceUpdateFulfillmentCenter: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelFulfillmentCenter) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter)));
            
        }
        
        /// <summary>
        /// Find fulfillment center by id 
        /// </summary>
        /// <param name="id">fulfillment center id</param> 
        /// <returns>VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public VirtoCommerceCoreModuleWebModelFulfillmentCenter CommerceGetFulfillmentCenter (string id)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> response = CommerceGetFulfillmentCenterWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Find fulfillment center by id 
        /// </summary>
        /// <param name="id">fulfillment center id</param> 
        /// <returns>ApiResponse of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public ApiResponse< VirtoCommerceCoreModuleWebModelFulfillmentCenter > CommerceGetFulfillmentCenterWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CommerceCoreModuleApi->CommerceGetFulfillmentCenter");
            
    
            var path_ = "/api/fulfillment/centers/{id}";
    
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
                throw new ApiException (statusCode, "Error calling CommerceGetFulfillmentCenter: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceGetFulfillmentCenter: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelFulfillmentCenter) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter)));
            
        }
    
        /// <summary>
        /// Find fulfillment center by id 
        /// </summary>
        /// <param name="id">fulfillment center id</param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelFulfillmentCenter</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelFulfillmentCenter> CommerceGetFulfillmentCenterAsync (string id)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter> response = await CommerceGetFulfillmentCenterAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Find fulfillment center by id 
        /// </summary>
        /// <param name="id">fulfillment center id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelFulfillmentCenter)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>> CommerceGetFulfillmentCenterAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CommerceGetFulfillmentCenter");
            
    
            var path_ = "/api/fulfillment/centers/{id}";
    
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
                throw new ApiException (statusCode, "Error calling CommerceGetFulfillmentCenter: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceGetFulfillmentCenter: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelFulfillmentCenter>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelFulfillmentCenter) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelFulfillmentCenter)));
            
        }
        
        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system 
        /// </summary>
        /// <param name="callback">payment callback parameters</param> 
        /// <returns>VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        public VirtoCommerceDomainPaymentModelPostProcessPaymentResult CommercePostProcessPayment (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback)
        {
             ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> response = CommercePostProcessPaymentWithHttpInfo(callback);
             return response.Data;
        }

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system 
        /// </summary>
        /// <param name="callback">payment callback parameters</param> 
        /// <returns>ApiResponse of VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        public ApiResponse< VirtoCommerceDomainPaymentModelPostProcessPaymentResult > CommercePostProcessPaymentWithHttpInfo (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback)
        {
            
            // verify the required parameter 'callback' is set
            if (callback == null)
                throw new ApiException(400, "Missing required parameter 'callback' when calling CommerceCoreModuleApi->CommercePostProcessPayment");
            
    
            var path_ = "/api/paymentcallback";
    
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
            
            
            
            
            if (callback.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(callback); // http body (model) parameter
            }
            else
            {
                postBody = callback; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommercePostProcessPayment: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommercePostProcessPayment: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainPaymentModelPostProcessPaymentResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainPaymentModelPostProcessPaymentResult)));
            
        }
    
        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system 
        /// </summary>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>Task of VirtoCommerceDomainPaymentModelPostProcessPaymentResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> CommercePostProcessPaymentAsync (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback)
        {
             ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult> response = await CommercePostProcessPaymentAsyncWithHttpInfo(callback);
             return response.Data;

        }

        /// <summary>
        /// Payment callback operation used by external payment services to inform post process payment in our system 
        /// </summary>
        /// <param name="callback">payment callback parameters</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainPaymentModelPostProcessPaymentResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult>> CommercePostProcessPaymentAsyncWithHttpInfo (VirtoCommerceCoreModuleWebModelPaymentCallbackParameters callback)
        {
            // verify the required parameter 'callback' is set
            if (callback == null) throw new ApiException(400, "Missing required parameter 'callback' when calling CommercePostProcessPayment");
            
    
            var path_ = "/api/paymentcallback";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(callback); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommercePostProcessPayment: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommercePostProcessPayment: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainPaymentModelPostProcessPaymentResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainPaymentModelPostProcessPaymentResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainPaymentModelPostProcessPaymentResult)));
            
        }
        
        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <param name="slug">slug</param> 
        /// <returns>List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        public List<VirtoCommerceDomainCommerceModelSeoInfo> CommerceGetSeoInfoBySlug (string slug)
        {
             ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>> response = CommerceGetSeoInfoBySlugWithHttpInfo(slug);
             return response.Data;
        }

        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <param name="slug">slug</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        public ApiResponse< List<VirtoCommerceDomainCommerceModelSeoInfo> > CommerceGetSeoInfoBySlugWithHttpInfo (string slug)
        {
            
            // verify the required parameter 'slug' is set
            if (slug == null)
                throw new ApiException(400, "Missing required parameter 'slug' when calling CommerceCoreModuleApi->CommerceGetSeoInfoBySlug");
            
    
            var path_ = "/api/seoinfos/{slug}";
    
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
            if (slug != null) pathParams.Add("slug", Configuration.ApiClient.ParameterToString(slug)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceGetSeoInfoBySlug: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceGetSeoInfoBySlug: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCommerceModelSeoInfo>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceDomainCommerceModelSeoInfo>)));
            
        }
    
        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <param name="slug">slug</param>
        /// <returns>Task of List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceDomainCommerceModelSeoInfo>> CommerceGetSeoInfoBySlugAsync (string slug)
        {
             ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>> response = await CommerceGetSeoInfoBySlugAsyncWithHttpInfo(slug);
             return response.Data;

        }

        /// <summary>
        /// Find all SEO records for object by slug 
        /// </summary>
        /// <param name="slug">slug</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCommerceModelSeoInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>> CommerceGetSeoInfoBySlugAsyncWithHttpInfo (string slug)
        {
            // verify the required parameter 'slug' is set
            if (slug == null) throw new ApiException(400, "Missing required parameter 'slug' when calling CommerceGetSeoInfoBySlug");
            
    
            var path_ = "/api/seoinfos/{slug}";
    
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
            if (slug != null) pathParams.Add("slug", Configuration.ApiClient.ParameterToString(slug)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceGetSeoInfoBySlug: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceGetSeoInfoBySlug: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCommerceModelSeoInfo>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCommerceModelSeoInfo>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceDomainCommerceModelSeoInfo>)));
            
        }
        
        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <param name="user"></param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult StorefrontSecurityCreate (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = StorefrontSecurityCreateWithHttpInfo(user);
             return response.Data;
        }

        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <param name="user"></param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecuritySecurityResult > StorefrontSecurityCreateWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling CommerceCoreModuleApi->StorefrontSecurityCreate");
            
    
            var path_ = "/api/storefront/security/user";
    
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
            
            
            
            
            if (user.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(user); // http body (model) parameter
            }
            else
            {
                postBody = user; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityCreate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
    
        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = await StorefrontSecurityCreateAsyncWithHttpInfo(user);
             return response.Data;

        }

        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> StorefrontSecurityCreateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling StorefrontSecurityCreate");
            
    
            var path_ = "/api/storefront/security/user";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(user); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityCreate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityCreate: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
        
        /// <summary>
        /// Get user details by external login provider 
        /// </summary>
        /// <param name="loginProvider"></param> 
        /// <param name="providerKey"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended StorefrontSecurityGetUserByLogin (string loginProvider, string providerKey)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = StorefrontSecurityGetUserByLoginWithHttpInfo(loginProvider, providerKey);
             return response.Data;
        }

        /// <summary>
        /// Get user details by external login provider 
        /// </summary>
        /// <param name="loginProvider"></param> 
        /// <param name="providerKey"></param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > StorefrontSecurityGetUserByLoginWithHttpInfo (string loginProvider, string providerKey)
        {
            
            // verify the required parameter 'loginProvider' is set
            if (loginProvider == null)
                throw new ApiException(400, "Missing required parameter 'loginProvider' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserByLogin");
            
            // verify the required parameter 'providerKey' is set
            if (providerKey == null)
                throw new ApiException(400, "Missing required parameter 'providerKey' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserByLogin");
            
    
            var path_ = "/api/storefront/security/user/external";
    
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
            
            if (loginProvider != null) queryParams.Add("loginProvider", Configuration.ApiClient.ParameterToString(loginProvider)); // query parameter
            if (providerKey != null) queryParams.Add("providerKey", Configuration.ApiClient.ParameterToString(providerKey)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserByLogin: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserByLogin: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
    
        /// <summary>
        /// Get user details by external login provider 
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByLoginAsync (string loginProvider, string providerKey)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = await StorefrontSecurityGetUserByLoginAsyncWithHttpInfo(loginProvider, providerKey);
             return response.Data;

        }

        /// <summary>
        /// Get user details by external login provider 
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> StorefrontSecurityGetUserByLoginAsyncWithHttpInfo (string loginProvider, string providerKey)
        {
            // verify the required parameter 'loginProvider' is set
            if (loginProvider == null) throw new ApiException(400, "Missing required parameter 'loginProvider' when calling StorefrontSecurityGetUserByLogin");
            // verify the required parameter 'providerKey' is set
            if (providerKey == null) throw new ApiException(400, "Missing required parameter 'providerKey' when calling StorefrontSecurityGetUserByLogin");
            
    
            var path_ = "/api/storefront/security/user/external";
    
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
            
            if (loginProvider != null) queryParams.Add("loginProvider", Configuration.ApiClient.ParameterToString(loginProvider)); // query parameter
            if (providerKey != null) queryParams.Add("providerKey", Configuration.ApiClient.ParameterToString(providerKey)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserByLogin: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserByLogin: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
        
        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended StorefrontSecurityGetUserById (string userId)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = StorefrontSecurityGetUserByIdWithHttpInfo(userId);
             return response.Data;
        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > StorefrontSecurityGetUserByIdWithHttpInfo (string userId)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserById");
            
    
            var path_ = "/api/storefront/security/user/id/{userId}";
    
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
            if (userId != null) pathParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
    
        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByIdAsync (string userId)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = await StorefrontSecurityGetUserByIdAsyncWithHttpInfo(userId);
             return response.Data;

        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> StorefrontSecurityGetUserByIdAsyncWithHttpInfo (string userId)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling StorefrontSecurityGetUserById");
            
    
            var path_ = "/api/storefront/security/user/id/{userId}";
    
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
            if (userId != null) pathParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
        
        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended StorefrontSecurityGetUserByName (string userName)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = StorefrontSecurityGetUserByNameWithHttpInfo(userName);
             return response.Data;
        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > StorefrontSecurityGetUserByNameWithHttpInfo (string userName)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling CommerceCoreModuleApi->StorefrontSecurityGetUserByName");
            
    
            var path_ = "/api/storefront/security/user/name/{userName}";
    
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
            if (userName != null) pathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserByName: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserByName: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
    
        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> StorefrontSecurityGetUserByNameAsync (string userName)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = await StorefrontSecurityGetUserByNameAsyncWithHttpInfo(userName);
             return response.Data;

        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> StorefrontSecurityGetUserByNameAsyncWithHttpInfo (string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling StorefrontSecurityGetUserByName");
            
    
            var path_ = "/api/storefront/security/user/name/{userName}";
    
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
            if (userName != null) pathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserByName: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGetUserByName: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
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
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = StorefrontSecurityResetPasswordWithHttpInfo(userId, token, newPassword);
             return response.Data;
        }

        /// <summary>
        /// Reset a password for the user 
        /// </summary>
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
            
    
            var path_ = "/api/storefront/security/user/password/reset";
    
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
            
            if (userId != null) queryParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // query parameter
            if (token != null) queryParams.Add("token", Configuration.ApiClient.ParameterToString(token)); // query parameter
            if (newPassword != null) queryParams.Add("newPassword", Configuration.ApiClient.ParameterToString(newPassword)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityResetPassword: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityResetPassword: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
    
        /// <summary>
        /// Reset a password for the user 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> StorefrontSecurityResetPasswordAsync (string userId, string token, string newPassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = await StorefrontSecurityResetPasswordAsyncWithHttpInfo(userId, token, newPassword);
             return response.Data;

        }

        /// <summary>
        /// Reset a password for the user 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> StorefrontSecurityResetPasswordAsyncWithHttpInfo (string userId, string token, string newPassword)
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
            
            if (userId != null) queryParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // query parameter
            if (token != null) queryParams.Add("token", Configuration.ApiClient.ParameterToString(token)); // query parameter
            if (newPassword != null) queryParams.Add("newPassword", Configuration.ApiClient.ParameterToString(newPassword)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityResetPassword: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityResetPassword: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
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
             StorefrontSecurityGenerateResetPasswordTokenWithHttpInfo(userId, storeName, language, callbackUrl);
        }

        /// <summary>
        /// Generate a password reset token Generates a password reset token and sends a password reset link to the user via email.
        /// </summary>
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
            
    
            var path_ = "/api/storefront/security/user/password/resettoken";
    
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
            
            if (userId != null) queryParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // query parameter
            if (storeName != null) queryParams.Add("storeName", Configuration.ApiClient.ParameterToString(storeName)); // query parameter
            if (language != null) queryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            if (callbackUrl != null) queryParams.Add("callbackUrl", Configuration.ApiClient.ParameterToString(callbackUrl)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Generate a password reset token Generates a password reset token and sends a password reset link to the user via email.
        /// </summary>
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
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> StorefrontSecurityGenerateResetPasswordTokenAsyncWithHttpInfo (string userId, string storeName, string language, string callbackUrl)
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
            
            if (userId != null) queryParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // query parameter
            if (storeName != null) queryParams.Add("storeName", Configuration.ApiClient.ParameterToString(storeName)); // query parameter
            if (language != null) queryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            if (callbackUrl != null) queryParams.Add("callbackUrl", Configuration.ApiClient.ParameterToString(callbackUrl)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityGenerateResetPasswordToken: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Sign in with user name and password 
        /// </summary>
        /// <param name="userName"></param> 
        /// <param name="password"></param> 
        /// <returns>VirtoCommerceCoreModuleWebModelSignInResult</returns>
        public VirtoCommerceCoreModuleWebModelSignInResult StorefrontSecurityPasswordSignIn (string userName, string password)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult> response = StorefrontSecurityPasswordSignInWithHttpInfo(userName, password);
             return response.Data;
        }

        /// <summary>
        /// Sign in with user name and password 
        /// </summary>
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
            
    
            var path_ = "/api/storefront/security/user/signin";
    
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
            
            if (userName != null) queryParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // query parameter
            if (password != null) queryParams.Add("password", Configuration.ApiClient.ParameterToString(password)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityPasswordSignIn: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityPasswordSignIn: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelSignInResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelSignInResult)));
            
        }
    
        /// <summary>
        /// Sign in with user name and password 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Task of VirtoCommerceCoreModuleWebModelSignInResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCoreModuleWebModelSignInResult> StorefrontSecurityPasswordSignInAsync (string userName, string password)
        {
             ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult> response = await StorefrontSecurityPasswordSignInAsyncWithHttpInfo(userName, password);
             return response.Data;

        }

        /// <summary>
        /// Sign in with user name and password 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCoreModuleWebModelSignInResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult>> StorefrontSecurityPasswordSignInAsyncWithHttpInfo (string userName, string password)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling StorefrontSecurityPasswordSignIn");
            // verify the required parameter 'password' is set
            if (password == null) throw new ApiException(400, "Missing required parameter 'password' when calling StorefrontSecurityPasswordSignIn");
            
    
            var path_ = "/api/storefront/security/user/signin";
    
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
            
            if (userName != null) queryParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // query parameter
            if (password != null) queryParams.Add("password", Configuration.ApiClient.ParameterToString(password)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityPasswordSignIn: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StorefrontSecurityPasswordSignIn: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCoreModuleWebModelSignInResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCoreModuleWebModelSignInResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCoreModuleWebModelSignInResult)));
            
        }
        
        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context 
        /// </summary>
        /// <param name="storeId"></param> 
        /// <param name="evalContext"></param> 
        /// <returns>List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;</returns>
        public List<VirtoCommerceDomainTaxModelTaxRate> CommerceEvaluateTaxes (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>> response = CommerceEvaluateTaxesWithHttpInfo(storeId, evalContext);
             return response.Data;
        }

        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context 
        /// </summary>
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
            
    
            var path_ = "/api/taxes/{storeId}/evaluate";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            if (evalContext.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(evalContext); // http body (model) parameter
            }
            else
            {
                postBody = evalContext; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceEvaluateTaxes: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceEvaluateTaxes: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainTaxModelTaxRate>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceDomainTaxModelTaxRate>)));
            
        }
    
        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>Task of List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceDomainTaxModelTaxRate>> CommerceEvaluateTaxesAsync (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>> response = await CommerceEvaluateTaxesAsyncWithHttpInfo(storeId, evalContext);
             return response.Data;

        }

        /// <summary>
        /// Evaluate and return all tax rates for specified store and evaluation context 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="evalContext"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainTaxModelTaxRate&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>>> CommerceEvaluateTaxesAsyncWithHttpInfo (string storeId, VirtoCommerceDomainTaxModelTaxEvaluationContext evalContext)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling CommerceEvaluateTaxes");
            // verify the required parameter 'evalContext' is set
            if (evalContext == null) throw new ApiException(400, "Missing required parameter 'evalContext' when calling CommerceEvaluateTaxes");
            
    
            var path_ = "/api/taxes/{storeId}/evaluate";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(evalContext); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CommerceEvaluateTaxes: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CommerceEvaluateTaxes: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainTaxModelTaxRate>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainTaxModelTaxRate>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceDomainTaxModelTaxRate>)));
            
        }
        
    }
    
}
