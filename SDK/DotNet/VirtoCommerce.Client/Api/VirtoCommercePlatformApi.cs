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
        #region Synchronous Operations
        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns></returns>
        void AssetsCreateBlobFolder (VirtoCommercePlatformCoreAssetBlobFolder folder);

        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> AssetsCreateBlobFolderWithHttpInfo (VirtoCommercePlatformCoreAssetBlobFolder folder);
        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns></returns>
        void AssetsDeleteBlobs (List<string> urls);

        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> AssetsDeleteBlobsWithHttpInfo (List<string> urls);
        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        List<VirtoCommercePlatformWebModelAssetAssetListItem> AssetsSearchAssetItems (string folderUrl = null, string keyword = null);

        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>> AssetsSearchAssetItemsWithHttpInfo (string folderUrl = null, string keyword = null);
        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        List<VirtoCommercePlatformWebModelAssetBlobInfo> AssetsUploadAsset (string folderUrl, string url = null);

        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetWithHttpInfo (string folderUrl, string url = null);
        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        List<VirtoCommercePlatformWebModelAssetBlobInfo> AssetsUploadAssetToLocalFileSystem ();

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetToLocalFileSystemWithHttpInfo ();
        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesCreatePropertyWithHttpInfo (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);
        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesDeleteDictionaryItemWithHttpInfo (string typeName, string propertyId, List<string> ids);
        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesDeletePropertyWithHttpInfo (string typeName, string propertyId);
        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsWithHttpInfo (string typeName, string propertyId);
        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;string&gt;</returns>
        List<string> DynamicPropertiesGetObjectTypes ();

        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        ApiResponse<List<string>> DynamicPropertiesGetObjectTypesWithHttpInfo ();
        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesGetProperties (string typeName);

        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesGetPropertiesWithHttpInfo (string typeName);
        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesSaveDictionaryItemsWithHttpInfo (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items);
        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesUpdatePropertyWithHttpInfo (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);
        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>VirtoCommercePlatformWebModelJobsJob</returns>
        VirtoCommercePlatformWebModelJobsJob JobsGetStatus (string id);

        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelJobsJob</returns>
        ApiResponse<VirtoCommercePlatformWebModelJobsJob> JobsGetStatusWithHttpInfo (string id);
        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;string&gt;</returns>
        List<string> LocalizationGetLocales ();

        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        ApiResponse<List<string>> LocalizationGetLocalesWithHttpInfo ();
        /// <summary>
        /// Get module details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesGetModuleById (string id);

        /// <summary>
        /// Get module details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModuleByIdWithHttpInfo (string id);
        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        List<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModules ();

        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModulesWithHttpInfo ();
        /// <summary>
        /// Install module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesInstallModule (string fileName);

        /// <summary>
        /// Install module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesInstallModuleWithHttpInfo (string fileName);
        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns></returns>
        void ModulesRestart ();

        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ModulesRestartWithHttpInfo ();
        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesUninstallModule (string id);

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUninstallModuleWithHttpInfo (string id);
        /// <summary>
        /// Update module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUpdateModuleWithHttpInfo (string id, string fileName);
        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesUpload ();

        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesUploadWithHttpInfo ();
        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns></returns>
        void NotificationsDeleteNotificationTemplate (string id);

        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> NotificationsDeleteNotificationTemplateWithHttpInfo (string id);
        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotification</returns>
        VirtoCommercePlatformWebModelNotificationsNotification NotificationsGetNotification (string id);

        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsNotification</returns>
        ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotificationWithHttpInfo (string id);
        /// <summary>
        /// Get notification journal page
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> NotificationsGetNotificationJournalWithHttpInfo (string objectId, string objectTypeId, int? start, int? count);
        /// <summary>
        /// Get notification template
        /// </summary>
        /// <remarks>
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplateWithHttpInfo (string type, string objectId, string objectTypeId, string language);
        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplates (string type, string objectId, string objectTypeId);

        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplatesWithHttpInfo (string type, string objectId, string objectTypeId);
        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        List<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotifications ();

        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationsWithHttpInfo ();
        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        List<VirtoCommercePlatformCoreNotificationsNotificationParameter> NotificationsGetTestingParameters (string type);

        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> NotificationsGetTestingParametersWithHttpInfo (string type);
        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult NotificationsRenderNotificationContent (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);

        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> NotificationsRenderNotificationContentWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);
        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>string</returns>
        string NotificationsSendNotification (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);

        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>ApiResponse of string</returns>
        ApiResponse<string> NotificationsSendNotificationWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);
        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns></returns>
        void NotificationsStopSendingNotifications (List<string> ids);

        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> NotificationsStopSendingNotificationsWithHttpInfo (List<string> ids);
        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns></returns>
        void NotificationsUpdateNotificationTemplate (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate);

        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> NotificationsUpdateNotificationTemplateWithHttpInfo (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate);
        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationMarkAllAsRead ();

        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationMarkAllAsReadWithHttpInfo ();
        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationSearch (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria);

        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationSearchWithHttpInfo (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria);
        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityChangePasswordWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword);
        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult SecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityCreateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns></returns>
        void SecurityDeleteAsync (List<string> names);

        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SecurityDeleteAsyncWithHttpInfo (List<string> names);
        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns></returns>
        void SecurityDeleteRoles (List<string> ids);

        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SecurityDeleteRolesWithHttpInfo (List<string> ids);
        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApiAccount</returns>
        VirtoCommercePlatformCoreSecurityApiAccount SecurityGenerateNewApiAccount (string type);

        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApiAccount</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount> SecurityGenerateNewApiAccountWithHttpInfo (string type);
        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetCurrentUser ();

        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetCurrentUserWithHttpInfo ();
        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        List<VirtoCommercePlatformCoreSecurityPermission> SecurityGetPermissions ();

        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>> SecurityGetPermissionsWithHttpInfo ();
        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        VirtoCommercePlatformCoreSecurityRole SecurityGetRole (string roleId);

        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRole</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityRole> SecurityGetRoleWithHttpInfo (string roleId);
        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetUserById (string id);

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByIdWithHttpInfo (string id);
        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetUserByName (string userName);

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByNameWithHttpInfo (string userName);
        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityLogin (VirtoCommercePlatformWebModelSecurityUserLogin model);

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityLoginWithHttpInfo (VirtoCommercePlatformWebModelSecurityUserLogin model);
        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns></returns>
        void SecurityLogout ();

        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SecurityLogoutWithHttpInfo ();
        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityResetPasswordWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword);
        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        VirtoCommercePlatformCoreSecurityRoleSearchResponse SecuritySearchRoles (VirtoCommercePlatformCoreSecurityRoleSearchRequest request);

        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse> SecuritySearchRolesWithHttpInfo (VirtoCommercePlatformCoreSecurityRoleSearchRequest request);
        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        VirtoCommercePlatformCoreSecurityUserSearchResponse SecuritySearchUsersAsync (VirtoCommercePlatformCoreSecurityUserSearchRequest request);

        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse> SecuritySearchUsersAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityUserSearchRequest request);
        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult SecurityUpdateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityUpdateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        VirtoCommercePlatformCoreSecurityRole SecurityUpdateRole (VirtoCommercePlatformCoreSecurityRole role);

        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRole</returns>
        ApiResponse<VirtoCommercePlatformCoreSecurityRole> SecurityUpdateRoleWithHttpInfo (VirtoCommercePlatformCoreSecurityRole role);
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetAllSettings ();

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetAllSettingsWithHttpInfo ();
        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetModuleSettings (string id);

        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetModuleSettingsWithHttpInfo (string id);
        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Setting system name.</param>
        /// <returns>VirtoCommercePlatformWebModelSettingsSetting</returns>
        VirtoCommercePlatformWebModelSettingsSetting SettingGetSetting (string id);

        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Setting system name.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelSettingsSetting</returns>
        ApiResponse<VirtoCommercePlatformWebModelSettingsSetting> SettingGetSettingWithHttpInfo (string id);
        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns></returns>
        void SettingUpdate (List<VirtoCommercePlatformWebModelSettingsSetting> settings);

        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SettingUpdateWithHttpInfo (List<VirtoCommercePlatformWebModelSettingsSetting> settings);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task AssetsCreateBlobFolderAsync (VirtoCommercePlatformCoreAssetBlobFolder folder);

        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> AssetsCreateBlobFolderAsyncWithHttpInfo (VirtoCommercePlatformCoreAssetBlobFolder folder);
        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task AssetsDeleteBlobsAsync (List<string> urls);

        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> AssetsDeleteBlobsAsyncWithHttpInfo (List<string> urls);
        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetAssetListItem>> AssetsSearchAssetItemsAsync (string folderUrl = null, string keyword = null);

        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>>> AssetsSearchAssetItemsAsyncWithHttpInfo (string folderUrl = null, string keyword = null);
        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetAsync (string folderUrl, string url = null);

        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>> AssetsUploadAssetAsyncWithHttpInfo (string folderUrl, string url = null);
        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetToLocalFileSystemAsync ();

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>> AssetsUploadAssetToLocalFileSystemAsyncWithHttpInfo ();
        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesCreatePropertyAsyncWithHttpInfo (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);
        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesDeleteDictionaryItemAsyncWithHttpInfo (string typeName, string propertyId, List<string> ids);
        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>>> DynamicPropertiesGetDictionaryItemsAsyncWithHttpInfo (string typeName, string propertyId);
        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;string&gt;</returns>
        System.Threading.Tasks.Task<List<string>> DynamicPropertiesGetObjectTypesAsync ();

        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<string>>> DynamicPropertiesGetObjectTypesAsyncWithHttpInfo ();
        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesGetPropertiesAsync (string typeName);

        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>>> DynamicPropertiesGetPropertiesAsyncWithHttpInfo (string typeName);
        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesSaveDictionaryItemsAsyncWithHttpInfo (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items);
        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesUpdatePropertyAsyncWithHttpInfo (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);
        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelJobsJob</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelJobsJob> JobsGetStatusAsync (string id);

        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelJobsJob)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelJobsJob>> JobsGetStatusAsyncWithHttpInfo (string id);
        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;string&gt;</returns>
        System.Threading.Tasks.Task<List<string>> LocalizationGetLocalesAsync ();

        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<string>>> LocalizationGetLocalesAsyncWithHttpInfo ();
        /// <summary>
        /// Get module details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModuleByIdAsync (string id);

        /// <summary>
        /// Get module details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModuleDescriptor)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModuleByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModulesAsync ();

        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>>> ModulesGetModulesAsyncWithHttpInfo ();
        /// <summary>
        /// Install module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesInstallModuleAsync (string fileName);

        /// <summary>
        /// Install module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesInstallModuleAsyncWithHttpInfo (string fileName);
        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ModulesRestartAsync ();

        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ModulesRestartAsyncWithHttpInfo ();
        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUninstallModuleAsync (string id);

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesUninstallModuleAsyncWithHttpInfo (string id);
        /// <summary>
        /// Update module from uploaded file
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesUpdateModuleAsyncWithHttpInfo (string id, string fileName);
        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesUploadAsync ();

        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModuleDescriptor)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesUploadAsyncWithHttpInfo ();
        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task NotificationsDeleteNotificationTemplateAsync (string id);

        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsDeleteNotificationTemplateAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotificationAsync (string id);

        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get notification journal page
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult>> NotificationsGetNotificationJournalAsyncWithHttpInfo (string objectId, string objectTypeId, int? start, int? count);
        /// <summary>
        /// Get notification template
        /// </summary>
        /// <remarks>
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsNotificationTemplate)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplateAsyncWithHttpInfo (string type, string objectId, string objectTypeId, string language);
        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplatesAsync (string type, string objectId, string objectTypeId);

        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>>> NotificationsGetNotificationTemplatesAsyncWithHttpInfo (string type, string objectId, string objectTypeId);
        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationsAsync ();

        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>>> NotificationsGetNotificationsAsyncWithHttpInfo ();
        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> NotificationsGetTestingParametersAsync (string type);

        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>>> NotificationsGetTestingParametersAsyncWithHttpInfo (string type);
        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> NotificationsRenderNotificationContentAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);

        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult>> NotificationsRenderNotificationContentAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);
        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of string</returns>
        System.Threading.Tasks.Task<string> NotificationsSendNotificationAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);

        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (string)</returns>
        System.Threading.Tasks.Task<ApiResponse<string>> NotificationsSendNotificationAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);
        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task NotificationsStopSendingNotificationsAsync (List<string> ids);

        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsStopSendingNotificationsAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task NotificationsUpdateNotificationTemplateAsync (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate);

        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsUpdateNotificationTemplateAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate);
        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationMarkAllAsReadAsync ();

        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>> PushNotificationMarkAllAsReadAsyncWithHttpInfo ();
        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationSearchAsync (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria);

        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>> PushNotificationSearchAsyncWithHttpInfo (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria);
        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityChangePasswordAsyncWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword);
        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityCreateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityCreateAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SecurityDeleteAsyncAsync (List<string> names);

        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> SecurityDeleteAsyncAsyncWithHttpInfo (List<string> names);
        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SecurityDeleteRolesAsync (List<string> ids);

        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> SecurityDeleteRolesAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApiAccount</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApiAccount> SecurityGenerateNewApiAccountAsync (string type);

        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApiAccount)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount>> SecurityGenerateNewApiAccountAsyncWithHttpInfo (string type);
        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetCurrentUserAsync ();

        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetCurrentUserAsyncWithHttpInfo ();
        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreSecurityPermission>> SecurityGetPermissionsAsync ();

        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>>> SecurityGetPermissionsAsyncWithHttpInfo ();
        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRole</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityGetRoleAsync (string roleId);

        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRole)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRole>> SecurityGetRoleAsyncWithHttpInfo (string roleId);
        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByIdAsync (string id);

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetUserByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByNameAsync (string userName);

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetUserByNameAsyncWithHttpInfo (string userName);
        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityLoginAsync (VirtoCommercePlatformWebModelSecurityUserLogin model);

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityLoginAsyncWithHttpInfo (VirtoCommercePlatformWebModelSecurityUserLogin model);
        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SecurityLogoutAsync ();

        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> SecurityLogoutAsyncWithHttpInfo ();
        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityResetPasswordAsyncWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword);
        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRoleSearchResponse> SecuritySearchRolesAsync (VirtoCommercePlatformCoreSecurityRoleSearchRequest request);

        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRoleSearchResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse>> SecuritySearchRolesAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityRoleSearchRequest request);
        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityUserSearchResponse> SecuritySearchUsersAsyncAsync (VirtoCommercePlatformCoreSecurityUserSearchRequest request);

        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityUserSearchResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse>> SecuritySearchUsersAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityUserSearchRequest request);
        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityUpdateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);

        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityUpdateAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRole</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityUpdateRoleAsync (VirtoCommercePlatformCoreSecurityRole role);

        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRole)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRole>> SecurityUpdateRoleAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityRole role);
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetAllSettingsAsync ();

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>> SettingGetAllSettingsAsyncWithHttpInfo ();
        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetModuleSettingsAsync (string id);

        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>> SettingGetModuleSettingsAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Setting system name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelSettingsSetting</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelSettingsSetting> SettingGetSettingAsync (string id);

        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Setting system name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelSettingsSetting)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetSettingAsyncWithHttpInfo (string id);
        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SettingUpdateAsync (List<VirtoCommercePlatformWebModelSettingsSetting> settings);

        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> SettingUpdateAsyncWithHttpInfo (List<VirtoCommercePlatformWebModelSettingsSetting> settings);
        #endregion Asynchronous Operations
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
        /// Create new blob folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns></returns>
        public void AssetsCreateBlobFolder (VirtoCommercePlatformCoreAssetBlobFolder folder)
        {
             AssetsCreateBlobFolderWithHttpInfo(folder);
        }

        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> AssetsCreateBlobFolderWithHttpInfo (VirtoCommercePlatformCoreAssetBlobFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling VirtoCommercePlatformApi->AssetsCreateBlobFolder");

            var localVarPath = "/api/platform/assets/folder";
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
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling AssetsCreateBlobFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling AssetsCreateBlobFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task AssetsCreateBlobFolderAsync (VirtoCommercePlatformCoreAssetBlobFolder folder)
        {
             await AssetsCreateBlobFolderAsyncWithHttpInfo(folder);

        }

        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> AssetsCreateBlobFolderAsyncWithHttpInfo (VirtoCommercePlatformCoreAssetBlobFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling VirtoCommercePlatformApi->AssetsCreateBlobFolder");

            var localVarPath = "/api/platform/assets/folder";
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
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling AssetsCreateBlobFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling AssetsCreateBlobFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns></returns>
        public void AssetsDeleteBlobs (List<string> urls)
        {
             AssetsDeleteBlobsWithHttpInfo(urls);
        }

        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> AssetsDeleteBlobsWithHttpInfo (List<string> urls)
        {
            // verify the required parameter 'urls' is set
            if (urls == null)
                throw new ApiException(400, "Missing required parameter 'urls' when calling VirtoCommercePlatformApi->AssetsDeleteBlobs");

            var localVarPath = "/api/platform/assets";
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
            if (urls != null) localVarQueryParams.Add("urls", Configuration.ApiClient.ParameterToString(urls)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling AssetsDeleteBlobs: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling AssetsDeleteBlobs: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task AssetsDeleteBlobsAsync (List<string> urls)
        {
             await AssetsDeleteBlobsAsyncWithHttpInfo(urls);

        }

        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> AssetsDeleteBlobsAsyncWithHttpInfo (List<string> urls)
        {
            // verify the required parameter 'urls' is set
            if (urls == null)
                throw new ApiException(400, "Missing required parameter 'urls' when calling VirtoCommercePlatformApi->AssetsDeleteBlobs");

            var localVarPath = "/api/platform/assets";
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
            if (urls != null) localVarQueryParams.Add("urls", Configuration.ApiClient.ParameterToString(urls)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling AssetsDeleteBlobs: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling AssetsDeleteBlobs: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        public List<VirtoCommercePlatformWebModelAssetAssetListItem> AssetsSearchAssetItems (string folderUrl = null, string keyword = null)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>> localVarResponse = AssetsSearchAssetItemsWithHttpInfo(folderUrl, keyword);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelAssetAssetListItem> > AssetsSearchAssetItemsWithHttpInfo (string folderUrl = null, string keyword = null)
        {

            var localVarPath = "/api/platform/assets";
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
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (keyword != null) localVarQueryParams.Add("keyword", Configuration.ApiClient.ParameterToString(keyword)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling AssetsSearchAssetItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling AssetsSearchAssetItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelAssetAssetListItem>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelAssetAssetListItem>)));
            
        }

        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetAssetListItem>> AssetsSearchAssetItemsAsync (string folderUrl = null, string keyword = null)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>> localVarResponse = await AssetsSearchAssetItemsAsyncWithHttpInfo(folderUrl, keyword);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelAssetAssetListItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>>> AssetsSearchAssetItemsAsyncWithHttpInfo (string folderUrl = null, string keyword = null)
        {

            var localVarPath = "/api/platform/assets";
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
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (keyword != null) localVarQueryParams.Add("keyword", Configuration.ApiClient.ParameterToString(keyword)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling AssetsSearchAssetItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling AssetsSearchAssetItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelAssetAssetListItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelAssetAssetListItem>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelAssetAssetListItem>)));
            
        }

        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        public List<VirtoCommercePlatformWebModelAssetBlobInfo> AssetsUploadAsset (string folderUrl, string url = null)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>> localVarResponse = AssetsUploadAssetWithHttpInfo(folderUrl, url);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelAssetBlobInfo> > AssetsUploadAssetWithHttpInfo (string folderUrl, string url = null)
        {
            // verify the required parameter 'folderUrl' is set
            if (folderUrl == null)
                throw new ApiException(400, "Missing required parameter 'folderUrl' when calling VirtoCommercePlatformApi->AssetsUploadAsset");

            var localVarPath = "/api/platform/assets";
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
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (url != null) localVarQueryParams.Add("url", Configuration.ApiClient.ParameterToString(url)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling AssetsUploadAsset: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling AssetsUploadAsset: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelAssetBlobInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelAssetBlobInfo>)));
            
        }

        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetAsync (string folderUrl, string url = null)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>> localVarResponse = await AssetsUploadAssetAsyncWithHttpInfo(folderUrl, url);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>> AssetsUploadAssetAsyncWithHttpInfo (string folderUrl, string url = null)
        {
            // verify the required parameter 'folderUrl' is set
            if (folderUrl == null)
                throw new ApiException(400, "Missing required parameter 'folderUrl' when calling VirtoCommercePlatformApi->AssetsUploadAsset");

            var localVarPath = "/api/platform/assets";
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
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (url != null) localVarQueryParams.Add("url", Configuration.ApiClient.ParameterToString(url)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling AssetsUploadAsset: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling AssetsUploadAsset: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelAssetBlobInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelAssetBlobInfo>)));
            
        }

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        public List<VirtoCommercePlatformWebModelAssetBlobInfo> AssetsUploadAssetToLocalFileSystem ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>> localVarResponse = AssetsUploadAssetToLocalFileSystemWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelAssetBlobInfo> > AssetsUploadAssetToLocalFileSystemWithHttpInfo ()
        {

            var localVarPath = "/api/platform/assets/localstorage";
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
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling AssetsUploadAssetToLocalFileSystem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling AssetsUploadAssetToLocalFileSystem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelAssetBlobInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelAssetBlobInfo>)));
            
        }

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetToLocalFileSystemAsync ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>> localVarResponse = await AssetsUploadAssetToLocalFileSystemAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelAssetBlobInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>> AssetsUploadAssetToLocalFileSystemAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/assets/localstorage";
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
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling AssetsUploadAssetToLocalFileSystem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling AssetsUploadAssetToLocalFileSystem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelAssetBlobInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelAssetBlobInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelAssetBlobInfo>)));
            
        }

        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        public VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty DynamicPropertiesCreateProperty (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
             ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> localVarResponse = DynamicPropertiesCreatePropertyWithHttpInfo(typeName, property);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (property.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                localVarPostBody = property; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesCreateProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesCreateProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty)));
            
        }

        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesCreatePropertyAsync (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
             ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> localVarResponse = await DynamicPropertiesCreatePropertyAsyncWithHttpInfo(typeName, property);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesCreatePropertyAsyncWithHttpInfo (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesCreateProperty");
            // verify the required parameter 'property' is set
            if (property == null)
                throw new ApiException(400, "Missing required parameter 'property' when calling VirtoCommercePlatformApi->DynamicPropertiesCreateProperty");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (property.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                localVarPostBody = property; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesCreateProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesCreateProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty)));
            
        }

        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesDeleteDictionaryItemAsyncWithHttpInfo (string typeName, string propertyId, List<string> ids)
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

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesDeleteProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesDeleteProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesDeletePropertyAsyncWithHttpInfo (string typeName, string propertyId)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteProperty");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteProperty");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesDeleteProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesDeleteProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;</returns>
        public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> DynamicPropertiesGetDictionaryItems (string typeName, string propertyId)
        {
             ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> localVarResponse = DynamicPropertiesGetDictionaryItemsWithHttpInfo(typeName, propertyId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>)));
            
        }

        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsAsync (string typeName, string propertyId)
        {
             ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> localVarResponse = await DynamicPropertiesGetDictionaryItemsAsyncWithHttpInfo(typeName, propertyId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>>> DynamicPropertiesGetDictionaryItemsAsyncWithHttpInfo (string typeName, string propertyId)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesGetDictionaryItems");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesGetDictionaryItems");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>)));
            
        }

        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;string&gt;</returns>
        public List<string> DynamicPropertiesGetObjectTypes ()
        {
             ApiResponse<List<string>> localVarResponse = DynamicPropertiesGetObjectTypesWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        public ApiResponse< List<string> > DynamicPropertiesGetObjectTypesWithHttpInfo ()
        {

            var localVarPath = "/api/platform/dynamic/types";
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
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<string>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<string>)));
            
        }

        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;string&gt;</returns>
        public async System.Threading.Tasks.Task<List<string>> DynamicPropertiesGetObjectTypesAsync ()
        {
             ApiResponse<List<string>> localVarResponse = await DynamicPropertiesGetObjectTypesAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<string>>> DynamicPropertiesGetObjectTypesAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/dynamic/types";
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
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<string>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<string>)));
            
        }

        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesGetProperties (string typeName)
        {
             ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> localVarResponse = DynamicPropertiesGetPropertiesWithHttpInfo(typeName);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> > DynamicPropertiesGetPropertiesWithHttpInfo (string typeName)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesGetProperties");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetProperties: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetProperties: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>)));
            
        }

        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesGetPropertiesAsync (string typeName)
        {
             ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> localVarResponse = await DynamicPropertiesGetPropertiesAsyncWithHttpInfo(typeName);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>>> DynamicPropertiesGetPropertiesAsyncWithHttpInfo (string typeName)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesGetProperties");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetProperties: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesGetProperties: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>)));
            
        }

        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            if (items.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(items); // http body (model) parameter
            }
            else
            {
                localVarPostBody = items; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesSaveDictionaryItemsAsyncWithHttpInfo (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items)
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

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            if (items.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(items); // http body (model) parameter
            }
            else
            {
                localVarPostBody = items; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            if (property.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                localVarPostBody = property; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesUpdateProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesUpdateProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DynamicPropertiesUpdatePropertyAsyncWithHttpInfo (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
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

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
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
            if (typeName != null) localVarPathParams.Add("typeName", Configuration.ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            if (property.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                localVarPostBody = property; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesUpdateProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling DynamicPropertiesUpdateProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>VirtoCommercePlatformWebModelJobsJob</returns>
        public VirtoCommercePlatformWebModelJobsJob JobsGetStatus (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelJobsJob> localVarResponse = JobsGetStatusWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelJobsJob</returns>
        public ApiResponse< VirtoCommercePlatformWebModelJobsJob > JobsGetStatusWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->JobsGetStatus");

            var localVarPath = "/api/platform/jobs/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling JobsGetStatus: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling JobsGetStatus: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelJobsJob>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelJobsJob) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelJobsJob)));
            
        }

        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelJobsJob</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelJobsJob> JobsGetStatusAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelJobsJob> localVarResponse = await JobsGetStatusAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelJobsJob)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelJobsJob>> JobsGetStatusAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->JobsGetStatus");

            var localVarPath = "/api/platform/jobs/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling JobsGetStatus: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling JobsGetStatus: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelJobsJob>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelJobsJob) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelJobsJob)));
            
        }

        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;string&gt;</returns>
        public List<string> LocalizationGetLocales ()
        {
             ApiResponse<List<string>> localVarResponse = LocalizationGetLocalesWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        public ApiResponse< List<string> > LocalizationGetLocalesWithHttpInfo ()
        {

            var localVarPath = "/api/platform/localization/locales";
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
                throw new ApiException (localVarStatusCode, "Error calling LocalizationGetLocales: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling LocalizationGetLocales: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<string>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<string>)));
            
        }

        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;string&gt;</returns>
        public async System.Threading.Tasks.Task<List<string>> LocalizationGetLocalesAsync ()
        {
             ApiResponse<List<string>> localVarResponse = await LocalizationGetLocalesAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<string>>> LocalizationGetLocalesAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/localization/locales";
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
                throw new ApiException (localVarStatusCode, "Error calling LocalizationGetLocales: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling LocalizationGetLocales: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<string>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<string>)));
            
        }

        /// <summary>
        /// Get module details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesGetModuleById (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> localVarResponse = ModulesGetModuleByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get module details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public ApiResponse< VirtoCommercePlatformWebModelPackagingModuleDescriptor > ModulesGetModuleByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->ModulesGetModuleById");

            var localVarPath = "/api/platform/modules/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling ModulesGetModuleById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesGetModuleById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModuleDescriptor) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor)));
            
        }

        /// <summary>
        /// Get module details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModuleByIdAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> localVarResponse = await ModulesGetModuleByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get module details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModuleDescriptor)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModuleByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->ModulesGetModuleById");

            var localVarPath = "/api/platform/modules/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling ModulesGetModuleById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesGetModuleById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModuleDescriptor) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor)));
            
        }

        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        public List<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModules ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> localVarResponse = ModulesGetModulesWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelPackagingModuleDescriptor> > ModulesGetModulesWithHttpInfo ()
        {

            var localVarPath = "/api/platform/modules";
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
                throw new ApiException (localVarStatusCode, "Error calling ModulesGetModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesGetModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>)));
            
        }

        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModulesAsync ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> localVarResponse = await ModulesGetModulesAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelPackagingModuleDescriptor&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>>> ModulesGetModulesAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/modules";
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
                throw new ApiException (localVarStatusCode, "Error calling ModulesGetModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesGetModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>)));
            
        }

        /// <summary>
        /// Install module from uploaded file 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesInstallModule (string fileName)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> localVarResponse = ModulesInstallModuleWithHttpInfo(fileName);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Install module from uploaded file 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public ApiResponse< VirtoCommercePlatformWebModelPackagingModulePushNotification > ModulesInstallModuleWithHttpInfo (string fileName)
        {
            // verify the required parameter 'fileName' is set
            if (fileName == null)
                throw new ApiException(400, "Missing required parameter 'fileName' when calling VirtoCommercePlatformApi->ModulesInstallModule");

            var localVarPath = "/api/platform/modules/install";
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
                "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (fileName != null) localVarQueryParams.Add("fileName", Configuration.ApiClient.ParameterToString(fileName)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ModulesInstallModule: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesInstallModule: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }

        /// <summary>
        /// Install module from uploaded file 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesInstallModuleAsync (string fileName)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> localVarResponse = await ModulesInstallModuleAsyncWithHttpInfo(fileName);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Install module from uploaded file 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesInstallModuleAsyncWithHttpInfo (string fileName)
        {
            // verify the required parameter 'fileName' is set
            if (fileName == null)
                throw new ApiException(400, "Missing required parameter 'fileName' when calling VirtoCommercePlatformApi->ModulesInstallModule");

            var localVarPath = "/api/platform/modules/install";
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
                "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (fileName != null) localVarQueryParams.Add("fileName", Configuration.ApiClient.ParameterToString(fileName)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ModulesInstallModule: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesInstallModule: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }

        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns></returns>
        public void ModulesRestart ()
        {
             ModulesRestartWithHttpInfo();
        }

        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ModulesRestartWithHttpInfo ()
        {

            var localVarPath = "/api/platform/modules/restart";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ModulesRestart: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesRestart: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ModulesRestartAsync ()
        {
             await ModulesRestartAsyncWithHttpInfo();

        }

        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ModulesRestartAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/modules/restart";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ModulesRestart: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesRestart: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesUninstallModule (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> localVarResponse = ModulesUninstallModuleWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public ApiResponse< VirtoCommercePlatformWebModelPackagingModulePushNotification > ModulesUninstallModuleWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->ModulesUninstallModule");

            var localVarPath = "/api/platform/modules/{id}/uninstall";
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
                "text/json"
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
                throw new ApiException (localVarStatusCode, "Error calling ModulesUninstallModule: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesUninstallModule: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }

        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUninstallModuleAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> localVarResponse = await ModulesUninstallModuleAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesUninstallModuleAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->ModulesUninstallModule");

            var localVarPath = "/api/platform/modules/{id}/uninstall";
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
                "text/json"
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
                throw new ApiException (localVarStatusCode, "Error calling ModulesUninstallModule: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesUninstallModule: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }

        /// <summary>
        /// Update module from uploaded file 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesUpdateModule (string id, string fileName)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> localVarResponse = ModulesUpdateModuleWithHttpInfo(id, fileName);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update module from uploaded file 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/modules/{id}/update";
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
                "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (fileName != null) localVarQueryParams.Add("fileName", Configuration.ApiClient.ParameterToString(fileName)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ModulesUpdateModule: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesUpdateModule: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }

        /// <summary>
        /// Update module from uploaded file 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUpdateModuleAsync (string id, string fileName)
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification> localVarResponse = await ModulesUpdateModuleAsyncWithHttpInfo(id, fileName);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update module from uploaded file 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModulePushNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>> ModulesUpdateModuleAsyncWithHttpInfo (string id, string fileName)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->ModulesUpdateModule");
            // verify the required parameter 'fileName' is set
            if (fileName == null)
                throw new ApiException(400, "Missing required parameter 'fileName' when calling VirtoCommercePlatformApi->ModulesUpdateModule");

            var localVarPath = "/api/platform/modules/{id}/update";
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
                "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (fileName != null) localVarQueryParams.Add("fileName", Configuration.ApiClient.ParameterToString(fileName)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ModulesUpdateModule: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesUpdateModule: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModulePushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModulePushNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification)));
            
        }

        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesUpload ()
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> localVarResponse = ModulesUploadWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public ApiResponse< VirtoCommercePlatformWebModelPackagingModuleDescriptor > ModulesUploadWithHttpInfo ()
        {

            var localVarPath = "/api/platform/modules";
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
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ModulesUpload: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesUpload: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModuleDescriptor) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor)));
            
        }

        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesUploadAsync ()
        {
             ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor> localVarResponse = await ModulesUploadAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelPackagingModuleDescriptor)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesUploadAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/modules";
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
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ModulesUpload: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ModulesUpload: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelPackagingModuleDescriptor>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelPackagingModuleDescriptor) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor)));
            
        }

        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns></returns>
        public void NotificationsDeleteNotificationTemplate (string id)
        {
             NotificationsDeleteNotificationTemplateWithHttpInfo(id);
        }

        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> NotificationsDeleteNotificationTemplateWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsDeleteNotificationTemplate");

            var localVarPath = "/api/platform/notification/template/{id}";
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
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task NotificationsDeleteNotificationTemplateAsync (string id)
        {
             await NotificationsDeleteNotificationTemplateAsyncWithHttpInfo(id);

        }

        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsDeleteNotificationTemplateAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsDeleteNotificationTemplate");

            var localVarPath = "/api/platform/notification/template/{id}";
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
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotification</returns>
        public VirtoCommercePlatformWebModelNotificationsNotification NotificationsGetNotification (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification> localVarResponse = NotificationsGetNotificationWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsNotification</returns>
        public ApiResponse< VirtoCommercePlatformWebModelNotificationsNotification > NotificationsGetNotificationWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsGetNotification");

            var localVarPath = "/api/platform/notification/notification/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotification: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotification: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelNotificationsNotification)));
            
        }

        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotificationAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification> localVarResponse = await NotificationsGetNotificationAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsGetNotification");

            var localVarPath = "/api/platform/notification/notification/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotification: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotification: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelNotificationsNotification)));
            
        }

        /// <summary>
        /// Get notification journal page Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        public VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult NotificationsGetNotificationJournal (string objectId, string objectTypeId, int? start, int? count)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> localVarResponse = NotificationsGetNotificationJournalWithHttpInfo(objectId, objectTypeId, start, count);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get notification journal page Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/notification/journal/{objectId}/{objectTypeId}";
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
            if (objectId != null) localVarPathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) localVarPathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter
            if (start != null) localVarQueryParams.Add("start", Configuration.ApiClient.ParameterToString(start)); // query parameter
            if (count != null) localVarQueryParams.Add("count", Configuration.ApiClient.ParameterToString(count)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationJournal: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationJournal: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult)));
            
        }

        /// <summary>
        /// Get notification journal page Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> NotificationsGetNotificationJournalAsync (string objectId, string objectTypeId, int? start, int? count)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> localVarResponse = await NotificationsGetNotificationJournalAsyncWithHttpInfo(objectId, objectTypeId, start, count);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get notification journal page Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult>> NotificationsGetNotificationJournalAsyncWithHttpInfo (string objectId, string objectTypeId, int? start, int? count)
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

            var localVarPath = "/api/platform/notification/journal/{objectId}/{objectTypeId}";
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
            if (objectId != null) localVarPathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) localVarPathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter
            if (start != null) localVarQueryParams.Add("start", Configuration.ApiClient.ParameterToString(start)); // query parameter
            if (count != null) localVarQueryParams.Add("count", Configuration.ApiClient.ParameterToString(count)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationJournal: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationJournal: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult)));
            
        }

        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        public VirtoCommercePlatformWebModelNotificationsNotificationTemplate NotificationsGetNotificationTemplate (string type, string objectId, string objectTypeId, string language)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> localVarResponse = NotificationsGetNotificationTemplateWithHttpInfo(type, objectId, objectTypeId, language);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}/{language}";
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
            if (type != null) localVarPathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) localVarPathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) localVarPathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter
            if (language != null) localVarPathParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsNotificationTemplate) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelNotificationsNotificationTemplate)));
            
        }

        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplateAsync (string type, string objectId, string objectTypeId, string language)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> localVarResponse = await NotificationsGetNotificationTemplateAsyncWithHttpInfo(type, objectId, objectTypeId, language);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsNotificationTemplate)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplateAsyncWithHttpInfo (string type, string objectId, string objectTypeId, string language)
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

            var localVarPath = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}/{language}";
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
            if (type != null) localVarPathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) localVarPathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) localVarPathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter
            if (language != null) localVarPathParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsNotificationTemplate) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelNotificationsNotificationTemplate)));
            
        }

        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        public List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplates (string type, string objectId, string objectTypeId)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> localVarResponse = NotificationsGetNotificationTemplatesWithHttpInfo(type, objectId, objectTypeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}";
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
            if (type != null) localVarPathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) localVarPathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) localVarPathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationTemplates: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationTemplates: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>)));
            
        }

        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplatesAsync (string type, string objectId, string objectTypeId)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> localVarResponse = await NotificationsGetNotificationTemplatesAsyncWithHttpInfo(type, objectId, objectTypeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelNotificationsNotificationTemplate&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>>> NotificationsGetNotificationTemplatesAsyncWithHttpInfo (string type, string objectId, string objectTypeId)
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

            var localVarPath = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}";
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
            if (type != null) localVarPathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) localVarPathParams.Add("objectId", Configuration.ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) localVarPathParams.Add("objectTypeId", Configuration.ApiClient.ParameterToString(objectTypeId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationTemplates: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotificationTemplates: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>)));
            
        }

        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        public List<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotifications ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>> localVarResponse = NotificationsGetNotificationsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelNotificationsNotification> > NotificationsGetNotificationsWithHttpInfo ()
        {

            var localVarPath = "/api/platform/notification";
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
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotifications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotifications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelNotificationsNotification>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelNotificationsNotification>)));
            
        }

        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationsAsync ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>> localVarResponse = await NotificationsGetNotificationsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelNotificationsNotification&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>>> NotificationsGetNotificationsAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/notification";
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
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotifications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetNotifications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelNotificationsNotification>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelNotificationsNotification>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelNotificationsNotification>)));
            
        }

        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        public List<VirtoCommercePlatformCoreNotificationsNotificationParameter> NotificationsGetTestingParameters (string type)
        {
             ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> localVarResponse = NotificationsGetTestingParametersWithHttpInfo(type);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformCoreNotificationsNotificationParameter> > NotificationsGetTestingParametersWithHttpInfo (string type)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetTestingParameters");

            var localVarPath = "/api/platform/notification/template/{type}/getTestingParameters";
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
            if (type != null) localVarPathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetTestingParameters: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetTestingParameters: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreNotificationsNotificationParameter>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformCoreNotificationsNotificationParameter>)));
            
        }

        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> NotificationsGetTestingParametersAsync (string type)
        {
             ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> localVarResponse = await NotificationsGetTestingParametersAsyncWithHttpInfo(type);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreNotificationsNotificationParameter&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>>> NotificationsGetTestingParametersAsyncWithHttpInfo (string type)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetTestingParameters");

            var localVarPath = "/api/platform/notification/template/{type}/getTestingParameters";
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
            if (type != null) localVarPathParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetTestingParameters: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsGetTestingParameters: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreNotificationsNotificationParameter>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformCoreNotificationsNotificationParameter>)));
            
        }

        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        public VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult NotificationsRenderNotificationContent (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> localVarResponse = NotificationsRenderNotificationContentWithHttpInfo(request);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        public ApiResponse< VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult > NotificationsRenderNotificationContentWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->NotificationsRenderNotificationContent");

            var localVarPath = "/api/platform/notification/template/rendernotificationcontent";
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
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsRenderNotificationContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsRenderNotificationContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult)));
            
        }

        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> NotificationsRenderNotificationContentAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
             ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> localVarResponse = await NotificationsRenderNotificationContentAsyncWithHttpInfo(request);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult>> NotificationsRenderNotificationContentAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->NotificationsRenderNotificationContent");

            var localVarPath = "/api/platform/notification/template/rendernotificationcontent";
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
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsRenderNotificationContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsRenderNotificationContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult)));
            
        }

        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>string</returns>
        public string NotificationsSendNotification (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
             ApiResponse<string> localVarResponse = NotificationsSendNotificationWithHttpInfo(request);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>ApiResponse of string</returns>
        public ApiResponse< string > NotificationsSendNotificationWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->NotificationsSendNotification");

            var localVarPath = "/api/platform/notification/template/sendnotification";
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
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsSendNotification: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsSendNotification: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<string>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (string) Configuration.ApiClient.Deserialize(localVarResponse, typeof(string)));
            
        }

        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of string</returns>
        public async System.Threading.Tasks.Task<string> NotificationsSendNotificationAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
             ApiResponse<string> localVarResponse = await NotificationsSendNotificationAsyncWithHttpInfo(request);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (string)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<string>> NotificationsSendNotificationAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->NotificationsSendNotification");

            var localVarPath = "/api/platform/notification/template/sendnotification";
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
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsSendNotification: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsSendNotification: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<string>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (string) Configuration.ApiClient.Deserialize(localVarResponse, typeof(string)));
            
        }

        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns></returns>
        public void NotificationsStopSendingNotifications (List<string> ids)
        {
             NotificationsStopSendingNotificationsWithHttpInfo(ids);
        }

        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> NotificationsStopSendingNotificationsWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->NotificationsStopSendingNotifications");

            var localVarPath = "/api/platform/notification/stopnotifications";
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
            if (ids.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(ids); // http body (model) parameter
            }
            else
            {
                localVarPostBody = ids; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsStopSendingNotifications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsStopSendingNotifications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task NotificationsStopSendingNotificationsAsync (List<string> ids)
        {
             await NotificationsStopSendingNotificationsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsStopSendingNotificationsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->NotificationsStopSendingNotifications");

            var localVarPath = "/api/platform/notification/stopnotifications";
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
            if (ids.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(ids); // http body (model) parameter
            }
            else
            {
                localVarPostBody = ids; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsStopSendingNotifications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsStopSendingNotifications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns></returns>
        public void NotificationsUpdateNotificationTemplate (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate)
        {
             NotificationsUpdateNotificationTemplateWithHttpInfo(notificationTemplate);
        }

        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> NotificationsUpdateNotificationTemplateWithHttpInfo (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate)
        {
            // verify the required parameter 'notificationTemplate' is set
            if (notificationTemplate == null)
                throw new ApiException(400, "Missing required parameter 'notificationTemplate' when calling VirtoCommercePlatformApi->NotificationsUpdateNotificationTemplate");

            var localVarPath = "/api/platform/notification/template";
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
            if (notificationTemplate.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(notificationTemplate); // http body (model) parameter
            }
            else
            {
                localVarPostBody = notificationTemplate; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task NotificationsUpdateNotificationTemplateAsync (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate)
        {
             await NotificationsUpdateNotificationTemplateAsyncWithHttpInfo(notificationTemplate);

        }

        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> NotificationsUpdateNotificationTemplateAsyncWithHttpInfo (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate)
        {
            // verify the required parameter 'notificationTemplate' is set
            if (notificationTemplate == null)
                throw new ApiException(400, "Missing required parameter 'notificationTemplate' when calling VirtoCommercePlatformApi->NotificationsUpdateNotificationTemplate");

            var localVarPath = "/api/platform/notification/template";
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
            if (notificationTemplate.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(notificationTemplate); // http body (model) parameter
            }
            else
            {
                localVarPostBody = notificationTemplate; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationMarkAllAsRead ()
        {
             ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> localVarResponse = PushNotificationMarkAllAsReadWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public ApiResponse< VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult > PushNotificationMarkAllAsReadWithHttpInfo ()
        {

            var localVarPath = "/api/platform/pushnotifications/markAllAsRead";
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
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PushNotificationMarkAllAsRead: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PushNotificationMarkAllAsRead: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)));
            
        }

        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationMarkAllAsReadAsync ()
        {
             ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> localVarResponse = await PushNotificationMarkAllAsReadAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>> PushNotificationMarkAllAsReadAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/pushnotifications/markAllAsRead";
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
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PushNotificationMarkAllAsRead: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PushNotificationMarkAllAsRead: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)));
            
        }

        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationSearch (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria)
        {
             ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> localVarResponse = PushNotificationSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public ApiResponse< VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult > PushNotificationSearchWithHttpInfo (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling VirtoCommercePlatformApi->PushNotificationSearch");

            var localVarPath = "/api/platform/pushnotifications";
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
                throw new ApiException (localVarStatusCode, "Error calling PushNotificationSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PushNotificationSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)));
            
        }

        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationSearchAsync (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria)
        {
             ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> localVarResponse = await PushNotificationSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>> PushNotificationSearchAsyncWithHttpInfo (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling VirtoCommercePlatformApi->PushNotificationSearch");

            var localVarPath = "/api/platform/pushnotifications";
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
                throw new ApiException (localVarStatusCode, "Error calling PushNotificationSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PushNotificationSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult)));
            
        }

        /// <summary>
        /// Change password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityChangePassword (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = SecurityChangePasswordWithHttpInfo(userName, changePassword);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Change password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/security/users/{userName}/changepassword";
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
            if (userName != null) localVarPathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter
            if (changePassword.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(changePassword); // http body (model) parameter
            }
            else
            {
                localVarPostBody = changePassword; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityChangePassword: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityChangePassword: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Change password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityChangePasswordAsync (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = await SecurityChangePasswordAsyncWithHttpInfo(userName, changePassword);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Change password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityChangePasswordAsyncWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityChangePassword");
            // verify the required parameter 'changePassword' is set
            if (changePassword == null)
                throw new ApiException(400, "Missing required parameter 'changePassword' when calling VirtoCommercePlatformApi->SecurityChangePassword");

            var localVarPath = "/api/platform/security/users/{userName}/changepassword";
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
            if (userName != null) localVarPathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter
            if (changePassword.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(changePassword); // http body (model) parameter
            }
            else
            {
                localVarPostBody = changePassword; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityChangePassword: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityChangePassword: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Create new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = SecurityCreateAsyncWithHttpInfo(user);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecuritySecurityResult > SecurityCreateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling VirtoCommercePlatformApi->SecurityCreateAsync");

            var localVarPath = "/api/platform/security/users/create";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityCreateAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityCreateAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Create new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityCreateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = await SecurityCreateAsyncAsyncWithHttpInfo(user);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityCreateAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling VirtoCommercePlatformApi->SecurityCreateAsync");

            var localVarPath = "/api/platform/security/users/create";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityCreateAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityCreateAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns></returns>
        public void SecurityDeleteAsync (List<string> names)
        {
             SecurityDeleteAsyncWithHttpInfo(names);
        }

        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> SecurityDeleteAsyncWithHttpInfo (List<string> names)
        {
            // verify the required parameter 'names' is set
            if (names == null)
                throw new ApiException(400, "Missing required parameter 'names' when calling VirtoCommercePlatformApi->SecurityDeleteAsync");

            var localVarPath = "/api/platform/security/users";
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
            if (names != null) localVarQueryParams.Add("names", Configuration.ApiClient.ParameterToString(names)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityDeleteAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityDeleteAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SecurityDeleteAsyncAsync (List<string> names)
        {
             await SecurityDeleteAsyncAsyncWithHttpInfo(names);

        }

        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> SecurityDeleteAsyncAsyncWithHttpInfo (List<string> names)
        {
            // verify the required parameter 'names' is set
            if (names == null)
                throw new ApiException(400, "Missing required parameter 'names' when calling VirtoCommercePlatformApi->SecurityDeleteAsync");

            var localVarPath = "/api/platform/security/users";
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
            if (names != null) localVarQueryParams.Add("names", Configuration.ApiClient.ParameterToString(names)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityDeleteAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityDeleteAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void SecurityDeleteRoles (List<string> ids)
        {
             SecurityDeleteRolesWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> SecurityDeleteRolesWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->SecurityDeleteRoles");

            var localVarPath = "/api/platform/security/roles";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityDeleteRoles: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityDeleteRoles: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SecurityDeleteRolesAsync (List<string> ids)
        {
             await SecurityDeleteRolesAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> SecurityDeleteRolesAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->SecurityDeleteRoles");

            var localVarPath = "/api/platform/security/roles";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityDeleteRoles: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityDeleteRoles: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApiAccount</returns>
        public VirtoCommercePlatformCoreSecurityApiAccount SecurityGenerateNewApiAccount (string type)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount> localVarResponse = SecurityGenerateNewApiAccountWithHttpInfo(type);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApiAccount</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApiAccount > SecurityGenerateNewApiAccountWithHttpInfo (string type)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->SecurityGenerateNewApiAccount");

            var localVarPath = "/api/platform/security/apiaccounts/new";
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
            if (type != null) localVarQueryParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityGenerateNewApiAccount: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGenerateNewApiAccount: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApiAccount) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityApiAccount)));
            
        }

        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApiAccount</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApiAccount> SecurityGenerateNewApiAccountAsync (string type)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount> localVarResponse = await SecurityGenerateNewApiAccountAsyncWithHttpInfo(type);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApiAccount)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount>> SecurityGenerateNewApiAccountAsyncWithHttpInfo (string type)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->SecurityGenerateNewApiAccount");

            var localVarPath = "/api/platform/security/apiaccounts/new";
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
            if (type != null) localVarQueryParams.Add("type", Configuration.ApiClient.ParameterToString(type)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityGenerateNewApiAccount: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGenerateNewApiAccount: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApiAccount>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApiAccount) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityApiAccount)));
            
        }

        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetCurrentUser ()
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> localVarResponse = SecurityGetCurrentUserWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > SecurityGetCurrentUserWithHttpInfo ()
        {

            var localVarPath = "/api/platform/security/currentuser";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetCurrentUser: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetCurrentUser: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }

        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetCurrentUserAsync ()
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> localVarResponse = await SecurityGetCurrentUserAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetCurrentUserAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/security/currentuser";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetCurrentUser: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetCurrentUser: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }

        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        public List<VirtoCommercePlatformCoreSecurityPermission> SecurityGetPermissions ()
        {
             ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>> localVarResponse = SecurityGetPermissionsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformCoreSecurityPermission> > SecurityGetPermissionsWithHttpInfo ()
        {

            var localVarPath = "/api/platform/security/permissions";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetPermissions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetPermissions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreSecurityPermission>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformCoreSecurityPermission>)));
            
        }

        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreSecurityPermission>> SecurityGetPermissionsAsync ()
        {
             ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>> localVarResponse = await SecurityGetPermissionsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformCoreSecurityPermission&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>>> SecurityGetPermissionsAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/security/permissions";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetPermissions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetPermissions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformCoreSecurityPermission>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformCoreSecurityPermission>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformCoreSecurityPermission>)));
            
        }

        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        public VirtoCommercePlatformCoreSecurityRole SecurityGetRole (string roleId)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRole> localVarResponse = SecurityGetRoleWithHttpInfo(roleId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRole</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityRole > SecurityGetRoleWithHttpInfo (string roleId)
        {
            // verify the required parameter 'roleId' is set
            if (roleId == null)
                throw new ApiException(400, "Missing required parameter 'roleId' when calling VirtoCommercePlatformApi->SecurityGetRole");

            var localVarPath = "/api/platform/security/roles/{roleId}";
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
            if (roleId != null) localVarPathParams.Add("roleId", Configuration.ApiClient.ParameterToString(roleId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetRole: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetRole: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityRole>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRole) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityRole)));
            
        }

        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRole</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityGetRoleAsync (string roleId)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRole> localVarResponse = await SecurityGetRoleAsyncWithHttpInfo(roleId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRole)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRole>> SecurityGetRoleAsyncWithHttpInfo (string roleId)
        {
            // verify the required parameter 'roleId' is set
            if (roleId == null)
                throw new ApiException(400, "Missing required parameter 'roleId' when calling VirtoCommercePlatformApi->SecurityGetRole");

            var localVarPath = "/api/platform/security/roles/{roleId}";
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
            if (roleId != null) localVarPathParams.Add("roleId", Configuration.ApiClient.ParameterToString(roleId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetRole: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetRole: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityRole>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRole) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityRole)));
            
        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetUserById (string id)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> localVarResponse = SecurityGetUserByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > SecurityGetUserByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SecurityGetUserById");

            var localVarPath = "/api/platform/security/users/id/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetUserById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetUserById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByIdAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> localVarResponse = await SecurityGetUserByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetUserByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SecurityGetUserById");

            var localVarPath = "/api/platform/security/users/id/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetUserById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetUserById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetUserByName (string userName)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> localVarResponse = SecurityGetUserByNameWithHttpInfo(userName);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > SecurityGetUserByNameWithHttpInfo (string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityGetUserByName");

            var localVarPath = "/api/platform/security/users/{userName}";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetUserByName: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetUserByName: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByNameAsync (string userName)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> localVarResponse = await SecurityGetUserByNameAsyncWithHttpInfo(userName);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityGetUserByNameAsyncWithHttpInfo (string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityGetUserByName");

            var localVarPath = "/api/platform/security/users/{userName}";
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
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetUserByName: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityGetUserByName: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }

        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityLogin (VirtoCommercePlatformWebModelSecurityUserLogin model)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> localVarResponse = SecurityLoginWithHttpInfo(model);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityApplicationUserExtended > SecurityLoginWithHttpInfo (VirtoCommercePlatformWebModelSecurityUserLogin model)
        {
            // verify the required parameter 'model' is set
            if (model == null)
                throw new ApiException(400, "Missing required parameter 'model' when calling VirtoCommercePlatformApi->SecurityLogin");

            var localVarPath = "/api/platform/security/login";
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
            if (model.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(model); // http body (model) parameter
            }
            else
            {
                localVarPostBody = model; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityLogin: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityLogin: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }

        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityLoginAsync (VirtoCommercePlatformWebModelSecurityUserLogin model)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended> localVarResponse = await SecurityLoginAsyncWithHttpInfo(model);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>> SecurityLoginAsyncWithHttpInfo (VirtoCommercePlatformWebModelSecurityUserLogin model)
        {
            // verify the required parameter 'model' is set
            if (model == null)
                throw new ApiException(400, "Missing required parameter 'model' when calling VirtoCommercePlatformApi->SecurityLogin");

            var localVarPath = "/api/platform/security/login";
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
            if (model.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(model); // http body (model) parameter
            }
            else
            {
                localVarPostBody = model; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityLogin: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityLogin: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityApplicationUserExtended) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended)));
            
        }

        /// <summary>
        /// Sign out 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns></returns>
        public void SecurityLogout ()
        {
             SecurityLogoutWithHttpInfo();
        }

        /// <summary>
        /// Sign out 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> SecurityLogoutWithHttpInfo ()
        {

            var localVarPath = "/api/platform/security/logout";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityLogout: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityLogout: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Sign out 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SecurityLogoutAsync ()
        {
             await SecurityLogoutAsyncWithHttpInfo();

        }

        /// <summary>
        /// Sign out 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> SecurityLogoutAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/security/logout";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityLogout: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityLogout: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Reset password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityResetPassword (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = SecurityResetPasswordWithHttpInfo(userName, resetPassword);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Reset password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/platform/security/users/{userName}/resetpassword";
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
            if (userName != null) localVarPathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter
            if (resetPassword.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(resetPassword); // http body (model) parameter
            }
            else
            {
                localVarPostBody = resetPassword; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityResetPassword: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityResetPassword: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Reset password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityResetPasswordAsync (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = await SecurityResetPasswordAsyncWithHttpInfo(userName, resetPassword);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Reset password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityResetPasswordAsyncWithHttpInfo (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityResetPassword");
            // verify the required parameter 'resetPassword' is set
            if (resetPassword == null)
                throw new ApiException(400, "Missing required parameter 'resetPassword' when calling VirtoCommercePlatformApi->SecurityResetPassword");

            var localVarPath = "/api/platform/security/users/{userName}/resetpassword";
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
            if (userName != null) localVarPathParams.Add("userName", Configuration.ApiClient.ParameterToString(userName)); // path parameter
            if (resetPassword.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(resetPassword); // http body (model) parameter
            }
            else
            {
                localVarPostBody = resetPassword; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityResetPassword: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityResetPassword: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        public VirtoCommercePlatformCoreSecurityRoleSearchResponse SecuritySearchRoles (VirtoCommercePlatformCoreSecurityRoleSearchRequest request)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse> localVarResponse = SecuritySearchRolesWithHttpInfo(request);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityRoleSearchResponse > SecuritySearchRolesWithHttpInfo (VirtoCommercePlatformCoreSecurityRoleSearchRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->SecuritySearchRoles");

            var localVarPath = "/api/platform/security/roles";
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
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecuritySearchRoles: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecuritySearchRoles: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRoleSearchResponse) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityRoleSearchResponse)));
            
        }

        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRoleSearchResponse> SecuritySearchRolesAsync (VirtoCommercePlatformCoreSecurityRoleSearchRequest request)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse> localVarResponse = await SecuritySearchRolesAsyncWithHttpInfo(request);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRoleSearchResponse)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse>> SecuritySearchRolesAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityRoleSearchRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->SecuritySearchRoles");

            var localVarPath = "/api/platform/security/roles";
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
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecuritySearchRoles: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecuritySearchRoles: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityRoleSearchResponse>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRoleSearchResponse) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityRoleSearchResponse)));
            
        }

        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        public VirtoCommercePlatformCoreSecurityUserSearchResponse SecuritySearchUsersAsync (VirtoCommercePlatformCoreSecurityUserSearchRequest request)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse> localVarResponse = SecuritySearchUsersAsyncWithHttpInfo(request);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityUserSearchResponse > SecuritySearchUsersAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityUserSearchRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->SecuritySearchUsersAsync");

            var localVarPath = "/api/platform/security/users";
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
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecuritySearchUsersAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecuritySearchUsersAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityUserSearchResponse) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityUserSearchResponse)));
            
        }

        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityUserSearchResponse> SecuritySearchUsersAsyncAsync (VirtoCommercePlatformCoreSecurityUserSearchRequest request)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse> localVarResponse = await SecuritySearchUsersAsyncAsyncWithHttpInfo(request);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityUserSearchResponse)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse>> SecuritySearchUsersAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityUserSearchRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->SecuritySearchUsersAsync");

            var localVarPath = "/api/platform/security/users";
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
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecuritySearchUsersAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecuritySearchUsersAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityUserSearchResponse>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityUserSearchResponse) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityUserSearchResponse)));
            
        }

        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityUpdateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = SecurityUpdateAsyncWithHttpInfo(user);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecuritySecurityResult > SecurityUpdateAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling VirtoCommercePlatformApi->SecurityUpdateAsync");

            var localVarPath = "/api/platform/security/users";
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
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityUpdateAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityUpdateAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityUpdateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
             ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult> localVarResponse = await SecurityUpdateAsyncAsyncWithHttpInfo(user);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecuritySecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>> SecurityUpdateAsyncAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling VirtoCommercePlatformApi->SecurityUpdateAsync");

            var localVarPath = "/api/platform/security/users";
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
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityUpdateAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityUpdateAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecuritySecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecuritySecurityResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecuritySecurityResult)));
            
        }

        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        public VirtoCommercePlatformCoreSecurityRole SecurityUpdateRole (VirtoCommercePlatformCoreSecurityRole role)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRole> localVarResponse = SecurityUpdateRoleWithHttpInfo(role);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>ApiResponse of VirtoCommercePlatformCoreSecurityRole</returns>
        public ApiResponse< VirtoCommercePlatformCoreSecurityRole > SecurityUpdateRoleWithHttpInfo (VirtoCommercePlatformCoreSecurityRole role)
        {
            // verify the required parameter 'role' is set
            if (role == null)
                throw new ApiException(400, "Missing required parameter 'role' when calling VirtoCommercePlatformApi->SecurityUpdateRole");

            var localVarPath = "/api/platform/security/roles";
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
            if (role.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(role); // http body (model) parameter
            }
            else
            {
                localVarPostBody = role; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityUpdateRole: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityUpdateRole: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityRole>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRole) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityRole)));
            
        }

        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>Task of VirtoCommercePlatformCoreSecurityRole</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityUpdateRoleAsync (VirtoCommercePlatformCoreSecurityRole role)
        {
             ApiResponse<VirtoCommercePlatformCoreSecurityRole> localVarResponse = await SecurityUpdateRoleAsyncWithHttpInfo(role);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformCoreSecurityRole)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformCoreSecurityRole>> SecurityUpdateRoleAsyncWithHttpInfo (VirtoCommercePlatformCoreSecurityRole role)
        {
            // verify the required parameter 'role' is set
            if (role == null)
                throw new ApiException(400, "Missing required parameter 'role' when calling VirtoCommercePlatformApi->SecurityUpdateRole");

            var localVarPath = "/api/platform/security/roles";
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
            if (role.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(role); // http body (model) parameter
            }
            else
            {
                localVarPostBody = role; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SecurityUpdateRole: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SecurityUpdateRole: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformCoreSecurityRole>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformCoreSecurityRole) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformCoreSecurityRole)));
            
        }

        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetAllSettings ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> localVarResponse = SettingGetAllSettingsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelSettingsSetting> > SettingGetAllSettingsWithHttpInfo ()
        {

            var localVarPath = "/api/platform/settings";
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
                throw new ApiException (localVarStatusCode, "Error calling SettingGetAllSettings: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SettingGetAllSettings: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelSettingsSetting>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>)));
            
        }

        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetAllSettingsAsync ()
        {
             ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> localVarResponse = await SettingGetAllSettingsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>> SettingGetAllSettingsAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/platform/settings";
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
                throw new ApiException (localVarStatusCode, "Error calling SettingGetAllSettings: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SettingGetAllSettings: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelSettingsSetting>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>)));
            
        }

        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetModuleSettings (string id)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> localVarResponse = SettingGetModuleSettingsWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public ApiResponse< List<VirtoCommercePlatformWebModelSettingsSetting> > SettingGetModuleSettingsWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SettingGetModuleSettings");

            var localVarPath = "/api/platform/settings/modules/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling SettingGetModuleSettings: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SettingGetModuleSettings: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelSettingsSetting>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>)));
            
        }

        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetModuleSettingsAsync (string id)
        {
             ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>> localVarResponse = await SettingGetModuleSettingsAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommercePlatformWebModelSettingsSetting&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>> SettingGetModuleSettingsAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SettingGetModuleSettings");

            var localVarPath = "/api/platform/settings/modules/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling SettingGetModuleSettings: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SettingGetModuleSettings: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommercePlatformWebModelSettingsSetting>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommercePlatformWebModelSettingsSetting>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>)));
            
        }

        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Setting system name.</param>
        /// <returns>VirtoCommercePlatformWebModelSettingsSetting</returns>
        public VirtoCommercePlatformWebModelSettingsSetting SettingGetSetting (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelSettingsSetting> localVarResponse = SettingGetSettingWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Setting system name.</param>
        /// <returns>ApiResponse of VirtoCommercePlatformWebModelSettingsSetting</returns>
        public ApiResponse< VirtoCommercePlatformWebModelSettingsSetting > SettingGetSettingWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SettingGetSetting");

            var localVarPath = "/api/platform/settings/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling SettingGetSetting: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SettingGetSetting: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelSettingsSetting>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelSettingsSetting) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelSettingsSetting)));
            
        }

        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Setting system name.</param>
        /// <returns>Task of VirtoCommercePlatformWebModelSettingsSetting</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelSettingsSetting> SettingGetSettingAsync (string id)
        {
             ApiResponse<VirtoCommercePlatformWebModelSettingsSetting> localVarResponse = await SettingGetSettingAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Setting system name.</param>
        /// <returns>Task of ApiResponse (VirtoCommercePlatformWebModelSettingsSetting)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetSettingAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SettingGetSetting");

            var localVarPath = "/api/platform/settings/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling SettingGetSetting: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SettingGetSetting: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommercePlatformWebModelSettingsSetting>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommercePlatformWebModelSettingsSetting) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommercePlatformWebModelSettingsSetting)));
            
        }

        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns></returns>
        public void SettingUpdate (List<VirtoCommercePlatformWebModelSettingsSetting> settings)
        {
             SettingUpdateWithHttpInfo(settings);
        }

        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> SettingUpdateWithHttpInfo (List<VirtoCommercePlatformWebModelSettingsSetting> settings)
        {
            // verify the required parameter 'settings' is set
            if (settings == null)
                throw new ApiException(400, "Missing required parameter 'settings' when calling VirtoCommercePlatformApi->SettingUpdate");

            var localVarPath = "/api/platform/settings";
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
            if (settings.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(settings); // http body (model) parameter
            }
            else
            {
                localVarPostBody = settings; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SettingUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SettingUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SettingUpdateAsync (List<VirtoCommercePlatformWebModelSettingsSetting> settings)
        {
             await SettingUpdateAsyncWithHttpInfo(settings);

        }

        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> SettingUpdateAsyncWithHttpInfo (List<VirtoCommercePlatformWebModelSettingsSetting> settings)
        {
            // verify the required parameter 'settings' is set
            if (settings == null)
                throw new ApiException(400, "Missing required parameter 'settings' when calling VirtoCommercePlatformApi->SettingUpdate");

            var localVarPath = "/api/platform/settings";
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
            if (settings.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(settings); // http body (model) parameter
            }
            else
            {
                localVarPostBody = settings; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling SettingUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling SettingUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

    }
}
