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
    public interface IPricingModuleApi
    {
        #region Synchronous Operations
        /// <summary>
        /// Create pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        VirtoCommercePricingModuleWebModelPricelist PricingModuleCreatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Create pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelist</returns>
        ApiResponse<VirtoCommercePricingModuleWebModelPricelist> PricingModuleCreatePriceListWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList);
        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleCreatePricelistAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);

        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleCreatePricelistAssignmentWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>
        /// Delete pricelist assignment by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns></returns>
        void PricingModuleDeleteAssignments (List<string> ids);

        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>
        /// Delete pricelist assignment by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PricingModuleDeleteAssignmentsWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete pricelists
        /// </summary>
        /// <remarks>
        /// Delete pricelists by given array of pricelist ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns></returns>
        void PricingModuleDeletePriceLists (List<string> ids);

        /// <summary>
        /// Delete pricelists
        /// </summary>
        /// <remarks>
        /// Delete pricelists by given array of pricelist ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PricingModuleDeletePriceListsWithHttpInfo (List<string> ids);
        /// <summary>
        /// Evaluate pricelists by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleEvaluatePriceLists (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);

        /// <summary>
        /// Evaluate pricelists by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleEvaluatePriceListsWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);
        /// <summary>
        /// Evaluate prices by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPrice> PricingModuleEvaluatePrices (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);

        /// <summary>
        /// Evaluate prices by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleEvaluatePricesWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);
        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>
        /// Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleGetNewPricelistAssignments ();

        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>
        /// Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetNewPricelistAssignmentsWithHttpInfo ();
        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist id</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        VirtoCommercePricingModuleWebModelPricelist PricingModuleGetPriceListById (string id);

        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist id</param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelist</returns>
        ApiResponse<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceListByIdWithHttpInfo (string id);
        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>
        /// Get all pricelists for all catalogs.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceLists ();

        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>
        /// Get all pricelists for all catalogs.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListsWithHttpInfo ();
        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleGetPricelistAssignmentById (string id);

        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignmentByIdWithHttpInfo (string id);
        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>
        /// Get array of all pricelist assignments for all catalogs.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignments ();

        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>
        /// Get array of all pricelist assignments for all catalogs.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentsWithHttpInfo ();
        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>
        /// Get all pricelists for given product.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetProductPriceLists (string productId);

        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>
        /// Get all pricelists for given product.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetProductPriceListsWithHttpInfo (string productId);
        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>
        /// Get an array of valid product prices for each currency.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPrice> PricingModuleGetProductPrices (string productId);

        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>
        /// Get an array of valid product prices for each currency.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleGetProductPricesWithHttpInfo (string productId);
        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns></returns>
        void PricingModuleUpdatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PricingModuleUpdatePriceListWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList);
        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns></returns>
        void PricingModuleUpdatePriceListAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);

        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PricingModuleUpdatePriceListAssignmentWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
        /// <summary>
        /// Update prices
        /// </summary>
        /// <remarks>
        /// Update prices of product for given pricelist.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns></returns>
        void PricingModuleUpdateProductPriceLists (string productId, VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Update prices
        /// </summary>
        /// <remarks>
        /// Update prices of product for given pricelist.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PricingModuleUpdateProductPriceListsWithHttpInfo (string productId, VirtoCommercePricingModuleWebModelPricelist priceList);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Create pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelist</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelist> PricingModuleCreatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Create pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelist)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleCreatePriceListAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList);
        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleCreatePricelistAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);

        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleCreatePricelistAssignmentAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>
        /// Delete pricelist assignment by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PricingModuleDeleteAssignmentsAsync (List<string> ids);

        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>
        /// Delete pricelist assignment by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleDeleteAssignmentsAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete pricelists
        /// </summary>
        /// <remarks>
        /// Delete pricelists by given array of pricelist ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PricingModuleDeletePriceListsAsync (List<string> ids);

        /// <summary>
        /// Delete pricelists
        /// </summary>
        /// <remarks>
        /// Delete pricelists by given array of pricelist ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleDeletePriceListsAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Evaluate pricelists by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleEvaluatePriceListsAsync (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);

        /// <summary>
        /// Evaluate pricelists by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleEvaluatePriceListsAsyncWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);
        /// <summary>
        /// Evaluate prices by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleEvaluatePricesAsync (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);

        /// <summary>
        /// Evaluate prices by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPrice&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>> PricingModuleEvaluatePricesAsyncWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);
        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>
        /// Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetNewPricelistAssignmentsAsync ();

        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>
        /// Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetNewPricelistAssignmentsAsyncWithHttpInfo ();
        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist id</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelist</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceListByIdAsync (string id);

        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelist)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>
        /// Get all pricelists for all catalogs.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListsAsync ();

        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>
        /// Get all pricelists for all catalogs.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleGetPriceListsAsyncWithHttpInfo ();
        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignmentByIdAsync (string id);

        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>
        /// Get array of all pricelist assignments for all catalogs.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentsAsync ();

        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>
        /// Get array of all pricelist assignments for all catalogs.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>>> PricingModuleGetPricelistAssignmentsAsyncWithHttpInfo ();
        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>
        /// Get all pricelists for given product.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetProductPriceListsAsync (string productId);

        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>
        /// Get all pricelists for given product.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleGetProductPriceListsAsyncWithHttpInfo (string productId);
        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>
        /// Get an array of valid product prices for each currency.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleGetProductPricesAsync (string productId);

        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>
        /// Get an array of valid product prices for each currency.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPrice&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>> PricingModuleGetProductPricesAsyncWithHttpInfo (string productId);
        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PricingModuleUpdatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdatePriceListAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList);
        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PricingModuleUpdatePriceListAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);

        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdatePriceListAssignmentAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
        /// <summary>
        /// Update prices
        /// </summary>
        /// <remarks>
        /// Update prices of product for given pricelist.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PricingModuleUpdateProductPriceListsAsync (string productId, VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Update prices
        /// </summary>
        /// <remarks>
        /// Update prices of product for given pricelist.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdateProductPriceListsAsyncWithHttpInfo (string productId, VirtoCommercePricingModuleWebModelPricelist priceList);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class PricingModuleApi : IPricingModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PricingModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public PricingModuleApi(Configuration configuration)
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
        /// Create pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        public VirtoCommercePricingModuleWebModelPricelist PricingModuleCreatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelist> localVarResponse = PricingModuleCreatePriceListWithHttpInfo(priceList);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelist</returns>
        public ApiResponse< VirtoCommercePricingModuleWebModelPricelist > PricingModuleCreatePriceListWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'priceList' is set
            if (priceList == null)
                throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleApi->PricingModuleCreatePriceList");

            var localVarPath = "/api/pricing/pricelists";
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
            if (priceList.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            }
            else
            {
                localVarPostBody = priceList; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleCreatePriceList: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleCreatePriceList: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelist>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelist) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePricingModuleWebModelPricelist)));
            
        }

        /// <summary>
        /// Create pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelist</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelist> PricingModuleCreatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelist> localVarResponse = await PricingModuleCreatePriceListAsyncWithHttpInfo(priceList);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelist)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleCreatePriceListAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'priceList' is set
            if (priceList == null)
                throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleApi->PricingModuleCreatePriceList");

            var localVarPath = "/api/pricing/pricelists";
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
            if (priceList.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            }
            else
            {
                localVarPostBody = priceList; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleCreatePriceList: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleCreatePriceList: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelist>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelist) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePricingModuleWebModelPricelist)));
            
        }

        /// <summary>
        /// Create pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleCreatePricelistAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> localVarResponse = PricingModuleCreatePricelistAssignmentWithHttpInfo(assignment);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public ApiResponse< VirtoCommercePricingModuleWebModelPricelistAssignment > PricingModuleCreatePricelistAssignmentWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            // verify the required parameter 'assignment' is set
            if (assignment == null)
                throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleApi->PricingModuleCreatePricelistAssignment");

            var localVarPath = "/api/pricing/assignments";
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
            if (assignment.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(assignment); // http body (model) parameter
            }
            else
            {
                localVarPostBody = assignment; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleCreatePricelistAssignment: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleCreatePricelistAssignment: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }

        /// <summary>
        /// Create pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleCreatePricelistAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> localVarResponse = await PricingModuleCreatePricelistAssignmentAsyncWithHttpInfo(assignment);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleCreatePricelistAssignmentAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            // verify the required parameter 'assignment' is set
            if (assignment == null)
                throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleApi->PricingModuleCreatePricelistAssignment");

            var localVarPath = "/api/pricing/assignments";
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
            if (assignment.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(assignment); // http body (model) parameter
            }
            else
            {
                localVarPostBody = assignment; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleCreatePricelistAssignment: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleCreatePricelistAssignment: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }

        /// <summary>
        /// Delete pricelist assignments Delete pricelist assignment by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns></returns>
        public void PricingModuleDeleteAssignments (List<string> ids)
        {
             PricingModuleDeleteAssignmentsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete pricelist assignments Delete pricelist assignment by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PricingModuleDeleteAssignmentsWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleApi->PricingModuleDeleteAssignments");

            var localVarPath = "/api/pricing/assignments";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleDeleteAssignments: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleDeleteAssignments: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete pricelist assignments Delete pricelist assignment by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PricingModuleDeleteAssignmentsAsync (List<string> ids)
        {
             await PricingModuleDeleteAssignmentsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete pricelist assignments Delete pricelist assignment by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleDeleteAssignmentsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleApi->PricingModuleDeleteAssignments");

            var localVarPath = "/api/pricing/assignments";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleDeleteAssignments: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleDeleteAssignments: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete pricelists Delete pricelists by given array of pricelist ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns></returns>
        public void PricingModuleDeletePriceLists (List<string> ids)
        {
             PricingModuleDeletePriceListsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete pricelists Delete pricelists by given array of pricelist ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PricingModuleDeletePriceListsWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleApi->PricingModuleDeletePriceLists");

            var localVarPath = "/api/pricing/pricelists";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleDeletePriceLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleDeletePriceLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete pricelists Delete pricelists by given array of pricelist ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PricingModuleDeletePriceListsAsync (List<string> ids)
        {
             await PricingModuleDeletePriceListsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete pricelists Delete pricelists by given array of pricelist ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleDeletePriceListsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleApi->PricingModuleDeletePriceLists");

            var localVarPath = "/api/pricing/pricelists";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleDeletePriceLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleDeletePriceLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Evaluate pricelists by given context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleEvaluatePriceLists (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> localVarResponse = PricingModuleEvaluatePriceListsWithHttpInfo(evalContext);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Evaluate pricelists by given context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPricelist> > PricingModuleEvaluatePriceListsWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling PricingModuleApi->PricingModuleEvaluatePriceLists");

            var localVarPath = "/api/pricing/pricelists/evaluate";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleEvaluatePriceLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleEvaluatePriceLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }

        /// <summary>
        /// Evaluate pricelists by given context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleEvaluatePriceListsAsync (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> localVarResponse = await PricingModuleEvaluatePriceListsAsyncWithHttpInfo(evalContext);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Evaluate pricelists by given context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleEvaluatePriceListsAsyncWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling PricingModuleApi->PricingModuleEvaluatePriceLists");

            var localVarPath = "/api/pricing/pricelists/evaluate";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleEvaluatePriceLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleEvaluatePriceLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }

        /// <summary>
        /// Evaluate prices by given context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPrice> PricingModuleEvaluatePrices (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> localVarResponse = PricingModuleEvaluatePricesWithHttpInfo(evalContext);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Evaluate prices by given context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPrice> > PricingModuleEvaluatePricesWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling PricingModuleApi->PricingModuleEvaluatePrices");

            var localVarPath = "/api/pricing/evaluate";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleEvaluatePrices: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleEvaluatePrices: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPrice>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPrice>)));
            
        }

        /// <summary>
        /// Evaluate prices by given context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleEvaluatePricesAsync (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> localVarResponse = await PricingModuleEvaluatePricesAsyncWithHttpInfo(evalContext);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Evaluate prices by given context 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPrice&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>> PricingModuleEvaluatePricesAsyncWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling PricingModuleApi->PricingModuleEvaluatePrices");

            var localVarPath = "/api/pricing/evaluate";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleEvaluatePrices: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleEvaluatePrices: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPrice>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPrice>)));
            
        }

        /// <summary>
        /// Get a new pricelist assignment Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleGetNewPricelistAssignments ()
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> localVarResponse = PricingModuleGetNewPricelistAssignmentsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get a new pricelist assignment Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public ApiResponse< VirtoCommercePricingModuleWebModelPricelistAssignment > PricingModuleGetNewPricelistAssignmentsWithHttpInfo ()
        {

            var localVarPath = "/api/pricing/assignments/new";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }

        /// <summary>
        /// Get a new pricelist assignment Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetNewPricelistAssignmentsAsync ()
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> localVarResponse = await PricingModuleGetNewPricelistAssignmentsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get a new pricelist assignment Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetNewPricelistAssignmentsAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/pricing/assignments/new";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }

        /// <summary>
        /// Get pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist id</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        public VirtoCommercePricingModuleWebModelPricelist PricingModuleGetPriceListById (string id)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelist> localVarResponse = PricingModuleGetPriceListByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist id</param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelist</returns>
        public ApiResponse< VirtoCommercePricingModuleWebModelPricelist > PricingModuleGetPriceListByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleApi->PricingModuleGetPriceListById");

            var localVarPath = "/api/pricing/pricelists/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPriceListById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPriceListById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelist>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelist) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePricingModuleWebModelPricelist)));
            
        }

        /// <summary>
        /// Get pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist id</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelist</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceListByIdAsync (string id)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelist> localVarResponse = await PricingModuleGetPriceListByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelist)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleApi->PricingModuleGetPriceListById");

            var localVarPath = "/api/pricing/pricelists/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPriceListById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPriceListById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelist>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelist) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePricingModuleWebModelPricelist)));
            
        }

        /// <summary>
        /// Get pricelists Get all pricelists for all catalogs.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceLists ()
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> localVarResponse = PricingModuleGetPriceListsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get pricelists Get all pricelists for all catalogs.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPricelist> > PricingModuleGetPriceListsWithHttpInfo ()
        {

            var localVarPath = "/api/pricing/pricelists";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPriceLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPriceLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }

        /// <summary>
        /// Get pricelists Get all pricelists for all catalogs.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListsAsync ()
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> localVarResponse = await PricingModuleGetPriceListsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get pricelists Get all pricelists for all catalogs.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleGetPriceListsAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/pricing/pricelists";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPriceLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPriceLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }

        /// <summary>
        /// Get pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleGetPricelistAssignmentById (string id)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> localVarResponse = PricingModuleGetPricelistAssignmentByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public ApiResponse< VirtoCommercePricingModuleWebModelPricelistAssignment > PricingModuleGetPricelistAssignmentByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleApi->PricingModuleGetPricelistAssignmentById");

            var localVarPath = "/api/pricing/assignments/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }

        /// <summary>
        /// Get pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignmentByIdAsync (string id)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> localVarResponse = await PricingModuleGetPricelistAssignmentByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleApi->PricingModuleGetPricelistAssignmentById");

            var localVarPath = "/api/pricing/assignments/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }

        /// <summary>
        /// Get pricelist assignments Get array of all pricelist assignments for all catalogs.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignments ()
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> localVarResponse = PricingModuleGetPricelistAssignmentsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get pricelist assignments Get array of all pricelist assignments for all catalogs.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPricelistAssignment> > PricingModuleGetPricelistAssignmentsWithHttpInfo ()
        {

            var localVarPath = "/api/pricing/assignments";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPricelistAssignments: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPricelistAssignments: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelistAssignment>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPricelistAssignment>)));
            
        }

        /// <summary>
        /// Get pricelist assignments Get array of all pricelist assignments for all catalogs.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentsAsync ()
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> localVarResponse = await PricingModuleGetPricelistAssignmentsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get pricelist assignments Get array of all pricelist assignments for all catalogs.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>>> PricingModuleGetPricelistAssignmentsAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/pricing/assignments";
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
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPricelistAssignments: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetPricelistAssignments: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelistAssignment>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPricelistAssignment>)));
            
        }

        /// <summary>
        /// Get pricelists for a product Get all pricelists for given product.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetProductPriceLists (string productId)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> localVarResponse = PricingModuleGetProductPriceListsWithHttpInfo(productId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get pricelists for a product Get all pricelists for given product.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPricelist> > PricingModuleGetProductPriceListsWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleApi->PricingModuleGetProductPriceLists");

            var localVarPath = "/api/catalog/products/{productId}/pricelists";
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
            if (productId != null) localVarPathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetProductPriceLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetProductPriceLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }

        /// <summary>
        /// Get pricelists for a product Get all pricelists for given product.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetProductPriceListsAsync (string productId)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> localVarResponse = await PricingModuleGetProductPriceListsAsyncWithHttpInfo(productId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get pricelists for a product Get all pricelists for given product.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleGetProductPriceListsAsyncWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleApi->PricingModuleGetProductPriceLists");

            var localVarPath = "/api/catalog/products/{productId}/pricelists";
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
            if (productId != null) localVarPathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetProductPriceLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetProductPriceLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }

        /// <summary>
        /// Get array of product prices Get an array of valid product prices for each currency.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPrice> PricingModuleGetProductPrices (string productId)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> localVarResponse = PricingModuleGetProductPricesWithHttpInfo(productId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get array of product prices Get an array of valid product prices for each currency.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPrice> > PricingModuleGetProductPricesWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleApi->PricingModuleGetProductPrices");

            var localVarPath = "/api/products/{productId}/prices";
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
            if (productId != null) localVarPathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetProductPrices: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetProductPrices: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPrice>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPrice>)));
            
        }

        /// <summary>
        /// Get array of product prices Get an array of valid product prices for each currency.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleGetProductPricesAsync (string productId)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> localVarResponse = await PricingModuleGetProductPricesAsyncWithHttpInfo(productId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get array of product prices Get an array of valid product prices for each currency.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPrice&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>> PricingModuleGetProductPricesAsyncWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleApi->PricingModuleGetProductPrices");

            var localVarPath = "/api/products/{productId}/prices";
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
            if (productId != null) localVarPathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetProductPrices: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleGetProductPrices: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPrice>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePricingModuleWebModelPrice>)));
            
        }

        /// <summary>
        /// Update pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns></returns>
        public void PricingModuleUpdatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
             PricingModuleUpdatePriceListWithHttpInfo(priceList);
        }

        /// <summary>
        /// Update pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PricingModuleUpdatePriceListWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'priceList' is set
            if (priceList == null)
                throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleApi->PricingModuleUpdatePriceList");

            var localVarPath = "/api/pricing/pricelists";
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
            if (priceList.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            }
            else
            {
                localVarPostBody = priceList; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdatePriceList: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdatePriceList: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PricingModuleUpdatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
             await PricingModuleUpdatePriceListAsyncWithHttpInfo(priceList);

        }

        /// <summary>
        /// Update pricelist 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="priceList"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdatePriceListAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'priceList' is set
            if (priceList == null)
                throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleApi->PricingModuleUpdatePriceList");

            var localVarPath = "/api/pricing/pricelists";
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
            if (priceList.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            }
            else
            {
                localVarPostBody = priceList; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdatePriceList: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdatePriceList: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns></returns>
        public void PricingModuleUpdatePriceListAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
             PricingModuleUpdatePriceListAssignmentWithHttpInfo(assignment);
        }

        /// <summary>
        /// Update pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PricingModuleUpdatePriceListAssignmentWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            // verify the required parameter 'assignment' is set
            if (assignment == null)
                throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleApi->PricingModuleUpdatePriceListAssignment");

            var localVarPath = "/api/pricing/assignments";
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
            if (assignment.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(assignment); // http body (model) parameter
            }
            else
            {
                localVarPostBody = assignment; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PricingModuleUpdatePriceListAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
             await PricingModuleUpdatePriceListAssignmentAsyncWithHttpInfo(assignment);

        }

        /// <summary>
        /// Update pricelist assignment 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdatePriceListAssignmentAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            // verify the required parameter 'assignment' is set
            if (assignment == null)
                throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleApi->PricingModuleUpdatePriceListAssignment");

            var localVarPath = "/api/pricing/assignments";
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
            if (assignment.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(assignment); // http body (model) parameter
            }
            else
            {
                localVarPostBody = assignment; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update prices Update prices of product for given pricelist.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns></returns>
        public void PricingModuleUpdateProductPriceLists (string productId, VirtoCommercePricingModuleWebModelPricelist priceList)
        {
             PricingModuleUpdateProductPriceListsWithHttpInfo(productId, priceList);
        }

        /// <summary>
        /// Update prices Update prices of product for given pricelist.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PricingModuleUpdateProductPriceListsWithHttpInfo (string productId, VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleApi->PricingModuleUpdateProductPriceLists");
            // verify the required parameter 'priceList' is set
            if (priceList == null)
                throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleApi->PricingModuleUpdateProductPriceLists");

            var localVarPath = "/api/catalog/products/{productId}/pricelists";
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
            if (productId != null) localVarPathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            if (priceList.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            }
            else
            {
                localVarPostBody = priceList; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdateProductPriceLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdateProductPriceLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update prices Update prices of product for given pricelist.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PricingModuleUpdateProductPriceListsAsync (string productId, VirtoCommercePricingModuleWebModelPricelist priceList)
        {
             await PricingModuleUpdateProductPriceListsAsyncWithHttpInfo(productId, priceList);

        }

        /// <summary>
        /// Update prices Update prices of product for given pricelist.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdateProductPriceListsAsyncWithHttpInfo (string productId, VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleApi->PricingModuleUpdateProductPriceLists");
            // verify the required parameter 'priceList' is set
            if (priceList == null)
                throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleApi->PricingModuleUpdateProductPriceLists");

            var localVarPath = "/api/catalog/products/{productId}/pricelists";
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
            if (productId != null) localVarPathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            if (priceList.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            }
            else
            {
                localVarPostBody = priceList; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdateProductPriceLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PricingModuleUpdateProductPriceLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

    }
}
