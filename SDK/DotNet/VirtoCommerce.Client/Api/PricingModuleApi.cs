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
        
        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>
        /// Get all pricelists for given product.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetProductPriceLists (string productId);
  
        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>
        /// Get all pricelists for given product.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetProductPriceListsWithHttpInfo (string productId);

        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>
        /// Get all pricelists for given product.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetProductPriceListsAsync (string productId);

        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>
        /// Get all pricelists for given product.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleGetProductPriceListsAsyncWithHttpInfo (string productId);
        
        /// <summary>
        /// Update prices
        /// </summary>
        /// <remarks>
        /// Update prices of product for given pricelist.
        /// </remarks>
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
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PricingModuleUpdateProductPriceListsWithHttpInfo (string productId, VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Update prices
        /// </summary>
        /// <remarks>
        /// Update prices of product for given pricelist.
        /// </remarks>
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
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdateProductPriceListsAsyncWithHttpInfo (string productId, VirtoCommercePricingModuleWebModelPricelist priceList);
        
        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>
        /// Get array of all pricelist assignments for all catalogs.
        /// </remarks>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignments ();
  
        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>
        /// Get array of all pricelist assignments for all catalogs.
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentsWithHttpInfo ();

        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>
        /// Get array of all pricelist assignments for all catalogs.
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentsAsync ();

        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>
        /// Get array of all pricelist assignments for all catalogs.
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>>> PricingModuleGetPricelistAssignmentsAsyncWithHttpInfo ();
        
        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns></returns>
        void PricingModuleUpdatePriceListAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
  
        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PricingModuleUpdatePriceListAssignmentWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);

        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PricingModuleUpdatePriceListAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);

        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdatePriceListAssignmentAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
        
        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleCreatePricelistAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
  
        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleCreatePricelistAssignmentWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);

        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleCreatePricelistAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);

        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleCreatePricelistAssignmentAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
        
        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>
        /// Delete pricelist assignment by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns></returns>
        void PricingModuleDeleteAssignments (List<string> ids);
  
        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>
        /// Delete pricelist assignment by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PricingModuleDeleteAssignmentsWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>
        /// Delete pricelist assignment by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PricingModuleDeleteAssignmentsAsync (List<string> ids);

        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>
        /// Delete pricelist assignment by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleDeleteAssignmentsAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>
        /// Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </remarks>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleGetNewPricelistAssignments ();
  
        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>
        /// Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </remarks>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetNewPricelistAssignmentsWithHttpInfo ();

        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>
        /// Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </remarks>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetNewPricelistAssignmentsAsync ();

        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>
        /// Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </remarks>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetNewPricelistAssignmentsAsyncWithHttpInfo ();
        
        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleGetPricelistAssignmentById (string id);
  
        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignmentByIdWithHttpInfo (string id);

        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignmentByIdAsync (string id);

        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Evaluate prices by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPrice> PricingModuleEvaluatePrices (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);
  
        /// <summary>
        /// Evaluate prices by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleEvaluatePricesWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);

        /// <summary>
        /// Evaluate prices by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleEvaluatePricesAsync (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);

        /// <summary>
        /// Evaluate prices by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPrice&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>> PricingModuleEvaluatePricesAsyncWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);
        
        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>
        /// Get all pricelists for all catalogs.
        /// </remarks>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceLists ();
  
        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>
        /// Get all pricelists for all catalogs.
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListsWithHttpInfo ();

        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>
        /// Get all pricelists for all catalogs.
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListsAsync ();

        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>
        /// Get all pricelists for all catalogs.
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleGetPriceListsAsyncWithHttpInfo ();
        
        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns></returns>
        void PricingModuleUpdatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList);
  
        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PricingModuleUpdatePriceListWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PricingModuleUpdatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdatePriceListAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList);
        
        /// <summary>
        /// Create pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        VirtoCommercePricingModuleWebModelPricelist PricingModuleCreatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList);
  
        /// <summary>
        /// Create pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelist</returns>
        ApiResponse<VirtoCommercePricingModuleWebModelPricelist> PricingModuleCreatePriceListWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Create pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelist</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelist> PricingModuleCreatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList);

        /// <summary>
        /// Create pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelist)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleCreatePriceListAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList);
        
        /// <summary>
        /// Delete pricelists
        /// </summary>
        /// <remarks>
        /// Delete pricelists by given array of pricelist ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns></returns>
        void PricingModuleDeletePriceLists (List<string> ids);
  
        /// <summary>
        /// Delete pricelists
        /// </summary>
        /// <remarks>
        /// Delete pricelists by given array of pricelist ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PricingModuleDeletePriceListsWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete pricelists
        /// </summary>
        /// <remarks>
        /// Delete pricelists by given array of pricelist ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PricingModuleDeletePriceListsAsync (List<string> ids);

        /// <summary>
        /// Delete pricelists
        /// </summary>
        /// <remarks>
        /// Delete pricelists by given array of pricelist ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleDeletePriceListsAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Evaluate pricelists by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleEvaluatePriceLists (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);
  
        /// <summary>
        /// Evaluate pricelists by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleEvaluatePriceListsWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);

        /// <summary>
        /// Evaluate pricelists by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleEvaluatePriceListsAsync (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);

        /// <summary>
        /// Evaluate pricelists by given context
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleEvaluatePriceListsAsyncWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext);
        
        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist id</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        VirtoCommercePricingModuleWebModelPricelist PricingModuleGetPriceListById (string id);
  
        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist id</param>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelist</returns>
        ApiResponse<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceListByIdWithHttpInfo (string id);

        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist id</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelist</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceListByIdAsync (string id);

        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelist)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>
        /// Get an array of valid product prices for each currency.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        List<VirtoCommercePricingModuleWebModelPrice> PricingModuleGetProductPrices (string productId);
  
        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>
        /// Get an array of valid product prices for each currency.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleGetProductPricesWithHttpInfo (string productId);

        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>
        /// Get an array of valid product prices for each currency.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleGetProductPricesAsync (string productId);

        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>
        /// Get an array of valid product prices for each currency.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPrice&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>> PricingModuleGetProductPricesAsyncWithHttpInfo (string productId);
        
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
        /// Get pricelists for a product Get all pricelists for given product.
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetProductPriceLists (string productId)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> response = PricingModuleGetProductPriceListsWithHttpInfo(productId);
             return response.Data;
        }

        /// <summary>
        /// Get pricelists for a product Get all pricelists for given product.
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPricelist> > PricingModuleGetProductPriceListsWithHttpInfo (string productId)
        {
            
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleApi->PricingModuleGetProductPriceLists");
            
    
            var path_ = "/api/catalog/products/{productId}/pricelists";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleGetProductPriceLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetProductPriceLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }
    
        /// <summary>
        /// Get pricelists for a product Get all pricelists for given product.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetProductPriceListsAsync (string productId)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> response = await PricingModuleGetProductPriceListsAsyncWithHttpInfo(productId);
             return response.Data;

        }

        /// <summary>
        /// Get pricelists for a product Get all pricelists for given product.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleGetProductPriceListsAsyncWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleGetProductPriceLists");
            
    
            var path_ = "/api/catalog/products/{productId}/pricelists";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleGetProductPriceLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetProductPriceLists: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }
        
        /// <summary>
        /// Update prices Update prices of product for given pricelist.
        /// </summary>
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
            
    
            var path_ = "/api/catalog/products/{productId}/pricelists";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            if (priceList.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            }
            else
            {
                postBody = priceList; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleUpdateProductPriceLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleUpdateProductPriceLists: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update prices Update prices of product for given pricelist.
        /// </summary>
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
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdateProductPriceListsAsyncWithHttpInfo (string productId, VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleUpdateProductPriceLists");
            // verify the required parameter 'priceList' is set
            if (priceList == null) throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleUpdateProductPriceLists");
            
    
            var path_ = "/api/catalog/products/{productId}/pricelists";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleUpdateProductPriceLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleUpdateProductPriceLists: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get pricelist assignments Get array of all pricelist assignments for all catalogs.
        /// </summary>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignments ()
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> response = PricingModuleGetPricelistAssignmentsWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get pricelist assignments Get array of all pricelist assignments for all catalogs.
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPricelistAssignment> > PricingModuleGetPricelistAssignmentsWithHttpInfo ()
        {
            
    
            var path_ = "/api/pricing/assignments";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleGetPricelistAssignments: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetPricelistAssignments: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelistAssignment>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPricelistAssignment>)));
            
        }
    
        /// <summary>
        /// Get pricelist assignments Get array of all pricelist assignments for all catalogs.
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentsAsync ()
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> response = await PricingModuleGetPricelistAssignmentsAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get pricelist assignments Get array of all pricelist assignments for all catalogs.
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelistAssignment&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>>> PricingModuleGetPricelistAssignmentsAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/pricing/assignments";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleGetPricelistAssignments: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetPricelistAssignments: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelistAssignment>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelistAssignment>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPricelistAssignment>)));
            
        }
        
        /// <summary>
        /// Update pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param> 
        /// <returns></returns>
        public void PricingModuleUpdatePriceListAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
             PricingModuleUpdatePriceListAssignmentWithHttpInfo(assignment);
        }

        /// <summary>
        /// Update pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PricingModuleUpdatePriceListAssignmentWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            
            // verify the required parameter 'assignment' is set
            if (assignment == null)
                throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleApi->PricingModuleUpdatePriceListAssignment");
            
    
            var path_ = "/api/pricing/assignments";
    
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
            
            
            
            
            if (assignment.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(assignment); // http body (model) parameter
            }
            else
            {
                postBody = assignment; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PricingModuleUpdatePriceListAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
             await PricingModuleUpdatePriceListAssignmentAsyncWithHttpInfo(assignment);

        }

        /// <summary>
        /// Update pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdatePriceListAssignmentAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            // verify the required parameter 'assignment' is set
            if (assignment == null) throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleUpdatePriceListAssignment");
            
    
            var path_ = "/api/pricing/assignments";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(assignment); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param> 
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleCreatePricelistAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> response = PricingModuleCreatePricelistAssignmentWithHttpInfo(assignment);
             return response.Data;
        }

        /// <summary>
        /// Create pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param> 
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public ApiResponse< VirtoCommercePricingModuleWebModelPricelistAssignment > PricingModuleCreatePricelistAssignmentWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            
            // verify the required parameter 'assignment' is set
            if (assignment == null)
                throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleApi->PricingModuleCreatePricelistAssignment");
            
    
            var path_ = "/api/pricing/assignments";
    
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
            
            
            
            
            if (assignment.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(assignment); // http body (model) parameter
            }
            else
            {
                postBody = assignment; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleCreatePricelistAssignment: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleCreatePricelistAssignment: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }
    
        /// <summary>
        /// Create pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleCreatePricelistAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> response = await PricingModuleCreatePricelistAssignmentAsyncWithHttpInfo(assignment);
             return response.Data;

        }

        /// <summary>
        /// Create pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleCreatePricelistAssignmentAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            // verify the required parameter 'assignment' is set
            if (assignment == null) throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleCreatePricelistAssignment");
            
    
            var path_ = "/api/pricing/assignments";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(assignment); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleCreatePricelistAssignment: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleCreatePricelistAssignment: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }
        
        /// <summary>
        /// Delete pricelist assignments Delete pricelist assignment by given array of ids.
        /// </summary>
        /// <param name="ids">An array of pricelist assignment ids</param> 
        /// <returns></returns>
        public void PricingModuleDeleteAssignments (List<string> ids)
        {
             PricingModuleDeleteAssignmentsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete pricelist assignments Delete pricelist assignment by given array of ids.
        /// </summary>
        /// <param name="ids">An array of pricelist assignment ids</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PricingModuleDeleteAssignmentsWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleApi->PricingModuleDeleteAssignments");
            
    
            var path_ = "/api/pricing/assignments";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleDeleteAssignments: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleDeleteAssignments: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete pricelist assignments Delete pricelist assignment by given array of ids.
        /// </summary>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PricingModuleDeleteAssignmentsAsync (List<string> ids)
        {
             await PricingModuleDeleteAssignmentsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete pricelist assignments Delete pricelist assignment by given array of ids.
        /// </summary>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleDeleteAssignmentsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleDeleteAssignments");
            
    
            var path_ = "/api/pricing/assignments";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleDeleteAssignments: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleDeleteAssignments: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get a new pricelist assignment Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </summary>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleGetNewPricelistAssignments ()
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> response = PricingModuleGetNewPricelistAssignmentsWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get a new pricelist assignment Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </summary>
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public ApiResponse< VirtoCommercePricingModuleWebModelPricelistAssignment > PricingModuleGetNewPricelistAssignmentsWithHttpInfo ()
        {
            
    
            var path_ = "/api/pricing/assignments/new";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }
    
        /// <summary>
        /// Get a new pricelist assignment Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </summary>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetNewPricelistAssignmentsAsync ()
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> response = await PricingModuleGetNewPricelistAssignmentsAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get a new pricelist assignment Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </summary>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetNewPricelistAssignmentsAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/pricing/assignments/new";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }
        
        /// <summary>
        /// Get pricelist assignment 
        /// </summary>
        /// <param name="id">Pricelist assignment id</param> 
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleGetPricelistAssignmentById (string id)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> response = PricingModuleGetPricelistAssignmentByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get pricelist assignment 
        /// </summary>
        /// <param name="id">Pricelist assignment id</param> 
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public ApiResponse< VirtoCommercePricingModuleWebModelPricelistAssignment > PricingModuleGetPricelistAssignmentByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleApi->PricingModuleGetPricelistAssignmentById");
            
    
            var path_ = "/api/pricing/assignments/{id}";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }
    
        /// <summary>
        /// Get pricelist assignment 
        /// </summary>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignmentByIdAsync (string id)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment> response = await PricingModuleGetPricelistAssignmentByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get pricelist assignment 
        /// </summary>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelistAssignment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleGetPricelistAssignmentById");
            
    
            var path_ = "/api/pricing/assignments/{id}";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelistAssignment>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelistAssignment) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment)));
            
        }
        
        /// <summary>
        /// Evaluate prices by given context 
        /// </summary>
        /// <param name="evalContext">Pricing evaluation context</param> 
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPrice> PricingModuleEvaluatePrices (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> response = PricingModuleEvaluatePricesWithHttpInfo(evalContext);
             return response.Data;
        }

        /// <summary>
        /// Evaluate prices by given context 
        /// </summary>
        /// <param name="evalContext">Pricing evaluation context</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPrice> > PricingModuleEvaluatePricesWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
            
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling PricingModuleApi->PricingModuleEvaluatePrices");
            
    
            var path_ = "/api/pricing/evaluate";
    
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleEvaluatePrices: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleEvaluatePrices: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPrice>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPrice>)));
            
        }
    
        /// <summary>
        /// Evaluate prices by given context 
        /// </summary>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleEvaluatePricesAsync (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> response = await PricingModuleEvaluatePricesAsyncWithHttpInfo(evalContext);
             return response.Data;

        }

        /// <summary>
        /// Evaluate prices by given context 
        /// </summary>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPrice&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>> PricingModuleEvaluatePricesAsyncWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null) throw new ApiException(400, "Missing required parameter 'evalContext' when calling PricingModuleEvaluatePrices");
            
    
            var path_ = "/api/pricing/evaluate";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(evalContext); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleEvaluatePrices: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleEvaluatePrices: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPrice>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPrice>)));
            
        }
        
        /// <summary>
        /// Get pricelists Get all pricelists for all catalogs.
        /// </summary>
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceLists ()
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> response = PricingModuleGetPriceListsWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get pricelists Get all pricelists for all catalogs.
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPricelist> > PricingModuleGetPriceListsWithHttpInfo ()
        {
            
    
            var path_ = "/api/pricing/pricelists";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleGetPriceLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetPriceLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }
    
        /// <summary>
        /// Get pricelists Get all pricelists for all catalogs.
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListsAsync ()
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> response = await PricingModuleGetPriceListsAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get pricelists Get all pricelists for all catalogs.
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleGetPriceListsAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/pricing/pricelists";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleGetPriceLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetPriceLists: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }
        
        /// <summary>
        /// Update pricelist 
        /// </summary>
        /// <param name="priceList"></param> 
        /// <returns></returns>
        public void PricingModuleUpdatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
             PricingModuleUpdatePriceListWithHttpInfo(priceList);
        }

        /// <summary>
        /// Update pricelist 
        /// </summary>
        /// <param name="priceList"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PricingModuleUpdatePriceListWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            
            // verify the required parameter 'priceList' is set
            if (priceList == null)
                throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleApi->PricingModuleUpdatePriceList");
            
    
            var path_ = "/api/pricing/pricelists";
    
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
            
            
            
            
            if (priceList.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            }
            else
            {
                postBody = priceList; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleUpdatePriceList: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleUpdatePriceList: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update pricelist 
        /// </summary>
        /// <param name="priceList"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PricingModuleUpdatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
             await PricingModuleUpdatePriceListAsyncWithHttpInfo(priceList);

        }

        /// <summary>
        /// Update pricelist 
        /// </summary>
        /// <param name="priceList"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleUpdatePriceListAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'priceList' is set
            if (priceList == null) throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleUpdatePriceList");
            
    
            var path_ = "/api/pricing/pricelists";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleUpdatePriceList: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleUpdatePriceList: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create pricelist 
        /// </summary>
        /// <param name="priceList"></param> 
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        public VirtoCommercePricingModuleWebModelPricelist PricingModuleCreatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelist> response = PricingModuleCreatePriceListWithHttpInfo(priceList);
             return response.Data;
        }

        /// <summary>
        /// Create pricelist 
        /// </summary>
        /// <param name="priceList"></param> 
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelist</returns>
        public ApiResponse< VirtoCommercePricingModuleWebModelPricelist > PricingModuleCreatePriceListWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            
            // verify the required parameter 'priceList' is set
            if (priceList == null)
                throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleApi->PricingModuleCreatePriceList");
            
    
            var path_ = "/api/pricing/pricelists";
    
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
            
            
            
            
            if (priceList.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            }
            else
            {
                postBody = priceList; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleCreatePriceList: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleCreatePriceList: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelist>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelist) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePricingModuleWebModelPricelist)));
            
        }
    
        /// <summary>
        /// Create pricelist 
        /// </summary>
        /// <param name="priceList"></param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelist</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelist> PricingModuleCreatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelist> response = await PricingModuleCreatePriceListAsyncWithHttpInfo(priceList);
             return response.Data;

        }

        /// <summary>
        /// Create pricelist 
        /// </summary>
        /// <param name="priceList"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelist)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleCreatePriceListAsyncWithHttpInfo (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'priceList' is set
            if (priceList == null) throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleCreatePriceList");
            
    
            var path_ = "/api/pricing/pricelists";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(priceList); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleCreatePriceList: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleCreatePriceList: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelist>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelist) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePricingModuleWebModelPricelist)));
            
        }
        
        /// <summary>
        /// Delete pricelists Delete pricelists by given array of pricelist ids.
        /// </summary>
        /// <param name="ids">An array of pricelist ids</param> 
        /// <returns></returns>
        public void PricingModuleDeletePriceLists (List<string> ids)
        {
             PricingModuleDeletePriceListsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete pricelists Delete pricelists by given array of pricelist ids.
        /// </summary>
        /// <param name="ids">An array of pricelist ids</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PricingModuleDeletePriceListsWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleApi->PricingModuleDeletePriceLists");
            
    
            var path_ = "/api/pricing/pricelists";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleDeletePriceLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleDeletePriceLists: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete pricelists Delete pricelists by given array of pricelist ids.
        /// </summary>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PricingModuleDeletePriceListsAsync (List<string> ids)
        {
             await PricingModuleDeletePriceListsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete pricelists Delete pricelists by given array of pricelist ids.
        /// </summary>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PricingModuleDeletePriceListsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleDeletePriceLists");
            
    
            var path_ = "/api/pricing/pricelists";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleDeletePriceLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleDeletePriceLists: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Evaluate pricelists by given context 
        /// </summary>
        /// <param name="evalContext">Pricing evaluation context</param> 
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleEvaluatePriceLists (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> response = PricingModuleEvaluatePriceListsWithHttpInfo(evalContext);
             return response.Data;
        }

        /// <summary>
        /// Evaluate pricelists by given context 
        /// </summary>
        /// <param name="evalContext">Pricing evaluation context</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPricelist> > PricingModuleEvaluatePriceListsWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
            
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling PricingModuleApi->PricingModuleEvaluatePriceLists");
            
    
            var path_ = "/api/pricing/pricelists/evaluate";
    
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
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleEvaluatePriceLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleEvaluatePriceLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }
    
        /// <summary>
        /// Evaluate pricelists by given context 
        /// </summary>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleEvaluatePriceListsAsync (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>> response = await PricingModuleEvaluatePriceListsAsyncWithHttpInfo(evalContext);
             return response.Data;

        }

        /// <summary>
        /// Evaluate pricelists by given context 
        /// </summary>
        /// <param name="evalContext">Pricing evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPricelist&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>> PricingModuleEvaluatePriceListsAsyncWithHttpInfo (VirtoCommerceDomainPricingModelPriceEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null) throw new ApiException(400, "Missing required parameter 'evalContext' when calling PricingModuleEvaluatePriceLists");
            
    
            var path_ = "/api/pricing/pricelists/evaluate";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(evalContext); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleEvaluatePriceLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleEvaluatePriceLists: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPricelist>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPricelist>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPricelist>)));
            
        }
        
        /// <summary>
        /// Get pricelist 
        /// </summary>
        /// <param name="id">Pricelist id</param> 
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        public VirtoCommercePricingModuleWebModelPricelist PricingModuleGetPriceListById (string id)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelist> response = PricingModuleGetPriceListByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get pricelist 
        /// </summary>
        /// <param name="id">Pricelist id</param> 
        /// <returns>ApiResponse of VirtoCommercePricingModuleWebModelPricelist</returns>
        public ApiResponse< VirtoCommercePricingModuleWebModelPricelist > PricingModuleGetPriceListByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleApi->PricingModuleGetPriceListById");
            
    
            var path_ = "/api/pricing/pricelists/{id}";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleGetPriceListById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetPriceListById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelist>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelist) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePricingModuleWebModelPricelist)));
            
        }
    
        /// <summary>
        /// Get pricelist 
        /// </summary>
        /// <param name="id">Pricelist id</param>
        /// <returns>Task of VirtoCommercePricingModuleWebModelPricelist</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceListByIdAsync (string id)
        {
             ApiResponse<VirtoCommercePricingModuleWebModelPricelist> response = await PricingModuleGetPriceListByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get pricelist 
        /// </summary>
        /// <param name="id">Pricelist id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePricingModuleWebModelPricelist)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleGetPriceListById");
            
    
            var path_ = "/api/pricing/pricelists/{id}";
    
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
                throw new ApiException (statusCode, "Error calling PricingModuleGetPriceListById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetPriceListById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePricingModuleWebModelPricelist>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePricingModuleWebModelPricelist) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePricingModuleWebModelPricelist)));
            
        }
        
        /// <summary>
        /// Get array of product prices Get an array of valid product prices for each currency.
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <returns>List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public List<VirtoCommercePricingModuleWebModelPrice> PricingModuleGetProductPrices (string productId)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> response = PricingModuleGetProductPricesWithHttpInfo(productId);
             return response.Data;
        }

        /// <summary>
        /// Get array of product prices Get an array of valid product prices for each currency.
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public ApiResponse< List<VirtoCommercePricingModuleWebModelPrice> > PricingModuleGetProductPricesWithHttpInfo (string productId)
        {
            
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleApi->PricingModuleGetProductPrices");
            
    
            var path_ = "/api/products/{productId}/prices";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleGetProductPrices: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetProductPrices: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPrice>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPrice>)));
            
        }
    
        /// <summary>
        /// Get array of product prices Get an array of valid product prices for each currency.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommercePricingModuleWebModelPrice&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPrice>> PricingModuleGetProductPricesAsync (string productId)
        {
             ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>> response = await PricingModuleGetProductPricesAsyncWithHttpInfo(productId);
             return response.Data;

        }

        /// <summary>
        /// Get array of product prices Get an array of valid product prices for each currency.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePricingModuleWebModelPrice&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>> PricingModuleGetProductPricesAsyncWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleGetProductPrices");
            
    
            var path_ = "/api/products/{productId}/prices";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PricingModuleGetProductPrices: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PricingModuleGetProductPrices: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePricingModuleWebModelPrice>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePricingModuleWebModelPrice>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePricingModuleWebModelPrice>)));
            
        }
        
    }
    
}
