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
    public interface IVirtoCommercePlatformApi
    {
        
        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folderUrl"></param>
        /// <param name="keyword"></param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        List<VirtoCommercePlatformWebModelAssetAssetListItem> AssetsSearchAssetItems (string folderUrl = null, string keyword = null);
  
        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folderUrl"></param>
        /// <param name="keyword"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>> AssetsSearchAssetItemsWithHttpInfo (string folderUrl = null, string keyword = null);

        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folderUrl"></param>
        /// <param name="keyword"></param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetAssetListItem>> AssetsSearchAssetItemsAsync (string folderUrl = null, string keyword = null);

        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folderUrl"></param>
        /// <param name="keyword"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>>> AssetsSearchAssetItemsAsyncWithHttpInfo (string folderUrl = null, string keyword = null);
        
        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional)</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        List<VirtoCommercePlatformWebModelAssetBlobInfo> AssetsUploadAsset (string folderUrl, string url = null);
  
        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetWithHttpInfo (string folderUrl, string url = null);

        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional)</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetAsync (string folderUrl, string url = null);

        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>> AssetsUploadAssetAsyncWithHttpInfo (string folderUrl, string url = null);
        
        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="urls"></param>
        /// <returns></returns>
        void AssetsDeleteBlobs (List<string> urls);
  
        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="urls"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> AssetsDeleteBlobsWithHttpInfo (List<string> urls);

        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="urls"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task AssetsDeleteBlobsAsync (List<string> urls);

        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="urls"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> AssetsDeleteBlobsAsyncWithHttpInfo (List<string> urls);
        
        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder"></param>
        /// <returns></returns>
        void AssetsCreateBlobFolder (VirtoCommercePlatformCoreAssetBlobFolder folder);
  
        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> AssetsCreateBlobFolderWithHttpInfo (VirtoCommercePlatformCoreAssetBlobFolder folder);

        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task AssetsCreateBlobFolderAsync (VirtoCommercePlatformCoreAssetBlobFolder folder);

        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> AssetsCreateBlobFolderAsyncWithHttpInfo (VirtoCommercePlatformCoreAssetBlobFolder folder);
        
        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>List&lt;string&gt;</returns>
        List<string> DynamicPropertiesGetObjectTypes ();
  
        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        ApiResponse<List<string>> DynamicPropertiesGetObjectTypesWithHttpInfo ();

        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of List&lt;string&gt;</returns>
        System.Threading.Tasks.Task<List<string>> DynamicPropertiesGetObjectTypesAsync ();

        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<string>>> DynamicPropertiesGetObjectTypesAsyncWithHttpInfo ();
        
        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <returns>List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesGetProperties (string typeName);
  
        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesGetPropertiesWithHttpInfo (string typeName);

        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesGetPropertiesAsync (string typeName);

        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>>> DynamicPropertiesGetPropertiesAsyncWithHttpInfo (string typeName);
        
        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty DynamicPropertiesCreateProperty (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);
  
        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesCreatePropertyWithHttpInfo (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);

        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesCreatePropertyAsync (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);

        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesCreatePropertyAsyncWithHttpInfo (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);
        
        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        void DynamicPropertiesUpdateProperty (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);
  
        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesUpdatePropertyWithHttpInfo (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);

        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DynamicPropertiesUpdatePropertyAsync (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);

        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesUpdatePropertyAsyncWithHttpInfo (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);
        
        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        void DynamicPropertiesDeleteProperty (string typeName, string propertyId);
  
        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesDeletePropertyWithHttpInfo (string typeName, string propertyId);

        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DynamicPropertiesDeletePropertyAsync (string typeName, string propertyId);

        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesDeletePropertyAsyncWithHttpInfo (string typeName, string propertyId);
        
        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;</returns>
        List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> DynamicPropertiesGetDictionaryItems (string typeName, string propertyId);
  
        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsWithHttpInfo (string typeName, string propertyId);

        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsAsync (string typeName, string propertyId);

        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>>> DynamicPropertiesGetDictionaryItemsAsyncWithHttpInfo (string typeName, string propertyId);
        
        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        void DynamicPropertiesSaveDictionaryItems (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items);
  
        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesSaveDictionaryItemsWithHttpInfo (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items);

        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DynamicPropertiesSaveDictionaryItemsAsync (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items);

        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesSaveDictionaryItemsAsyncWithHttpInfo (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items);
        
        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns></returns>
        void DynamicPropertiesDeleteDictionaryItem (string typeName, string propertyId, List<string> ids);
  
        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesDeleteDictionaryItemWithHttpInfo (string typeName, string propertyId, List<string> ids);

        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DynamicPropertiesDeleteDictionaryItemAsync (string typeName, string propertyId, List<string> ids);

        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesDeleteDictionaryItemAsyncWithHttpInfo (string typeName, string propertyId, List<string> ids);
        
        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Job ID.</param>
        /// <returns>VirtoCommercePlatformWebModelJobsJob</returns>
        VirtoCommercePlatformWebModelJobsJob JobsGetStatus (string id);
  
        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Job ID.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelJobsJob</returns>
        ApiResponse<VirtoCommercePlatformWebModelJobsJob> JobsGetStatusWithHttpInfo (string id);

        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelJobsJob</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelJobsJob> JobsGetStatusAsync (string id);

        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelJobsJob)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelJobsJob>> JobsGetStatusAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>List&lt;string&gt;</returns>
        List<string> LocalizationGetLocales ();
  
        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        ApiResponse<List<string>> LocalizationGetLocalesWithHttpInfo ();

        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of List&lt;string&gt;</returns>
        System.Threading.Tasks.Task<List<string>> LocalizationGetLocalesAsync ();

        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<string>>> LocalizationGetLocalesAsyncWithHttpInfo ();
        
        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        List<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModules ();
  
        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModulesWithHttpInfo ();

        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModulesAsync ();

        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>>> ModulesGetModulesAsyncWithHttpInfo ();
        
        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesUpload ();
  
        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesUploadWithHttpInfo ();

        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesUploadAsync ();

        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModuleDescriptor)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesUploadAsyncWithHttpInfo ();
        
        /// <summary>
        /// Install module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesInstallModule (string fileName);
  
        /// <summary>
        /// Install module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesInstallModuleWithHttpInfo (string fileName);

        /// <summary>
        /// Install module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesInstallModuleAsync (string fileName);

        /// <summary>
        /// Install module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesInstallModuleAsyncWithHttpInfo (string fileName);
        
        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        void ModulesRestart ();
  
        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ModulesRestartWithHttpInfo ();

        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ModulesRestartAsync ();

        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ModulesRestartAsyncWithHttpInfo ();
        
        /// <summary>
        /// Get module details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesGetModuleById (string id);
  
        /// <summary>
        /// Get module details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModuleByIdWithHttpInfo (string id);

        /// <summary>
        /// Get module details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModuleByIdAsync (string id);

        /// <summary>
        /// Get module details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModuleDescriptor)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModuleByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesUninstallModule (string id);
  
        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUninstallModuleWithHttpInfo (string id);

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUninstallModuleAsync (string id);

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesUninstallModuleAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Update module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesUpdateModule (string id, string fileName);
  
        /// <summary>
        /// Update module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUpdateModuleWithHttpInfo (string id, string fileName);

        /// <summary>
        /// Update module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUpdateModuleAsync (string id, string fileName);

        /// <summary>
        /// Update module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesUpdateModuleAsyncWithHttpInfo (string id, string fileName);
        
        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <returns>List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        List<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotifications ();
  
        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationsWithHttpInfo ();

        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationsAsync ();

        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>>> NotificationsGetNotificationsAsyncWithHttpInfo ();
        
        /// <summary>
        /// Get notification journal page
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </remarks>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult NotificationsGetNotificationJournal (string objectId, string objectTypeId, int? start, int? count);
  
        /// <summary>
        /// Get notification journal page
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </remarks>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> NotificationsGetNotificationJournalWithHttpInfo (string objectId, string objectTypeId, int? start, int? count);

        /// <summary>
        /// Get notification journal page
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </remarks>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> NotificationsGetNotificationJournalAsync (string objectId, string objectTypeId, int? start, int? count);

        /// <summary>
        /// Get notification journal page
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </remarks>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult>> NotificationsGetNotificationJournalAsyncWithHttpInfo (string objectId, string objectTypeId, int? start, int? count);
        
        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Sending notification id</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotification</returns>
        VirtoCommercePlatformWebModelNotificationsNotification NotificationsGetNotification (string id);
  
        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Sending notification id</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsNotification</returns>
        ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotificationWithHttpInfo (string id);

        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotificationAsync (string id);

        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns></returns>
        void NotificationsStopSendingNotifications (List<string> ids);
  
        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> NotificationsStopSendingNotificationsWithHttpInfo (List<string> ids);

        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task NotificationsStopSendingNotificationsAsync (List<string> ids);

        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsStopSendingNotificationsAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns></returns>
        void NotificationsUpdateNotificationTemplate (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate);
  
        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> NotificationsUpdateNotificationTemplateWithHttpInfo (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate);

        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task NotificationsUpdateNotificationTemplateAsync (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate);

        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsUpdateNotificationTemplateAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate);
        
        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <param name="request">Test notification request</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult NotificationsRenderNotificationContent (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);
  
        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <param name="request">Test notification request</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> NotificationsRenderNotificationContentWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);

        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> NotificationsRenderNotificationContentAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);

        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult>> NotificationsRenderNotificationContentAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);
        
        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <param name="request">Test notification request</param>
        /// <returns>string</returns>
        string NotificationsSendNotification (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);
  
        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <param name="request">Test notification request</param>
        /// <returns>ApiResponse of string</returns>
        ApiResponse<string> NotificationsSendNotificationWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);

        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of string</returns>
        System.Threading.Tasks.Task<string> NotificationsSendNotificationAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);

        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (string)</returns>
        System.Threading.Tasks.Task<ApiResponse<string>> NotificationsSendNotificationAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);
        
        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Template id</param>
        /// <returns></returns>
        void NotificationsDeleteNotificationTemplate (string id);
  
        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Template id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> NotificationsDeleteNotificationTemplateWithHttpInfo (string id);

        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Template id</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task NotificationsDeleteNotificationTemplateAsync (string id);

        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Template id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsDeleteNotificationTemplateAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <param name="type">Notification type</param>
        /// <returns>List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        List<VirtoCommercePlatformCoreNotificationsNotificationParameter> NotificationsGetTestingParameters (string type);
  
        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <param name="type">Notification type</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> NotificationsGetTestingParametersWithHttpInfo (string type);

        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <param name="type">Notification type</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> NotificationsGetTestingParametersAsync (string type);

        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <param name="type">Notification type</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>>> NotificationsGetTestingParametersAsyncWithHttpInfo (string type);
        
        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </remarks>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplates (string type, string objectId, string objectTypeId);
  
        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </remarks>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplatesWithHttpInfo (string type, string objectId, string objectTypeId);

        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </remarks>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplatesAsync (string type, string objectId, string objectTypeId);

        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </remarks>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>>> NotificationsGetNotificationTemplatesAsyncWithHttpInfo (string type, string objectId, string objectTypeId);
        
        /// <summary>
        /// Get notification template
        /// </summary>
        /// <remarks>
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </remarks>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        VirtoCommercePlatformWebModelNotificationsNotificationTemplate NotificationsGetNotificationTemplate (string type, string objectId, string objectTypeId, string language);
  
        /// <summary>
        /// Get notification template
        /// </summary>
        /// <remarks>
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </remarks>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplateWithHttpInfo (string type, string objectId, string objectTypeId, string language);

        /// <summary>
        /// Get notification template
        /// </summary>
        /// <remarks>
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </remarks>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplateAsync (string type, string objectId, string objectTypeId, string language);

        /// <summary>
        /// Get notification template
        /// </summary>
        /// <remarks>
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </remarks>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsNotificationTemplate)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplateAsyncWithHttpInfo (string type, string objectId, string objectTypeId, string language);
        
        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationSearch (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria);
  
        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationSearchWithHttpInfo (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria);

        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationSearchAsync (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria);

        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>> PushNotificationSearchAsyncWithHttpInfo (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria);
        
        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationMarkAllAsRead ();
  
        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationMarkAllAsReadWithHttpInfo ();

        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationMarkAllAsReadAsync ();

        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>> PushNotificationMarkAllAsReadAsyncWithHttpInfo ();
        
        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <param name="type"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApiAccount</returns>
        VirtoCommercePlatformCoreSecurityApiAccount SecurityGenerateNewApiAccount (string type);
  
        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <param name="type"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApiAccount</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount> SecurityGenerateNewApiAccountWithHttpInfo (string type);

        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <param name="type"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApiAccount</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApiAccount> SecurityGenerateNewApiAccountAsync (string type);

        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <param name="type"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApiAccount)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount>> SecurityGenerateNewApiAccountAsyncWithHttpInfo (string type);
        
        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetCurrentUser ();
  
        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetCurrentUserWithHttpInfo ();

        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetCurrentUserAsync ();

        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetCurrentUserAsyncWithHttpInfo ();
        
        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <param name="model">User credentials.</param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityLogin (VirtoCommercePlatformWebModelSecurityUserLogin model);
  
        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <param name="model">User credentials.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityLoginWithHttpInfo (VirtoCommercePlatformWebModelSecurityUserLogin model);

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityLoginAsync (VirtoCommercePlatformWebModelSecurityUserLogin model);

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityLoginAsyncWithHttpInfo (VirtoCommercePlatformWebModelSecurityUserLogin model);
        
        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        void SecurityLogout ();
  
        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SecurityLogoutWithHttpInfo ();

        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SecurityLogoutAsync ();

        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> SecurityLogoutAsyncWithHttpInfo ();
        
        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        List<VirtoCommercePlatformCoreSecurityPermission> SecurityGetPermissions ();
  
        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>> SecurityGetPermissionsWithHttpInfo ();

        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreSecurityPermission>> SecurityGetPermissionsAsync ();

        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>>> SecurityGetPermissionsAsyncWithHttpInfo ();
        
        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="role"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        VirtoCommercePlatformCoreSecurityRole SecurityUpdateRole (VirtoCommercePlatformCoreSecurityRole role);
  
        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="role"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRole</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityRole> SecurityUpdateRoleWithHttpInfo (VirtoCommercePlatformCoreSecurityRole role);

        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="role"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRole</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityUpdateRoleAsync (VirtoCommercePlatformCoreSecurityRole role);

        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="role"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRole)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRole>> SecurityUpdateRoleAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityRole role);
        
        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request">Search parameters.</param>
        /// <returns>VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        VirtoCommercePlatformCoreSecurityRoleSearchResponse SecuritySearchRoles (VirtoCommercePlatformCoreSecurityRoleSearchRequest request);
  
        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request">Search parameters.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse> SecuritySearchRolesWithHttpInfo (VirtoCommercePlatformCoreSecurityRoleSearchRequest request);

        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRoleSearchResponse> SecuritySearchRolesAsync (VirtoCommercePlatformCoreSecurityRoleSearchRequest request);

        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRoleSearchResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse>> SecuritySearchRolesAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityRoleSearchRequest request);
        
        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids"></param>
        /// <returns></returns>
        void SecurityDeleteRoles (List<string> ids);
  
        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SecurityDeleteRolesWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SecurityDeleteRolesAsync (List<string> ids);

        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> SecurityDeleteRolesAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="roleId"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        VirtoCommercePlatformCoreSecurityRole SecurityGetRole (string roleId);
  
        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="roleId"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRole</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityRole> SecurityGetRoleWithHttpInfo (string roleId);

        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="roleId"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRole</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityGetRoleAsync (string roleId);

        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="roleId"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRole)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRole>> SecurityGetRoleAsyncWithHttpInfo (string roleId);
        
        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user">User details.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult SecurityUpdateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
  
        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user">User details.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityUpdateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user">User details.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityUpdateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityUpdateAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        
        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request">Search parameters.</param>
        /// <returns>VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        VirtoCommercePlatformCoreSecurityUserSearchResponse SecuritySearchUsersAsync (VirtoCommercePlatformCoreSecurityUserSearchRequest request);
  
        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request">Search parameters.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse> SecuritySearchUsersAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityUserSearchRequest request);

        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityUserSearchResponse> SecuritySearchUsersAsyncAsync (VirtoCommercePlatformCoreSecurityUserSearchRequest request);

        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityUserSearchResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse>> SecuritySearchUsersAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityUserSearchRequest request);
        
        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="names">An array of user names.</param>
        /// <returns></returns>
        void SecurityDeleteAsync (List<string> names);
  
        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="names">An array of user names.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SecurityDeleteAsyncWithHttpInfo (List<string> names);

        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SecurityDeleteAsyncAsync (List<string> names);

        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> SecurityDeleteAsyncAsyncWithHttpInfo (List<string> names);
        
        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user">User details.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult SecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
  
        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user">User details.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityCreateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user">User details.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityCreateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityCreateAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        
        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetUserById (string id);
  
        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByIdWithHttpInfo (string id);

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByIdAsync (string id);

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetUserByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetUserByName (string userName);
  
        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByNameWithHttpInfo (string userName);

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByNameAsync (string userName);

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetUserByNameAsyncWithHttpInfo (string userName);
        
        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult SecurityChangePassword (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword);
  
        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityChangePasswordWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword);

        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityChangePasswordAsync (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword);

        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityChangePasswordAsyncWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword);
        
        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult SecurityResetPassword (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword);
  
        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityResetPasswordWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword);

        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityResetPasswordAsync (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword);

        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityResetPasswordAsyncWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword);
        
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetAllSettings ();
  
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetAllSettingsWithHttpInfo ();

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetAllSettingsAsync ();

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>> SettingGetAllSettingsAsyncWithHttpInfo ();
        
        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="settings"></param>
        /// <returns></returns>
        void SettingUpdate (List<VirtoCommercePlatformWebModelSettingsSetting> settings);
  
        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="settings"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SettingUpdateWithHttpInfo (List<VirtoCommercePlatformWebModelSettingsSetting> settings);

        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="settings"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SettingUpdateAsync (List<VirtoCommercePlatformWebModelSettingsSetting> settings);

        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="settings"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> SettingUpdateAsyncWithHttpInfo (List<VirtoCommercePlatformWebModelSettingsSetting> settings);
        
        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetModuleSettings (string id);
  
        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetModuleSettingsWithHttpInfo (string id);

        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetModuleSettingsAsync (string id);

        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>> SettingGetModuleSettingsAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Setting system name.</param>
        /// <returns>VirtoCommercePlatformWebModelSettingsSetting</returns>
        VirtoCommercePlatformWebModelSettingsSetting SettingGetSetting (string id);
  
        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Setting system name.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelSettingsSetting</returns>
        ApiResponse<VirtoCommercePlatformWebModelSettingsSetting> SettingGetSettingWithHttpInfo (string id);

        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Setting system name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelSettingsSetting</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelSettingsSetting> SettingGetSettingAsync (string id);

        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Setting system name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelSettingsSetting)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetSettingAsyncWithHttpInfo (string id);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class VirtoCommercePlatformApi : IVirtoCommercePlatformApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommercePlatformApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public VirtoCommercePlatformApi(Configuration configuration)
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
        /// Search asset folders and blobs 
        /// </summary>
        /// <param name="folderUrl"></param> 
        /// <param name="keyword"></param> 
        /// <returns>List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        public List<VirtoCommercePlatformWebModelAssetAssetListItem> AssetsSearchAssetItems (string folderUrl = null, string keyword = null)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>> response = AssetsSearchAssetItemsWithHttpInfo(folderUrl, keyword);
             return response.Data;
        }

        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <param name="folderUrl"></param> 
        /// <param name="keyword"></param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelAssetAssetListItem> > AssetsSearchAssetItemsWithHttpInfo (string folderUrl = null, string keyword = null)
        {
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (folderUrl != null) queryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (keyword != null) queryParams.Add("keyword", Configuration.ApiClient.ParameterToString(keyword)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling AssetsSearchAssetItems: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling AssetsSearchAssetItems: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelAssetAssetListItem>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelAssetAssetListItem>)));
            
        }
    
        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <param name="folderUrl"></param>
        /// <param name="keyword"></param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetAssetListItem>> AssetsSearchAssetItemsAsync (string folderUrl = null, string keyword = null)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>> response = await AssetsSearchAssetItemsAsyncWithHttpInfo(folderUrl, keyword);
             return response.Data;

        }

        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <param name="folderUrl"></param>
        /// <param name="keyword"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>>> AssetsSearchAssetItemsAsyncWithHttpInfo (string folderUrl = null, string keyword = null)
        {
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (folderUrl != null) queryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (keyword != null) queryParams.Add("keyword", Configuration.ApiClient.ParameterToString(keyword)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling AssetsSearchAssetItems: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling AssetsSearchAssetItems: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelAssetAssetListItem>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelAssetAssetListItem>)));
            
        }
        
        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param> 
        /// <param name="url">Url for uploaded remote resource (optional)</param> 
        /// <returns>List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        public List<VirtoCommercePlatformWebModelAssetBlobInfo> AssetsUploadAsset (string folderUrl, string url = null)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>> response = AssetsUploadAssetWithHttpInfo(folderUrl, url);
             return response.Data;
        }

        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param> 
        /// <param name="url">Url for uploaded remote resource (optional)</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelAssetBlobInfo> > AssetsUploadAssetWithHttpInfo (string folderUrl, string url = null)
        {
            
            // verify the required parameter 'folderUrl' is set
            if (folderUrl == null)
                throw new ApiException(400, "Missing required parameter 'folderUrl' when calling VirtoCommercePlatformApi->AssetsUploadAsset");
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (folderUrl != null) queryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (url != null) queryParams.Add("url", Configuration.ApiClient.ParameterToString(url)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling AssetsUploadAsset: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling AssetsUploadAsset: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelAssetBlobInfo>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelAssetBlobInfo>)));
            
        }
    
        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional)</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetAsync (string folderUrl, string url = null)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>> response = await AssetsUploadAssetAsyncWithHttpInfo(folderUrl, url);
             return response.Data;

        }

        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>> AssetsUploadAssetAsyncWithHttpInfo (string folderUrl, string url = null)
        {
            // verify the required parameter 'folderUrl' is set
            if (folderUrl == null) throw new ApiException(400, "Missing required parameter 'folderUrl' when calling AssetsUploadAsset");
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (folderUrl != null) queryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (url != null) queryParams.Add("url", Configuration.ApiClient.ParameterToString(url)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling AssetsUploadAsset: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling AssetsUploadAsset: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelAssetBlobInfo>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelAssetBlobInfo>)));
            
        }
        
        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <param name="urls"></param> 
        /// <returns></returns>
        public void AssetsDeleteBlobs (List<string> urls)
        {
             AssetsDeleteBlobsWithHttpInfo(urls);
        }

        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <param name="urls"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> AssetsDeleteBlobsWithHttpInfo (List<string> urls)
        {
            
            // verify the required parameter 'urls' is set
            if (urls == null)
                throw new ApiException(400, "Missing required parameter 'urls' when calling VirtoCommercePlatformApi->AssetsDeleteBlobs");
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (urls != null) queryParams.Add("urls", Configuration.ApiClient.ParameterToString(urls)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling AssetsDeleteBlobs: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling AssetsDeleteBlobs: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <param name="urls"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task AssetsDeleteBlobsAsync (List<string> urls)
        {
             await AssetsDeleteBlobsAsyncWithHttpInfo(urls);

        }

        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <param name="urls"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> AssetsDeleteBlobsAsyncWithHttpInfo (List<string> urls)
        {
            // verify the required parameter 'urls' is set
            if (urls == null) throw new ApiException(400, "Missing required parameter 'urls' when calling AssetsDeleteBlobs");
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (urls != null) queryParams.Add("urls", Configuration.ApiClient.ParameterToString(urls)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling AssetsDeleteBlobs: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling AssetsDeleteBlobs: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <param name="folder"></param> 
        /// <returns></returns>
        public void AssetsCreateBlobFolder (VirtoCommercePlatformCoreAssetBlobFolder folder)
        {
             AssetsCreateBlobFolderWithHttpInfo(folder);
        }

        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <param name="folder"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> AssetsCreateBlobFolderWithHttpInfo (VirtoCommercePlatformCoreAssetBlobFolder folder)
        {
            
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling VirtoCommercePlatformApi->AssetsCreateBlobFolder");
            
    
            var path_ = "/api/platform/assets/folder";
    
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
            
            
            
            
            if (folder.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                postBody = folder; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling AssetsCreateBlobFolder: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling AssetsCreateBlobFolder: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task AssetsCreateBlobFolderAsync (VirtoCommercePlatformCoreAssetBlobFolder folder)
        {
             await AssetsCreateBlobFolderAsyncWithHttpInfo(folder);

        }

        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> AssetsCreateBlobFolderAsyncWithHttpInfo (VirtoCommercePlatformCoreAssetBlobFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null) throw new ApiException(400, "Missing required parameter 'folder' when calling AssetsCreateBlobFolder");
            
    
            var path_ = "/api/platform/assets/folder";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling AssetsCreateBlobFolder: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling AssetsCreateBlobFolder: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <returns>List&lt;string&gt;</returns>
        public List<string> DynamicPropertiesGetObjectTypes ()
        {
             ApiResponse<List<string>> response = DynamicPropertiesGetObjectTypesWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        public ApiResponse< List<string> > DynamicPropertiesGetObjectTypesWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/dynamic/types";
    
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
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetObjectTypes: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetObjectTypes: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<string>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>) Configuration.ApiClient.Deserialize(response, typeof(List<string>)));
            
        }
    
        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <returns>Task of List&lt;string&gt;</returns>
        public async System.Threading.Tasks.Task<List<string>> DynamicPropertiesGetObjectTypesAsync ()
        {
             ApiResponse<List<string>> response = await DynamicPropertiesGetObjectTypesAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<string>>> DynamicPropertiesGetObjectTypesAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/dynamic/types";
    
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
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetObjectTypes: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetObjectTypes: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<string>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>) Configuration.ApiClient.Deserialize(response, typeof(List<string>)));
            
        }
        
        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <returns>List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesGetProperties (string typeName)
        {
             ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> response = DynamicPropertiesGetPropertiesWithHttpInfo(typeName);
             return response.Data;
        }

        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> > DynamicPropertiesGetPropertiesWithHttpInfo (string typeName)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesGetProperties");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetProperties: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetProperties: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>)));
            
        }
    
        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesGetPropertiesAsync (string typeName)
        {
             ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> response = await DynamicPropertiesGetPropertiesAsyncWithHttpInfo(typeName);
             return response.Data;

        }

        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>>> DynamicPropertiesGetPropertiesAsyncWithHttpInfo (string typeName)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesGetProperties");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetProperties: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetProperties: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>)));
            
        }
        
        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="property"></param> 
        /// <returns>VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        public VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty DynamicPropertiesCreateProperty (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
             ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> response = DynamicPropertiesCreatePropertyWithHttpInfo(typeName, property);
             return response.Data;
        }

        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="property"></param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        public ApiResponse< VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty > DynamicPropertiesCreatePropertyWithHttpInfo (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesCreateProperty");
            
            // verify the required parameter 'property' is set
            if (property == null)
                throw new ApiException(400, "Missing required parameter 'property' when calling VirtoCommercePlatformApi->DynamicPropertiesCreateProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            
            
            
            
            if (property.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                postBody = property; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesCreateProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesCreateProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty)));
            
        }
    
        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesCreatePropertyAsync (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
             ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> response = await DynamicPropertiesCreatePropertyAsyncWithHttpInfo(typeName, property);
             return response.Data;

        }

        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesCreatePropertyAsyncWithHttpInfo (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesCreateProperty");
            // verify the required parameter 'property' is set
            if (property == null) throw new ApiException(400, "Missing required parameter 'property' when calling DynamicPropertiesCreateProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesCreateProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesCreateProperty: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty)));
            
        }
        
        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <param name="property"></param> 
        /// <returns></returns>
        public void DynamicPropertiesUpdateProperty (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
             DynamicPropertiesUpdatePropertyWithHttpInfo(typeName, propertyId, property);
        }

        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <param name="property"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> DynamicPropertiesUpdatePropertyWithHttpInfo (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesUpdateProperty");
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesUpdateProperty");
            
            // verify the required parameter 'property' is set
            if (property == null)
                throw new ApiException(400, "Missing required parameter 'property' when calling VirtoCommercePlatformApi->DynamicPropertiesUpdateProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            if (property.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                postBody = property; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesUpdateProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesUpdateProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DynamicPropertiesUpdatePropertyAsync (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
             await DynamicPropertiesUpdatePropertyAsyncWithHttpInfo(typeName, propertyId, property);

        }

        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesUpdatePropertyAsyncWithHttpInfo (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesUpdateProperty");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesUpdateProperty");
            // verify the required parameter 'property' is set
            if (property == null) throw new ApiException(400, "Missing required parameter 'property' when calling DynamicPropertiesUpdateProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesUpdateProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesUpdateProperty: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <returns></returns>
        public void DynamicPropertiesDeleteProperty (string typeName, string propertyId)
        {
             DynamicPropertiesDeletePropertyWithHttpInfo(typeName, propertyId);
        }

        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> DynamicPropertiesDeletePropertyWithHttpInfo (string typeName, string propertyId)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteProperty");
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesDeleteProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesDeleteProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DynamicPropertiesDeletePropertyAsync (string typeName, string propertyId)
        {
             await DynamicPropertiesDeletePropertyAsyncWithHttpInfo(typeName, propertyId);

        }

        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesDeletePropertyAsyncWithHttpInfo (string typeName, string propertyId)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesDeleteProperty");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesDeleteProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesDeleteProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesDeleteProperty: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <returns>List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;</returns>
        public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> DynamicPropertiesGetDictionaryItems (string typeName, string propertyId)
        {
             ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> response = DynamicPropertiesGetDictionaryItemsWithHttpInfo(typeName, propertyId);
             return response.Data;
        }

        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> > DynamicPropertiesGetDictionaryItemsWithHttpInfo (string typeName, string propertyId)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesGetDictionaryItems");
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesGetDictionaryItems");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>)));
            
        }
    
        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsAsync (string typeName, string propertyId)
        {
             ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> response = await DynamicPropertiesGetDictionaryItemsAsyncWithHttpInfo(typeName, propertyId);
             return response.Data;

        }

        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>>> DynamicPropertiesGetDictionaryItemsAsyncWithHttpInfo (string typeName, string propertyId)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesGetDictionaryItems");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesGetDictionaryItems");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>)));
            
        }
        
        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <param name="items"></param> 
        /// <returns></returns>
        public void DynamicPropertiesSaveDictionaryItems (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items)
        {
             DynamicPropertiesSaveDictionaryItemsWithHttpInfo(typeName, propertyId, items);
        }

        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <param name="items"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> DynamicPropertiesSaveDictionaryItemsWithHttpInfo (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesSaveDictionaryItems");
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesSaveDictionaryItems");
            
            // verify the required parameter 'items' is set
            if (items == null)
                throw new ApiException(400, "Missing required parameter 'items' when calling VirtoCommercePlatformApi->DynamicPropertiesSaveDictionaryItems");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            if (items.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(items); // http body (model) parameter
            }
            else
            {
                postBody = items; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DynamicPropertiesSaveDictionaryItemsAsync (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items)
        {
             await DynamicPropertiesSaveDictionaryItemsAsyncWithHttpInfo(typeName, propertyId, items);

        }

        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesSaveDictionaryItemsAsyncWithHttpInfo (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesSaveDictionaryItems");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesSaveDictionaryItems");
            // verify the required parameter 'items' is set
            if (items == null) throw new ApiException(400, "Missing required parameter 'items' when calling DynamicPropertiesSaveDictionaryItems");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(items); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <param name="ids">IDs of dictionary items to delete.</param> 
        /// <returns></returns>
        public void DynamicPropertiesDeleteDictionaryItem (string typeName, string propertyId, List<string> ids)
        {
             DynamicPropertiesDeleteDictionaryItemWithHttpInfo(typeName, propertyId, ids);
        }

        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <param name="ids">IDs of dictionary items to delete.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> DynamicPropertiesDeleteDictionaryItemWithHttpInfo (string typeName, string propertyId, List<string> ids)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteDictionaryItem");
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteDictionaryItem");
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteDictionaryItem");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DynamicPropertiesDeleteDictionaryItemAsync (string typeName, string propertyId, List<string> ids)
        {
             await DynamicPropertiesDeleteDictionaryItemAsyncWithHttpInfo(typeName, propertyId, ids);

        }

        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesDeleteDictionaryItemAsyncWithHttpInfo (string typeName, string propertyId, List<string> ids)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesDeleteDictionaryItem");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesDeleteDictionaryItem");
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling DynamicPropertiesDeleteDictionaryItem");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            if (typeName != null) pathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <param name="id">Job ID.</param> 
        /// <returns>VirtoCommercePlatformWebModelJobsJob</returns>
        public VirtoCommercePlatformWebModelJobsJob JobsGetStatus (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelJobsJob> response = JobsGetStatusWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <param name="id">Job ID.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelJobsJob</returns>
        public ApiResponse< VirtoCommercePlatformWebModelJobsJob > JobsGetStatusWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->JobsGetStatus");
            
    
            var path_ = "/api/platform/jobs/{id}";
    
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
                throw new ApiException (statusCode, "Error calling JobsGetStatus: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling JobsGetStatus: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelJobsJob>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelJobsJob) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelJobsJob)));
            
        }
    
        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelJobsJob</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelJobsJob> JobsGetStatusAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelJobsJob> response = await JobsGetStatusAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelJobsJob)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelJobsJob>> JobsGetStatusAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling JobsGetStatus");
            
    
            var path_ = "/api/platform/jobs/{id}";
    
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
                throw new ApiException (statusCode, "Error calling JobsGetStatus: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling JobsGetStatus: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelJobsJob>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelJobsJob) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelJobsJob)));
            
        }
        
        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <returns>List&lt;string&gt;</returns>
        public List<string> LocalizationGetLocales ()
        {
             ApiResponse<List<string>> response = LocalizationGetLocalesWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        public ApiResponse< List<string> > LocalizationGetLocalesWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/localization/locales";
    
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
                throw new ApiException (statusCode, "Error calling LocalizationGetLocales: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling LocalizationGetLocales: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<string>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>) Configuration.ApiClient.Deserialize(response, typeof(List<string>)));
            
        }
    
        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <returns>Task of List&lt;string&gt;</returns>
        public async System.Threading.Tasks.Task<List<string>> LocalizationGetLocalesAsync ()
        {
             ApiResponse<List<string>> response = await LocalizationGetLocalesAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<string>>> LocalizationGetLocalesAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/localization/locales";
    
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
                throw new ApiException (statusCode, "Error calling LocalizationGetLocales: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling LocalizationGetLocales: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<string>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>) Configuration.ApiClient.Deserialize(response, typeof(List<string>)));
            
        }
        
        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <returns>List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        public List<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModules ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> response = ModulesGetModulesWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelPackagingModuleDescriptor> > ModulesGetModulesWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/modules";
    
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
                throw new ApiException (statusCode, "Error calling ModulesGetModules: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesGetModules: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>)));
            
        }
    
        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModulesAsync ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> response = await ModulesGetModulesAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>>> ModulesGetModulesAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/modules";
    
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
                throw new ApiException (statusCode, "Error calling ModulesGetModules: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesGetModules: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>)));
            
        }
        
        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesUpload ()
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> response = ModulesUploadWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public ApiResponse< VirtoCommercePlatformWebModelPackagingModuleDescriptor > ModulesUploadWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/modules";
    
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
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling ModulesUpload: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesUpload: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModuleDescriptor) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor)));
            
        }
    
        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesUploadAsync ()
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> response = await ModulesUploadAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModuleDescriptor)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesUploadAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/modules";
    
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
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling ModulesUpload: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesUpload: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModuleDescriptor) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor)));
            
        }
        
        /// <summary>
        /// Install module from uploaded file 
        /// </summary>
        /// <param name="fileName">Module package file name.</param> 
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesInstallModule (string fileName)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> response = ModulesInstallModuleWithHttpInfo(fileName);
             return response.Data;
        }

        /// <summary>
        /// Install module from uploaded file 
        /// </summary>
        /// <param name="fileName">Module package file name.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public ApiResponse< VirtoCommercePlatformWebModelPackagingModulePushNotification > ModulesInstallModuleWithHttpInfo (string fileName)
        {
            
            // verify the required parameter 'fileName' is set
            if (fileName == null)
                throw new ApiException(400, "Missing required parameter 'fileName' when calling VirtoCommercePlatformApi->ModulesInstallModule");
            
    
            var path_ = "/api/platform/modules/install";
    
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
            
            if (fileName != null) queryParams.Add("fileName", Configuration.ApiClient.ParameterToString(fileName)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling ModulesInstallModule: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesInstallModule: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }
    
        /// <summary>
        /// Install module from uploaded file 
        /// </summary>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesInstallModuleAsync (string fileName)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> response = await ModulesInstallModuleAsyncWithHttpInfo(fileName);
             return response.Data;

        }

        /// <summary>
        /// Install module from uploaded file 
        /// </summary>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesInstallModuleAsyncWithHttpInfo (string fileName)
        {
            // verify the required parameter 'fileName' is set
            if (fileName == null) throw new ApiException(400, "Missing required parameter 'fileName' when calling ModulesInstallModule");
            
    
            var path_ = "/api/platform/modules/install";
    
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
            
            if (fileName != null) queryParams.Add("fileName", Configuration.ApiClient.ParameterToString(fileName)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling ModulesInstallModule: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesInstallModule: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }
        
        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <returns></returns>
        public void ModulesRestart ()
        {
             ModulesRestartWithHttpInfo();
        }

        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ModulesRestartWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/modules/restart";
    
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
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling ModulesRestart: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesRestart: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ModulesRestartAsync ()
        {
             await ModulesRestartAsyncWithHttpInfo();

        }

        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ModulesRestartAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/modules/restart";
    
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
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling ModulesRestart: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesRestart: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get module details 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesGetModuleById (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> response = ModulesGetModuleByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get module details 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public ApiResponse< VirtoCommercePlatformWebModelPackagingModuleDescriptor > ModulesGetModuleByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->ModulesGetModuleById");
            
    
            var path_ = "/api/platform/modules/{id}";
    
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
                throw new ApiException (statusCode, "Error calling ModulesGetModuleById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesGetModuleById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModuleDescriptor) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor)));
            
        }
    
        /// <summary>
        /// Get module details 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModuleByIdAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> response = await ModulesGetModuleByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get module details 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModuleDescriptor)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModuleByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ModulesGetModuleById");
            
    
            var path_ = "/api/platform/modules/{id}";
    
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
                throw new ApiException (statusCode, "Error calling ModulesGetModuleById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesGetModuleById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModuleDescriptor) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor)));
            
        }
        
        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesUninstallModule (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> response = ModulesUninstallModuleWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public ApiResponse< VirtoCommercePlatformWebModelPackagingModulePushNotification > ModulesUninstallModuleWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->ModulesUninstallModule");
            
    
            var path_ = "/api/platform/modules/{id}/uninstall";
    
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
                throw new ApiException (statusCode, "Error calling ModulesUninstallModule: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesUninstallModule: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }
    
        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUninstallModuleAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> response = await ModulesUninstallModuleAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesUninstallModuleAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ModulesUninstallModule");
            
    
            var path_ = "/api/platform/modules/{id}/uninstall";
    
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
                throw new ApiException (statusCode, "Error calling ModulesUninstallModule: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesUninstallModule: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }
        
        /// <summary>
        /// Update module from uploaded file 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <param name="fileName">Module package file name.</param> 
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesUpdateModule (string id, string fileName)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> response = ModulesUpdateModuleWithHttpInfo(id, fileName);
             return response.Data;
        }

        /// <summary>
        /// Update module from uploaded file 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <param name="fileName">Module package file name.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public ApiResponse< VirtoCommercePlatformWebModelPackagingModulePushNotification > ModulesUpdateModuleWithHttpInfo (string id, string fileName)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->ModulesUpdateModule");
            
            // verify the required parameter 'fileName' is set
            if (fileName == null)
                throw new ApiException(400, "Missing required parameter 'fileName' when calling VirtoCommercePlatformApi->ModulesUpdateModule");
            
    
            var path_ = "/api/platform/modules/{id}/update";
    
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
            
            if (fileName != null) queryParams.Add("fileName", Configuration.ApiClient.ParameterToString(fileName)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling ModulesUpdateModule: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesUpdateModule: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }
    
        /// <summary>
        /// Update module from uploaded file 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUpdateModuleAsync (string id, string fileName)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> response = await ModulesUpdateModuleAsyncWithHttpInfo(id, fileName);
             return response.Data;

        }

        /// <summary>
        /// Update module from uploaded file 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesUpdateModuleAsyncWithHttpInfo (string id, string fileName)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ModulesUpdateModule");
            // verify the required parameter 'fileName' is set
            if (fileName == null) throw new ApiException(400, "Missing required parameter 'fileName' when calling ModulesUpdateModule");
            
    
            var path_ = "/api/platform/modules/{id}/update";
    
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
            
            if (fileName != null) queryParams.Add("fileName", Configuration.ApiClient.ParameterToString(fileName)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling ModulesUpdateModule: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ModulesUpdateModule: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }
        
        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <returns>List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        public List<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotifications ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>> response = NotificationsGetNotificationsWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelNotificationsNotification> > NotificationsGetNotificationsWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/notification";
    
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
                throw new ApiException (statusCode, "Error calling NotificationsGetNotifications: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotifications: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelNotificationsNotification>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelNotificationsNotification>)));
            
        }
    
        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationsAsync ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>> response = await NotificationsGetNotificationsAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>>> NotificationsGetNotificationsAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/notification";
    
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
                throw new ApiException (statusCode, "Error calling NotificationsGetNotifications: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotifications: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelNotificationsNotification>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelNotificationsNotification>)));
            
        }
        
        /// <summary>
        /// Get notification journal page Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </summary>
        /// <param name="objectId">Object id</param> 
        /// <param name="objectTypeId">Object type id</param> 
        /// <param name="start">Page setting start</param> 
        /// <param name="count">Page setting count</param> 
        /// <returns>VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        public VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult NotificationsGetNotificationJournal (string objectId, string objectTypeId, int? start, int? count)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> response = NotificationsGetNotificationJournalWithHttpInfo(objectId, objectTypeId, start, count);
             return response.Data;
        }

        /// <summary>
        /// Get notification journal page Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </summary>
        /// <param name="objectId">Object id</param> 
        /// <param name="objectTypeId">Object type id</param> 
        /// <param name="start">Page setting start</param> 
        /// <param name="count">Page setting count</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        public ApiResponse< VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult > NotificationsGetNotificationJournalWithHttpInfo (string objectId, string objectTypeId, int? start, int? count)
        {
            
            // verify the required parameter 'objectId' is set
            if (objectId == null)
                throw new ApiException(400, "Missing required parameter 'objectId' when calling VirtoCommercePlatformApi->NotificationsGetNotificationJournal");
            
            // verify the required parameter 'objectTypeId' is set
            if (objectTypeId == null)
                throw new ApiException(400, "Missing required parameter 'objectTypeId' when calling VirtoCommercePlatformApi->NotificationsGetNotificationJournal");
            
            // verify the required parameter 'start' is set
            if (start == null)
                throw new ApiException(400, "Missing required parameter 'start' when calling VirtoCommercePlatformApi->NotificationsGetNotificationJournal");
            
            // verify the required parameter 'count' is set
            if (count == null)
                throw new ApiException(400, "Missing required parameter 'count' when calling VirtoCommercePlatformApi->NotificationsGetNotificationJournal");
            
    
            var path_ = "/api/platform/notification/journal/{objectId}/{objectTypeId}";
    
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
            if (objectId != null) pathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter
            
            if (start != null) queryParams.Add("start", Configuration.ApiClient.ParameterToString(start)); // query parameter
            if (count != null) queryParams.Add("count", Configuration.ApiClient.ParameterToString(count)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationJournal: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationJournal: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult)));
            
        }
    
        /// <summary>
        /// Get notification journal page Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </summary>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> NotificationsGetNotificationJournalAsync (string objectId, string objectTypeId, int? start, int? count)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> response = await NotificationsGetNotificationJournalAsyncWithHttpInfo(objectId, objectTypeId, start, count);
             return response.Data;

        }

        /// <summary>
        /// Get notification journal page Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </summary>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult>> NotificationsGetNotificationJournalAsyncWithHttpInfo (string objectId, string objectTypeId, int? start, int? count)
        {
            // verify the required parameter 'objectId' is set
            if (objectId == null) throw new ApiException(400, "Missing required parameter 'objectId' when calling NotificationsGetNotificationJournal");
            // verify the required parameter 'objectTypeId' is set
            if (objectTypeId == null) throw new ApiException(400, "Missing required parameter 'objectTypeId' when calling NotificationsGetNotificationJournal");
            // verify the required parameter 'start' is set
            if (start == null) throw new ApiException(400, "Missing required parameter 'start' when calling NotificationsGetNotificationJournal");
            // verify the required parameter 'count' is set
            if (count == null) throw new ApiException(400, "Missing required parameter 'count' when calling NotificationsGetNotificationJournal");
            
    
            var path_ = "/api/platform/notification/journal/{objectId}/{objectTypeId}";
    
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
            if (objectId != null) pathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter
            
            if (start != null) queryParams.Add("start", Configuration.ApiClient.ParameterToString(start)); // query parameter
            if (count != null) queryParams.Add("count", Configuration.ApiClient.ParameterToString(count)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationJournal: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationJournal: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult)));
            
        }
        
        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <param name="id">Sending notification id</param> 
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotification</returns>
        public VirtoCommercePlatformWebModelNotificationsNotification NotificationsGetNotification (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification> response = NotificationsGetNotificationWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <param name="id">Sending notification id</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsNotification</returns>
        public ApiResponse< VirtoCommercePlatformWebModelNotificationsNotification > NotificationsGetNotificationWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsGetNotification");
            
    
            var path_ = "/api/platform/notification/notification/{id}";
    
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
                throw new ApiException (statusCode, "Error calling NotificationsGetNotification: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotification: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsNotification)));
            
        }
    
        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotificationAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification> response = await NotificationsGetNotificationAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling NotificationsGetNotification");
            
    
            var path_ = "/api/platform/notification/notification/{id}";
    
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
                throw new ApiException (statusCode, "Error calling NotificationsGetNotification: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotification: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsNotification)));
            
        }
        
        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <param name="ids">Stop sending notification ids</param> 
        /// <returns></returns>
        public void NotificationsStopSendingNotifications (List<string> ids)
        {
             NotificationsStopSendingNotificationsWithHttpInfo(ids);
        }

        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <param name="ids">Stop sending notification ids</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> NotificationsStopSendingNotificationsWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->NotificationsStopSendingNotifications");
            
    
            var path_ = "/api/platform/notification/stopnotifications";
    
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
            
            
            
            
            if (ids.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(ids); // http body (model) parameter
            }
            else
            {
                postBody = ids; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsStopSendingNotifications: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsStopSendingNotifications: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task NotificationsStopSendingNotificationsAsync (List<string> ids)
        {
             await NotificationsStopSendingNotificationsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsStopSendingNotificationsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling NotificationsStopSendingNotifications");
            
    
            var path_ = "/api/platform/notification/stopnotifications";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(ids); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsStopSendingNotifications: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsStopSendingNotifications: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <param name="notificationTemplate">Notification template</param> 
        /// <returns></returns>
        public void NotificationsUpdateNotificationTemplate (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate)
        {
             NotificationsUpdateNotificationTemplateWithHttpInfo(notificationTemplate);
        }

        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <param name="notificationTemplate">Notification template</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> NotificationsUpdateNotificationTemplateWithHttpInfo (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate)
        {
            
            // verify the required parameter 'notificationTemplate' is set
            if (notificationTemplate == null)
                throw new ApiException(400, "Missing required parameter 'notificationTemplate' when calling VirtoCommercePlatformApi->NotificationsUpdateNotificationTemplate");
            
    
            var path_ = "/api/platform/notification/template";
    
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
            
            
            
            
            if (notificationTemplate.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(notificationTemplate); // http body (model) parameter
            }
            else
            {
                postBody = notificationTemplate; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsUpdateNotificationTemplate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsUpdateNotificationTemplate: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task NotificationsUpdateNotificationTemplateAsync (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate)
        {
             await NotificationsUpdateNotificationTemplateAsyncWithHttpInfo(notificationTemplate);

        }

        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsUpdateNotificationTemplateAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate)
        {
            // verify the required parameter 'notificationTemplate' is set
            if (notificationTemplate == null) throw new ApiException(400, "Missing required parameter 'notificationTemplate' when calling NotificationsUpdateNotificationTemplate");
            
    
            var path_ = "/api/platform/notification/template";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(notificationTemplate); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsUpdateNotificationTemplate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsUpdateNotificationTemplate: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <param name="request">Test notification request</param> 
        /// <returns>VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        public VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult NotificationsRenderNotificationContent (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> response = NotificationsRenderNotificationContentWithHttpInfo(request);
             return response.Data;
        }

        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <param name="request">Test notification request</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        public ApiResponse< VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult > NotificationsRenderNotificationContentWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->NotificationsRenderNotificationContent");
            
    
            var path_ = "/api/platform/notification/template/rendernotificationcontent";
    
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
            
            
            
            
            if (request.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                postBody = request; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsRenderNotificationContent: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsRenderNotificationContent: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult)));
            
        }
    
        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> NotificationsRenderNotificationContentAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> response = await NotificationsRenderNotificationContentAsyncWithHttpInfo(request);
             return response.Data;

        }

        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult>> NotificationsRenderNotificationContentAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling NotificationsRenderNotificationContent");
            
    
            var path_ = "/api/platform/notification/template/rendernotificationcontent";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsRenderNotificationContent: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsRenderNotificationContent: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult)));
            
        }
        
        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </summary>
        /// <param name="request">Test notification request</param> 
        /// <returns>string</returns>
        public string NotificationsSendNotification (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
             ApiResponse<string> response = NotificationsSendNotificationWithHttpInfo(request);
             return response.Data;
        }

        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </summary>
        /// <param name="request">Test notification request</param> 
        /// <returns>ApiResponse of string</returns>
        public ApiResponse< string > NotificationsSendNotificationWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->NotificationsSendNotification");
            
    
            var path_ = "/api/platform/notification/template/sendnotification";
    
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
            
            
            
            
            if (request.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                postBody = request; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsSendNotification: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsSendNotification: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<string>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (string) Configuration.ApiClient.Deserialize(response, typeof(string)));
            
        }
    
        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </summary>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of string</returns>
        public async System.Threading.Tasks.Task<string> NotificationsSendNotificationAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
             ApiResponse<string> response = await NotificationsSendNotificationAsyncWithHttpInfo(request);
             return response.Data;

        }

        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </summary>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (string)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<string>> NotificationsSendNotificationAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling NotificationsSendNotification");
            
    
            var path_ = "/api/platform/notification/template/sendnotification";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsSendNotification: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsSendNotification: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<string>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (string) Configuration.ApiClient.Deserialize(response, typeof(string)));
            
        }
        
        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <param name="id">Template id</param> 
        /// <returns></returns>
        public void NotificationsDeleteNotificationTemplate (string id)
        {
             NotificationsDeleteNotificationTemplateWithHttpInfo(id);
        }

        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <param name="id">Template id</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> NotificationsDeleteNotificationTemplateWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsDeleteNotificationTemplate");
            
    
            var path_ = "/api/platform/notification/template/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsDeleteNotificationTemplate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsDeleteNotificationTemplate: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <param name="id">Template id</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task NotificationsDeleteNotificationTemplateAsync (string id)
        {
             await NotificationsDeleteNotificationTemplateAsyncWithHttpInfo(id);

        }

        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <param name="id">Template id</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsDeleteNotificationTemplateAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling NotificationsDeleteNotificationTemplate");
            
    
            var path_ = "/api/platform/notification/template/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsDeleteNotificationTemplate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsDeleteNotificationTemplate: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <param name="type">Notification type</param> 
        /// <returns>List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        public List<VirtoCommercePlatformCoreNotificationsNotificationParameter> NotificationsGetTestingParameters (string type)
        {
             ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> response = NotificationsGetTestingParametersWithHttpInfo(type);
             return response.Data;
        }

        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <param name="type">Notification type</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformCoreNotificationsNotificationParameter> > NotificationsGetTestingParametersWithHttpInfo (string type)
        {
            
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetTestingParameters");
            
    
            var path_ = "/api/platform/notification/template/{type}/getTestingParameters";
    
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
            if (type != null) pathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsGetTestingParameters: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetTestingParameters: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreNotificationsNotificationParameter>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreNotificationsNotificationParameter>)));
            
        }
    
        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> NotificationsGetTestingParametersAsync (string type)
        {
             ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> response = await NotificationsGetTestingParametersAsyncWithHttpInfo(type);
             return response.Data;

        }

        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>>> NotificationsGetTestingParametersAsyncWithHttpInfo (string type)
        {
            // verify the required parameter 'type' is set
            if (type == null) throw new ApiException(400, "Missing required parameter 'type' when calling NotificationsGetTestingParameters");
            
    
            var path_ = "/api/platform/notification/template/{type}/getTestingParameters";
    
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
            if (type != null) pathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsGetTestingParameters: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetTestingParameters: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreNotificationsNotificationParameter>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreNotificationsNotificationParameter>)));
            
        }
        
        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param> 
        /// <param name="objectId">Object id of template</param> 
        /// <param name="objectTypeId">Object type id of template</param> 
        /// <returns>List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        public List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplates (string type, string objectId, string objectTypeId)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> response = NotificationsGetNotificationTemplatesWithHttpInfo(type, objectId, objectTypeId);
             return response.Data;
        }

        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param> 
        /// <param name="objectId">Object id of template</param> 
        /// <param name="objectTypeId">Object type id of template</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> > NotificationsGetNotificationTemplatesWithHttpInfo (string type, string objectId, string objectTypeId)
        {
            
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplates");
            
            // verify the required parameter 'objectId' is set
            if (objectId == null)
                throw new ApiException(400, "Missing required parameter 'objectId' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplates");
            
            // verify the required parameter 'objectTypeId' is set
            if (objectTypeId == null)
                throw new ApiException(400, "Missing required parameter 'objectTypeId' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplates");
            
    
            var path_ = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}";
    
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
            if (type != null) pathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) pathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationTemplates: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationTemplates: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>)));
            
        }
    
        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplatesAsync (string type, string objectId, string objectTypeId)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> response = await NotificationsGetNotificationTemplatesAsyncWithHttpInfo(type, objectId, objectTypeId);
             return response.Data;

        }

        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>>> NotificationsGetNotificationTemplatesAsyncWithHttpInfo (string type, string objectId, string objectTypeId)
        {
            // verify the required parameter 'type' is set
            if (type == null) throw new ApiException(400, "Missing required parameter 'type' when calling NotificationsGetNotificationTemplates");
            // verify the required parameter 'objectId' is set
            if (objectId == null) throw new ApiException(400, "Missing required parameter 'objectId' when calling NotificationsGetNotificationTemplates");
            // verify the required parameter 'objectTypeId' is set
            if (objectTypeId == null) throw new ApiException(400, "Missing required parameter 'objectTypeId' when calling NotificationsGetNotificationTemplates");
            
    
            var path_ = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}";
    
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
            if (type != null) pathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) pathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationTemplates: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationTemplates: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>)));
            
        }
        
        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param> 
        /// <param name="objectId">Object id of template</param> 
        /// <param name="objectTypeId">Object type id of template</param> 
        /// <param name="language">Locale of template</param> 
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        public VirtoCommercePlatformWebModelNotificationsNotificationTemplate NotificationsGetNotificationTemplate (string type, string objectId, string objectTypeId, string language)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> response = NotificationsGetNotificationTemplateWithHttpInfo(type, objectId, objectTypeId, language);
             return response.Data;
        }

        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param> 
        /// <param name="objectId">Object id of template</param> 
        /// <param name="objectTypeId">Object type id of template</param> 
        /// <param name="language">Locale of template</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        public ApiResponse< VirtoCommercePlatformWebModelNotificationsNotificationTemplate > NotificationsGetNotificationTemplateWithHttpInfo (string type, string objectId, string objectTypeId, string language)
        {
            
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplate");
            
            // verify the required parameter 'objectId' is set
            if (objectId == null)
                throw new ApiException(400, "Missing required parameter 'objectId' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplate");
            
            // verify the required parameter 'objectTypeId' is set
            if (objectTypeId == null)
                throw new ApiException(400, "Missing required parameter 'objectTypeId' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplate");
            
            // verify the required parameter 'language' is set
            if (language == null)
                throw new ApiException(400, "Missing required parameter 'language' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplate");
            
    
            var path_ = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}/{language}";
    
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
            if (type != null) pathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) pathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter
            if (language != null) pathParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationTemplate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationTemplate: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsNotificationTemplate) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsNotificationTemplate)));
            
        }
    
        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplateAsync (string type, string objectId, string objectTypeId, string language)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> response = await NotificationsGetNotificationTemplateAsyncWithHttpInfo(type, objectId, objectTypeId, language);
             return response.Data;

        }

        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsNotificationTemplate)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplateAsyncWithHttpInfo (string type, string objectId, string objectTypeId, string language)
        {
            // verify the required parameter 'type' is set
            if (type == null) throw new ApiException(400, "Missing required parameter 'type' when calling NotificationsGetNotificationTemplate");
            // verify the required parameter 'objectId' is set
            if (objectId == null) throw new ApiException(400, "Missing required parameter 'objectId' when calling NotificationsGetNotificationTemplate");
            // verify the required parameter 'objectTypeId' is set
            if (objectTypeId == null) throw new ApiException(400, "Missing required parameter 'objectTypeId' when calling NotificationsGetNotificationTemplate");
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling NotificationsGetNotificationTemplate");
            
    
            var path_ = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}/{language}";
    
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
            if (type != null) pathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) pathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter
            if (language != null) pathParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationTemplate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling NotificationsGetNotificationTemplate: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsNotificationTemplate) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsNotificationTemplate)));
            
        }
        
        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <param name="criteria">Search parameters.</param> 
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationSearch (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria)
        {
             ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> response = PushNotificationSearchWithHttpInfo(criteria);
             return response.Data;
        }

        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <param name="criteria">Search parameters.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public ApiResponse< VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult > PushNotificationSearchWithHttpInfo (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria)
        {
            
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling VirtoCommercePlatformApi->PushNotificationSearch");
            
    
            var path_ = "/api/platform/pushnotifications";
    
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
                throw new ApiException (statusCode, "Error calling PushNotificationSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PushNotificationSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)));
            
        }
    
        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationSearchAsync (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria)
        {
             ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> response = await PushNotificationSearchAsyncWithHttpInfo(criteria);
             return response.Data;

        }

        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>> PushNotificationSearchAsyncWithHttpInfo (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null) throw new ApiException(400, "Missing required parameter 'criteria' when calling PushNotificationSearch");
            
    
            var path_ = "/api/platform/pushnotifications";
    
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
                throw new ApiException (statusCode, "Error calling PushNotificationSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PushNotificationSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)));
            
        }
        
        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationMarkAllAsRead ()
        {
             ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> response = PushNotificationMarkAllAsReadWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <returns>ApiResponse of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public ApiResponse< VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult > PushNotificationMarkAllAsReadWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/pushnotifications/markAllAsRead";
    
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
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling PushNotificationMarkAllAsRead: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PushNotificationMarkAllAsRead: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)));
            
        }
    
        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <returns>Task of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationMarkAllAsReadAsync ()
        {
             ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> response = await PushNotificationMarkAllAsReadAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>> PushNotificationMarkAllAsReadAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/pushnotifications/markAllAsRead";
    
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
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling PushNotificationMarkAllAsRead: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PushNotificationMarkAllAsRead: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)));
            
        }
        
        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <param name="type"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApiAccount</returns>
        public VirtoCommercePlatformCoreSecurityApiAccount SecurityGenerateNewApiAccount (string type)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount> response = SecurityGenerateNewApiAccountWithHttpInfo(type);
             return response.Data;
        }

        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <param name="type"></param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApiAccount</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApiAccount > SecurityGenerateNewApiAccountWithHttpInfo (string type)
        {
            
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->SecurityGenerateNewApiAccount");
            
    
            var path_ = "/api/platform/security/apiaccounts/new";
    
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
            
            if (type != null) queryParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityGenerateNewApiAccount: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGenerateNewApiAccount: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApiAccount) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApiAccount)));
            
        }
    
        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApiAccount</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApiAccount> SecurityGenerateNewApiAccountAsync (string type)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount> response = await SecurityGenerateNewApiAccountAsyncWithHttpInfo(type);
             return response.Data;

        }

        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApiAccount)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount>> SecurityGenerateNewApiAccountAsyncWithHttpInfo (string type)
        {
            // verify the required parameter 'type' is set
            if (type == null) throw new ApiException(400, "Missing required parameter 'type' when calling SecurityGenerateNewApiAccount");
            
    
            var path_ = "/api/platform/security/apiaccounts/new";
    
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
            
            if (type != null) queryParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityGenerateNewApiAccount: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGenerateNewApiAccount: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApiAccount) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApiAccount)));
            
        }
        
        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetCurrentUser ()
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = SecurityGetCurrentUserWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > SecurityGetCurrentUserWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/security/currentuser";
    
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
                throw new ApiException (statusCode, "Error calling SecurityGetCurrentUser: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGetCurrentUser: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
    
        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetCurrentUserAsync ()
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = await SecurityGetCurrentUserAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetCurrentUserAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/security/currentuser";
    
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
                throw new ApiException (statusCode, "Error calling SecurityGetCurrentUser: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGetCurrentUser: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
        
        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <param name="model">User credentials.</param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityLogin (VirtoCommercePlatformWebModelSecurityUserLogin model)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = SecurityLoginWithHttpInfo(model);
             return response.Data;
        }

        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <param name="model">User credentials.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > SecurityLoginWithHttpInfo (VirtoCommercePlatformWebModelSecurityUserLogin model)
        {
            
            // verify the required parameter 'model' is set
            if (model == null)
                throw new ApiException(400, "Missing required parameter 'model' when calling VirtoCommercePlatformApi->SecurityLogin");
            
    
            var path_ = "/api/platform/security/login";
    
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
            
            
            
            
            if (model.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(model); // http body (model) parameter
            }
            else
            {
                postBody = model; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityLogin: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityLogin: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
    
        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityLoginAsync (VirtoCommercePlatformWebModelSecurityUserLogin model)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = await SecurityLoginAsyncWithHttpInfo(model);
             return response.Data;

        }

        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityLoginAsyncWithHttpInfo (VirtoCommercePlatformWebModelSecurityUserLogin model)
        {
            // verify the required parameter 'model' is set
            if (model == null) throw new ApiException(400, "Missing required parameter 'model' when calling SecurityLogin");
            
    
            var path_ = "/api/platform/security/login";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(model); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityLogin: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityLogin: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
        
        /// <summary>
        /// Sign out 
        /// </summary>
        /// <returns></returns>
        public void SecurityLogout ()
        {
             SecurityLogoutWithHttpInfo();
        }

        /// <summary>
        /// Sign out 
        /// </summary>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> SecurityLogoutWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/security/logout";
    
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
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityLogout: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityLogout: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Sign out 
        /// </summary>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SecurityLogoutAsync ()
        {
             await SecurityLogoutAsyncWithHttpInfo();

        }

        /// <summary>
        /// Sign out 
        /// </summary>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> SecurityLogoutAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/security/logout";
    
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
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityLogout: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityLogout: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <returns>List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        public List<VirtoCommercePlatformCoreSecurityPermission> SecurityGetPermissions ()
        {
             ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>> response = SecurityGetPermissionsWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformCoreSecurityPermission> > SecurityGetPermissionsWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/security/permissions";
    
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
                throw new ApiException (statusCode, "Error calling SecurityGetPermissions: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGetPermissions: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreSecurityPermission>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreSecurityPermission>)));
            
        }
    
        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreSecurityPermission>> SecurityGetPermissionsAsync ()
        {
             ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>> response = await SecurityGetPermissionsAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>>> SecurityGetPermissionsAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/security/permissions";
    
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
                throw new ApiException (statusCode, "Error calling SecurityGetPermissions: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGetPermissions: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreSecurityPermission>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreSecurityPermission>)));
            
        }
        
        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <param name="role"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        public VirtoCommercePlatformCoreSecurityRole SecurityUpdateRole (VirtoCommercePlatformCoreSecurityRole role)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRole> response = SecurityUpdateRoleWithHttpInfo(role);
             return response.Data;
        }

        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <param name="role"></param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRole</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityRole > SecurityUpdateRoleWithHttpInfo (VirtoCommercePlatformCoreSecurityRole role)
        {
            
            // verify the required parameter 'role' is set
            if (role == null)
                throw new ApiException(400, "Missing required parameter 'role' when calling VirtoCommercePlatformApi->SecurityUpdateRole");
            
    
            var path_ = "/api/platform/security/roles";
    
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
            
            
            
            
            if (role.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(role); // http body (model) parameter
            }
            else
            {
                postBody = role; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityUpdateRole: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityUpdateRole: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityRole>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRole) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRole)));
            
        }
    
        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <param name="role"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRole</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityUpdateRoleAsync (VirtoCommercePlatformCoreSecurityRole role)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRole> response = await SecurityUpdateRoleAsyncWithHttpInfo(role);
             return response.Data;

        }

        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <param name="role"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRole)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRole>> SecurityUpdateRoleAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityRole role)
        {
            // verify the required parameter 'role' is set
            if (role == null) throw new ApiException(400, "Missing required parameter 'role' when calling SecurityUpdateRole");
            
    
            var path_ = "/api/platform/security/roles";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(role); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityUpdateRole: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityUpdateRole: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityRole>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRole) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRole)));
            
        }
        
        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <param name="request">Search parameters.</param> 
        /// <returns>VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        public VirtoCommercePlatformCoreSecurityRoleSearchResponse SecuritySearchRoles (VirtoCommercePlatformCoreSecurityRoleSearchRequest request)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse> response = SecuritySearchRolesWithHttpInfo(request);
             return response.Data;
        }

        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <param name="request">Search parameters.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityRoleSearchResponse > SecuritySearchRolesWithHttpInfo (VirtoCommercePlatformCoreSecurityRoleSearchRequest request)
        {
            
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->SecuritySearchRoles");
            
    
            var path_ = "/api/platform/security/roles";
    
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
            
            
            
            
            if (request.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                postBody = request; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecuritySearchRoles: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecuritySearchRoles: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRoleSearchResponse) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRoleSearchResponse)));
            
        }
    
        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRoleSearchResponse> SecuritySearchRolesAsync (VirtoCommercePlatformCoreSecurityRoleSearchRequest request)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse> response = await SecuritySearchRolesAsyncWithHttpInfo(request);
             return response.Data;

        }

        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRoleSearchResponse)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse>> SecuritySearchRolesAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityRoleSearchRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling SecuritySearchRoles");
            
    
            var path_ = "/api/platform/security/roles";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecuritySearchRoles: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecuritySearchRoles: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRoleSearchResponse) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRoleSearchResponse)));
            
        }
        
        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <param name="ids"></param> 
        /// <returns></returns>
        public void SecurityDeleteRoles (List<string> ids)
        {
             SecurityDeleteRolesWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <param name="ids"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> SecurityDeleteRolesWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->SecurityDeleteRoles");
            
    
            var path_ = "/api/platform/security/roles";
    
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
                throw new ApiException (statusCode, "Error calling SecurityDeleteRoles: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityDeleteRoles: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SecurityDeleteRolesAsync (List<string> ids)
        {
             await SecurityDeleteRolesAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> SecurityDeleteRolesAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling SecurityDeleteRoles");
            
    
            var path_ = "/api/platform/security/roles";
    
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
                throw new ApiException (statusCode, "Error calling SecurityDeleteRoles: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityDeleteRoles: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <param name="roleId"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        public VirtoCommercePlatformCoreSecurityRole SecurityGetRole (string roleId)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRole> response = SecurityGetRoleWithHttpInfo(roleId);
             return response.Data;
        }

        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <param name="roleId"></param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRole</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityRole > SecurityGetRoleWithHttpInfo (string roleId)
        {
            
            // verify the required parameter 'roleId' is set
            if (roleId == null)
                throw new ApiException(400, "Missing required parameter 'roleId' when calling VirtoCommercePlatformApi->SecurityGetRole");
            
    
            var path_ = "/api/platform/security/roles/{roleId}";
    
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
            if (roleId != null) pathParams.Add("roleId", Configuration.ApiClient.ParameterToString(roleId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityGetRole: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGetRole: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityRole>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRole) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRole)));
            
        }
    
        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRole</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityGetRoleAsync (string roleId)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRole> response = await SecurityGetRoleAsyncWithHttpInfo(roleId);
             return response.Data;

        }

        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRole)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRole>> SecurityGetRoleAsyncWithHttpInfo (string roleId)
        {
            // verify the required parameter 'roleId' is set
            if (roleId == null) throw new ApiException(400, "Missing required parameter 'roleId' when calling SecurityGetRole");
            
    
            var path_ = "/api/platform/security/roles/{roleId}";
    
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
            if (roleId != null) pathParams.Add("roleId", Configuration.ApiClient.ParameterToString(roleId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityGetRole: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGetRole: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityRole>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRole) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRole)));
            
        }
        
        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <param name="user">User details.</param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityUpdateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = SecurityUpdateAsyncWithHttpInfo(user);
             return response.Data;
        }

        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <param name="user">User details.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecuritySecurityResult > SecurityUpdateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling VirtoCommercePlatformApi->SecurityUpdateAsync");
            
    
            var path_ = "/api/platform/security/users";
    
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
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityUpdateAsync: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityUpdateAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
    
        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <param name="user">User details.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityUpdateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = await SecurityUpdateAsyncAsyncWithHttpInfo(user);
             return response.Data;

        }

        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityUpdateAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling SecurityUpdateAsync");
            
    
            var path_ = "/api/platform/security/users";
    
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
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityUpdateAsync: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityUpdateAsync: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
        
        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <param name="request">Search parameters.</param> 
        /// <returns>VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        public VirtoCommercePlatformCoreSecurityUserSearchResponse SecuritySearchUsersAsync (VirtoCommercePlatformCoreSecurityUserSearchRequest request)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse> response = SecuritySearchUsersAsyncWithHttpInfo(request);
             return response.Data;
        }

        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <param name="request">Search parameters.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityUserSearchResponse > SecuritySearchUsersAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityUserSearchRequest request)
        {
            
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->SecuritySearchUsersAsync");
            
    
            var path_ = "/api/platform/security/users";
    
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
            
            
            
            
            if (request.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                postBody = request; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecuritySearchUsersAsync: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecuritySearchUsersAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityUserSearchResponse) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityUserSearchResponse)));
            
        }
    
        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityUserSearchResponse> SecuritySearchUsersAsyncAsync (VirtoCommercePlatformCoreSecurityUserSearchRequest request)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse> response = await SecuritySearchUsersAsyncAsyncWithHttpInfo(request);
             return response.Data;

        }

        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityUserSearchResponse)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse>> SecuritySearchUsersAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityUserSearchRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling SecuritySearchUsersAsync");
            
    
            var path_ = "/api/platform/security/users";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecuritySearchUsersAsync: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecuritySearchUsersAsync: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityUserSearchResponse) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityUserSearchResponse)));
            
        }
        
        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <param name="names">An array of user names.</param> 
        /// <returns></returns>
        public void SecurityDeleteAsync (List<string> names)
        {
             SecurityDeleteAsyncWithHttpInfo(names);
        }

        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <param name="names">An array of user names.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> SecurityDeleteAsyncWithHttpInfo (List<string> names)
        {
            
            // verify the required parameter 'names' is set
            if (names == null)
                throw new ApiException(400, "Missing required parameter 'names' when calling VirtoCommercePlatformApi->SecurityDeleteAsync");
            
    
            var path_ = "/api/platform/security/users";
    
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
            
            if (names != null) queryParams.Add("names", Configuration.ApiClient.ParameterToString(names)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityDeleteAsync: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityDeleteAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SecurityDeleteAsyncAsync (List<string> names)
        {
             await SecurityDeleteAsyncAsyncWithHttpInfo(names);

        }

        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> SecurityDeleteAsyncAsyncWithHttpInfo (List<string> names)
        {
            // verify the required parameter 'names' is set
            if (names == null) throw new ApiException(400, "Missing required parameter 'names' when calling SecurityDeleteAsync");
            
    
            var path_ = "/api/platform/security/users";
    
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
            
            if (names != null) queryParams.Add("names", Configuration.ApiClient.ParameterToString(names)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityDeleteAsync: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityDeleteAsync: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create new user 
        /// </summary>
        /// <param name="user">User details.</param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = SecurityCreateAsyncWithHttpInfo(user);
             return response.Data;
        }

        /// <summary>
        /// Create new user 
        /// </summary>
        /// <param name="user">User details.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecuritySecurityResult > SecurityCreateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling VirtoCommercePlatformApi->SecurityCreateAsync");
            
    
            var path_ = "/api/platform/security/users/create";
    
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
                throw new ApiException (statusCode, "Error calling SecurityCreateAsync: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityCreateAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
    
        /// <summary>
        /// Create new user 
        /// </summary>
        /// <param name="user">User details.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityCreateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = await SecurityCreateAsyncAsyncWithHttpInfo(user);
             return response.Data;

        }

        /// <summary>
        /// Create new user 
        /// </summary>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityCreateAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling SecurityCreateAsync");
            
    
            var path_ = "/api/platform/security/users/create";
    
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
                throw new ApiException (statusCode, "Error calling SecurityCreateAsync: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityCreateAsync: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
        
        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetUserById (string id)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = SecurityGetUserByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > SecurityGetUserByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SecurityGetUserById");
            
    
            var path_ = "/api/platform/security/users/id/{id}";
    
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
                throw new ApiException (statusCode, "Error calling SecurityGetUserById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGetUserById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
    
        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByIdAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = await SecurityGetUserByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetUserByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SecurityGetUserById");
            
    
            var path_ = "/api/platform/security/users/id/{id}";
    
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
                throw new ApiException (statusCode, "Error calling SecurityGetUserById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGetUserById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
        
        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetUserByName (string userName)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = SecurityGetUserByNameWithHttpInfo(userName);
             return response.Data;
        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > SecurityGetUserByNameWithHttpInfo (string userName)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityGetUserByName");
            
    
            var path_ = "/api/platform/security/users/{userName}";
    
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
                throw new ApiException (statusCode, "Error calling SecurityGetUserByName: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGetUserByName: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
    
        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByNameAsync (string userName)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> response = await SecurityGetUserByNameAsyncWithHttpInfo(userName);
             return response.Data;

        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetUserByNameAsyncWithHttpInfo (string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling SecurityGetUserByName");
            
    
            var path_ = "/api/platform/security/users/{userName}";
    
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
                throw new ApiException (statusCode, "Error calling SecurityGetUserByName: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityGetUserByName: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }
        
        /// <summary>
        /// Change password 
        /// </summary>
        /// <param name="userName"></param> 
        /// <param name="changePassword">Old and new passwords.</param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityChangePassword (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = SecurityChangePasswordWithHttpInfo(userName, changePassword);
             return response.Data;
        }

        /// <summary>
        /// Change password 
        /// </summary>
        /// <param name="userName"></param> 
        /// <param name="changePassword">Old and new passwords.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecuritySecurityResult > SecurityChangePasswordWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityChangePassword");
            
            // verify the required parameter 'changePassword' is set
            if (changePassword == null)
                throw new ApiException(400, "Missing required parameter 'changePassword' when calling VirtoCommercePlatformApi->SecurityChangePassword");
            
    
            var path_ = "/api/platform/security/users/{userName}/changepassword";
    
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
            if (userName != null) pathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            if (changePassword.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(changePassword); // http body (model) parameter
            }
            else
            {
                postBody = changePassword; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityChangePassword: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityChangePassword: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
    
        /// <summary>
        /// Change password 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityChangePasswordAsync (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = await SecurityChangePasswordAsyncWithHttpInfo(userName, changePassword);
             return response.Data;

        }

        /// <summary>
        /// Change password 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityChangePasswordAsyncWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling SecurityChangePassword");
            // verify the required parameter 'changePassword' is set
            if (changePassword == null) throw new ApiException(400, "Missing required parameter 'changePassword' when calling SecurityChangePassword");
            
    
            var path_ = "/api/platform/security/users/{userName}/changepassword";
    
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
            if (userName != null) pathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(changePassword); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityChangePassword: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityChangePassword: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
        
        /// <summary>
        /// Reset password 
        /// </summary>
        /// <param name="userName"></param> 
        /// <param name="resetPassword">New password.</param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityResetPassword (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = SecurityResetPasswordWithHttpInfo(userName, resetPassword);
             return response.Data;
        }

        /// <summary>
        /// Reset password 
        /// </summary>
        /// <param name="userName"></param> 
        /// <param name="resetPassword">New password.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecuritySecurityResult > SecurityResetPasswordWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityResetPassword");
            
            // verify the required parameter 'resetPassword' is set
            if (resetPassword == null)
                throw new ApiException(400, "Missing required parameter 'resetPassword' when calling VirtoCommercePlatformApi->SecurityResetPassword");
            
    
            var path_ = "/api/platform/security/users/{userName}/resetpassword";
    
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
            if (userName != null) pathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            if (resetPassword.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(resetPassword); // http body (model) parameter
            }
            else
            {
                postBody = resetPassword; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityResetPassword: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityResetPassword: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
    
        /// <summary>
        /// Reset password 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityResetPasswordAsync (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> response = await SecurityResetPasswordAsyncWithHttpInfo(userName, resetPassword);
             return response.Data;

        }

        /// <summary>
        /// Reset password 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityResetPasswordAsyncWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling SecurityResetPassword");
            // verify the required parameter 'resetPassword' is set
            if (resetPassword == null) throw new ApiException(400, "Missing required parameter 'resetPassword' when calling SecurityResetPassword");
            
    
            var path_ = "/api/platform/security/users/{userName}/resetpassword";
    
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
            if (userName != null) pathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(resetPassword); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SecurityResetPassword: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SecurityResetPassword: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }
        
        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <returns>List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetAllSettings ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> response = SettingGetAllSettingsWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelSettingsSetting> > SettingGetAllSettingsWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/settings";
    
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
                throw new ApiException (statusCode, "Error calling SettingGetAllSettings: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SettingGetAllSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelSettingsSetting>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>)));
            
        }
    
        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetAllSettingsAsync ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> response = await SettingGetAllSettingsAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>> SettingGetAllSettingsAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/platform/settings";
    
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
                throw new ApiException (statusCode, "Error calling SettingGetAllSettings: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SettingGetAllSettings: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelSettingsSetting>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>)));
            
        }
        
        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <param name="settings"></param> 
        /// <returns></returns>
        public void SettingUpdate (List<VirtoCommercePlatformWebModelSettingsSetting> settings)
        {
             SettingUpdateWithHttpInfo(settings);
        }

        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <param name="settings"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> SettingUpdateWithHttpInfo (List<VirtoCommercePlatformWebModelSettingsSetting> settings)
        {
            
            // verify the required parameter 'settings' is set
            if (settings == null)
                throw new ApiException(400, "Missing required parameter 'settings' when calling VirtoCommercePlatformApi->SettingUpdate");
            
    
            var path_ = "/api/platform/settings";
    
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
            
            
            
            
            if (settings.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(settings); // http body (model) parameter
            }
            else
            {
                postBody = settings; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SettingUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SettingUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SettingUpdateAsync (List<VirtoCommercePlatformWebModelSettingsSetting> settings)
        {
             await SettingUpdateAsyncWithHttpInfo(settings);

        }

        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> SettingUpdateAsyncWithHttpInfo (List<VirtoCommercePlatformWebModelSettingsSetting> settings)
        {
            // verify the required parameter 'settings' is set
            if (settings == null) throw new ApiException(400, "Missing required parameter 'settings' when calling SettingUpdate");
            
    
            var path_ = "/api/platform/settings";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(settings); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SettingUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SettingUpdate: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <returns>List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetModuleSettings (string id)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> response = SettingGetModuleSettingsWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelSettingsSetting> > SettingGetModuleSettingsWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SettingGetModuleSettings");
            
    
            var path_ = "/api/platform/settings/modules/{id}";
    
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
                throw new ApiException (statusCode, "Error calling SettingGetModuleSettings: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SettingGetModuleSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelSettingsSetting>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>)));
            
        }
    
        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetModuleSettingsAsync (string id)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> response = await SettingGetModuleSettingsAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>> SettingGetModuleSettingsAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SettingGetModuleSettings");
            
    
            var path_ = "/api/platform/settings/modules/{id}";
    
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
                throw new ApiException (statusCode, "Error calling SettingGetModuleSettings: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SettingGetModuleSettings: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelSettingsSetting>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>)));
            
        }
        
        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <param name="id">Setting system name.</param> 
        /// <returns>VirtoCommercePlatformWebModelSettingsSetting</returns>
        public VirtoCommercePlatformWebModelSettingsSetting SettingGetSetting (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelSettingsSetting> response = SettingGetSettingWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <param name="id">Setting system name.</param> 
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelSettingsSetting</returns>
        public ApiResponse< VirtoCommercePlatformWebModelSettingsSetting > SettingGetSettingWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SettingGetSetting");
            
    
            var path_ = "/api/platform/settings/{id}";
    
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
                throw new ApiException (statusCode, "Error calling SettingGetSetting: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SettingGetSetting: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommercePlatformWebModelSettingsSetting>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelSettingsSetting) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelSettingsSetting)));
            
        }
    
        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <param name="id">Setting system name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelSettingsSetting</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelSettingsSetting> SettingGetSettingAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelSettingsSetting> response = await SettingGetSettingAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <param name="id">Setting system name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelSettingsSetting)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetSettingAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SettingGetSetting");
            
    
            var path_ = "/api/platform/settings/{id}";
    
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
                throw new ApiException (statusCode, "Error calling SettingGetSetting: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SettingGetSetting: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelSettingsSetting>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelSettingsSetting) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelSettingsSetting)));
            
        }
        
    }
    
}
