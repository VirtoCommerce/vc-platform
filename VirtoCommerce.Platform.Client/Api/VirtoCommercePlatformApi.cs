using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp;
using VirtoCommerce.Platform.Client.Client;
using VirtoCommerce.Platform.Client.Model;

namespace VirtoCommerce.Platform.Client.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IVirtoCommercePlatformApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns></returns>
        void AssetsCreateBlobFolder(BlobFolder folder);

        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> AssetsCreateBlobFolderWithHttpInfo(BlobFolder folder);
        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns></returns>
        void AssetsDeleteBlobs(List<string> urls);

        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> AssetsDeleteBlobsWithHttpInfo(List<string> urls);
        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>List&lt;AssetListItem&gt;</returns>
        List<AssetListItem> AssetsSearchAssetItems(string folderUrl = null, string keyword = null);

        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>ApiResponse of List&lt;AssetListItem&gt;</returns>
        ApiResponse<List<AssetListItem>> AssetsSearchAssetItemsWithHttpInfo(string folderUrl = null, string keyword = null);
        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>List&lt;BlobInfo&gt;</returns>
        List<BlobInfo> AssetsUploadAsset(string folderUrl, string url = null);

        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>ApiResponse of List&lt;BlobInfo&gt;</returns>
        ApiResponse<List<BlobInfo>> AssetsUploadAssetWithHttpInfo(string folderUrl, string url = null);
        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;BlobInfo&gt;</returns>
        List<BlobInfo> AssetsUploadAssetToLocalFileSystem();

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;BlobInfo&gt;</returns>
        ApiResponse<List<BlobInfo>> AssetsUploadAssetToLocalFileSystemWithHttpInfo();
        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>DynamicProperty</returns>
        DynamicProperty DynamicPropertiesCreateProperty(string typeName, DynamicProperty property);

        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>ApiResponse of DynamicProperty</returns>
        ApiResponse<DynamicProperty> DynamicPropertiesCreatePropertyWithHttpInfo(string typeName, DynamicProperty property);
        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns></returns>
        void DynamicPropertiesDeleteDictionaryItem(string typeName, string propertyId, List<string> ids);

        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesDeleteDictionaryItemWithHttpInfo(string typeName, string propertyId, List<string> ids);
        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        void DynamicPropertiesDeleteProperty(string typeName, string propertyId);

        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesDeletePropertyWithHttpInfo(string typeName, string propertyId);
        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>List&lt;DynamicPropertyDictionaryItem&gt;</returns>
        List<DynamicPropertyDictionaryItem> DynamicPropertiesGetDictionaryItems(string typeName, string propertyId);

        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>ApiResponse of List&lt;DynamicPropertyDictionaryItem&gt;</returns>
        ApiResponse<List<DynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsWithHttpInfo(string typeName, string propertyId);
        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;string&gt;</returns>
        List<string> DynamicPropertiesGetObjectTypes();

        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        ApiResponse<List<string>> DynamicPropertiesGetObjectTypesWithHttpInfo();
        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>List&lt;DynamicProperty&gt;</returns>
        List<DynamicProperty> DynamicPropertiesGetProperties(string typeName);

        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>ApiResponse of List&lt;DynamicProperty&gt;</returns>
        ApiResponse<List<DynamicProperty>> DynamicPropertiesGetPropertiesWithHttpInfo(string typeName);
        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        void DynamicPropertiesSaveDictionaryItems(string typeName, string propertyId, List<DynamicPropertyDictionaryItem> items);

        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesSaveDictionaryItemsWithHttpInfo(string typeName, string propertyId, List<DynamicPropertyDictionaryItem> items);
        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        void DynamicPropertiesUpdateProperty(string typeName, string propertyId, DynamicProperty property);

        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DynamicPropertiesUpdatePropertyWithHttpInfo(string typeName, string propertyId, DynamicProperty property);
        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>Job</returns>
        Job JobsGetStatus(string id);

        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>ApiResponse of Job</returns>
        ApiResponse<Job> JobsGetStatusWithHttpInfo(string id);
        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;string&gt;</returns>
        List<string> LocalizationGetLocales();

        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        ApiResponse<List<string>> LocalizationGetLocalesWithHttpInfo();
        /// <summary>
        /// Get all dependent modules for module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>List&lt;ModuleDescriptor&gt;</returns>
        List<ModuleDescriptor> ModulesGetDependingModules(List<ModuleDescriptor> moduleDescriptors);

        /// <summary>
        /// Get all dependent modules for module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>ApiResponse of List&lt;ModuleDescriptor&gt;</returns>
        ApiResponse<List<ModuleDescriptor>> ModulesGetDependingModulesWithHttpInfo(List<ModuleDescriptor> moduleDescriptors);
        /// <summary>
        /// Returns a flat expanded  list of modules that depend on passed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>List&lt;ModuleDescriptor&gt;</returns>
        List<ModuleDescriptor> ModulesGetMissingDependencies(List<ModuleDescriptor> moduleDescriptors);

        /// <summary>
        /// Returns a flat expanded  list of modules that depend on passed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>ApiResponse of List&lt;ModuleDescriptor&gt;</returns>
        ApiResponse<List<ModuleDescriptor>> ModulesGetMissingDependenciesWithHttpInfo(List<ModuleDescriptor> moduleDescriptors);
        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;ModuleDescriptor&gt;</returns>
        List<ModuleDescriptor> ModulesGetModules();

        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;ModuleDescriptor&gt;</returns>
        ApiResponse<List<ModuleDescriptor>> ModulesGetModulesWithHttpInfo();
        /// <summary>
        /// Install modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules for install</param>
        /// <returns>ModulePushNotification</returns>
        ModulePushNotification ModulesInstallModules(List<ModuleDescriptor> modules);

        /// <summary>
        /// Install modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules for install</param>
        /// <returns>ApiResponse of ModulePushNotification</returns>
        ApiResponse<ModulePushNotification> ModulesInstallModulesWithHttpInfo(List<ModuleDescriptor> modules);
        /// <summary>
        /// Reload  modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns></returns>
        void ModulesReloadModules();

        /// <summary>
        /// Reload  modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ModulesReloadModulesWithHttpInfo();
        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns></returns>
        void ModulesRestart();

        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ModulesRestartWithHttpInfo();
        /// <summary>
        /// Auto-install modules with specified groups
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ModuleAutoInstallPushNotification</returns>
        ModuleAutoInstallPushNotification ModulesTryToAutoInstallModules();

        /// <summary>
        /// Auto-install modules with specified groups
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of ModuleAutoInstallPushNotification</returns>
        ApiResponse<ModuleAutoInstallPushNotification> ModulesTryToAutoInstallModulesWithHttpInfo();
        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules</param>
        /// <returns>ModulePushNotification</returns>
        ModulePushNotification ModulesUninstallModule(List<ModuleDescriptor> modules);

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules</param>
        /// <returns>ApiResponse of ModulePushNotification</returns>
        ApiResponse<ModulePushNotification> ModulesUninstallModuleWithHttpInfo(List<ModuleDescriptor> modules);
        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ModuleDescriptor</returns>
        ModuleDescriptor ModulesUploadModuleArchive();

        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of ModuleDescriptor</returns>
        ApiResponse<ModuleDescriptor> ModulesUploadModuleArchiveWithHttpInfo();
        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns></returns>
        void NotificationsDeleteNotificationTemplate(string id);

        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> NotificationsDeleteNotificationTemplateWithHttpInfo(string id);
        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>Notification</returns>
        Notification NotificationsGetNotification(string id);

        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>ApiResponse of Notification</returns>
        ApiResponse<Notification> NotificationsGetNotificationWithHttpInfo(string id);
        /// <summary>
        /// Get all notification journal
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>SearchNotificationsResult</returns>
        SearchNotificationsResult NotificationsGetNotificationJournal(int? start, int? count);

        /// <summary>
        /// Get all notification journal
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>ApiResponse of SearchNotificationsResult</returns>
        ApiResponse<SearchNotificationsResult> NotificationsGetNotificationJournalWithHttpInfo(int? start, int? count);
        /// <summary>
        /// Get notification template
        /// </summary>
        /// <remarks>
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <param name="language">Locale of template (optional)</param>
        /// <returns>NotificationTemplate</returns>
        NotificationTemplate NotificationsGetNotificationTemplate(string type, string objectId = null, string objectTypeId = null, string language = null);

        /// <summary>
        /// Get notification template
        /// </summary>
        /// <remarks>
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <param name="language">Locale of template (optional)</param>
        /// <returns>ApiResponse of NotificationTemplate</returns>
        ApiResponse<NotificationTemplate> NotificationsGetNotificationTemplateWithHttpInfo(string type, string objectId = null, string objectTypeId = null, string language = null);
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>NotificationTemplate</returns>
        NotificationTemplate NotificationsGetNotificationTemplateById(string id);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>ApiResponse of NotificationTemplate</returns>
        ApiResponse<NotificationTemplate> NotificationsGetNotificationTemplateByIdWithHttpInfo(string id);
        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <returns>List&lt;NotificationTemplate&gt;</returns>
        List<NotificationTemplate> NotificationsGetNotificationTemplates(string type, string objectId = null, string objectTypeId = null);

        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <returns>ApiResponse of List&lt;NotificationTemplate&gt;</returns>
        ApiResponse<List<NotificationTemplate>> NotificationsGetNotificationTemplatesWithHttpInfo(string type, string objectId = null, string objectTypeId = null);
        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;Notification&gt;</returns>
        List<Notification> NotificationsGetNotifications();

        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;Notification&gt;</returns>
        ApiResponse<List<Notification>> NotificationsGetNotificationsWithHttpInfo();
        /// <summary>
        /// Get notification journal for object
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>SearchNotificationsResult</returns>
        SearchNotificationsResult NotificationsGetObjectNotificationJournal(string objectId, string objectTypeId, int? start, int? count);

        /// <summary>
        /// Get notification journal for object
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>ApiResponse of SearchNotificationsResult</returns>
        ApiResponse<SearchNotificationsResult> NotificationsGetObjectNotificationJournalWithHttpInfo(string objectId, string objectTypeId, int? start, int? count);
        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>List&lt;NotificationParameter&gt;</returns>
        List<NotificationParameter> NotificationsGetTestingParameters(string type);

        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>ApiResponse of List&lt;NotificationParameter&gt;</returns>
        ApiResponse<List<NotificationParameter>> NotificationsGetTestingParametersWithHttpInfo(string type);
        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>RenderNotificationContentResult</returns>
        RenderNotificationContentResult NotificationsRenderNotificationContent(TestNotificationRequest request);

        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>ApiResponse of RenderNotificationContentResult</returns>
        ApiResponse<RenderNotificationContentResult> NotificationsRenderNotificationContentWithHttpInfo(TestNotificationRequest request);
        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status              this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>string</returns>
        string NotificationsSendNotification(TestNotificationRequest request);

        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status              this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>ApiResponse of string</returns>
        ApiResponse<string> NotificationsSendNotificationWithHttpInfo(TestNotificationRequest request);
        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns></returns>
        void NotificationsStopSendingNotifications(List<string> ids);

        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> NotificationsStopSendingNotificationsWithHttpInfo(List<string> ids);
        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns></returns>
        void NotificationsUpdateNotificationTemplate(NotificationTemplate notificationTemplate);

        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> NotificationsUpdateNotificationTemplateWithHttpInfo(NotificationTemplate notificationTemplate);
        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>PushNotificationSearchResult</returns>
        PushNotificationSearchResult PushNotificationMarkAllAsRead();

        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of PushNotificationSearchResult</returns>
        ApiResponse<PushNotificationSearchResult> PushNotificationMarkAllAsReadWithHttpInfo();
        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>PushNotificationSearchResult</returns>
        PushNotificationSearchResult PushNotificationSearch(PushNotificationSearchCriteria criteria);

        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>ApiResponse of PushNotificationSearchResult</returns>
        ApiResponse<PushNotificationSearchResult> PushNotificationSearchWithHttpInfo(PushNotificationSearchCriteria criteria);
        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>SecurityResult</returns>
        SecurityResult SecurityChangePassword(string userName, ChangePasswordInfo changePassword);

        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>ApiResponse of SecurityResult</returns>
        ApiResponse<SecurityResult> SecurityChangePasswordWithHttpInfo(string userName, ChangePasswordInfo changePassword);
        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>SecurityResult</returns>
        SecurityResult SecurityCreateAsync(ApplicationUserExtended user);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>ApiResponse of SecurityResult</returns>
        ApiResponse<SecurityResult> SecurityCreateAsyncWithHttpInfo(ApplicationUserExtended user);
        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns></returns>
        void SecurityDeleteAsync(List<string> names);

        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SecurityDeleteAsyncWithHttpInfo(List<string> names);
        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns></returns>
        void SecurityDeleteRoles(List<string> ids);

        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SecurityDeleteRolesWithHttpInfo(List<string> ids);
        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>ApiAccount</returns>
        ApiAccount SecurityGenerateNewApiAccount(string type);

        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>ApiResponse of ApiAccount</returns>
        ApiResponse<ApiAccount> SecurityGenerateNewApiAccountWithHttpInfo(string type);
        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApplicationUserExtended</returns>
        ApplicationUserExtended SecurityGetCurrentUser();

        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of ApplicationUserExtended</returns>
        ApiResponse<ApplicationUserExtended> SecurityGetCurrentUserWithHttpInfo();
        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;Permission&gt;</returns>
        List<Permission> SecurityGetPermissions();

        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;Permission&gt;</returns>
        ApiResponse<List<Permission>> SecurityGetPermissionsWithHttpInfo();
        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>Role</returns>
        Role SecurityGetRole(string roleId);

        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>ApiResponse of Role</returns>
        ApiResponse<Role> SecurityGetRoleWithHttpInfo(string roleId);
        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>ApplicationUserExtended</returns>
        ApplicationUserExtended SecurityGetUserById(string id);

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>ApiResponse of ApplicationUserExtended</returns>
        ApiResponse<ApplicationUserExtended> SecurityGetUserByIdWithHttpInfo(string id);
        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>ApplicationUserExtended</returns>
        ApplicationUserExtended SecurityGetUserByName(string userName);

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>ApiResponse of ApplicationUserExtended</returns>
        ApiResponse<ApplicationUserExtended> SecurityGetUserByNameWithHttpInfo(string userName);
        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>ApplicationUserExtended</returns>
        ApplicationUserExtended SecurityLogin(UserLogin model);

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>ApiResponse of ApplicationUserExtended</returns>
        ApiResponse<ApplicationUserExtended> SecurityLoginWithHttpInfo(UserLogin model);
        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns></returns>
        void SecurityLogout();

        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SecurityLogoutWithHttpInfo();
        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>SecurityResult</returns>
        SecurityResult SecurityResetPassword(string userName, ResetPasswordInfo resetPassword);

        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>ApiResponse of SecurityResult</returns>
        ApiResponse<SecurityResult> SecurityResetPasswordWithHttpInfo(string userName, ResetPasswordInfo resetPassword);
        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>RoleSearchResponse</returns>
        RoleSearchResponse SecuritySearchRoles(RoleSearchRequest request);

        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>ApiResponse of RoleSearchResponse</returns>
        ApiResponse<RoleSearchResponse> SecuritySearchRolesWithHttpInfo(RoleSearchRequest request);
        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>UserSearchResponse</returns>
        UserSearchResponse SecuritySearchUsersAsync(UserSearchRequest request);

        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>ApiResponse of UserSearchResponse</returns>
        ApiResponse<UserSearchResponse> SecuritySearchUsersAsyncWithHttpInfo(UserSearchRequest request);
        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>SecurityResult</returns>
        SecurityResult SecurityUpdateAsync(ApplicationUserExtended user);

        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>ApiResponse of SecurityResult</returns>
        ApiResponse<SecurityResult> SecurityUpdateAsyncWithHttpInfo(ApplicationUserExtended user);
        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>Role</returns>
        Role SecurityUpdateRole(Role role);

        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>ApiResponse of Role</returns>
        ApiResponse<Role> SecurityUpdateRoleWithHttpInfo(Role role);
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;Setting&gt;</returns>
        List<Setting> SettingGetAllSettings();

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;Setting&gt;</returns>
        ApiResponse<List<Setting>> SettingGetAllSettingsWithHttpInfo();
        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>List&lt;Setting&gt;</returns>
        List<Setting> SettingGetModuleSettings(string id);

        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of List&lt;Setting&gt;</returns>
        ApiResponse<List<Setting>> SettingGetModuleSettingsWithHttpInfo(string id);
        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">Setting system name.</param>
        /// <returns>Setting</returns>
        Setting SettingGetSetting(string name);

        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">Setting system name.</param>
        /// <returns>ApiResponse of Setting</returns>
        ApiResponse<Setting> SettingGetSettingWithHttpInfo(string name);
        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns></returns>
        void SettingUpdate(List<Setting> settings);

        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SettingUpdateWithHttpInfo(List<Setting> settings);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task AssetsCreateBlobFolderAsync(BlobFolder folder);

        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> AssetsCreateBlobFolderAsyncWithHttpInfo(BlobFolder folder);
        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task AssetsDeleteBlobsAsync(List<string> urls);

        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> AssetsDeleteBlobsAsyncWithHttpInfo(List<string> urls);
        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>Task of List&lt;AssetListItem&gt;</returns>
        System.Threading.Tasks.Task<List<AssetListItem>> AssetsSearchAssetItemsAsync(string folderUrl = null, string keyword = null);

        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;AssetListItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<AssetListItem>>> AssetsSearchAssetItemsAsyncWithHttpInfo(string folderUrl = null, string keyword = null);
        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>Task of List&lt;BlobInfo&gt;</returns>
        System.Threading.Tasks.Task<List<BlobInfo>> AssetsUploadAssetAsync(string folderUrl, string url = null);

        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;BlobInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<BlobInfo>>> AssetsUploadAssetAsyncWithHttpInfo(string folderUrl, string url = null);
        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;BlobInfo&gt;</returns>
        System.Threading.Tasks.Task<List<BlobInfo>> AssetsUploadAssetToLocalFileSystemAsync();

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;BlobInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<BlobInfo>>> AssetsUploadAssetToLocalFileSystemAsyncWithHttpInfo();
        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of DynamicProperty</returns>
        System.Threading.Tasks.Task<DynamicProperty> DynamicPropertiesCreatePropertyAsync(string typeName, DynamicProperty property);

        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse (DynamicProperty)</returns>
        System.Threading.Tasks.Task<ApiResponse<DynamicProperty>> DynamicPropertiesCreatePropertyAsyncWithHttpInfo(string typeName, DynamicProperty property);
        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DynamicPropertiesDeleteDictionaryItemAsync(string typeName, string propertyId, List<string> ids);

        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> DynamicPropertiesDeleteDictionaryItemAsyncWithHttpInfo(string typeName, string propertyId, List<string> ids);
        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DynamicPropertiesDeletePropertyAsync(string typeName, string propertyId);

        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> DynamicPropertiesDeletePropertyAsyncWithHttpInfo(string typeName, string propertyId);
        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of List&lt;DynamicPropertyDictionaryItem&gt;</returns>
        System.Threading.Tasks.Task<List<DynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsAsync(string typeName, string propertyId);

        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse (List&lt;DynamicPropertyDictionaryItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<DynamicPropertyDictionaryItem>>> DynamicPropertiesGetDictionaryItemsAsyncWithHttpInfo(string typeName, string propertyId);
        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;string&gt;</returns>
        System.Threading.Tasks.Task<List<string>> DynamicPropertiesGetObjectTypesAsync();

        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<string>>> DynamicPropertiesGetObjectTypesAsyncWithHttpInfo();
        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>Task of List&lt;DynamicProperty&gt;</returns>
        System.Threading.Tasks.Task<List<DynamicProperty>> DynamicPropertiesGetPropertiesAsync(string typeName);

        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>Task of ApiResponse (List&lt;DynamicProperty&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<DynamicProperty>>> DynamicPropertiesGetPropertiesAsyncWithHttpInfo(string typeName);
        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DynamicPropertiesSaveDictionaryItemsAsync(string typeName, string propertyId, List<DynamicPropertyDictionaryItem> items);

        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> DynamicPropertiesSaveDictionaryItemsAsyncWithHttpInfo(string typeName, string propertyId, List<DynamicPropertyDictionaryItem> items);
        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DynamicPropertiesUpdatePropertyAsync(string typeName, string propertyId, DynamicProperty property);

        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> DynamicPropertiesUpdatePropertyAsyncWithHttpInfo(string typeName, string propertyId, DynamicProperty property);
        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of Job</returns>
        System.Threading.Tasks.Task<Job> JobsGetStatusAsync(string id);

        /// <summary>
        /// Get background job status
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of ApiResponse (Job)</returns>
        System.Threading.Tasks.Task<ApiResponse<Job>> JobsGetStatusAsyncWithHttpInfo(string id);
        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;string&gt;</returns>
        System.Threading.Tasks.Task<List<string>> LocalizationGetLocalesAsync();

        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<string>>> LocalizationGetLocalesAsyncWithHttpInfo();
        /// <summary>
        /// Get all dependent modules for module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>Task of List&lt;ModuleDescriptor&gt;</returns>
        System.Threading.Tasks.Task<List<ModuleDescriptor>> ModulesGetDependingModulesAsync(List<ModuleDescriptor> moduleDescriptors);

        /// <summary>
        /// Get all dependent modules for module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>Task of ApiResponse (List&lt;ModuleDescriptor&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<ModuleDescriptor>>> ModulesGetDependingModulesAsyncWithHttpInfo(List<ModuleDescriptor> moduleDescriptors);
        /// <summary>
        /// Returns a flat expanded  list of modules that depend on passed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>Task of List&lt;ModuleDescriptor&gt;</returns>
        System.Threading.Tasks.Task<List<ModuleDescriptor>> ModulesGetMissingDependenciesAsync(List<ModuleDescriptor> moduleDescriptors);

        /// <summary>
        /// Returns a flat expanded  list of modules that depend on passed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>Task of ApiResponse (List&lt;ModuleDescriptor&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<ModuleDescriptor>>> ModulesGetMissingDependenciesAsyncWithHttpInfo(List<ModuleDescriptor> moduleDescriptors);
        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;ModuleDescriptor&gt;</returns>
        System.Threading.Tasks.Task<List<ModuleDescriptor>> ModulesGetModulesAsync();

        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;ModuleDescriptor&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<ModuleDescriptor>>> ModulesGetModulesAsyncWithHttpInfo();
        /// <summary>
        /// Install modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules for install</param>
        /// <returns>Task of ModulePushNotification</returns>
        System.Threading.Tasks.Task<ModulePushNotification> ModulesInstallModulesAsync(List<ModuleDescriptor> modules);

        /// <summary>
        /// Install modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules for install</param>
        /// <returns>Task of ApiResponse (ModulePushNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<ModulePushNotification>> ModulesInstallModulesAsyncWithHttpInfo(List<ModuleDescriptor> modules);
        /// <summary>
        /// Reload  modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ModulesReloadModulesAsync();

        /// <summary>
        /// Reload  modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> ModulesReloadModulesAsyncWithHttpInfo();
        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ModulesRestartAsync();

        /// <summary>
        /// Restart web application
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> ModulesRestartAsyncWithHttpInfo();
        /// <summary>
        /// Auto-install modules with specified groups
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ModuleAutoInstallPushNotification</returns>
        System.Threading.Tasks.Task<ModuleAutoInstallPushNotification> ModulesTryToAutoInstallModulesAsync();

        /// <summary>
        /// Auto-install modules with specified groups
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (ModuleAutoInstallPushNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<ModuleAutoInstallPushNotification>> ModulesTryToAutoInstallModulesAsyncWithHttpInfo();
        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules</param>
        /// <returns>Task of ModulePushNotification</returns>
        System.Threading.Tasks.Task<ModulePushNotification> ModulesUninstallModuleAsync(List<ModuleDescriptor> modules);

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules</param>
        /// <returns>Task of ApiResponse (ModulePushNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<ModulePushNotification>> ModulesUninstallModuleAsyncWithHttpInfo(List<ModuleDescriptor> modules);
        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ModuleDescriptor</returns>
        System.Threading.Tasks.Task<ModuleDescriptor> ModulesUploadModuleArchiveAsync();

        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (ModuleDescriptor)</returns>
        System.Threading.Tasks.Task<ApiResponse<ModuleDescriptor>> ModulesUploadModuleArchiveAsyncWithHttpInfo();
        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task NotificationsDeleteNotificationTemplateAsync(string id);

        /// <summary>
        /// Delete notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> NotificationsDeleteNotificationTemplateAsyncWithHttpInfo(string id);
        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of Notification</returns>
        System.Threading.Tasks.Task<Notification> NotificationsGetNotificationAsync(string id);

        /// <summary>
        /// Get sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of ApiResponse (Notification)</returns>
        System.Threading.Tasks.Task<ApiResponse<Notification>> NotificationsGetNotificationAsyncWithHttpInfo(string id);
        /// <summary>
        /// Get all notification journal
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of SearchNotificationsResult</returns>
        System.Threading.Tasks.Task<SearchNotificationsResult> NotificationsGetNotificationJournalAsync(int? start, int? count);

        /// <summary>
        /// Get all notification journal
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of ApiResponse (SearchNotificationsResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<SearchNotificationsResult>> NotificationsGetNotificationJournalAsyncWithHttpInfo(int? start, int? count);
        /// <summary>
        /// Get notification template
        /// </summary>
        /// <remarks>
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <param name="language">Locale of template (optional)</param>
        /// <returns>Task of NotificationTemplate</returns>
        System.Threading.Tasks.Task<NotificationTemplate> NotificationsGetNotificationTemplateAsync(string type, string objectId = null, string objectTypeId = null, string language = null);

        /// <summary>
        /// Get notification template
        /// </summary>
        /// <remarks>
        /// Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <param name="language">Locale of template (optional)</param>
        /// <returns>Task of ApiResponse (NotificationTemplate)</returns>
        System.Threading.Tasks.Task<ApiResponse<NotificationTemplate>> NotificationsGetNotificationTemplateAsyncWithHttpInfo(string type, string objectId = null, string objectTypeId = null, string language = null);
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of NotificationTemplate</returns>
        System.Threading.Tasks.Task<NotificationTemplate> NotificationsGetNotificationTemplateByIdAsync(string id);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of ApiResponse (NotificationTemplate)</returns>
        System.Threading.Tasks.Task<ApiResponse<NotificationTemplate>> NotificationsGetNotificationTemplateByIdAsyncWithHttpInfo(string id);
        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <returns>Task of List&lt;NotificationTemplate&gt;</returns>
        System.Threading.Tasks.Task<List<NotificationTemplate>> NotificationsGetNotificationTemplatesAsync(string type, string objectId = null, string objectTypeId = null);

        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;NotificationTemplate&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<NotificationTemplate>>> NotificationsGetNotificationTemplatesAsyncWithHttpInfo(string type, string objectId = null, string objectTypeId = null);
        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;Notification&gt;</returns>
        System.Threading.Tasks.Task<List<Notification>> NotificationsGetNotificationsAsync();

        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;Notification&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<Notification>>> NotificationsGetNotificationsAsyncWithHttpInfo();
        /// <summary>
        /// Get notification journal for object
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of SearchNotificationsResult</returns>
        System.Threading.Tasks.Task<SearchNotificationsResult> NotificationsGetObjectNotificationJournalAsync(string objectId, string objectTypeId, int? start, int? count);

        /// <summary>
        /// Get notification journal for object
        /// </summary>
        /// <remarks>
        /// Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of ApiResponse (SearchNotificationsResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<SearchNotificationsResult>> NotificationsGetObjectNotificationJournalAsyncWithHttpInfo(string objectId, string objectTypeId, int? start, int? count);
        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>Task of List&lt;NotificationParameter&gt;</returns>
        System.Threading.Tasks.Task<List<NotificationParameter>> NotificationsGetTestingParametersAsync(string type);

        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>Task of ApiResponse (List&lt;NotificationParameter&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<NotificationParameter>>> NotificationsGetTestingParametersAsyncWithHttpInfo(string type);
        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of RenderNotificationContentResult</returns>
        System.Threading.Tasks.Task<RenderNotificationContentResult> NotificationsRenderNotificationContentAsync(TestNotificationRequest request);

        /// <summary>
        /// Get rendered notification content
        /// </summary>
        /// <remarks>
        /// Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (RenderNotificationContentResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<RenderNotificationContentResult>> NotificationsRenderNotificationContentAsyncWithHttpInfo(TestNotificationRequest request);
        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status              this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of string</returns>
        System.Threading.Tasks.Task<string> NotificationsSendNotificationAsync(TestNotificationRequest request);

        /// <summary>
        /// Sending test notification
        /// </summary>
        /// <remarks>
        /// Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status              this string is empty, otherwise string contains error message.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (string)</returns>
        System.Threading.Tasks.Task<ApiResponse<string>> NotificationsSendNotificationAsyncWithHttpInfo(TestNotificationRequest request);
        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task NotificationsStopSendingNotificationsAsync(List<string> ids);

        /// <summary>
        /// Stop sending notification
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> NotificationsStopSendingNotificationsAsyncWithHttpInfo(List<string> ids);
        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task NotificationsUpdateNotificationTemplateAsync(NotificationTemplate notificationTemplate);

        /// <summary>
        /// Update notification template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> NotificationsUpdateNotificationTemplateAsyncWithHttpInfo(NotificationTemplate notificationTemplate);
        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of PushNotificationSearchResult</returns>
        System.Threading.Tasks.Task<PushNotificationSearchResult> PushNotificationMarkAllAsReadAsync();

        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (PushNotificationSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<PushNotificationSearchResult>> PushNotificationMarkAllAsReadAsyncWithHttpInfo();
        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of PushNotificationSearchResult</returns>
        System.Threading.Tasks.Task<PushNotificationSearchResult> PushNotificationSearchAsync(PushNotificationSearchCriteria criteria);

        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of ApiResponse (PushNotificationSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<PushNotificationSearchResult>> PushNotificationSearchAsyncWithHttpInfo(PushNotificationSearchCriteria criteria);
        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of SecurityResult</returns>
        System.Threading.Tasks.Task<SecurityResult> SecurityChangePasswordAsync(string userName, ChangePasswordInfo changePassword);

        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of ApiResponse (SecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<SecurityResult>> SecurityChangePasswordAsyncWithHttpInfo(string userName, ChangePasswordInfo changePassword);
        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of SecurityResult</returns>
        System.Threading.Tasks.Task<SecurityResult> SecurityCreateAsyncAsync(ApplicationUserExtended user);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (SecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<SecurityResult>> SecurityCreateAsyncAsyncWithHttpInfo(ApplicationUserExtended user);
        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SecurityDeleteAsyncAsync(List<string> names);

        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> SecurityDeleteAsyncAsyncWithHttpInfo(List<string> names);
        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SecurityDeleteRolesAsync(List<string> ids);

        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> SecurityDeleteRolesAsyncWithHttpInfo(List<string> ids);
        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>Task of ApiAccount</returns>
        System.Threading.Tasks.Task<ApiAccount> SecurityGenerateNewApiAccountAsync(string type);

        /// <summary>
        /// Generate new API key
        /// </summary>
        /// <remarks>
        /// Generates new key but does not save it.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>Task of ApiResponse (ApiAccount)</returns>
        System.Threading.Tasks.Task<ApiResponse<ApiAccount>> SecurityGenerateNewApiAccountAsyncWithHttpInfo(string type);
        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApplicationUserExtended</returns>
        System.Threading.Tasks.Task<ApplicationUserExtended> SecurityGetCurrentUserAsync();

        /// <summary>
        /// Get current user details
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (ApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<ApplicationUserExtended>> SecurityGetCurrentUserAsyncWithHttpInfo();
        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;Permission&gt;</returns>
        System.Threading.Tasks.Task<List<Permission>> SecurityGetPermissionsAsync();

        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;Permission&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<Permission>>> SecurityGetPermissionsAsyncWithHttpInfo();
        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>Task of Role</returns>
        System.Threading.Tasks.Task<Role> SecurityGetRoleAsync(string roleId);

        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>Task of ApiResponse (Role)</returns>
        System.Threading.Tasks.Task<ApiResponse<Role>> SecurityGetRoleAsyncWithHttpInfo(string roleId);
        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of ApplicationUserExtended</returns>
        System.Threading.Tasks.Task<ApplicationUserExtended> SecurityGetUserByIdAsync(string id);

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of ApiResponse (ApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<ApplicationUserExtended>> SecurityGetUserByIdAsyncWithHttpInfo(string id);
        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of ApplicationUserExtended</returns>
        System.Threading.Tasks.Task<ApplicationUserExtended> SecurityGetUserByNameAsync(string userName);

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of ApiResponse (ApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<ApplicationUserExtended>> SecurityGetUserByNameAsyncWithHttpInfo(string userName);
        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of ApplicationUserExtended</returns>
        System.Threading.Tasks.Task<ApplicationUserExtended> SecurityLoginAsync(UserLogin model);

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of ApiResponse (ApplicationUserExtended)</returns>
        System.Threading.Tasks.Task<ApiResponse<ApplicationUserExtended>> SecurityLoginAsyncWithHttpInfo(UserLogin model);
        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SecurityLogoutAsync();

        /// <summary>
        /// Sign out
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> SecurityLogoutAsyncWithHttpInfo();
        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of SecurityResult</returns>
        System.Threading.Tasks.Task<SecurityResult> SecurityResetPasswordAsync(string userName, ResetPasswordInfo resetPassword);

        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of ApiResponse (SecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<SecurityResult>> SecurityResetPasswordAsyncWithHttpInfo(string userName, ResetPasswordInfo resetPassword);
        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of RoleSearchResponse</returns>
        System.Threading.Tasks.Task<RoleSearchResponse> SecuritySearchRolesAsync(RoleSearchRequest request);

        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (RoleSearchResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<RoleSearchResponse>> SecuritySearchRolesAsyncWithHttpInfo(RoleSearchRequest request);
        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of UserSearchResponse</returns>
        System.Threading.Tasks.Task<UserSearchResponse> SecuritySearchUsersAsyncAsync(UserSearchRequest request);

        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (UserSearchResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<UserSearchResponse>> SecuritySearchUsersAsyncAsyncWithHttpInfo(UserSearchRequest request);
        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of SecurityResult</returns>
        System.Threading.Tasks.Task<SecurityResult> SecurityUpdateAsyncAsync(ApplicationUserExtended user);

        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (SecurityResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<SecurityResult>> SecurityUpdateAsyncAsyncWithHttpInfo(ApplicationUserExtended user);
        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>Task of Role</returns>
        System.Threading.Tasks.Task<Role> SecurityUpdateRoleAsync(Role role);

        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>Task of ApiResponse (Role)</returns>
        System.Threading.Tasks.Task<ApiResponse<Role>> SecurityUpdateRoleAsyncWithHttpInfo(Role role);
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;Setting&gt;</returns>
        System.Threading.Tasks.Task<List<Setting>> SettingGetAllSettingsAsync();

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;Setting&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<Setting>>> SettingGetAllSettingsAsyncWithHttpInfo();
        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of List&lt;Setting&gt;</returns>
        System.Threading.Tasks.Task<List<Setting>> SettingGetModuleSettingsAsync(string id);

        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (List&lt;Setting&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<Setting>>> SettingGetModuleSettingsAsyncWithHttpInfo(string id);
        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">Setting system name.</param>
        /// <returns>Task of Setting</returns>
        System.Threading.Tasks.Task<Setting> SettingGetSettingAsync(string name);

        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">Setting system name.</param>
        /// <returns>Task of ApiResponse (Setting)</returns>
        System.Threading.Tasks.Task<ApiResponse<Setting>> SettingGetSettingAsyncWithHttpInfo(string name);
        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SettingUpdateAsync(List<Setting> settings);

        /// <summary>
        /// Update settings values
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> SettingUpdateAsyncWithHttpInfo(List<Setting> settings);
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
        /// <param name="apiClient">An instance of ApiClient.</param>
        /// <returns></returns>
        public VirtoCommercePlatformApi(ApiClient apiClient)
        {
            ApiClient = apiClient;
            Configuration = apiClient.Configuration;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the API client object
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient { get; set; }

        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns></returns>
        public void AssetsCreateBlobFolder(BlobFolder folder)
        {
             AssetsCreateBlobFolderWithHttpInfo(folder);
        }

        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> AssetsCreateBlobFolderWithHttpInfo(BlobFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling VirtoCommercePlatformApi->AssetsCreateBlobFolder");

            var localVarPath = "/api/platform/assets/folder";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling AssetsCreateBlobFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling AssetsCreateBlobFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task AssetsCreateBlobFolderAsync(BlobFolder folder)
        {
             await AssetsCreateBlobFolderAsyncWithHttpInfo(folder);

        }

        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> AssetsCreateBlobFolderAsyncWithHttpInfo(BlobFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling VirtoCommercePlatformApi->AssetsCreateBlobFolder");

            var localVarPath = "/api/platform/assets/folder";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling AssetsCreateBlobFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling AssetsCreateBlobFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns></returns>
        public void AssetsDeleteBlobs(List<string> urls)
        {
             AssetsDeleteBlobsWithHttpInfo(urls);
        }

        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> AssetsDeleteBlobsWithHttpInfo(List<string> urls)
        {
            // verify the required parameter 'urls' is set
            if (urls == null)
                throw new ApiException(400, "Missing required parameter 'urls' when calling VirtoCommercePlatformApi->AssetsDeleteBlobs");

            var localVarPath = "/api/platform/assets";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (urls != null) localVarQueryParams.Add("urls", ApiClient.ParameterToString(urls)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling AssetsDeleteBlobs: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling AssetsDeleteBlobs: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task AssetsDeleteBlobsAsync(List<string> urls)
        {
             await AssetsDeleteBlobsAsyncWithHttpInfo(urls);

        }

        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="urls"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> AssetsDeleteBlobsAsyncWithHttpInfo(List<string> urls)
        {
            // verify the required parameter 'urls' is set
            if (urls == null)
                throw new ApiException(400, "Missing required parameter 'urls' when calling VirtoCommercePlatformApi->AssetsDeleteBlobs");

            var localVarPath = "/api/platform/assets";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (urls != null) localVarQueryParams.Add("urls", ApiClient.ParameterToString(urls)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling AssetsDeleteBlobs: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling AssetsDeleteBlobs: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>List&lt;AssetListItem&gt;</returns>
        public List<AssetListItem> AssetsSearchAssetItems(string folderUrl = null, string keyword = null)
        {
             ApiResponse<List<AssetListItem>> localVarResponse = AssetsSearchAssetItemsWithHttpInfo(folderUrl, keyword);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>ApiResponse of List&lt;AssetListItem&gt;</returns>
        public ApiResponse<List<AssetListItem>> AssetsSearchAssetItemsWithHttpInfo(string folderUrl = null, string keyword = null)
        {

            var localVarPath = "/api/platform/assets";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", ApiClient.ParameterToString(folderUrl)); // query parameter
            if (keyword != null) localVarQueryParams.Add("keyword", ApiClient.ParameterToString(keyword)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling AssetsSearchAssetItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling AssetsSearchAssetItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<AssetListItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<AssetListItem>)ApiClient.Deserialize(localVarResponse, typeof(List<AssetListItem>)));
            
        }

        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>Task of List&lt;AssetListItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<AssetListItem>> AssetsSearchAssetItemsAsync(string folderUrl = null, string keyword = null)
        {
             ApiResponse<List<AssetListItem>> localVarResponse = await AssetsSearchAssetItemsAsyncWithHttpInfo(folderUrl, keyword);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl"> (optional)</param>
        /// <param name="keyword"> (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;AssetListItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<AssetListItem>>> AssetsSearchAssetItemsAsyncWithHttpInfo(string folderUrl = null, string keyword = null)
        {

            var localVarPath = "/api/platform/assets";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", ApiClient.ParameterToString(folderUrl)); // query parameter
            if (keyword != null) localVarQueryParams.Add("keyword", ApiClient.ParameterToString(keyword)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling AssetsSearchAssetItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling AssetsSearchAssetItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<AssetListItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<AssetListItem>)ApiClient.Deserialize(localVarResponse, typeof(List<AssetListItem>)));
            
        }
        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>List&lt;BlobInfo&gt;</returns>
        public List<BlobInfo> AssetsUploadAsset(string folderUrl, string url = null)
        {
             ApiResponse<List<BlobInfo>> localVarResponse = AssetsUploadAssetWithHttpInfo(folderUrl, url);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>ApiResponse of List&lt;BlobInfo&gt;</returns>
        public ApiResponse<List<BlobInfo>> AssetsUploadAssetWithHttpInfo(string folderUrl, string url = null)
        {
            // verify the required parameter 'folderUrl' is set
            if (folderUrl == null)
                throw new ApiException(400, "Missing required parameter 'folderUrl' when calling VirtoCommercePlatformApi->AssetsUploadAsset");

            var localVarPath = "/api/platform/assets";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", ApiClient.ParameterToString(folderUrl)); // query parameter
            if (url != null) localVarQueryParams.Add("url", ApiClient.ParameterToString(url)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling AssetsUploadAsset: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling AssetsUploadAsset: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<BlobInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<BlobInfo>)ApiClient.Deserialize(localVarResponse, typeof(List<BlobInfo>)));
            
        }

        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>Task of List&lt;BlobInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<BlobInfo>> AssetsUploadAssetAsync(string folderUrl, string url = null)
        {
             ApiResponse<List<BlobInfo>> localVarResponse = await AssetsUploadAssetAsyncWithHttpInfo(folderUrl, url);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional) (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;BlobInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<BlobInfo>>> AssetsUploadAssetAsyncWithHttpInfo(string folderUrl, string url = null)
        {
            // verify the required parameter 'folderUrl' is set
            if (folderUrl == null)
                throw new ApiException(400, "Missing required parameter 'folderUrl' when calling VirtoCommercePlatformApi->AssetsUploadAsset");

            var localVarPath = "/api/platform/assets";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", ApiClient.ParameterToString(folderUrl)); // query parameter
            if (url != null) localVarQueryParams.Add("url", ApiClient.ParameterToString(url)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling AssetsUploadAsset: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling AssetsUploadAsset: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<BlobInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<BlobInfo>)ApiClient.Deserialize(localVarResponse, typeof(List<BlobInfo>)));
            
        }
        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;BlobInfo&gt;</returns>
        public List<BlobInfo> AssetsUploadAssetToLocalFileSystem()
        {
             ApiResponse<List<BlobInfo>> localVarResponse = AssetsUploadAssetToLocalFileSystemWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;BlobInfo&gt;</returns>
        public ApiResponse<List<BlobInfo>> AssetsUploadAssetToLocalFileSystemWithHttpInfo()
        {

            var localVarPath = "/api/platform/assets/localstorage";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling AssetsUploadAssetToLocalFileSystem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling AssetsUploadAssetToLocalFileSystem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<BlobInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<BlobInfo>)ApiClient.Deserialize(localVarResponse, typeof(List<BlobInfo>)));
            
        }

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;BlobInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<BlobInfo>> AssetsUploadAssetToLocalFileSystemAsync()
        {
             ApiResponse<List<BlobInfo>> localVarResponse = await AssetsUploadAssetToLocalFileSystemAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// This method used to upload files on local disk storage in special uploads folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;BlobInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<BlobInfo>>> AssetsUploadAssetToLocalFileSystemAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/assets/localstorage";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling AssetsUploadAssetToLocalFileSystem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling AssetsUploadAssetToLocalFileSystem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<BlobInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<BlobInfo>)ApiClient.Deserialize(localVarResponse, typeof(List<BlobInfo>)));
            
        }
        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>DynamicProperty</returns>
        public DynamicProperty DynamicPropertiesCreateProperty(string typeName, DynamicProperty property)
        {
             ApiResponse<DynamicProperty> localVarResponse = DynamicPropertiesCreatePropertyWithHttpInfo(typeName, property);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>ApiResponse of DynamicProperty</returns>
        public ApiResponse<DynamicProperty> DynamicPropertiesCreatePropertyWithHttpInfo(string typeName, DynamicProperty property)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesCreateProperty");
            // verify the required parameter 'property' is set
            if (property == null)
                throw new ApiException(400, "Missing required parameter 'property' when calling VirtoCommercePlatformApi->DynamicPropertiesCreateProperty");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (property.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                localVarPostBody = property; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesCreateProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesCreateProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicProperty>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicProperty)ApiClient.Deserialize(localVarResponse, typeof(DynamicProperty)));
            
        }

        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of DynamicProperty</returns>
        public async System.Threading.Tasks.Task<DynamicProperty> DynamicPropertiesCreatePropertyAsync(string typeName, DynamicProperty property)
        {
             ApiResponse<DynamicProperty> localVarResponse = await DynamicPropertiesCreatePropertyAsyncWithHttpInfo(typeName, property);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse (DynamicProperty)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DynamicProperty>> DynamicPropertiesCreatePropertyAsyncWithHttpInfo(string typeName, DynamicProperty property)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesCreateProperty");
            // verify the required parameter 'property' is set
            if (property == null)
                throw new ApiException(400, "Missing required parameter 'property' when calling VirtoCommercePlatformApi->DynamicPropertiesCreateProperty");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (property.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                localVarPostBody = property; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesCreateProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesCreateProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicProperty>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicProperty)ApiClient.Deserialize(localVarResponse, typeof(DynamicProperty)));
            
        }
        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns></returns>
        public void DynamicPropertiesDeleteDictionaryItem(string typeName, string propertyId, List<string> ids)
        {
             DynamicPropertiesDeleteDictionaryItemWithHttpInfo(typeName, propertyId, ids);
        }

        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> DynamicPropertiesDeleteDictionaryItemWithHttpInfo(string typeName, string propertyId, List<string> ids)
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
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DynamicPropertiesDeleteDictionaryItemAsync(string typeName, string propertyId, List<string> ids)
        {
             await DynamicPropertiesDeleteDictionaryItemAsyncWithHttpInfo(typeName, propertyId, ids);

        }

        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> DynamicPropertiesDeleteDictionaryItemAsyncWithHttpInfo(string typeName, string propertyId, List<string> ids)
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
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public void DynamicPropertiesDeleteProperty(string typeName, string propertyId)
        {
             DynamicPropertiesDeletePropertyWithHttpInfo(typeName, propertyId);
        }

        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> DynamicPropertiesDeletePropertyWithHttpInfo(string typeName, string propertyId)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteProperty");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteProperty");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesDeleteProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesDeleteProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DynamicPropertiesDeletePropertyAsync(string typeName, string propertyId)
        {
             await DynamicPropertiesDeletePropertyAsyncWithHttpInfo(typeName, propertyId);

        }

        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> DynamicPropertiesDeletePropertyAsyncWithHttpInfo(string typeName, string propertyId)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteProperty");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesDeleteProperty");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesDeleteProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesDeleteProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>List&lt;DynamicPropertyDictionaryItem&gt;</returns>
        public List<DynamicPropertyDictionaryItem> DynamicPropertiesGetDictionaryItems(string typeName, string propertyId)
        {
             ApiResponse<List<DynamicPropertyDictionaryItem>> localVarResponse = DynamicPropertiesGetDictionaryItemsWithHttpInfo(typeName, propertyId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>ApiResponse of List&lt;DynamicPropertyDictionaryItem&gt;</returns>
        public ApiResponse<List<DynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsWithHttpInfo(string typeName, string propertyId)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesGetDictionaryItems");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesGetDictionaryItems");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<DynamicPropertyDictionaryItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<DynamicPropertyDictionaryItem>)ApiClient.Deserialize(localVarResponse, typeof(List<DynamicPropertyDictionaryItem>)));
            
        }

        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of List&lt;DynamicPropertyDictionaryItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<DynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsAsync(string typeName, string propertyId)
        {
             ApiResponse<List<DynamicPropertyDictionaryItem>> localVarResponse = await DynamicPropertiesGetDictionaryItemsAsyncWithHttpInfo(typeName, propertyId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns>Task of ApiResponse (List&lt;DynamicPropertyDictionaryItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<DynamicPropertyDictionaryItem>>> DynamicPropertiesGetDictionaryItemsAsyncWithHttpInfo(string typeName, string propertyId)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesGetDictionaryItems");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling VirtoCommercePlatformApi->DynamicPropertiesGetDictionaryItems");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<DynamicPropertyDictionaryItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<DynamicPropertyDictionaryItem>)ApiClient.Deserialize(localVarResponse, typeof(List<DynamicPropertyDictionaryItem>)));
            
        }
        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;string&gt;</returns>
        public List<string> DynamicPropertiesGetObjectTypes()
        {
             ApiResponse<List<string>> localVarResponse = DynamicPropertiesGetObjectTypesWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        public ApiResponse<List<string>> DynamicPropertiesGetObjectTypesWithHttpInfo()
        {

            var localVarPath = "/api/platform/dynamic/types";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<string>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>)ApiClient.Deserialize(localVarResponse, typeof(List<string>)));
            
        }

        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;string&gt;</returns>
        public async System.Threading.Tasks.Task<List<string>> DynamicPropertiesGetObjectTypesAsync()
        {
             ApiResponse<List<string>> localVarResponse = await DynamicPropertiesGetObjectTypesAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<string>>> DynamicPropertiesGetObjectTypesAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/dynamic/types";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<string>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>)ApiClient.Deserialize(localVarResponse, typeof(List<string>)));
            
        }
        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>List&lt;DynamicProperty&gt;</returns>
        public List<DynamicProperty> DynamicPropertiesGetProperties(string typeName)
        {
             ApiResponse<List<DynamicProperty>> localVarResponse = DynamicPropertiesGetPropertiesWithHttpInfo(typeName);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>ApiResponse of List&lt;DynamicProperty&gt;</returns>
        public ApiResponse<List<DynamicProperty>> DynamicPropertiesGetPropertiesWithHttpInfo(string typeName)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesGetProperties");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetProperties: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetProperties: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<DynamicProperty>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<DynamicProperty>)ApiClient.Deserialize(localVarResponse, typeof(List<DynamicProperty>)));
            
        }

        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>Task of List&lt;DynamicProperty&gt;</returns>
        public async System.Threading.Tasks.Task<List<DynamicProperty>> DynamicPropertiesGetPropertiesAsync(string typeName)
        {
             ApiResponse<List<DynamicProperty>> localVarResponse = await DynamicPropertiesGetPropertiesAsyncWithHttpInfo(typeName);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <returns>Task of ApiResponse (List&lt;DynamicProperty&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<DynamicProperty>>> DynamicPropertiesGetPropertiesAsyncWithHttpInfo(string typeName)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null)
                throw new ApiException(400, "Missing required parameter 'typeName' when calling VirtoCommercePlatformApi->DynamicPropertiesGetProperties");

            var localVarPath = "/api/platform/dynamic/types/{typeName}/properties";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetProperties: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesGetProperties: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<DynamicProperty>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<DynamicProperty>)ApiClient.Deserialize(localVarResponse, typeof(List<DynamicProperty>)));
            
        }
        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public void DynamicPropertiesSaveDictionaryItems(string typeName, string propertyId, List<DynamicPropertyDictionaryItem> items)
        {
             DynamicPropertiesSaveDictionaryItemsWithHttpInfo(typeName, propertyId, items);
        }

        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> DynamicPropertiesSaveDictionaryItemsWithHttpInfo(string typeName, string propertyId, List<DynamicPropertyDictionaryItem> items)
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
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            if (items.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(items); // http body (model) parameter
            }
            else
            {
                localVarPostBody = items; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DynamicPropertiesSaveDictionaryItemsAsync(string typeName, string propertyId, List<DynamicPropertyDictionaryItem> items)
        {
             await DynamicPropertiesSaveDictionaryItemsAsyncWithHttpInfo(typeName, propertyId, items);

        }

        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> DynamicPropertiesSaveDictionaryItemsAsyncWithHttpInfo(string typeName, string propertyId, List<DynamicPropertyDictionaryItem> items)
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
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            if (items.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(items); // http body (model) parameter
            }
            else
            {
                localVarPostBody = items; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public void DynamicPropertiesUpdateProperty(string typeName, string propertyId, DynamicProperty property)
        {
             DynamicPropertiesUpdatePropertyWithHttpInfo(typeName, propertyId, property);
        }

        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> DynamicPropertiesUpdatePropertyWithHttpInfo(string typeName, string propertyId, DynamicProperty property)
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
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            if (property.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                localVarPostBody = property; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesUpdateProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesUpdateProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DynamicPropertiesUpdatePropertyAsync(string typeName, string propertyId, DynamicProperty property)
        {
             await DynamicPropertiesUpdatePropertyAsyncWithHttpInfo(typeName, propertyId, property);

        }

        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> DynamicPropertiesUpdatePropertyAsyncWithHttpInfo(string typeName, string propertyId, DynamicProperty property)
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
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (typeName != null) localVarPathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) localVarPathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            if (property.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                localVarPostBody = property; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesUpdateProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling DynamicPropertiesUpdateProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>Job</returns>
        public Job JobsGetStatus(string id)
        {
             ApiResponse<Job> localVarResponse = JobsGetStatusWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>ApiResponse of Job</returns>
        public ApiResponse<Job> JobsGetStatusWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->JobsGetStatus");

            var localVarPath = "/api/platform/jobs/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling JobsGetStatus: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling JobsGetStatus: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Job>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Job)ApiClient.Deserialize(localVarResponse, typeof(Job)));
            
        }

        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of Job</returns>
        public async System.Threading.Tasks.Task<Job> JobsGetStatusAsync(string id)
        {
             ApiResponse<Job> localVarResponse = await JobsGetStatusAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Job ID.</param>
        /// <returns>Task of ApiResponse (Job)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Job>> JobsGetStatusAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->JobsGetStatus");

            var localVarPath = "/api/platform/jobs/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling JobsGetStatus: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling JobsGetStatus: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Job>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Job)ApiClient.Deserialize(localVarResponse, typeof(Job)));
            
        }
        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;string&gt;</returns>
        public List<string> LocalizationGetLocales()
        {
             ApiResponse<List<string>> localVarResponse = LocalizationGetLocalesWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;string&gt;</returns>
        public ApiResponse<List<string>> LocalizationGetLocalesWithHttpInfo()
        {

            var localVarPath = "/api/platform/localization/locales";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling LocalizationGetLocales: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling LocalizationGetLocales: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<string>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>)ApiClient.Deserialize(localVarResponse, typeof(List<string>)));
            
        }

        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;string&gt;</returns>
        public async System.Threading.Tasks.Task<List<string>> LocalizationGetLocalesAsync()
        {
             ApiResponse<List<string>> localVarResponse = await LocalizationGetLocalesAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Return all aviable locales 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;string&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<string>>> LocalizationGetLocalesAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/localization/locales";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling LocalizationGetLocales: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling LocalizationGetLocales: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<string>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<string>)ApiClient.Deserialize(localVarResponse, typeof(List<string>)));
            
        }
        /// <summary>
        /// Get all dependent modules for module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>List&lt;ModuleDescriptor&gt;</returns>
        public List<ModuleDescriptor> ModulesGetDependingModules(List<ModuleDescriptor> moduleDescriptors)
        {
             ApiResponse<List<ModuleDescriptor>> localVarResponse = ModulesGetDependingModulesWithHttpInfo(moduleDescriptors);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get all dependent modules for module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>ApiResponse of List&lt;ModuleDescriptor&gt;</returns>
        public ApiResponse<List<ModuleDescriptor>> ModulesGetDependingModulesWithHttpInfo(List<ModuleDescriptor> moduleDescriptors)
        {
            // verify the required parameter 'moduleDescriptors' is set
            if (moduleDescriptors == null)
                throw new ApiException(400, "Missing required parameter 'moduleDescriptors' when calling VirtoCommercePlatformApi->ModulesGetDependingModules");

            var localVarPath = "/api/platform/modules/getdependents";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (moduleDescriptors.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(moduleDescriptors); // http body (model) parameter
            }
            else
            {
                localVarPostBody = moduleDescriptors; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetDependingModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetDependingModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<ModuleDescriptor>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<ModuleDescriptor>)ApiClient.Deserialize(localVarResponse, typeof(List<ModuleDescriptor>)));
            
        }

        /// <summary>
        /// Get all dependent modules for module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>Task of List&lt;ModuleDescriptor&gt;</returns>
        public async System.Threading.Tasks.Task<List<ModuleDescriptor>> ModulesGetDependingModulesAsync(List<ModuleDescriptor> moduleDescriptors)
        {
             ApiResponse<List<ModuleDescriptor>> localVarResponse = await ModulesGetDependingModulesAsyncWithHttpInfo(moduleDescriptors);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get all dependent modules for module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>Task of ApiResponse (List&lt;ModuleDescriptor&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<ModuleDescriptor>>> ModulesGetDependingModulesAsyncWithHttpInfo(List<ModuleDescriptor> moduleDescriptors)
        {
            // verify the required parameter 'moduleDescriptors' is set
            if (moduleDescriptors == null)
                throw new ApiException(400, "Missing required parameter 'moduleDescriptors' when calling VirtoCommercePlatformApi->ModulesGetDependingModules");

            var localVarPath = "/api/platform/modules/getdependents";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (moduleDescriptors.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(moduleDescriptors); // http body (model) parameter
            }
            else
            {
                localVarPostBody = moduleDescriptors; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetDependingModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetDependingModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<ModuleDescriptor>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<ModuleDescriptor>)ApiClient.Deserialize(localVarResponse, typeof(List<ModuleDescriptor>)));
            
        }
        /// <summary>
        /// Returns a flat expanded  list of modules that depend on passed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>List&lt;ModuleDescriptor&gt;</returns>
        public List<ModuleDescriptor> ModulesGetMissingDependencies(List<ModuleDescriptor> moduleDescriptors)
        {
             ApiResponse<List<ModuleDescriptor>> localVarResponse = ModulesGetMissingDependenciesWithHttpInfo(moduleDescriptors);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Returns a flat expanded  list of modules that depend on passed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>ApiResponse of List&lt;ModuleDescriptor&gt;</returns>
        public ApiResponse<List<ModuleDescriptor>> ModulesGetMissingDependenciesWithHttpInfo(List<ModuleDescriptor> moduleDescriptors)
        {
            // verify the required parameter 'moduleDescriptors' is set
            if (moduleDescriptors == null)
                throw new ApiException(400, "Missing required parameter 'moduleDescriptors' when calling VirtoCommercePlatformApi->ModulesGetMissingDependencies");

            var localVarPath = "/api/platform/modules/getmissingdependencies";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (moduleDescriptors.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(moduleDescriptors); // http body (model) parameter
            }
            else
            {
                localVarPostBody = moduleDescriptors; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetMissingDependencies: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetMissingDependencies: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<ModuleDescriptor>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<ModuleDescriptor>)ApiClient.Deserialize(localVarResponse, typeof(List<ModuleDescriptor>)));
            
        }

        /// <summary>
        /// Returns a flat expanded  list of modules that depend on passed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>Task of List&lt;ModuleDescriptor&gt;</returns>
        public async System.Threading.Tasks.Task<List<ModuleDescriptor>> ModulesGetMissingDependenciesAsync(List<ModuleDescriptor> moduleDescriptors)
        {
             ApiResponse<List<ModuleDescriptor>> localVarResponse = await ModulesGetMissingDependenciesAsyncWithHttpInfo(moduleDescriptors);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Returns a flat expanded  list of modules that depend on passed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns>Task of ApiResponse (List&lt;ModuleDescriptor&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<ModuleDescriptor>>> ModulesGetMissingDependenciesAsyncWithHttpInfo(List<ModuleDescriptor> moduleDescriptors)
        {
            // verify the required parameter 'moduleDescriptors' is set
            if (moduleDescriptors == null)
                throw new ApiException(400, "Missing required parameter 'moduleDescriptors' when calling VirtoCommercePlatformApi->ModulesGetMissingDependencies");

            var localVarPath = "/api/platform/modules/getmissingdependencies";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (moduleDescriptors.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(moduleDescriptors); // http body (model) parameter
            }
            else
            {
                localVarPostBody = moduleDescriptors; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetMissingDependencies: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetMissingDependencies: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<ModuleDescriptor>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<ModuleDescriptor>)ApiClient.Deserialize(localVarResponse, typeof(List<ModuleDescriptor>)));
            
        }
        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;ModuleDescriptor&gt;</returns>
        public List<ModuleDescriptor> ModulesGetModules()
        {
             ApiResponse<List<ModuleDescriptor>> localVarResponse = ModulesGetModulesWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;ModuleDescriptor&gt;</returns>
        public ApiResponse<List<ModuleDescriptor>> ModulesGetModulesWithHttpInfo()
        {

            var localVarPath = "/api/platform/modules";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<ModuleDescriptor>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<ModuleDescriptor>)ApiClient.Deserialize(localVarResponse, typeof(List<ModuleDescriptor>)));
            
        }

        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;ModuleDescriptor&gt;</returns>
        public async System.Threading.Tasks.Task<List<ModuleDescriptor>> ModulesGetModulesAsync()
        {
             ApiResponse<List<ModuleDescriptor>> localVarResponse = await ModulesGetModulesAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;ModuleDescriptor&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<ModuleDescriptor>>> ModulesGetModulesAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/modules";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesGetModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<ModuleDescriptor>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<ModuleDescriptor>)ApiClient.Deserialize(localVarResponse, typeof(List<ModuleDescriptor>)));
            
        }
        /// <summary>
        /// Install modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules for install</param>
        /// <returns>ModulePushNotification</returns>
        public ModulePushNotification ModulesInstallModules(List<ModuleDescriptor> modules)
        {
             ApiResponse<ModulePushNotification> localVarResponse = ModulesInstallModulesWithHttpInfo(modules);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Install modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules for install</param>
        /// <returns>ApiResponse of ModulePushNotification</returns>
        public ApiResponse<ModulePushNotification> ModulesInstallModulesWithHttpInfo(List<ModuleDescriptor> modules)
        {
            // verify the required parameter 'modules' is set
            if (modules == null)
                throw new ApiException(400, "Missing required parameter 'modules' when calling VirtoCommercePlatformApi->ModulesInstallModules");

            var localVarPath = "/api/platform/modules/install";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (modules.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(modules); // http body (model) parameter
            }
            else
            {
                localVarPostBody = modules; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesInstallModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesInstallModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ModulePushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ModulePushNotification)ApiClient.Deserialize(localVarResponse, typeof(ModulePushNotification)));
            
        }

        /// <summary>
        /// Install modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules for install</param>
        /// <returns>Task of ModulePushNotification</returns>
        public async System.Threading.Tasks.Task<ModulePushNotification> ModulesInstallModulesAsync(List<ModuleDescriptor> modules)
        {
             ApiResponse<ModulePushNotification> localVarResponse = await ModulesInstallModulesAsyncWithHttpInfo(modules);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Install modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules for install</param>
        /// <returns>Task of ApiResponse (ModulePushNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ModulePushNotification>> ModulesInstallModulesAsyncWithHttpInfo(List<ModuleDescriptor> modules)
        {
            // verify the required parameter 'modules' is set
            if (modules == null)
                throw new ApiException(400, "Missing required parameter 'modules' when calling VirtoCommercePlatformApi->ModulesInstallModules");

            var localVarPath = "/api/platform/modules/install";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (modules.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(modules); // http body (model) parameter
            }
            else
            {
                localVarPostBody = modules; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesInstallModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesInstallModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ModulePushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ModulePushNotification)ApiClient.Deserialize(localVarResponse, typeof(ModulePushNotification)));
            
        }
        /// <summary>
        /// Reload  modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns></returns>
        public void ModulesReloadModules()
        {
             ModulesReloadModulesWithHttpInfo();
        }

        /// <summary>
        /// Reload  modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> ModulesReloadModulesWithHttpInfo()
        {

            var localVarPath = "/api/platform/modules/reload";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesReloadModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesReloadModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Reload  modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ModulesReloadModulesAsync()
        {
             await ModulesReloadModulesAsyncWithHttpInfo();

        }

        /// <summary>
        /// Reload  modules 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> ModulesReloadModulesAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/modules/reload";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesReloadModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesReloadModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns></returns>
        public void ModulesRestart()
        {
             ModulesRestartWithHttpInfo();
        }

        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> ModulesRestartWithHttpInfo()
        {

            var localVarPath = "/api/platform/modules/restart";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesRestart: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesRestart: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ModulesRestartAsync()
        {
             await ModulesRestartAsyncWithHttpInfo();

        }

        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> ModulesRestartAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/modules/restart";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesRestart: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesRestart: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Auto-install modules with specified groups 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ModuleAutoInstallPushNotification</returns>
        public ModuleAutoInstallPushNotification ModulesTryToAutoInstallModules()
        {
             ApiResponse<ModuleAutoInstallPushNotification> localVarResponse = ModulesTryToAutoInstallModulesWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Auto-install modules with specified groups 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of ModuleAutoInstallPushNotification</returns>
        public ApiResponse<ModuleAutoInstallPushNotification> ModulesTryToAutoInstallModulesWithHttpInfo()
        {

            var localVarPath = "/api/platform/modules/autoinstall";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesTryToAutoInstallModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesTryToAutoInstallModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ModuleAutoInstallPushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ModuleAutoInstallPushNotification)ApiClient.Deserialize(localVarResponse, typeof(ModuleAutoInstallPushNotification)));
            
        }

        /// <summary>
        /// Auto-install modules with specified groups 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ModuleAutoInstallPushNotification</returns>
        public async System.Threading.Tasks.Task<ModuleAutoInstallPushNotification> ModulesTryToAutoInstallModulesAsync()
        {
             ApiResponse<ModuleAutoInstallPushNotification> localVarResponse = await ModulesTryToAutoInstallModulesAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Auto-install modules with specified groups 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (ModuleAutoInstallPushNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ModuleAutoInstallPushNotification>> ModulesTryToAutoInstallModulesAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/modules/autoinstall";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesTryToAutoInstallModules: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesTryToAutoInstallModules: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ModuleAutoInstallPushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ModuleAutoInstallPushNotification)ApiClient.Deserialize(localVarResponse, typeof(ModuleAutoInstallPushNotification)));
            
        }
        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules</param>
        /// <returns>ModulePushNotification</returns>
        public ModulePushNotification ModulesUninstallModule(List<ModuleDescriptor> modules)
        {
             ApiResponse<ModulePushNotification> localVarResponse = ModulesUninstallModuleWithHttpInfo(modules);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules</param>
        /// <returns>ApiResponse of ModulePushNotification</returns>
        public ApiResponse<ModulePushNotification> ModulesUninstallModuleWithHttpInfo(List<ModuleDescriptor> modules)
        {
            // verify the required parameter 'modules' is set
            if (modules == null)
                throw new ApiException(400, "Missing required parameter 'modules' when calling VirtoCommercePlatformApi->ModulesUninstallModule");

            var localVarPath = "/api/platform/modules/uninstall";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (modules.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(modules); // http body (model) parameter
            }
            else
            {
                localVarPostBody = modules; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesUninstallModule: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesUninstallModule: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ModulePushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ModulePushNotification)ApiClient.Deserialize(localVarResponse, typeof(ModulePushNotification)));
            
        }

        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules</param>
        /// <returns>Task of ModulePushNotification</returns>
        public async System.Threading.Tasks.Task<ModulePushNotification> ModulesUninstallModuleAsync(List<ModuleDescriptor> modules)
        {
             ApiResponse<ModulePushNotification> localVarResponse = await ModulesUninstallModuleAsyncWithHttpInfo(modules);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="modules">modules</param>
        /// <returns>Task of ApiResponse (ModulePushNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ModulePushNotification>> ModulesUninstallModuleAsyncWithHttpInfo(List<ModuleDescriptor> modules)
        {
            // verify the required parameter 'modules' is set
            if (modules == null)
                throw new ApiException(400, "Missing required parameter 'modules' when calling VirtoCommercePlatformApi->ModulesUninstallModule");

            var localVarPath = "/api/platform/modules/uninstall";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (modules.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(modules); // http body (model) parameter
            }
            else
            {
                localVarPostBody = modules; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesUninstallModule: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesUninstallModule: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ModulePushNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ModulePushNotification)ApiClient.Deserialize(localVarResponse, typeof(ModulePushNotification)));
            
        }
        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ModuleDescriptor</returns>
        public ModuleDescriptor ModulesUploadModuleArchive()
        {
             ApiResponse<ModuleDescriptor> localVarResponse = ModulesUploadModuleArchiveWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of ModuleDescriptor</returns>
        public ApiResponse<ModuleDescriptor> ModulesUploadModuleArchiveWithHttpInfo()
        {

            var localVarPath = "/api/platform/modules/localstorage";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesUploadModuleArchive: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesUploadModuleArchive: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ModuleDescriptor>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ModuleDescriptor)ApiClient.Deserialize(localVarResponse, typeof(ModuleDescriptor)));
            
        }

        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ModuleDescriptor</returns>
        public async System.Threading.Tasks.Task<ModuleDescriptor> ModulesUploadModuleArchiveAsync()
        {
             ApiResponse<ModuleDescriptor> localVarResponse = await ModulesUploadModuleArchiveAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (ModuleDescriptor)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ModuleDescriptor>> ModulesUploadModuleArchiveAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/modules/localstorage";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling ModulesUploadModuleArchive: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling ModulesUploadModuleArchive: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ModuleDescriptor>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ModuleDescriptor)ApiClient.Deserialize(localVarResponse, typeof(ModuleDescriptor)));
            
        }
        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns></returns>
        public void NotificationsDeleteNotificationTemplate(string id)
        {
             NotificationsDeleteNotificationTemplateWithHttpInfo(id);
        }

        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> NotificationsDeleteNotificationTemplateWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsDeleteNotificationTemplate");

            var localVarPath = "/api/platform/notification/template/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task NotificationsDeleteNotificationTemplateAsync(string id)
        {
             await NotificationsDeleteNotificationTemplateAsyncWithHttpInfo(id);

        }

        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Template id</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> NotificationsDeleteNotificationTemplateAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsDeleteNotificationTemplate");

            var localVarPath = "/api/platform/notification/template/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>Notification</returns>
        public Notification NotificationsGetNotification(string id)
        {
             ApiResponse<Notification> localVarResponse = NotificationsGetNotificationWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>ApiResponse of Notification</returns>
        public ApiResponse<Notification> NotificationsGetNotificationWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsGetNotification");

            var localVarPath = "/api/platform/notification/notification/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotification: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotification: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Notification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Notification)ApiClient.Deserialize(localVarResponse, typeof(Notification)));
            
        }

        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of Notification</returns>
        public async System.Threading.Tasks.Task<Notification> NotificationsGetNotificationAsync(string id)
        {
             ApiResponse<Notification> localVarResponse = await NotificationsGetNotificationAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Sending notification id</param>
        /// <returns>Task of ApiResponse (Notification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Notification>> NotificationsGetNotificationAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsGetNotification");

            var localVarPath = "/api/platform/notification/notification/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotification: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotification: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Notification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Notification)ApiClient.Deserialize(localVarResponse, typeof(Notification)));
            
        }
        /// <summary>
        /// Get all notification journal Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>SearchNotificationsResult</returns>
        public SearchNotificationsResult NotificationsGetNotificationJournal(int? start, int? count)
        {
             ApiResponse<SearchNotificationsResult> localVarResponse = NotificationsGetNotificationJournalWithHttpInfo(start, count);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get all notification journal Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>ApiResponse of SearchNotificationsResult</returns>
        public ApiResponse<SearchNotificationsResult> NotificationsGetNotificationJournalWithHttpInfo(int? start, int? count)
        {
            // verify the required parameter 'start' is set
            if (start == null)
                throw new ApiException(400, "Missing required parameter 'start' when calling VirtoCommercePlatformApi->NotificationsGetNotificationJournal");
            // verify the required parameter 'count' is set
            if (count == null)
                throw new ApiException(400, "Missing required parameter 'count' when calling VirtoCommercePlatformApi->NotificationsGetNotificationJournal");

            var localVarPath = "/api/platform/notification/journal";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (start != null) localVarQueryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
            if (count != null) localVarQueryParams.Add("count", ApiClient.ParameterToString(count)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationJournal: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationJournal: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SearchNotificationsResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SearchNotificationsResult)ApiClient.Deserialize(localVarResponse, typeof(SearchNotificationsResult)));
            
        }

        /// <summary>
        /// Get all notification journal Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of SearchNotificationsResult</returns>
        public async System.Threading.Tasks.Task<SearchNotificationsResult> NotificationsGetNotificationJournalAsync(int? start, int? count)
        {
             ApiResponse<SearchNotificationsResult> localVarResponse = await NotificationsGetNotificationJournalAsyncWithHttpInfo(start, count);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get all notification journal Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of ApiResponse (SearchNotificationsResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<SearchNotificationsResult>> NotificationsGetNotificationJournalAsyncWithHttpInfo(int? start, int? count)
        {
            // verify the required parameter 'start' is set
            if (start == null)
                throw new ApiException(400, "Missing required parameter 'start' when calling VirtoCommercePlatformApi->NotificationsGetNotificationJournal");
            // verify the required parameter 'count' is set
            if (count == null)
                throw new ApiException(400, "Missing required parameter 'count' when calling VirtoCommercePlatformApi->NotificationsGetNotificationJournal");

            var localVarPath = "/api/platform/notification/journal";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (start != null) localVarQueryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
            if (count != null) localVarQueryParams.Add("count", ApiClient.ParameterToString(count)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationJournal: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationJournal: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SearchNotificationsResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SearchNotificationsResult)ApiClient.Deserialize(localVarResponse, typeof(SearchNotificationsResult)));
            
        }
        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <param name="language">Locale of template (optional)</param>
        /// <returns>NotificationTemplate</returns>
        public NotificationTemplate NotificationsGetNotificationTemplate(string type, string objectId = null, string objectTypeId = null, string language = null)
        {
             ApiResponse<NotificationTemplate> localVarResponse = NotificationsGetNotificationTemplateWithHttpInfo(type, objectId, objectTypeId, language);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <param name="language">Locale of template (optional)</param>
        /// <returns>ApiResponse of NotificationTemplate</returns>
        public ApiResponse<NotificationTemplate> NotificationsGetNotificationTemplateWithHttpInfo(string type, string objectId = null, string objectTypeId = null, string language = null)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplate");

            var localVarPath = "/api/platform/notification/template";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (type != null) localVarQueryParams.Add("type", ApiClient.ParameterToString(type)); // query parameter
            if (objectId != null) localVarQueryParams.Add("objectId", ApiClient.ParameterToString(objectId)); // query parameter
            if (objectTypeId != null) localVarQueryParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // query parameter
            if (language != null) localVarQueryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<NotificationTemplate>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (NotificationTemplate)ApiClient.Deserialize(localVarResponse, typeof(NotificationTemplate)));
            
        }

        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <param name="language">Locale of template (optional)</param>
        /// <returns>Task of NotificationTemplate</returns>
        public async System.Threading.Tasks.Task<NotificationTemplate> NotificationsGetNotificationTemplateAsync(string type, string objectId = null, string objectTypeId = null, string language = null)
        {
             ApiResponse<NotificationTemplate> localVarResponse = await NotificationsGetNotificationTemplateAsyncWithHttpInfo(type, objectId, objectTypeId, language);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <param name="language">Locale of template (optional)</param>
        /// <returns>Task of ApiResponse (NotificationTemplate)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<NotificationTemplate>> NotificationsGetNotificationTemplateAsyncWithHttpInfo(string type, string objectId = null, string objectTypeId = null, string language = null)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplate");

            var localVarPath = "/api/platform/notification/template";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (type != null) localVarQueryParams.Add("type", ApiClient.ParameterToString(type)); // query parameter
            if (objectId != null) localVarQueryParams.Add("objectId", ApiClient.ParameterToString(objectId)); // query parameter
            if (objectTypeId != null) localVarQueryParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // query parameter
            if (language != null) localVarQueryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<NotificationTemplate>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (NotificationTemplate)ApiClient.Deserialize(localVarResponse, typeof(NotificationTemplate)));
            
        }
        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>NotificationTemplate</returns>
        public NotificationTemplate NotificationsGetNotificationTemplateById(string id)
        {
             ApiResponse<NotificationTemplate> localVarResponse = NotificationsGetNotificationTemplateByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>ApiResponse of NotificationTemplate</returns>
        public ApiResponse<NotificationTemplate> NotificationsGetNotificationTemplateByIdWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplateById");

            var localVarPath = "/api/platform/notification/template/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplateById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplateById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<NotificationTemplate>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (NotificationTemplate)ApiClient.Deserialize(localVarResponse, typeof(NotificationTemplate)));
            
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of NotificationTemplate</returns>
        public async System.Threading.Tasks.Task<NotificationTemplate> NotificationsGetNotificationTemplateByIdAsync(string id)
        {
             ApiResponse<NotificationTemplate> localVarResponse = await NotificationsGetNotificationTemplateByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of ApiResponse (NotificationTemplate)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<NotificationTemplate>> NotificationsGetNotificationTemplateByIdAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplateById");

            var localVarPath = "/api/platform/notification/template/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplateById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplateById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<NotificationTemplate>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (NotificationTemplate)ApiClient.Deserialize(localVarResponse, typeof(NotificationTemplate)));
            
        }
        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <returns>List&lt;NotificationTemplate&gt;</returns>
        public List<NotificationTemplate> NotificationsGetNotificationTemplates(string type, string objectId = null, string objectTypeId = null)
        {
             ApiResponse<List<NotificationTemplate>> localVarResponse = NotificationsGetNotificationTemplatesWithHttpInfo(type, objectId, objectTypeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <returns>ApiResponse of List&lt;NotificationTemplate&gt;</returns>
        public ApiResponse<List<NotificationTemplate>> NotificationsGetNotificationTemplatesWithHttpInfo(string type, string objectId = null, string objectTypeId = null)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplates");

            var localVarPath = "/api/platform/notification/templates";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (type != null) localVarQueryParams.Add("type", ApiClient.ParameterToString(type)); // query parameter
            if (objectId != null) localVarQueryParams.Add("objectId", ApiClient.ParameterToString(objectId)); // query parameter
            if (objectTypeId != null) localVarQueryParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplates: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplates: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<NotificationTemplate>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<NotificationTemplate>)ApiClient.Deserialize(localVarResponse, typeof(List<NotificationTemplate>)));
            
        }

        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <returns>Task of List&lt;NotificationTemplate&gt;</returns>
        public async System.Threading.Tasks.Task<List<NotificationTemplate>> NotificationsGetNotificationTemplatesAsync(string type, string objectId = null, string objectTypeId = null)
        {
             ApiResponse<List<NotificationTemplate>> localVarResponse = await NotificationsGetNotificationTemplatesAsyncWithHttpInfo(type, objectId, objectTypeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of              template. By default object id and object type id &#x3D; \&quot;Platform\&quot;. For example for store with id &#x3D; \&quot;SampleStore\&quot;, objectId &#x3D; \&quot;SampleStore\&quot;, objectTypeId &#x3D; \&quot;Store\&quot;.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template (optional)</param>
        /// <param name="objectTypeId">Object type id of template (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;NotificationTemplate&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<NotificationTemplate>>> NotificationsGetNotificationTemplatesAsyncWithHttpInfo(string type, string objectId = null, string objectTypeId = null)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetNotificationTemplates");

            var localVarPath = "/api/platform/notification/templates";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (type != null) localVarQueryParams.Add("type", ApiClient.ParameterToString(type)); // query parameter
            if (objectId != null) localVarQueryParams.Add("objectId", ApiClient.ParameterToString(objectId)); // query parameter
            if (objectTypeId != null) localVarQueryParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplates: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotificationTemplates: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<NotificationTemplate>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<NotificationTemplate>)ApiClient.Deserialize(localVarResponse, typeof(List<NotificationTemplate>)));
            
        }
        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;Notification&gt;</returns>
        public List<Notification> NotificationsGetNotifications()
        {
             ApiResponse<List<Notification>> localVarResponse = NotificationsGetNotificationsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;Notification&gt;</returns>
        public ApiResponse<List<Notification>> NotificationsGetNotificationsWithHttpInfo()
        {

            var localVarPath = "/api/platform/notification";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotifications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotifications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<Notification>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<Notification>)ApiClient.Deserialize(localVarResponse, typeof(List<Notification>)));
            
        }

        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;Notification&gt;</returns>
        public async System.Threading.Tasks.Task<List<Notification>> NotificationsGetNotificationsAsync()
        {
             ApiResponse<List<Notification>> localVarResponse = await NotificationsGetNotificationsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;Notification&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<Notification>>> NotificationsGetNotificationsAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/notification";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotifications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetNotifications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<Notification>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<Notification>)ApiClient.Deserialize(localVarResponse, typeof(List<Notification>)));
            
        }
        /// <summary>
        /// Get notification journal for object Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>SearchNotificationsResult</returns>
        public SearchNotificationsResult NotificationsGetObjectNotificationJournal(string objectId, string objectTypeId, int? start, int? count)
        {
             ApiResponse<SearchNotificationsResult> localVarResponse = NotificationsGetObjectNotificationJournalWithHttpInfo(objectId, objectTypeId, start, count);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get notification journal for object Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>ApiResponse of SearchNotificationsResult</returns>
        public ApiResponse<SearchNotificationsResult> NotificationsGetObjectNotificationJournalWithHttpInfo(string objectId, string objectTypeId, int? start, int? count)
        {
            // verify the required parameter 'objectId' is set
            if (objectId == null)
                throw new ApiException(400, "Missing required parameter 'objectId' when calling VirtoCommercePlatformApi->NotificationsGetObjectNotificationJournal");
            // verify the required parameter 'objectTypeId' is set
            if (objectTypeId == null)
                throw new ApiException(400, "Missing required parameter 'objectTypeId' when calling VirtoCommercePlatformApi->NotificationsGetObjectNotificationJournal");
            // verify the required parameter 'start' is set
            if (start == null)
                throw new ApiException(400, "Missing required parameter 'start' when calling VirtoCommercePlatformApi->NotificationsGetObjectNotificationJournal");
            // verify the required parameter 'count' is set
            if (count == null)
                throw new ApiException(400, "Missing required parameter 'count' when calling VirtoCommercePlatformApi->NotificationsGetObjectNotificationJournal");

            var localVarPath = "/api/platform/notification/journal/{objectId}/{objectTypeId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (objectId != null) localVarPathParams.Add("objectId", ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) localVarPathParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // path parameter
            if (start != null) localVarQueryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
            if (count != null) localVarQueryParams.Add("count", ApiClient.ParameterToString(count)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetObjectNotificationJournal: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetObjectNotificationJournal: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SearchNotificationsResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SearchNotificationsResult)ApiClient.Deserialize(localVarResponse, typeof(SearchNotificationsResult)));
            
        }

        /// <summary>
        /// Get notification journal for object Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of SearchNotificationsResult</returns>
        public async System.Threading.Tasks.Task<SearchNotificationsResult> NotificationsGetObjectNotificationJournalAsync(string objectId, string objectTypeId, int? start, int? count)
        {
             ApiResponse<SearchNotificationsResult> localVarResponse = await NotificationsGetObjectNotificationJournalAsyncWithHttpInfo(objectId, objectTypeId, start, count);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get notification journal for object Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used              for paging.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>Task of ApiResponse (SearchNotificationsResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<SearchNotificationsResult>> NotificationsGetObjectNotificationJournalAsyncWithHttpInfo(string objectId, string objectTypeId, int? start, int? count)
        {
            // verify the required parameter 'objectId' is set
            if (objectId == null)
                throw new ApiException(400, "Missing required parameter 'objectId' when calling VirtoCommercePlatformApi->NotificationsGetObjectNotificationJournal");
            // verify the required parameter 'objectTypeId' is set
            if (objectTypeId == null)
                throw new ApiException(400, "Missing required parameter 'objectTypeId' when calling VirtoCommercePlatformApi->NotificationsGetObjectNotificationJournal");
            // verify the required parameter 'start' is set
            if (start == null)
                throw new ApiException(400, "Missing required parameter 'start' when calling VirtoCommercePlatformApi->NotificationsGetObjectNotificationJournal");
            // verify the required parameter 'count' is set
            if (count == null)
                throw new ApiException(400, "Missing required parameter 'count' when calling VirtoCommercePlatformApi->NotificationsGetObjectNotificationJournal");

            var localVarPath = "/api/platform/notification/journal/{objectId}/{objectTypeId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (objectId != null) localVarPathParams.Add("objectId", ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) localVarPathParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // path parameter
            if (start != null) localVarQueryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
            if (count != null) localVarQueryParams.Add("count", ApiClient.ParameterToString(count)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetObjectNotificationJournal: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetObjectNotificationJournal: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SearchNotificationsResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SearchNotificationsResult)ApiClient.Deserialize(localVarResponse, typeof(SearchNotificationsResult)));
            
        }
        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>List&lt;NotificationParameter&gt;</returns>
        public List<NotificationParameter> NotificationsGetTestingParameters(string type)
        {
             ApiResponse<List<NotificationParameter>> localVarResponse = NotificationsGetTestingParametersWithHttpInfo(type);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>ApiResponse of List&lt;NotificationParameter&gt;</returns>
        public ApiResponse<List<NotificationParameter>> NotificationsGetTestingParametersWithHttpInfo(string type)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetTestingParameters");

            var localVarPath = "/api/platform/notification/template/{type}/getTestingParameters";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (type != null) localVarPathParams.Add("type", ApiClient.ParameterToString(type)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetTestingParameters: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetTestingParameters: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<NotificationParameter>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<NotificationParameter>)ApiClient.Deserialize(localVarResponse, typeof(List<NotificationParameter>)));
            
        }

        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>Task of List&lt;NotificationParameter&gt;</returns>
        public async System.Threading.Tasks.Task<List<NotificationParameter>> NotificationsGetTestingParametersAsync(string type)
        {
             ApiResponse<List<NotificationParameter>> localVarResponse = await NotificationsGetTestingParametersAsyncWithHttpInfo(type);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type">Notification type</param>
        /// <returns>Task of ApiResponse (List&lt;NotificationParameter&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<NotificationParameter>>> NotificationsGetTestingParametersAsyncWithHttpInfo(string type)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->NotificationsGetTestingParameters");

            var localVarPath = "/api/platform/notification/template/{type}/getTestingParameters";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (type != null) localVarPathParams.Add("type", ApiClient.ParameterToString(type)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetTestingParameters: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsGetTestingParameters: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<NotificationParameter>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<NotificationParameter>)ApiClient.Deserialize(localVarResponse, typeof(List<NotificationParameter>)));
            
        }
        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>RenderNotificationContentResult</returns>
        public RenderNotificationContentResult NotificationsRenderNotificationContent(TestNotificationRequest request)
        {
             ApiResponse<RenderNotificationContentResult> localVarResponse = NotificationsRenderNotificationContentWithHttpInfo(request);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>ApiResponse of RenderNotificationContentResult</returns>
        public ApiResponse<RenderNotificationContentResult> NotificationsRenderNotificationContentWithHttpInfo(TestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->NotificationsRenderNotificationContent");

            var localVarPath = "/api/platform/notification/template/rendernotificationcontent";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsRenderNotificationContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsRenderNotificationContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<RenderNotificationContentResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (RenderNotificationContentResult)ApiClient.Deserialize(localVarResponse, typeof(RenderNotificationContentResult)));
            
        }

        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of RenderNotificationContentResult</returns>
        public async System.Threading.Tasks.Task<RenderNotificationContentResult> NotificationsRenderNotificationContentAsync(TestNotificationRequest request)
        {
             ApiResponse<RenderNotificationContentResult> localVarResponse = await NotificationsRenderNotificationContentAsyncWithHttpInfo(request);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (RenderNotificationContentResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<RenderNotificationContentResult>> NotificationsRenderNotificationContentAsyncWithHttpInfo(TestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->NotificationsRenderNotificationContent");

            var localVarPath = "/api/platform/notification/template/rendernotificationcontent";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsRenderNotificationContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsRenderNotificationContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<RenderNotificationContentResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (RenderNotificationContentResult)ApiClient.Deserialize(localVarResponse, typeof(RenderNotificationContentResult)));
            
        }
        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status              this string is empty, otherwise string contains error message.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>string</returns>
        public string NotificationsSendNotification(TestNotificationRequest request)
        {
             ApiResponse<string> localVarResponse = NotificationsSendNotificationWithHttpInfo(request);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status              this string is empty, otherwise string contains error message.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>ApiResponse of string</returns>
        public ApiResponse<string> NotificationsSendNotificationWithHttpInfo(TestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->NotificationsSendNotification");

            var localVarPath = "/api/platform/notification/template/sendnotification";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsSendNotification: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsSendNotification: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<string>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (string)ApiClient.Deserialize(localVarResponse, typeof(string)));
            
        }

        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status              this string is empty, otherwise string contains error message.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of string</returns>
        public async System.Threading.Tasks.Task<string> NotificationsSendNotificationAsync(TestNotificationRequest request)
        {
             ApiResponse<string> localVarResponse = await NotificationsSendNotificationAsyncWithHttpInfo(request);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.              Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status              this string is empty, otherwise string contains error message.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Test notification request</param>
        /// <returns>Task of ApiResponse (string)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<string>> NotificationsSendNotificationAsyncWithHttpInfo(TestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->NotificationsSendNotification");

            var localVarPath = "/api/platform/notification/template/sendnotification";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsSendNotification: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsSendNotification: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<string>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (string)ApiClient.Deserialize(localVarResponse, typeof(string)));
            
        }
        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns></returns>
        public void NotificationsStopSendingNotifications(List<string> ids)
        {
             NotificationsStopSendingNotificationsWithHttpInfo(ids);
        }

        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> NotificationsStopSendingNotificationsWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->NotificationsStopSendingNotifications");

            var localVarPath = "/api/platform/notification/stopnotifications";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(ids); // http body (model) parameter
            }
            else
            {
                localVarPostBody = ids; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsStopSendingNotifications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsStopSendingNotifications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task NotificationsStopSendingNotificationsAsync(List<string> ids)
        {
             await NotificationsStopSendingNotificationsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> NotificationsStopSendingNotificationsAsyncWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->NotificationsStopSendingNotifications");

            var localVarPath = "/api/platform/notification/stopnotifications";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(ids); // http body (model) parameter
            }
            else
            {
                localVarPostBody = ids; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsStopSendingNotifications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsStopSendingNotifications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns></returns>
        public void NotificationsUpdateNotificationTemplate(NotificationTemplate notificationTemplate)
        {
             NotificationsUpdateNotificationTemplateWithHttpInfo(notificationTemplate);
        }

        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> NotificationsUpdateNotificationTemplateWithHttpInfo(NotificationTemplate notificationTemplate)
        {
            // verify the required parameter 'notificationTemplate' is set
            if (notificationTemplate == null)
                throw new ApiException(400, "Missing required parameter 'notificationTemplate' when calling VirtoCommercePlatformApi->NotificationsUpdateNotificationTemplate");

            var localVarPath = "/api/platform/notification/template";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (notificationTemplate.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(notificationTemplate); // http body (model) parameter
            }
            else
            {
                localVarPostBody = notificationTemplate; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task NotificationsUpdateNotificationTemplateAsync(NotificationTemplate notificationTemplate)
        {
             await NotificationsUpdateNotificationTemplateAsyncWithHttpInfo(notificationTemplate);

        }

        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> NotificationsUpdateNotificationTemplateAsyncWithHttpInfo(NotificationTemplate notificationTemplate)
        {
            // verify the required parameter 'notificationTemplate' is set
            if (notificationTemplate == null)
                throw new ApiException(400, "Missing required parameter 'notificationTemplate' when calling VirtoCommercePlatformApi->NotificationsUpdateNotificationTemplate");

            var localVarPath = "/api/platform/notification/template";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (notificationTemplate.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(notificationTemplate); // http body (model) parameter
            }
            else
            {
                localVarPostBody = notificationTemplate; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>PushNotificationSearchResult</returns>
        public PushNotificationSearchResult PushNotificationMarkAllAsRead()
        {
             ApiResponse<PushNotificationSearchResult> localVarResponse = PushNotificationMarkAllAsReadWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of PushNotificationSearchResult</returns>
        public ApiResponse<PushNotificationSearchResult> PushNotificationMarkAllAsReadWithHttpInfo()
        {

            var localVarPath = "/api/platform/pushnotifications/markAllAsRead";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling PushNotificationMarkAllAsRead: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling PushNotificationMarkAllAsRead: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<PushNotificationSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (PushNotificationSearchResult)ApiClient.Deserialize(localVarResponse, typeof(PushNotificationSearchResult)));
            
        }

        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of PushNotificationSearchResult</returns>
        public async System.Threading.Tasks.Task<PushNotificationSearchResult> PushNotificationMarkAllAsReadAsync()
        {
             ApiResponse<PushNotificationSearchResult> localVarResponse = await PushNotificationMarkAllAsReadAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (PushNotificationSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<PushNotificationSearchResult>> PushNotificationMarkAllAsReadAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/pushnotifications/markAllAsRead";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling PushNotificationMarkAllAsRead: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling PushNotificationMarkAllAsRead: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<PushNotificationSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (PushNotificationSearchResult)ApiClient.Deserialize(localVarResponse, typeof(PushNotificationSearchResult)));
            
        }
        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>PushNotificationSearchResult</returns>
        public PushNotificationSearchResult PushNotificationSearch(PushNotificationSearchCriteria criteria)
        {
             ApiResponse<PushNotificationSearchResult> localVarResponse = PushNotificationSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>ApiResponse of PushNotificationSearchResult</returns>
        public ApiResponse<PushNotificationSearchResult> PushNotificationSearchWithHttpInfo(PushNotificationSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling VirtoCommercePlatformApi->PushNotificationSearch");

            var localVarPath = "/api/platform/pushnotifications";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling PushNotificationSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling PushNotificationSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<PushNotificationSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (PushNotificationSearchResult)ApiClient.Deserialize(localVarResponse, typeof(PushNotificationSearchResult)));
            
        }

        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of PushNotificationSearchResult</returns>
        public async System.Threading.Tasks.Task<PushNotificationSearchResult> PushNotificationSearchAsync(PushNotificationSearchCriteria criteria)
        {
             ApiResponse<PushNotificationSearchResult> localVarResponse = await PushNotificationSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters.</param>
        /// <returns>Task of ApiResponse (PushNotificationSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<PushNotificationSearchResult>> PushNotificationSearchAsyncWithHttpInfo(PushNotificationSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling VirtoCommercePlatformApi->PushNotificationSearch");

            var localVarPath = "/api/platform/pushnotifications";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling PushNotificationSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling PushNotificationSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<PushNotificationSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (PushNotificationSearchResult)ApiClient.Deserialize(localVarResponse, typeof(PushNotificationSearchResult)));
            
        }
        /// <summary>
        /// Change password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>SecurityResult</returns>
        public SecurityResult SecurityChangePassword(string userName, ChangePasswordInfo changePassword)
        {
             ApiResponse<SecurityResult> localVarResponse = SecurityChangePasswordWithHttpInfo(userName, changePassword);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Change password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>ApiResponse of SecurityResult</returns>
        public ApiResponse<SecurityResult> SecurityChangePasswordWithHttpInfo(string userName, ChangePasswordInfo changePassword)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityChangePassword");
            // verify the required parameter 'changePassword' is set
            if (changePassword == null)
                throw new ApiException(400, "Missing required parameter 'changePassword' when calling VirtoCommercePlatformApi->SecurityChangePassword");

            var localVarPath = "/api/platform/security/users/{userName}/changepassword";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (userName != null) localVarPathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            if (changePassword.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(changePassword); // http body (model) parameter
            }
            else
            {
                localVarPostBody = changePassword; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityChangePassword: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityChangePassword: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SecurityResult)ApiClient.Deserialize(localVarResponse, typeof(SecurityResult)));
            
        }

        /// <summary>
        /// Change password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of SecurityResult</returns>
        public async System.Threading.Tasks.Task<SecurityResult> SecurityChangePasswordAsync(string userName, ChangePasswordInfo changePassword)
        {
             ApiResponse<SecurityResult> localVarResponse = await SecurityChangePasswordAsyncWithHttpInfo(userName, changePassword);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Change password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Task of ApiResponse (SecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<SecurityResult>> SecurityChangePasswordAsyncWithHttpInfo(string userName, ChangePasswordInfo changePassword)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityChangePassword");
            // verify the required parameter 'changePassword' is set
            if (changePassword == null)
                throw new ApiException(400, "Missing required parameter 'changePassword' when calling VirtoCommercePlatformApi->SecurityChangePassword");

            var localVarPath = "/api/platform/security/users/{userName}/changepassword";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (userName != null) localVarPathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            if (changePassword.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(changePassword); // http body (model) parameter
            }
            else
            {
                localVarPostBody = changePassword; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityChangePassword: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityChangePassword: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SecurityResult)ApiClient.Deserialize(localVarResponse, typeof(SecurityResult)));
            
        }
        /// <summary>
        /// Create new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>SecurityResult</returns>
        public SecurityResult SecurityCreateAsync(ApplicationUserExtended user)
        {
             ApiResponse<SecurityResult> localVarResponse = SecurityCreateAsyncWithHttpInfo(user);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>ApiResponse of SecurityResult</returns>
        public ApiResponse<SecurityResult> SecurityCreateAsyncWithHttpInfo(ApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling VirtoCommercePlatformApi->SecurityCreateAsync");

            var localVarPath = "/api/platform/security/users/create";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (user.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(user); // http body (model) parameter
            }
            else
            {
                localVarPostBody = user; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityCreateAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityCreateAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SecurityResult)ApiClient.Deserialize(localVarResponse, typeof(SecurityResult)));
            
        }

        /// <summary>
        /// Create new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of SecurityResult</returns>
        public async System.Threading.Tasks.Task<SecurityResult> SecurityCreateAsyncAsync(ApplicationUserExtended user)
        {
             ApiResponse<SecurityResult> localVarResponse = await SecurityCreateAsyncAsyncWithHttpInfo(user);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create new user 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (SecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<SecurityResult>> SecurityCreateAsyncAsyncWithHttpInfo(ApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling VirtoCommercePlatformApi->SecurityCreateAsync");

            var localVarPath = "/api/platform/security/users/create";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (user.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(user); // http body (model) parameter
            }
            else
            {
                localVarPostBody = user; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityCreateAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityCreateAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SecurityResult)ApiClient.Deserialize(localVarResponse, typeof(SecurityResult)));
            
        }
        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns></returns>
        public void SecurityDeleteAsync(List<string> names)
        {
             SecurityDeleteAsyncWithHttpInfo(names);
        }

        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> SecurityDeleteAsyncWithHttpInfo(List<string> names)
        {
            // verify the required parameter 'names' is set
            if (names == null)
                throw new ApiException(400, "Missing required parameter 'names' when calling VirtoCommercePlatformApi->SecurityDeleteAsync");

            var localVarPath = "/api/platform/security/users";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (names != null) localVarQueryParams.Add("names", ApiClient.ParameterToString(names)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityDeleteAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityDeleteAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SecurityDeleteAsyncAsync(List<string> names)
        {
             await SecurityDeleteAsyncAsyncWithHttpInfo(names);

        }

        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="names">An array of user names.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> SecurityDeleteAsyncAsyncWithHttpInfo(List<string> names)
        {
            // verify the required parameter 'names' is set
            if (names == null)
                throw new ApiException(400, "Missing required parameter 'names' when calling VirtoCommercePlatformApi->SecurityDeleteAsync");

            var localVarPath = "/api/platform/security/users";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (names != null) localVarQueryParams.Add("names", ApiClient.ParameterToString(names)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityDeleteAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityDeleteAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void SecurityDeleteRoles(List<string> ids)
        {
             SecurityDeleteRolesWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> SecurityDeleteRolesWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->SecurityDeleteRoles");

            var localVarPath = "/api/platform/security/roles";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityDeleteRoles: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityDeleteRoles: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SecurityDeleteRolesAsync(List<string> ids)
        {
             await SecurityDeleteRolesAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> SecurityDeleteRolesAsyncWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommercePlatformApi->SecurityDeleteRoles");

            var localVarPath = "/api/platform/security/roles";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityDeleteRoles: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityDeleteRoles: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>ApiAccount</returns>
        public ApiAccount SecurityGenerateNewApiAccount(string type)
        {
             ApiResponse<ApiAccount> localVarResponse = SecurityGenerateNewApiAccountWithHttpInfo(type);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>ApiResponse of ApiAccount</returns>
        public ApiResponse<ApiAccount> SecurityGenerateNewApiAccountWithHttpInfo(string type)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->SecurityGenerateNewApiAccount");

            var localVarPath = "/api/platform/security/apiaccounts/new";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (type != null) localVarQueryParams.Add("type", ApiClient.ParameterToString(type)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGenerateNewApiAccount: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGenerateNewApiAccount: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ApiAccount>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ApiAccount)ApiClient.Deserialize(localVarResponse, typeof(ApiAccount)));
            
        }

        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>Task of ApiAccount</returns>
        public async System.Threading.Tasks.Task<ApiAccount> SecurityGenerateNewApiAccountAsync(string type)
        {
             ApiResponse<ApiAccount> localVarResponse = await SecurityGenerateNewApiAccountAsyncWithHttpInfo(type);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="type"></param>
        /// <returns>Task of ApiResponse (ApiAccount)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ApiAccount>> SecurityGenerateNewApiAccountAsyncWithHttpInfo(string type)
        {
            // verify the required parameter 'type' is set
            if (type == null)
                throw new ApiException(400, "Missing required parameter 'type' when calling VirtoCommercePlatformApi->SecurityGenerateNewApiAccount");

            var localVarPath = "/api/platform/security/apiaccounts/new";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (type != null) localVarQueryParams.Add("type", ApiClient.ParameterToString(type)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGenerateNewApiAccount: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGenerateNewApiAccount: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ApiAccount>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ApiAccount)ApiClient.Deserialize(localVarResponse, typeof(ApiAccount)));
            
        }
        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApplicationUserExtended</returns>
        public ApplicationUserExtended SecurityGetCurrentUser()
        {
             ApiResponse<ApplicationUserExtended> localVarResponse = SecurityGetCurrentUserWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of ApplicationUserExtended</returns>
        public ApiResponse<ApplicationUserExtended> SecurityGetCurrentUserWithHttpInfo()
        {

            var localVarPath = "/api/platform/security/currentuser";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetCurrentUser: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetCurrentUser: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ApplicationUserExtended)ApiClient.Deserialize(localVarResponse, typeof(ApplicationUserExtended)));
            
        }

        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<ApplicationUserExtended> SecurityGetCurrentUserAsync()
        {
             ApiResponse<ApplicationUserExtended> localVarResponse = await SecurityGetCurrentUserAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (ApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ApplicationUserExtended>> SecurityGetCurrentUserAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/security/currentuser";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetCurrentUser: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetCurrentUser: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ApplicationUserExtended)ApiClient.Deserialize(localVarResponse, typeof(ApplicationUserExtended)));
            
        }
        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;Permission&gt;</returns>
        public List<Permission> SecurityGetPermissions()
        {
             ApiResponse<List<Permission>> localVarResponse = SecurityGetPermissionsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;Permission&gt;</returns>
        public ApiResponse<List<Permission>> SecurityGetPermissionsWithHttpInfo()
        {

            var localVarPath = "/api/platform/security/permissions";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetPermissions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetPermissions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<Permission>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<Permission>)ApiClient.Deserialize(localVarResponse, typeof(List<Permission>)));
            
        }

        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;Permission&gt;</returns>
        public async System.Threading.Tasks.Task<List<Permission>> SecurityGetPermissionsAsync()
        {
             ApiResponse<List<Permission>> localVarResponse = await SecurityGetPermissionsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;Permission&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<Permission>>> SecurityGetPermissionsAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/security/permissions";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetPermissions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetPermissions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<Permission>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<Permission>)ApiClient.Deserialize(localVarResponse, typeof(List<Permission>)));
            
        }
        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>Role</returns>
        public Role SecurityGetRole(string roleId)
        {
             ApiResponse<Role> localVarResponse = SecurityGetRoleWithHttpInfo(roleId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>ApiResponse of Role</returns>
        public ApiResponse<Role> SecurityGetRoleWithHttpInfo(string roleId)
        {
            // verify the required parameter 'roleId' is set
            if (roleId == null)
                throw new ApiException(400, "Missing required parameter 'roleId' when calling VirtoCommercePlatformApi->SecurityGetRole");

            var localVarPath = "/api/platform/security/roles/{roleId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (roleId != null) localVarPathParams.Add("roleId", ApiClient.ParameterToString(roleId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetRole: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetRole: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Role>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Role)ApiClient.Deserialize(localVarResponse, typeof(Role)));
            
        }

        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>Task of Role</returns>
        public async System.Threading.Tasks.Task<Role> SecurityGetRoleAsync(string roleId)
        {
             ApiResponse<Role> localVarResponse = await SecurityGetRoleAsyncWithHttpInfo(roleId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="roleId"></param>
        /// <returns>Task of ApiResponse (Role)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Role>> SecurityGetRoleAsyncWithHttpInfo(string roleId)
        {
            // verify the required parameter 'roleId' is set
            if (roleId == null)
                throw new ApiException(400, "Missing required parameter 'roleId' when calling VirtoCommercePlatformApi->SecurityGetRole");

            var localVarPath = "/api/platform/security/roles/{roleId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (roleId != null) localVarPathParams.Add("roleId", ApiClient.ParameterToString(roleId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetRole: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetRole: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Role>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Role)ApiClient.Deserialize(localVarResponse, typeof(Role)));
            
        }
        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>ApplicationUserExtended</returns>
        public ApplicationUserExtended SecurityGetUserById(string id)
        {
             ApiResponse<ApplicationUserExtended> localVarResponse = SecurityGetUserByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>ApiResponse of ApplicationUserExtended</returns>
        public ApiResponse<ApplicationUserExtended> SecurityGetUserByIdWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SecurityGetUserById");

            var localVarPath = "/api/platform/security/users/id/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetUserById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetUserById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ApplicationUserExtended)ApiClient.Deserialize(localVarResponse, typeof(ApplicationUserExtended)));
            
        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of ApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<ApplicationUserExtended> SecurityGetUserByIdAsync(string id)
        {
             ApiResponse<ApplicationUserExtended> localVarResponse = await SecurityGetUserByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"></param>
        /// <returns>Task of ApiResponse (ApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ApplicationUserExtended>> SecurityGetUserByIdAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SecurityGetUserById");

            var localVarPath = "/api/platform/security/users/id/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetUserById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetUserById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ApplicationUserExtended)ApiClient.Deserialize(localVarResponse, typeof(ApplicationUserExtended)));
            
        }
        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>ApplicationUserExtended</returns>
        public ApplicationUserExtended SecurityGetUserByName(string userName)
        {
             ApiResponse<ApplicationUserExtended> localVarResponse = SecurityGetUserByNameWithHttpInfo(userName);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>ApiResponse of ApplicationUserExtended</returns>
        public ApiResponse<ApplicationUserExtended> SecurityGetUserByNameWithHttpInfo(string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityGetUserByName");

            var localVarPath = "/api/platform/security/users/{userName}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (userName != null) localVarPathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetUserByName: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetUserByName: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ApplicationUserExtended)ApiClient.Deserialize(localVarResponse, typeof(ApplicationUserExtended)));
            
        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of ApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<ApplicationUserExtended> SecurityGetUserByNameAsync(string userName)
        {
             ApiResponse<ApplicationUserExtended> localVarResponse = await SecurityGetUserByNameAsyncWithHttpInfo(userName);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <returns>Task of ApiResponse (ApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ApplicationUserExtended>> SecurityGetUserByNameAsyncWithHttpInfo(string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityGetUserByName");

            var localVarPath = "/api/platform/security/users/{userName}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (userName != null) localVarPathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetUserByName: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityGetUserByName: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ApplicationUserExtended)ApiClient.Deserialize(localVarResponse, typeof(ApplicationUserExtended)));
            
        }
        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>ApplicationUserExtended</returns>
        public ApplicationUserExtended SecurityLogin(UserLogin model)
        {
             ApiResponse<ApplicationUserExtended> localVarResponse = SecurityLoginWithHttpInfo(model);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>ApiResponse of ApplicationUserExtended</returns>
        public ApiResponse<ApplicationUserExtended> SecurityLoginWithHttpInfo(UserLogin model)
        {
            // verify the required parameter 'model' is set
            if (model == null)
                throw new ApiException(400, "Missing required parameter 'model' when calling VirtoCommercePlatformApi->SecurityLogin");

            var localVarPath = "/api/platform/security/login";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (model.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(model); // http body (model) parameter
            }
            else
            {
                localVarPostBody = model; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityLogin: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityLogin: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ApplicationUserExtended)ApiClient.Deserialize(localVarResponse, typeof(ApplicationUserExtended)));
            
        }

        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of ApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<ApplicationUserExtended> SecurityLoginAsync(UserLogin model)
        {
             ApiResponse<ApplicationUserExtended> localVarResponse = await SecurityLoginAsyncWithHttpInfo(model);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="model">User credentials.</param>
        /// <returns>Task of ApiResponse (ApplicationUserExtended)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ApplicationUserExtended>> SecurityLoginAsyncWithHttpInfo(UserLogin model)
        {
            // verify the required parameter 'model' is set
            if (model == null)
                throw new ApiException(400, "Missing required parameter 'model' when calling VirtoCommercePlatformApi->SecurityLogin");

            var localVarPath = "/api/platform/security/login";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (model.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(model); // http body (model) parameter
            }
            else
            {
                localVarPostBody = model; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityLogin: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityLogin: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<ApplicationUserExtended>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (ApplicationUserExtended)ApiClient.Deserialize(localVarResponse, typeof(ApplicationUserExtended)));
            
        }
        /// <summary>
        /// Sign out 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns></returns>
        public void SecurityLogout()
        {
             SecurityLogoutWithHttpInfo();
        }

        /// <summary>
        /// Sign out 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> SecurityLogoutWithHttpInfo()
        {

            var localVarPath = "/api/platform/security/logout";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityLogout: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityLogout: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Sign out 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SecurityLogoutAsync()
        {
             await SecurityLogoutAsyncWithHttpInfo();

        }

        /// <summary>
        /// Sign out 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> SecurityLogoutAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/security/logout";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityLogout: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityLogout: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Reset password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>SecurityResult</returns>
        public SecurityResult SecurityResetPassword(string userName, ResetPasswordInfo resetPassword)
        {
             ApiResponse<SecurityResult> localVarResponse = SecurityResetPasswordWithHttpInfo(userName, resetPassword);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Reset password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>ApiResponse of SecurityResult</returns>
        public ApiResponse<SecurityResult> SecurityResetPasswordWithHttpInfo(string userName, ResetPasswordInfo resetPassword)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityResetPassword");
            // verify the required parameter 'resetPassword' is set
            if (resetPassword == null)
                throw new ApiException(400, "Missing required parameter 'resetPassword' when calling VirtoCommercePlatformApi->SecurityResetPassword");

            var localVarPath = "/api/platform/security/users/{userName}/resetpassword";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (userName != null) localVarPathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            if (resetPassword.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(resetPassword); // http body (model) parameter
            }
            else
            {
                localVarPostBody = resetPassword; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityResetPassword: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityResetPassword: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SecurityResult)ApiClient.Deserialize(localVarResponse, typeof(SecurityResult)));
            
        }

        /// <summary>
        /// Reset password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of SecurityResult</returns>
        public async System.Threading.Tasks.Task<SecurityResult> SecurityResetPasswordAsync(string userName, ResetPasswordInfo resetPassword)
        {
             ApiResponse<SecurityResult> localVarResponse = await SecurityResetPasswordAsyncWithHttpInfo(userName, resetPassword);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Reset password 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>Task of ApiResponse (SecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<SecurityResult>> SecurityResetPasswordAsyncWithHttpInfo(string userName, ResetPasswordInfo resetPassword)
        {
            // verify the required parameter 'userName' is set
            if (userName == null)
                throw new ApiException(400, "Missing required parameter 'userName' when calling VirtoCommercePlatformApi->SecurityResetPassword");
            // verify the required parameter 'resetPassword' is set
            if (resetPassword == null)
                throw new ApiException(400, "Missing required parameter 'resetPassword' when calling VirtoCommercePlatformApi->SecurityResetPassword");

            var localVarPath = "/api/platform/security/users/{userName}/resetpassword";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (userName != null) localVarPathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            if (resetPassword.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(resetPassword); // http body (model) parameter
            }
            else
            {
                localVarPostBody = resetPassword; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityResetPassword: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityResetPassword: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SecurityResult)ApiClient.Deserialize(localVarResponse, typeof(SecurityResult)));
            
        }
        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>RoleSearchResponse</returns>
        public RoleSearchResponse SecuritySearchRoles(RoleSearchRequest request)
        {
             ApiResponse<RoleSearchResponse> localVarResponse = SecuritySearchRolesWithHttpInfo(request);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>ApiResponse of RoleSearchResponse</returns>
        public ApiResponse<RoleSearchResponse> SecuritySearchRolesWithHttpInfo(RoleSearchRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->SecuritySearchRoles");

            var localVarPath = "/api/platform/security/roles";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecuritySearchRoles: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecuritySearchRoles: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<RoleSearchResponse>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (RoleSearchResponse)ApiClient.Deserialize(localVarResponse, typeof(RoleSearchResponse)));
            
        }

        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of RoleSearchResponse</returns>
        public async System.Threading.Tasks.Task<RoleSearchResponse> SecuritySearchRolesAsync(RoleSearchRequest request)
        {
             ApiResponse<RoleSearchResponse> localVarResponse = await SecuritySearchRolesAsyncWithHttpInfo(request);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (RoleSearchResponse)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<RoleSearchResponse>> SecuritySearchRolesAsyncWithHttpInfo(RoleSearchRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->SecuritySearchRoles");

            var localVarPath = "/api/platform/security/roles";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecuritySearchRoles: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecuritySearchRoles: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<RoleSearchResponse>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (RoleSearchResponse)ApiClient.Deserialize(localVarResponse, typeof(RoleSearchResponse)));
            
        }
        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>UserSearchResponse</returns>
        public UserSearchResponse SecuritySearchUsersAsync(UserSearchRequest request)
        {
             ApiResponse<UserSearchResponse> localVarResponse = SecuritySearchUsersAsyncWithHttpInfo(request);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>ApiResponse of UserSearchResponse</returns>
        public ApiResponse<UserSearchResponse> SecuritySearchUsersAsyncWithHttpInfo(UserSearchRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->SecuritySearchUsersAsync");

            var localVarPath = "/api/platform/security/users";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecuritySearchUsersAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecuritySearchUsersAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<UserSearchResponse>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (UserSearchResponse)ApiClient.Deserialize(localVarResponse, typeof(UserSearchResponse)));
            
        }

        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of UserSearchResponse</returns>
        public async System.Threading.Tasks.Task<UserSearchResponse> SecuritySearchUsersAsyncAsync(UserSearchRequest request)
        {
             ApiResponse<UserSearchResponse> localVarResponse = await SecuritySearchUsersAsyncAsyncWithHttpInfo(request);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request">Search parameters.</param>
        /// <returns>Task of ApiResponse (UserSearchResponse)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<UserSearchResponse>> SecuritySearchUsersAsyncAsyncWithHttpInfo(UserSearchRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling VirtoCommercePlatformApi->SecuritySearchUsersAsync");

            var localVarPath = "/api/platform/security/users";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecuritySearchUsersAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecuritySearchUsersAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<UserSearchResponse>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (UserSearchResponse)ApiClient.Deserialize(localVarResponse, typeof(UserSearchResponse)));
            
        }
        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>SecurityResult</returns>
        public SecurityResult SecurityUpdateAsync(ApplicationUserExtended user)
        {
             ApiResponse<SecurityResult> localVarResponse = SecurityUpdateAsyncWithHttpInfo(user);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>ApiResponse of SecurityResult</returns>
        public ApiResponse<SecurityResult> SecurityUpdateAsyncWithHttpInfo(ApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling VirtoCommercePlatformApi->SecurityUpdateAsync");

            var localVarPath = "/api/platform/security/users";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (user.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(user); // http body (model) parameter
            }
            else
            {
                localVarPostBody = user; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityUpdateAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityUpdateAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SecurityResult)ApiClient.Deserialize(localVarResponse, typeof(SecurityResult)));
            
        }

        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of SecurityResult</returns>
        public async System.Threading.Tasks.Task<SecurityResult> SecurityUpdateAsyncAsync(ApplicationUserExtended user)
        {
             ApiResponse<SecurityResult> localVarResponse = await SecurityUpdateAsyncAsyncWithHttpInfo(user);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="user">User details.</param>
        /// <returns>Task of ApiResponse (SecurityResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<SecurityResult>> SecurityUpdateAsyncAsyncWithHttpInfo(ApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null)
                throw new ApiException(400, "Missing required parameter 'user' when calling VirtoCommercePlatformApi->SecurityUpdateAsync");

            var localVarPath = "/api/platform/security/users";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (user.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(user); // http body (model) parameter
            }
            else
            {
                localVarPostBody = user; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityUpdateAsync: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityUpdateAsync: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<SecurityResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (SecurityResult)ApiClient.Deserialize(localVarResponse, typeof(SecurityResult)));
            
        }
        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>Role</returns>
        public Role SecurityUpdateRole(Role role)
        {
             ApiResponse<Role> localVarResponse = SecurityUpdateRoleWithHttpInfo(role);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>ApiResponse of Role</returns>
        public ApiResponse<Role> SecurityUpdateRoleWithHttpInfo(Role role)
        {
            // verify the required parameter 'role' is set
            if (role == null)
                throw new ApiException(400, "Missing required parameter 'role' when calling VirtoCommercePlatformApi->SecurityUpdateRole");

            var localVarPath = "/api/platform/security/roles";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (role.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(role); // http body (model) parameter
            }
            else
            {
                localVarPostBody = role; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityUpdateRole: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityUpdateRole: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Role>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Role)ApiClient.Deserialize(localVarResponse, typeof(Role)));
            
        }

        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>Task of Role</returns>
        public async System.Threading.Tasks.Task<Role> SecurityUpdateRoleAsync(Role role)
        {
             ApiResponse<Role> localVarResponse = await SecurityUpdateRoleAsyncWithHttpInfo(role);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="role"></param>
        /// <returns>Task of ApiResponse (Role)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Role>> SecurityUpdateRoleAsyncWithHttpInfo(Role role)
        {
            // verify the required parameter 'role' is set
            if (role == null)
                throw new ApiException(400, "Missing required parameter 'role' when calling VirtoCommercePlatformApi->SecurityUpdateRole");

            var localVarPath = "/api/platform/security/roles";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (role.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(role); // http body (model) parameter
            }
            else
            {
                localVarPostBody = role; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SecurityUpdateRole: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SecurityUpdateRole: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Role>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Role)ApiClient.Deserialize(localVarResponse, typeof(Role)));
            
        }
        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;Setting&gt;</returns>
        public List<Setting> SettingGetAllSettings()
        {
             ApiResponse<List<Setting>> localVarResponse = SettingGetAllSettingsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;Setting&gt;</returns>
        public ApiResponse<List<Setting>> SettingGetAllSettingsWithHttpInfo()
        {

            var localVarPath = "/api/platform/settings";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SettingGetAllSettings: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SettingGetAllSettings: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<Setting>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<Setting>)ApiClient.Deserialize(localVarResponse, typeof(List<Setting>)));
            
        }

        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;Setting&gt;</returns>
        public async System.Threading.Tasks.Task<List<Setting>> SettingGetAllSettingsAsync()
        {
             ApiResponse<List<Setting>> localVarResponse = await SettingGetAllSettingsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;Setting&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<Setting>>> SettingGetAllSettingsAsyncWithHttpInfo()
        {

            var localVarPath = "/api/platform/settings";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SettingGetAllSettings: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SettingGetAllSettings: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<Setting>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<Setting>)ApiClient.Deserialize(localVarResponse, typeof(List<Setting>)));
            
        }
        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>List&lt;Setting&gt;</returns>
        public List<Setting> SettingGetModuleSettings(string id)
        {
             ApiResponse<List<Setting>> localVarResponse = SettingGetModuleSettingsWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>ApiResponse of List&lt;Setting&gt;</returns>
        public ApiResponse<List<Setting>> SettingGetModuleSettingsWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SettingGetModuleSettings");

            var localVarPath = "/api/platform/settings/modules/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SettingGetModuleSettings: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SettingGetModuleSettings: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<Setting>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<Setting>)ApiClient.Deserialize(localVarResponse, typeof(List<Setting>)));
            
        }

        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of List&lt;Setting&gt;</returns>
        public async System.Threading.Tasks.Task<List<Setting>> SettingGetModuleSettingsAsync(string id)
        {
             ApiResponse<List<Setting>> localVarResponse = await SettingGetModuleSettingsAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Module ID.</param>
        /// <returns>Task of ApiResponse (List&lt;Setting&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<Setting>>> SettingGetModuleSettingsAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommercePlatformApi->SettingGetModuleSettings");

            var localVarPath = "/api/platform/settings/modules/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SettingGetModuleSettings: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SettingGetModuleSettings: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<Setting>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<Setting>)ApiClient.Deserialize(localVarResponse, typeof(List<Setting>)));
            
        }
        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">Setting system name.</param>
        /// <returns>Setting</returns>
        public Setting SettingGetSetting(string name)
        {
             ApiResponse<Setting> localVarResponse = SettingGetSettingWithHttpInfo(name);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">Setting system name.</param>
        /// <returns>ApiResponse of Setting</returns>
        public ApiResponse<Setting> SettingGetSettingWithHttpInfo(string name)
        {
            // verify the required parameter 'name' is set
            if (name == null)
                throw new ApiException(400, "Missing required parameter 'name' when calling VirtoCommercePlatformApi->SettingGetSetting");

            var localVarPath = "/api/platform/settings/{name}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (name != null) localVarPathParams.Add("name", ApiClient.ParameterToString(name)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SettingGetSetting: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SettingGetSetting: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Setting>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Setting)ApiClient.Deserialize(localVarResponse, typeof(Setting)));
            
        }

        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">Setting system name.</param>
        /// <returns>Task of Setting</returns>
        public async System.Threading.Tasks.Task<Setting> SettingGetSettingAsync(string name)
        {
             ApiResponse<Setting> localVarResponse = await SettingGetSettingAsyncWithHttpInfo(name);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">Setting system name.</param>
        /// <returns>Task of ApiResponse (Setting)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Setting>> SettingGetSettingAsyncWithHttpInfo(string name)
        {
            // verify the required parameter 'name' is set
            if (name == null)
                throw new ApiException(400, "Missing required parameter 'name' when calling VirtoCommercePlatformApi->SettingGetSetting");

            var localVarPath = "/api/platform/settings/{name}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (name != null) localVarPathParams.Add("name", ApiClient.ParameterToString(name)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SettingGetSetting: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SettingGetSetting: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Setting>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Setting)ApiClient.Deserialize(localVarResponse, typeof(Setting)));
            
        }
        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns></returns>
        public void SettingUpdate(List<Setting> settings)
        {
             SettingUpdateWithHttpInfo(settings);
        }

        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> SettingUpdateWithHttpInfo(List<Setting> settings)
        {
            // verify the required parameter 'settings' is set
            if (settings == null)
                throw new ApiException(400, "Missing required parameter 'settings' when calling VirtoCommercePlatformApi->SettingUpdate");

            var localVarPath = "/api/platform/settings";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (settings.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(settings); // http body (model) parameter
            }
            else
            {
                localVarPostBody = settings; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SettingUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SettingUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SettingUpdateAsync(List<Setting> settings)
        {
             await SettingUpdateAsyncWithHttpInfo(settings);

        }

        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <exception cref="VirtoCommerce.Platform.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="settings"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> SettingUpdateAsyncWithHttpInfo(List<Setting> settings)
        {
            // verify the required parameter 'settings' is set
            if (settings == null)
                throw new ApiException(400, "Missing required parameter 'settings' when calling VirtoCommercePlatformApi->SettingUpdate");

            var localVarPath = "/api/platform/settings";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (settings.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(settings); // http body (model) parameter
            }
            else
            {
                localVarPostBody = settings; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SettingUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SettingUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    }
}
