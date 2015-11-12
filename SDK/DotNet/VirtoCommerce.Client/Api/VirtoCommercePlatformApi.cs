using System;
using System.IO;
using System.Collections.Generic;
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
        /// <returns></returns>
        List<VirtoCommercePlatformWebModelAssetAssetListItem> AssetsSearchAssetItems (string folderUrl, string keyword);
  
        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folderUrl"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetAssetListItem>> AssetsSearchAssetItemsAsync (string folderUrl, string keyword);
        
        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional)</param>
        /// <returns></returns>
        List<VirtoCommercePlatformWebModelAssetBlobInfo> AssetsUploadAsset (string folderUrl, string url);
  
        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional)</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetAsync (string folderUrl, string url);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task AssetsDeleteBlobsAsync (List<string> urls);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task AssetsCreateBlobFolderAsync (VirtoCommercePlatformCoreAssetBlobFolder folder);
        
        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        List<string> DynamicPropertiesGetObjectTypes ();
  
        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<string>> DynamicPropertiesGetObjectTypesAsync ();
        
        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <returns></returns>
        List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesGetProperties (string typeName);
  
        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesGetPropertiesAsync (string typeName);
        
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
        /// <returns>VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesCreatePropertyAsync (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task DynamicPropertiesUpdatePropertyAsync (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task DynamicPropertiesDeletePropertyAsync (string typeName, string propertyId);
        
        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> DynamicPropertiesGetDictionaryItems (string typeName, string propertyId);
  
        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsAsync (string typeName, string propertyId);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task DynamicPropertiesSaveDictionaryItemsAsync (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task DynamicPropertiesDeleteDictionaryItemAsync (string typeName, string propertyId, List<string> ids);
        
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
        /// <returns>VirtoCommercePlatformWebModelJobsJob</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelJobsJob> JobsGetStatusAsync (string id);
        
        /// <summary>
        /// Get all localization files by given language
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="lang"></param>
        /// <returns>InlineResponse200</returns>
        InlineResponse200 LocalizationGetLocalizationFile (string lang);
  
        /// <summary>
        /// Get all localization files by given language
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="lang"></param>
        /// <returns>InlineResponse200</returns>
        System.Threading.Tasks.Task<InlineResponse200> LocalizationGetLocalizationFileAsync (string lang);
        
        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModules ();
  
        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModulesAsync ();
        
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
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesUploadAsync ();
        
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
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesInstallModuleAsync (string fileName);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task ModulesRestartAsync ();
        
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
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModuleByIdAsync (string id);
        
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
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUninstallModuleAsync (string id);
        
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
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUpdateModuleAsync (string id, string fileName);
        
        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotifications ();
  
        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>
        /// Get all registered notification types in platform
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationsAsync ();
        
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
        /// <returns>VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> NotificationsGetNotificationJournalAsync (string objectId, string objectTypeId, int? start, int? count);
        
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
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotification</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotificationAsync (string id);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task NotificationsStopSendingNotificationsAsync (List<string> ids);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task NotificationsUpdateNotificationTemplateAsync (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate);
        
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
        /// <returns>VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> NotificationsRenderNotificationContentAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);
        
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
        /// <returns>string</returns>
        System.Threading.Tasks.Task<string> NotificationsSendNotificationAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task NotificationsDeleteNotificationTemplateAsync (string id);
        
        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <param name="type">Notification type</param>
        /// <returns></returns>
        List<VirtoCommercePlatformCoreNotificationsNotificationParameter> NotificationsGetTestingParameters (string type);
  
        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </remarks>
        /// <param name="type">Notification type</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> NotificationsGetTestingParametersAsync (string type);
        
        /// <summary>
        /// Get notification templates
        /// </summary>
        /// <remarks>
        /// Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </remarks>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns></returns>
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
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplatesAsync (string type, string objectId, string objectTypeId);
        
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
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplateAsync (string type, string objectId, string objectTypeId, string language);
        
        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaIds"></param>
        /// <param name="criteriaOnlyNew"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <param name="criteriaOrderBy"></param>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationSearch (List<string> criteriaIds, bool? criteriaOnlyNew, DateTime? criteriaStartDate, DateTime? criteriaEndDate, int? criteriaStart, int? criteriaCount, string criteriaOrderBy);
  
        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaIds"></param>
        /// <param name="criteriaOnlyNew"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <param name="criteriaOrderBy"></param>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationSearchAsync (List<string> criteriaIds, bool? criteriaOnlyNew, DateTime? criteriaStartDate, DateTime? criteriaEndDate, int? criteriaStart, int? criteriaCount, string criteriaOrderBy);
        
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
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationMarkAllAsReadAsync ();
        
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
        /// <returns>VirtoCommercePlatformCoreSecurityApiAccount</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApiAccount> SecurityGenerateNewApiAccountAsync (string type);
        
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
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetCurrentUserAsync ();
        
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
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityLoginAsync (VirtoCommercePlatformWebModelSecurityUserLogin model);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task SecurityLogoutAsync ();
        
        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommercePlatformCoreSecurityPermission> SecurityGetPermissions ();
  
        /// <summary>
        /// Get all registered permissions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreSecurityPermission>> SecurityGetPermissionsAsync ();
        
        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="requestKeyword"></param>
        /// <param name="requestSkipCount"></param>
        /// <param name="requestTakeCount"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        VirtoCommercePlatformCoreSecurityRoleSearchResponse SecuritySearchRoles (string requestKeyword, int? requestSkipCount, int? requestTakeCount);
  
        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="requestKeyword"></param>
        /// <param name="requestSkipCount"></param>
        /// <param name="requestTakeCount"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRoleSearchResponse> SecuritySearchRolesAsync (string requestKeyword, int? requestSkipCount, int? requestTakeCount);
        
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
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityUpdateRoleAsync (VirtoCommercePlatformCoreSecurityRole role);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task SecurityDeleteRolesAsync (List<string> ids);
        
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
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityGetRoleAsync (string roleId);
        
        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="requestAccountTypes"></param>
        /// <param name="requestKeyword"></param>
        /// <param name="requestSkipCount"></param>
        /// <param name="requestTakeCount"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        VirtoCommercePlatformCoreSecurityUserSearchResponse SecuritySearchUsersAsync (List<string> requestAccountTypes, string requestKeyword, int? requestSkipCount, int? requestTakeCount);
  
        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="requestAccountTypes"></param>
        /// <param name="requestKeyword"></param>
        /// <param name="requestSkipCount"></param>
        /// <param name="requestTakeCount"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityUserSearchResponse> SecuritySearchUsersAsyncAsync (List<string> requestAccountTypes, string requestKeyword, int? requestSkipCount, int? requestTakeCount);
        
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
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityUpdateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task SecurityDeleteAsyncAsync (List<string> names);
        
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
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityCreateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        
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
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByNameAsync (string userName);
        
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
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityChangePasswordAsync (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword);
        
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
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityResetPasswordAsync (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword);
        
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetAllSettings ();
  
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetAllSettingsAsync ();
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task SettingUpdateAsync (List<VirtoCommercePlatformWebModelSettingsSetting> settings);
        
        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns></returns>
        List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetModuleSettings (string id);
  
        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Module ID.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetModuleSettingsAsync (string id);
        
        /// <summary>
        /// Get non-array setting value by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="name">Setting system name.</param>
        /// <returns>InlineResponse200</returns>
        InlineResponse200 SettingGetValue (string name);
  
        /// <summary>
        /// Get non-array setting value by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="name">Setting system name.</param>
        /// <returns>InlineResponse200</returns>
        System.Threading.Tasks.Task<InlineResponse200> SettingGetValueAsync (string name);
        
        /// <summary>
        /// Get array setting values by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="name">Setting system name.</param>
        /// <returns></returns>
        List<Object> SettingGetArray (string name);
  
        /// <summary>
        /// Get array setting values by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="name">Setting system name.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<Object>> SettingGetArrayAsync (string name);
        
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
        /// <returns>VirtoCommercePlatformWebModelSettingsSetting</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelSettingsSetting> SettingGetSettingAsync (string id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult FrontEndSecurityCreate (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> FrontEndSecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended FrontEndSecurityGetUserById (string userId);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> FrontEndSecurityGetUserByIdAsync (string userId);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended FrontEndSecurityGetUserByLogin (string loginProvider, string providerKey);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> FrontEndSecurityGetUserByLoginAsync (string loginProvider, string providerKey);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        VirtoCommercePlatformCoreSecurityApplicationUserExtended FrontEndSecurityGetUserByName (string userName);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> FrontEndSecurityGetUserByNameAsync (string userName);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        VirtoCommercePlatformCoreSecuritySecurityResult FrontEndSecurityResetPassword (string userId, string token, string newPassword);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> FrontEndSecurityResetPasswordAsync (string userId, string token, string newPassword);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        void FrontEndSecurityGenerateResetPasswordToken (string userId, string storeName, string callbackUrl);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task FrontEndSecurityGenerateResetPasswordTokenAsync (string userId, string storeName, string callbackUrl);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <returns>VirtoCommercePlatformWebModelSecuritySignInResult</returns>
        VirtoCommercePlatformWebModelSecuritySignInResult FrontEndSecurityPasswordSignIn (string userName, string password, bool? isPersistent);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <returns>VirtoCommercePlatformWebModelSecuritySignInResult</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelSecuritySignInResult> FrontEndSecurityPasswordSignInAsync (string userName, string password, bool? isPersistent);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class VirtoCommercePlatformApi : IVirtoCommercePlatformApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommercePlatformApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient</param>
        /// <returns></returns>
        public VirtoCommercePlatformApi(ApiClient apiClient)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
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
        /// Search asset folders and blobs 
        /// </summary>
        /// <param name="folderUrl"></param> 
        /// <param name="keyword"></param> 
        /// <returns></returns>            
        public List<VirtoCommercePlatformWebModelAssetAssetListItem> AssetsSearchAssetItems (string folderUrl, string keyword)
        {
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (folderUrl != null) queryParams.Add("folderUrl", ApiClient.ParameterToString(folderUrl)); // query parameter
            if (keyword != null) queryParams.Add("keyword", ApiClient.ParameterToString(keyword)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsSearchAssetItems: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsSearchAssetItems: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelAssetAssetListItem>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelAssetAssetListItem>));
        }
    
        /// <summary>
        /// Search asset folders and blobs 
        /// </summary>
        /// <param name="folderUrl"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetAssetListItem>> AssetsSearchAssetItemsAsync (string folderUrl, string keyword)
        {
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (folderUrl != null) queryParams.Add("folderUrl", ApiClient.ParameterToString(folderUrl)); // query parameter
            if (keyword != null) queryParams.Add("keyword", ApiClient.ParameterToString(keyword)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsSearchAssetItems: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelAssetAssetListItem>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelAssetAssetListItem>));
        }
        
        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param> 
        /// <param name="url">Url for uploaded remote resource (optional)</param> 
        /// <returns></returns>            
        public List<VirtoCommercePlatformWebModelAssetBlobInfo> AssetsUploadAsset (string folderUrl, string url)
        {
            
            // verify the required parameter 'folderUrl' is set
            if (folderUrl == null) throw new ApiException(400, "Missing required parameter 'folderUrl' when calling AssetsUploadAsset");
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (folderUrl != null) queryParams.Add("folderUrl", ApiClient.ParameterToString(folderUrl)); // query parameter
            if (url != null) queryParams.Add("url", ApiClient.ParameterToString(url)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsUploadAsset: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsUploadAsset: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelAssetBlobInfo>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelAssetBlobInfo>));
        }
    
        /// <summary>
        /// Upload assets to the folder Request body can contain multiple files.
        /// </summary>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional)</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelAssetBlobInfo>> AssetsUploadAssetAsync (string folderUrl, string url)
        {
            // verify the required parameter 'folderUrl' is set
            if (folderUrl == null) throw new ApiException(400, "Missing required parameter 'folderUrl' when calling AssetsUploadAsset");
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (folderUrl != null) queryParams.Add("folderUrl", ApiClient.ParameterToString(folderUrl)); // query parameter
            if (url != null) queryParams.Add("url", ApiClient.ParameterToString(url)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsUploadAsset: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelAssetBlobInfo>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelAssetBlobInfo>));
        }
        
        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <param name="urls"></param> 
        /// <returns></returns>            
        public void AssetsDeleteBlobs (List<string> urls)
        {
            
            // verify the required parameter 'urls' is set
            if (urls == null) throw new ApiException(400, "Missing required parameter 'urls' when calling AssetsDeleteBlobs");
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (urls != null) queryParams.Add("urls", ApiClient.ParameterToString(urls)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsDeleteBlobs: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsDeleteBlobs: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete blobs by urls 
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task AssetsDeleteBlobsAsync (List<string> urls)
        {
            // verify the required parameter 'urls' is set
            if (urls == null) throw new ApiException(400, "Missing required parameter 'urls' when calling AssetsDeleteBlobs");
            
    
            var path_ = "/api/platform/assets";
    
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
            
            if (urls != null) queryParams.Add("urls", ApiClient.ParameterToString(urls)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsDeleteBlobs: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <param name="folder"></param> 
        /// <returns></returns>            
        public void AssetsCreateBlobFolder (VirtoCommercePlatformCoreAssetBlobFolder folder)
        {
            
            // verify the required parameter 'folder' is set
            if (folder == null) throw new ApiException(400, "Missing required parameter 'folder' when calling AssetsCreateBlobFolder");
            
    
            var path_ = "/api/platform/assets/folder";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(folder); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsCreateBlobFolder: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsCreateBlobFolder: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Create new blob folder 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task AssetsCreateBlobFolderAsync (VirtoCommercePlatformCoreAssetBlobFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null) throw new ApiException(400, "Missing required parameter 'folder' when calling AssetsCreateBlobFolder");
            
    
            var path_ = "/api/platform/assets/folder";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(folder); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsCreateBlobFolder: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <returns></returns>            
        public List<string> DynamicPropertiesGetObjectTypes ()
        {
            
    
            var path_ = "/api/platform/dynamic/types";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response, typeof(List<string>));
        }
    
        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<string>> DynamicPropertiesGetObjectTypesAsync ()
        {
            
    
            var path_ = "/api/platform/dynamic/types";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + response.Content, response.Content);

            return (List<string>) ApiClient.Deserialize(response, typeof(List<string>));
        }
        
        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <returns></returns>            
        public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesGetProperties (string typeName)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesGetProperties");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetProperties: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetProperties: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>));
        }
    
        /// <summary>
        /// Get dynamic properties registered for object type 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>> DynamicPropertiesGetPropertiesAsync (string typeName)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesGetProperties");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetProperties: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>));
        }
        
        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="property"></param> 
        /// <returns>VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>            
        public VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty DynamicPropertiesCreateProperty (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesCreateProperty");
            
            // verify the required parameter 'property' is set
            if (property == null) throw new ApiException(400, "Missing required parameter 'property' when calling DynamicPropertiesCreateProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(property); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesCreateProperty: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesCreateProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty));
        }
    
        /// <summary>
        /// Add new dynamic property 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="property"></param>
        /// <returns>VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty> DynamicPropertiesCreatePropertyAsync (string typeName, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesCreateProperty");
            // verify the required parameter 'property' is set
            if (property == null) throw new ApiException(400, "Missing required parameter 'property' when calling DynamicPropertiesCreateProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(property); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesCreateProperty: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty));
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
            
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesUpdateProperty");
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesUpdateProperty");
            
            // verify the required parameter 'property' is set
            if (property == null) throw new ApiException(400, "Missing required parameter 'property' when calling DynamicPropertiesUpdateProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(property); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesUpdateProperty: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesUpdateProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update existing dynamic property 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task DynamicPropertiesUpdatePropertyAsync (string typeName, string propertyId, VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty property)
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(property); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesUpdateProperty: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <returns></returns>            
        public void DynamicPropertiesDeleteProperty (string typeName, string propertyId)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesDeleteProperty");
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesDeleteProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesDeleteProperty: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesDeleteProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete dynamic property 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task DynamicPropertiesDeletePropertyAsync (string typeName, string propertyId)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesDeleteProperty");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesDeleteProperty");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesDeleteProperty: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <param name="typeName"></param> 
        /// <param name="propertyId"></param> 
        /// <returns></returns>            
        public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> DynamicPropertiesGetDictionaryItems (string typeName, string propertyId)
        {
            
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesGetDictionaryItems");
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesGetDictionaryItems");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>));
        }
    
        /// <summary>
        /// Get dictionary items 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>> DynamicPropertiesGetDictionaryItemsAsync (string typeName, string propertyId)
        {
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesGetDictionaryItems");
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesGetDictionaryItems");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>));
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
            
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesSaveDictionaryItems");
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesSaveDictionaryItems");
            
            // verify the required parameter 'items' is set
            if (items == null) throw new ApiException(400, "Missing required parameter 'items' when calling DynamicPropertiesSaveDictionaryItems");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(items); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Add or update dictionary items Fill item ID to update existing item or leave it empty to create a new item.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task DynamicPropertiesSaveDictionaryItemsAsync (string typeName, string propertyId, List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem> items)
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(items); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesSaveDictionaryItems: " + response.Content, response.Content);

            
            return;
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
            
            // verify the required parameter 'typeName' is set
            if (typeName == null) throw new ApiException(400, "Missing required parameter 'typeName' when calling DynamicPropertiesDeleteDictionaryItem");
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling DynamicPropertiesDeleteDictionaryItem");
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling DynamicPropertiesDeleteDictionaryItem");
            
    
            var path_ = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete dictionary items 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task DynamicPropertiesDeleteDictionaryItemAsync (string typeName, string propertyId, List<string> ids)
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
            if (typeName != null) pathParams.Add("typeName", ApiClient.ParameterToString(typeName)); // path parameter
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesDeleteDictionaryItem: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <param name="id">Job ID.</param> 
        /// <returns>VirtoCommercePlatformWebModelJobsJob</returns>            
        public VirtoCommercePlatformWebModelJobsJob JobsGetStatus (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling JobsGetStatus");
            
    
            var path_ = "/api/platform/jobs/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling JobsGetStatus: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling JobsGetStatus: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelJobsJob) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelJobsJob));
        }
    
        /// <summary>
        /// Get background job status 
        /// </summary>
        /// <param name="id">Job ID.</param>
        /// <returns>VirtoCommercePlatformWebModelJobsJob</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelJobsJob> JobsGetStatusAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling JobsGetStatus");
            
    
            var path_ = "/api/platform/jobs/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling JobsGetStatus: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelJobsJob) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelJobsJob));
        }
        
        /// <summary>
        /// Get all localization files by given language 
        /// </summary>
        /// <param name="lang"></param> 
        /// <returns>InlineResponse200</returns>            
        public InlineResponse200 LocalizationGetLocalizationFile (string lang)
        {
            
    
            var path_ = "/api/platform/localization";
    
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
            
            if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling LocalizationGetLocalizationFile: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling LocalizationGetLocalizationFile: " + response.ErrorMessage, response.ErrorMessage);
    
            return (InlineResponse200) ApiClient.Deserialize(response, typeof(InlineResponse200));
        }
    
        /// <summary>
        /// Get all localization files by given language 
        /// </summary>
        /// <param name="lang"></param>
        /// <returns>InlineResponse200</returns>
        public async System.Threading.Tasks.Task<InlineResponse200> LocalizationGetLocalizationFileAsync (string lang)
        {
            
    
            var path_ = "/api/platform/localization";
    
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
            
            if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling LocalizationGetLocalizationFile: " + response.Content, response.Content);

            return (InlineResponse200) ApiClient.Deserialize(response, typeof(InlineResponse200));
        }
        
        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModules ()
        {
            
    
            var path_ = "/api/platform/modules";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModules: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModules: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>));
        }
    
        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModulesAsync ()
        {
            
    
            var path_ = "/api/platform/modules";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModules: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>));
        }
        
        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>            
        public VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesUpload ()
        {
            
    
            var path_ = "/api/platform/modules";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpload: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpload: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelPackagingModuleDescriptor) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor));
        }
    
        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesUploadAsync ()
        {
            
    
            var path_ = "/api/platform/modules";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpload: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelPackagingModuleDescriptor) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor));
        }
        
        /// <summary>
        /// Install module from uploaded file 
        /// </summary>
        /// <param name="fileName">Module package file name.</param> 
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>            
        public VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesInstallModule (string fileName)
        {
            
            // verify the required parameter 'fileName' is set
            if (fileName == null) throw new ApiException(400, "Missing required parameter 'fileName' when calling ModulesInstallModule");
            
    
            var path_ = "/api/platform/modules/install";
    
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
            
            if (fileName != null) queryParams.Add("fileName", ApiClient.ParameterToString(fileName)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesInstallModule: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesInstallModule: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification));
        }
    
        /// <summary>
        /// Install module from uploaded file 
        /// </summary>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesInstallModuleAsync (string fileName)
        {
            // verify the required parameter 'fileName' is set
            if (fileName == null) throw new ApiException(400, "Missing required parameter 'fileName' when calling ModulesInstallModule");
            
    
            var path_ = "/api/platform/modules/install";
    
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
            
            if (fileName != null) queryParams.Add("fileName", ApiClient.ParameterToString(fileName)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesInstallModule: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification));
        }
        
        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <returns></returns>            
        public void ModulesRestart ()
        {
            
    
            var path_ = "/api/platform/modules/restart";
    
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
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesRestart: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesRestart: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task ModulesRestartAsync ()
        {
            
    
            var path_ = "/api/platform/modules/restart";
    
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
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesRestart: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get module details 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>            
        public VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesGetModuleById (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ModulesGetModuleById");
            
    
            var path_ = "/api/platform/modules/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModuleById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModuleById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelPackagingModuleDescriptor) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor));
        }
    
        /// <summary>
        /// Get module details 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModuleByIdAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ModulesGetModuleById");
            
    
            var path_ = "/api/platform/modules/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModuleById: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelPackagingModuleDescriptor) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor));
        }
        
        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>            
        public VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesUninstallModule (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ModulesUninstallModule");
            
    
            var path_ = "/api/platform/modules/{id}/uninstall";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUninstallModule: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUninstallModule: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification));
        }
    
        /// <summary>
        /// Uninstall module 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUninstallModuleAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ModulesUninstallModule");
            
    
            var path_ = "/api/platform/modules/{id}/uninstall";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUninstallModule: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification));
        }
        
        /// <summary>
        /// Update module from uploaded file 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <param name="fileName">Module package file name.</param> 
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>            
        public VirtoCommercePlatformWebModelPackagingModulePushNotification ModulesUpdateModule (string id, string fileName)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ModulesUpdateModule");
            
            // verify the required parameter 'fileName' is set
            if (fileName == null) throw new ApiException(400, "Missing required parameter 'fileName' when calling ModulesUpdateModule");
            
    
            var path_ = "/api/platform/modules/{id}/update";
    
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
            
            if (fileName != null) queryParams.Add("fileName", ApiClient.ParameterToString(fileName)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpdateModule: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpdateModule: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification));
        }
    
        /// <summary>
        /// Update module from uploaded file 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns>VirtoCommercePlatformWebModelPackagingModulePushNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModulePushNotification> ModulesUpdateModuleAsync (string id, string fileName)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ModulesUpdateModule");
            // verify the required parameter 'fileName' is set
            if (fileName == null) throw new ApiException(400, "Missing required parameter 'fileName' when calling ModulesUpdateModule");
            
    
            var path_ = "/api/platform/modules/{id}/update";
    
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
            
            if (fileName != null) queryParams.Add("fileName", ApiClient.ParameterToString(fileName)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpdateModule: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification));
        }
        
        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotifications ()
        {
            
    
            var path_ = "/api/platform/notification";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotifications: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotifications: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelNotificationsNotification>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelNotificationsNotification>));
        }
    
        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationsAsync ()
        {
            
    
            var path_ = "/api/platform/notification";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotifications: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelNotificationsNotification>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelNotificationsNotification>));
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
            if (objectId != null) pathParams.Add("objectId", ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // path parameter
            
            if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
            if (count != null) queryParams.Add("count", ApiClient.ParameterToString(count)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationJournal: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationJournal: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult));
        }
    
        /// <summary>
        /// Get notification journal page Method returns notification journal page with array of notification, that was send, sending or will be send in future. Result contains total count, that can be used\r\n            for paging.
        /// </summary>
        /// <param name="objectId">Object id</param>
        /// <param name="objectTypeId">Object type id</param>
        /// <param name="start">Page setting start</param>
        /// <param name="count">Page setting count</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult> NotificationsGetNotificationJournalAsync (string objectId, string objectTypeId, int? start, int? count)
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
            if (objectId != null) pathParams.Add("objectId", ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // path parameter
            
            if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
            if (count != null) queryParams.Add("count", ApiClient.ParameterToString(count)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationJournal: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult));
        }
        
        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <param name="id">Sending notification id</param> 
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotification</returns>            
        public VirtoCommercePlatformWebModelNotificationsNotification NotificationsGetNotification (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling NotificationsGetNotification");
            
    
            var path_ = "/api/platform/notification/notification/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotification: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotification: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelNotificationsNotification) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsNotification));
        }
    
        /// <summary>
        /// Get sending notification 
        /// </summary>
        /// <param name="id">Sending notification id</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotificationAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling NotificationsGetNotification");
            
    
            var path_ = "/api/platform/notification/notification/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotification: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelNotificationsNotification) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsNotification));
        }
        
        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <param name="ids">Stop sending notification ids</param> 
        /// <returns></returns>            
        public void NotificationsStopSendingNotifications (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling NotificationsStopSendingNotifications");
            
    
            var path_ = "/api/platform/notification/stopnotifications";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(ids); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsStopSendingNotifications: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsStopSendingNotifications: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Stop sending notification 
        /// </summary>
        /// <param name="ids">Stop sending notification ids</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task NotificationsStopSendingNotificationsAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling NotificationsStopSendingNotifications");
            
    
            var path_ = "/api/platform/notification/stopnotifications";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(ids); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsStopSendingNotifications: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <param name="notificationTemplate">Notification template</param> 
        /// <returns></returns>            
        public void NotificationsUpdateNotificationTemplate (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate)
        {
            
            // verify the required parameter 'notificationTemplate' is set
            if (notificationTemplate == null) throw new ApiException(400, "Missing required parameter 'notificationTemplate' when calling NotificationsUpdateNotificationTemplate");
            
    
            var path_ = "/api/platform/notification/template";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(notificationTemplate); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update notification template 
        /// </summary>
        /// <param name="notificationTemplate">Notification template</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task NotificationsUpdateNotificationTemplateAsync (VirtoCommercePlatformWebModelNotificationsNotificationTemplate notificationTemplate)
        {
            // verify the required parameter 'notificationTemplate' is set
            if (notificationTemplate == null) throw new ApiException(400, "Missing required parameter 'notificationTemplate' when calling NotificationsUpdateNotificationTemplate");
            
    
            var path_ = "/api/platform/notification/template";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(notificationTemplate); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsUpdateNotificationTemplate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <param name="request">Test notification request</param> 
        /// <returns>VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>            
        public VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult NotificationsRenderNotificationContent (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling NotificationsRenderNotificationContent");
            
    
            var path_ = "/api/platform/notification/template/rendernotificationcontent";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(request); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsRenderNotificationContent: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsRenderNotificationContent: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult));
        }
    
        /// <summary>
        /// Get rendered notification content Method returns rendered content, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters.
        /// </summary>
        /// <param name="request">Test notification request</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult> NotificationsRenderNotificationContentAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling NotificationsRenderNotificationContent");
            
    
            var path_ = "/api/platform/notification/template/rendernotificationcontent";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(request); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsRenderNotificationContent: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult));
        }
        
        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </summary>
        /// <param name="request">Test notification request</param> 
        /// <returns>string</returns>            
        public string NotificationsSendNotification (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling NotificationsSendNotification");
            
    
            var path_ = "/api/platform/notification/template/sendnotification";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(request); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsSendNotification: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsSendNotification: " + response.ErrorMessage, response.ErrorMessage);
    
            return (string) ApiClient.Deserialize(response, typeof(string));
        }
    
        /// <summary>
        /// Sending test notification Method sending notification, that based on notification template. Template for rendering chosen by type, objectId, objectTypeId, language.\r\n            Parameters for template may be prepared by the method of getTestingParameters. Method returns string. If sending finished with success status\r\n            this string is empty, otherwise string contains error message.
        /// </summary>
        /// <param name="request">Test notification request</param>
        /// <returns>string</returns>
        public async System.Threading.Tasks.Task<string> NotificationsSendNotificationAsync (VirtoCommercePlatformWebModelNotificationsTestNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling NotificationsSendNotification");
            
    
            var path_ = "/api/platform/notification/template/sendnotification";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(request); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsSendNotification: " + response.Content, response.Content);

            return (string) ApiClient.Deserialize(response, typeof(string));
        }
        
        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <param name="id">Template id</param> 
        /// <returns></returns>            
        public void NotificationsDeleteNotificationTemplate (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling NotificationsDeleteNotificationTemplate");
            
    
            var path_ = "/api/platform/notification/template/{id}";
    
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
            if (id != null) pathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete notification template 
        /// </summary>
        /// <param name="id">Template id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task NotificationsDeleteNotificationTemplateAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling NotificationsDeleteNotificationTemplate");
            
    
            var path_ = "/api/platform/notification/template/{id}";
    
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
            if (id != null) pathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <param name="type">Notification type</param> 
        /// <returns></returns>            
        public List<VirtoCommercePlatformCoreNotificationsNotificationParameter> NotificationsGetTestingParameters (string type)
        {
            
            // verify the required parameter 'type' is set
            if (type == null) throw new ApiException(400, "Missing required parameter 'type' when calling NotificationsGetTestingParameters");
            
    
            var path_ = "/api/platform/notification/template/{type}/getTestingParameters";
    
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
            if (type != null) pathParams.Add("type", ApiClient.ParameterToString(type)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetTestingParameters: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetTestingParameters: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformCoreNotificationsNotificationParameter>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreNotificationsNotificationParameter>));
        }
    
        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this properties used in notification template.
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> NotificationsGetTestingParametersAsync (string type)
        {
            // verify the required parameter 'type' is set
            if (type == null) throw new ApiException(400, "Missing required parameter 'type' when calling NotificationsGetTestingParameters");
            
    
            var path_ = "/api/platform/notification/template/{type}/getTestingParameters";
    
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
            if (type != null) pathParams.Add("type", ApiClient.ParameterToString(type)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetTestingParameters: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformCoreNotificationsNotificationParameter>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreNotificationsNotificationParameter>));
        }
        
        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param> 
        /// <param name="objectId">Object id of template</param> 
        /// <param name="objectTypeId">Object type id of template</param> 
        /// <returns></returns>            
        public List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplates (string type, string objectId, string objectTypeId)
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
            if (type != null) pathParams.Add("type", ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) pathParams.Add("objectId", ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplates: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplates: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>));
        }
    
        /// <summary>
        /// Get notification templates Get all notification templates by notification type, objectId, objectTypeId. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>> NotificationsGetNotificationTemplatesAsync (string type, string objectId, string objectTypeId)
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
            if (type != null) pathParams.Add("type", ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) pathParams.Add("objectId", ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplates: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>));
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
            if (type != null) pathParams.Add("type", ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) pathParams.Add("objectId", ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // path parameter
            if (language != null) pathParams.Add("language", ApiClient.ParameterToString(language)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelNotificationsNotificationTemplate) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsNotificationTemplate));
        }
    
        /// <summary>
        /// Get notification template Get notification template by notification type, objectId, objectTypeId and language. Object id and object type id - params of object, that initialize creating of\r\n            template. By default object id and object type id = \&quot;Platform\&quot;. For example for store with id = \&quot;SampleStore\&quot;, objectId = \&quot;SampleStore\&quot;, objectTypeId = \&quot;Store\&quot;.
        /// </summary>
        /// <param name="type">Notification type of template</param>
        /// <param name="objectId">Object id of template</param>
        /// <param name="objectTypeId">Object type id of template</param>
        /// <param name="language">Locale of template</param>
        /// <returns>VirtoCommercePlatformWebModelNotificationsNotificationTemplate</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelNotificationsNotificationTemplate> NotificationsGetNotificationTemplateAsync (string type, string objectId, string objectTypeId, string language)
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
            if (type != null) pathParams.Add("type", ApiClient.ParameterToString(type)); // path parameter
            if (objectId != null) pathParams.Add("objectId", ApiClient.ParameterToString(objectId)); // path parameter
            if (objectTypeId != null) pathParams.Add("objectTypeId", ApiClient.ParameterToString(objectTypeId)); // path parameter
            if (language != null) pathParams.Add("language", ApiClient.ParameterToString(language)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplate: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelNotificationsNotificationTemplate) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelNotificationsNotificationTemplate));
        }
        
        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <param name="criteriaIds"></param> 
        /// <param name="criteriaOnlyNew"></param> 
        /// <param name="criteriaStartDate"></param> 
        /// <param name="criteriaEndDate"></param> 
        /// <param name="criteriaStart"></param> 
        /// <param name="criteriaCount"></param> 
        /// <param name="criteriaOrderBy"></param> 
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>            
        public VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationSearch (List<string> criteriaIds, bool? criteriaOnlyNew, DateTime? criteriaStartDate, DateTime? criteriaEndDate, int? criteriaStart, int? criteriaCount, string criteriaOrderBy)
        {
            
    
            var path_ = "/api/platform/pushnotifications";
    
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
            
            if (criteriaIds != null) queryParams.Add("criteria.ids", ApiClient.ParameterToString(criteriaIds)); // query parameter
            if (criteriaOnlyNew != null) queryParams.Add("criteria.onlyNew", ApiClient.ParameterToString(criteriaOnlyNew)); // query parameter
            if (criteriaStartDate != null) queryParams.Add("criteria.startDate", ApiClient.ParameterToString(criteriaStartDate)); // query parameter
            if (criteriaEndDate != null) queryParams.Add("criteria.endDate", ApiClient.ParameterToString(criteriaEndDate)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            if (criteriaOrderBy != null) queryParams.Add("criteria.orderBy", ApiClient.ParameterToString(criteriaOrderBy)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationSearch: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult));
        }
    
        /// <summary>
        /// Search push notifications 
        /// </summary>
        /// <param name="criteriaIds"></param>
        /// <param name="criteriaOnlyNew"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <param name="criteriaOrderBy"></param>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationSearchAsync (List<string> criteriaIds, bool? criteriaOnlyNew, DateTime? criteriaStartDate, DateTime? criteriaEndDate, int? criteriaStart, int? criteriaCount, string criteriaOrderBy)
        {
            
    
            var path_ = "/api/platform/pushnotifications";
    
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
            
            if (criteriaIds != null) queryParams.Add("criteria.ids", ApiClient.ParameterToString(criteriaIds)); // query parameter
            if (criteriaOnlyNew != null) queryParams.Add("criteria.onlyNew", ApiClient.ParameterToString(criteriaOnlyNew)); // query parameter
            if (criteriaStartDate != null) queryParams.Add("criteria.startDate", ApiClient.ParameterToString(criteriaStartDate)); // query parameter
            if (criteriaEndDate != null) queryParams.Add("criteria.endDate", ApiClient.ParameterToString(criteriaEndDate)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            if (criteriaOrderBy != null) queryParams.Add("criteria.orderBy", ApiClient.ParameterToString(criteriaOrderBy)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationSearch: " + response.Content, response.Content);

            return (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult));
        }
        
        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>            
        public VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationMarkAllAsRead ()
        {
            
    
            var path_ = "/api/platform/pushnotifications/markAllAsRead";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationMarkAllAsRead: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationMarkAllAsRead: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult));
        }
    
        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationMarkAllAsReadAsync ()
        {
            
    
            var path_ = "/api/platform/pushnotifications/markAllAsRead";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationMarkAllAsRead: " + response.Content, response.Content);

            return (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult));
        }
        
        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <param name="type"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApiAccount</returns>            
        public VirtoCommercePlatformCoreSecurityApiAccount SecurityGenerateNewApiAccount (string type)
        {
            
            // verify the required parameter 'type' is set
            if (type == null) throw new ApiException(400, "Missing required parameter 'type' when calling SecurityGenerateNewApiAccount");
            
    
            var path_ = "/api/platform/security/apiaccounts/new";
    
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
            
            if (type != null) queryParams.Add("type", ApiClient.ParameterToString(type)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGenerateNewApiAccount: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGenerateNewApiAccount: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApiAccount) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApiAccount));
        }
    
        /// <summary>
        /// Generate new API key Generates new key but does not save it.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApiAccount</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApiAccount> SecurityGenerateNewApiAccountAsync (string type)
        {
            // verify the required parameter 'type' is set
            if (type == null) throw new ApiException(400, "Missing required parameter 'type' when calling SecurityGenerateNewApiAccount");
            
    
            var path_ = "/api/platform/security/apiaccounts/new";
    
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
            
            if (type != null) queryParams.Add("type", ApiClient.ParameterToString(type)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGenerateNewApiAccount: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApiAccount) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApiAccount));
        }
        
        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>            
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetCurrentUser ()
        {
            
    
            var path_ = "/api/platform/security/currentuser";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetCurrentUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetCurrentUser: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
    
        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetCurrentUserAsync ()
        {
            
    
            var path_ = "/api/platform/security/currentuser";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetCurrentUser: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
        
        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <param name="model">User credentials.</param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>            
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityLogin (VirtoCommercePlatformWebModelSecurityUserLogin model)
        {
            
            // verify the required parameter 'model' is set
            if (model == null) throw new ApiException(400, "Missing required parameter 'model' when calling SecurityLogin");
            
    
            var path_ = "/api/platform/security/login";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(model); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityLogin: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityLogin: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
    
        /// <summary>
        /// Sign in with user name and password Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <param name="model">User credentials.</param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityLoginAsync (VirtoCommercePlatformWebModelSecurityUserLogin model)
        {
            // verify the required parameter 'model' is set
            if (model == null) throw new ApiException(400, "Missing required parameter 'model' when calling SecurityLogin");
            
    
            var path_ = "/api/platform/security/login";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(model); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityLogin: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
        
        /// <summary>
        /// Sign out 
        /// </summary>
        /// <returns></returns>            
        public void SecurityLogout ()
        {
            
    
            var path_ = "/api/platform/security/logout";
    
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
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityLogout: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityLogout: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Sign out 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task SecurityLogoutAsync ()
        {
            
    
            var path_ = "/api/platform/security/logout";
    
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
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityLogout: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommercePlatformCoreSecurityPermission> SecurityGetPermissions ()
        {
            
    
            var path_ = "/api/platform/security/permissions";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetPermissions: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetPermissions: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformCoreSecurityPermission>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreSecurityPermission>));
        }
    
        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreSecurityPermission>> SecurityGetPermissionsAsync ()
        {
            
    
            var path_ = "/api/platform/security/permissions";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetPermissions: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformCoreSecurityPermission>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformCoreSecurityPermission>));
        }
        
        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <param name="requestKeyword"></param> 
        /// <param name="requestSkipCount"></param> 
        /// <param name="requestTakeCount"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>            
        public VirtoCommercePlatformCoreSecurityRoleSearchResponse SecuritySearchRoles (string requestKeyword, int? requestSkipCount, int? requestTakeCount)
        {
            
    
            var path_ = "/api/platform/security/roles";
    
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
            
            if (requestKeyword != null) queryParams.Add("request.keyword", ApiClient.ParameterToString(requestKeyword)); // query parameter
            if (requestSkipCount != null) queryParams.Add("request.skipCount", ApiClient.ParameterToString(requestSkipCount)); // query parameter
            if (requestTakeCount != null) queryParams.Add("request.takeCount", ApiClient.ParameterToString(requestTakeCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchRoles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchRoles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityRoleSearchResponse) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRoleSearchResponse));
        }
    
        /// <summary>
        /// Search roles by keyword 
        /// </summary>
        /// <param name="requestKeyword"></param>
        /// <param name="requestSkipCount"></param>
        /// <param name="requestTakeCount"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRoleSearchResponse</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRoleSearchResponse> SecuritySearchRolesAsync (string requestKeyword, int? requestSkipCount, int? requestTakeCount)
        {
            
    
            var path_ = "/api/platform/security/roles";
    
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
            
            if (requestKeyword != null) queryParams.Add("request.keyword", ApiClient.ParameterToString(requestKeyword)); // query parameter
            if (requestSkipCount != null) queryParams.Add("request.skipCount", ApiClient.ParameterToString(requestSkipCount)); // query parameter
            if (requestTakeCount != null) queryParams.Add("request.takeCount", ApiClient.ParameterToString(requestTakeCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchRoles: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityRoleSearchResponse) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRoleSearchResponse));
        }
        
        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <param name="role"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>            
        public VirtoCommercePlatformCoreSecurityRole SecurityUpdateRole (VirtoCommercePlatformCoreSecurityRole role)
        {
            
            // verify the required parameter 'role' is set
            if (role == null) throw new ApiException(400, "Missing required parameter 'role' when calling SecurityUpdateRole");
            
    
            var path_ = "/api/platform/security/roles";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(role); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateRole: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateRole: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityRole) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRole));
        }
    
        /// <summary>
        /// Add a new role or update an existing role 
        /// </summary>
        /// <param name="role"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityUpdateRoleAsync (VirtoCommercePlatformCoreSecurityRole role)
        {
            // verify the required parameter 'role' is set
            if (role == null) throw new ApiException(400, "Missing required parameter 'role' when calling SecurityUpdateRole");
            
    
            var path_ = "/api/platform/security/roles";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(role); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateRole: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityRole) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRole));
        }
        
        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <param name="ids"></param> 
        /// <returns></returns>            
        public void SecurityDeleteRoles (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling SecurityDeleteRoles");
            
    
            var path_ = "/api/platform/security/roles";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityDeleteRoles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityDeleteRoles: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete roles by ID 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task SecurityDeleteRolesAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling SecurityDeleteRoles");
            
    
            var path_ = "/api/platform/security/roles";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityDeleteRoles: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <param name="roleId"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>            
        public VirtoCommercePlatformCoreSecurityRole SecurityGetRole (string roleId)
        {
            
            // verify the required parameter 'roleId' is set
            if (roleId == null) throw new ApiException(400, "Missing required parameter 'roleId' when calling SecurityGetRole");
            
    
            var path_ = "/api/platform/security/roles/{roleId}";
    
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
            if (roleId != null) pathParams.Add("roleId", ApiClient.ParameterToString(roleId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetRole: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetRole: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityRole) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRole));
        }
    
        /// <summary>
        /// Get role by ID 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityRole</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityRole> SecurityGetRoleAsync (string roleId)
        {
            // verify the required parameter 'roleId' is set
            if (roleId == null) throw new ApiException(400, "Missing required parameter 'roleId' when calling SecurityGetRole");
            
    
            var path_ = "/api/platform/security/roles/{roleId}";
    
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
            if (roleId != null) pathParams.Add("roleId", ApiClient.ParameterToString(roleId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetRole: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityRole) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityRole));
        }
        
        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <param name="requestAccountTypes"></param> 
        /// <param name="requestKeyword"></param> 
        /// <param name="requestSkipCount"></param> 
        /// <param name="requestTakeCount"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>            
        public VirtoCommercePlatformCoreSecurityUserSearchResponse SecuritySearchUsersAsync (List<string> requestAccountTypes, string requestKeyword, int? requestSkipCount, int? requestTakeCount)
        {
            
    
            var path_ = "/api/platform/security/users";
    
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
            
            if (requestAccountTypes != null) queryParams.Add("request.accountTypes", ApiClient.ParameterToString(requestAccountTypes)); // query parameter
            if (requestKeyword != null) queryParams.Add("request.keyword", ApiClient.ParameterToString(requestKeyword)); // query parameter
            if (requestSkipCount != null) queryParams.Add("request.skipCount", ApiClient.ParameterToString(requestSkipCount)); // query parameter
            if (requestTakeCount != null) queryParams.Add("request.takeCount", ApiClient.ParameterToString(requestTakeCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchUsersAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchUsersAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityUserSearchResponse) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityUserSearchResponse));
        }
    
        /// <summary>
        /// Search users by keyword 
        /// </summary>
        /// <param name="requestAccountTypes"></param>
        /// <param name="requestKeyword"></param>
        /// <param name="requestSkipCount"></param>
        /// <param name="requestTakeCount"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityUserSearchResponse</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityUserSearchResponse> SecuritySearchUsersAsyncAsync (List<string> requestAccountTypes, string requestKeyword, int? requestSkipCount, int? requestTakeCount)
        {
            
    
            var path_ = "/api/platform/security/users";
    
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
            
            if (requestAccountTypes != null) queryParams.Add("request.accountTypes", ApiClient.ParameterToString(requestAccountTypes)); // query parameter
            if (requestKeyword != null) queryParams.Add("request.keyword", ApiClient.ParameterToString(requestKeyword)); // query parameter
            if (requestSkipCount != null) queryParams.Add("request.skipCount", ApiClient.ParameterToString(requestSkipCount)); // query parameter
            if (requestTakeCount != null) queryParams.Add("request.takeCount", ApiClient.ParameterToString(requestTakeCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchUsersAsync: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityUserSearchResponse) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityUserSearchResponse));
        }
        
        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <param name="user">User details.</param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>            
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityUpdateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling SecurityUpdateAsync");
            
    
            var path_ = "/api/platform/security/users";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(user); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
    
        /// <summary>
        /// Update user details by user ID 
        /// </summary>
        /// <param name="user">User details.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityUpdateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling SecurityUpdateAsync");
            
    
            var path_ = "/api/platform/security/users";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(user); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateAsync: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
        
        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <param name="names">An array of user names.</param> 
        /// <returns></returns>            
        public void SecurityDeleteAsync (List<string> names)
        {
            
            // verify the required parameter 'names' is set
            if (names == null) throw new ApiException(400, "Missing required parameter 'names' when calling SecurityDeleteAsync");
            
    
            var path_ = "/api/platform/security/users";
    
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
            
            if (names != null) queryParams.Add("names", ApiClient.ParameterToString(names)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityDeleteAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityDeleteAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete users by name 
        /// </summary>
        /// <param name="names">An array of user names.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task SecurityDeleteAsyncAsync (List<string> names)
        {
            // verify the required parameter 'names' is set
            if (names == null) throw new ApiException(400, "Missing required parameter 'names' when calling SecurityDeleteAsync");
            
    
            var path_ = "/api/platform/security/users";
    
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
            
            if (names != null) queryParams.Add("names", ApiClient.ParameterToString(names)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityDeleteAsync: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Create new user 
        /// </summary>
        /// <param name="user">User details.</param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>            
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling SecurityCreateAsync");
            
    
            var path_ = "/api/platform/security/users/create";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(user); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityCreateAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityCreateAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
    
        /// <summary>
        /// Create new user 
        /// </summary>
        /// <param name="user">User details.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityCreateAsyncAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling SecurityCreateAsync");
            
    
            var path_ = "/api/platform/security/users/create";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(user); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityCreateAsync: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
        
        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>            
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetUserByName (string userName)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling SecurityGetUserByName");
            
    
            var path_ = "/api/platform/security/users/{userName}";
    
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
            if (userName != null) pathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetUserByName: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetUserByName: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
    
        /// <summary>
        /// Get user details by user name 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetUserByNameAsync (string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling SecurityGetUserByName");
            
    
            var path_ = "/api/platform/security/users/{userName}";
    
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
            if (userName != null) pathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetUserByName: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
        
        /// <summary>
        /// Change password 
        /// </summary>
        /// <param name="userName"></param> 
        /// <param name="changePassword">Old and new passwords.</param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>            
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityChangePassword (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling SecurityChangePassword");
            
            // verify the required parameter 'changePassword' is set
            if (changePassword == null) throw new ApiException(400, "Missing required parameter 'changePassword' when calling SecurityChangePassword");
            
    
            var path_ = "/api/platform/security/users/{userName}/changepassword";
    
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
            if (userName != null) pathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(changePassword); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityChangePassword: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityChangePassword: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
    
        /// <summary>
        /// Change password 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityChangePasswordAsync (string userName, VirtoCommercePlatformWebModelSecurityChangePasswordInfo changePassword)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling SecurityChangePassword");
            // verify the required parameter 'changePassword' is set
            if (changePassword == null) throw new ApiException(400, "Missing required parameter 'changePassword' when calling SecurityChangePassword");
            
    
            var path_ = "/api/platform/security/users/{userName}/changepassword";
    
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
            if (userName != null) pathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(changePassword); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityChangePassword: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
        
        /// <summary>
        /// Reset password 
        /// </summary>
        /// <param name="userName"></param> 
        /// <param name="resetPassword">New password.</param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>            
        public VirtoCommercePlatformCoreSecuritySecurityResult SecurityResetPassword (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling SecurityResetPassword");
            
            // verify the required parameter 'resetPassword' is set
            if (resetPassword == null) throw new ApiException(400, "Missing required parameter 'resetPassword' when calling SecurityResetPassword");
            
    
            var path_ = "/api/platform/security/users/{userName}/resetpassword";
    
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
            if (userName != null) pathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(resetPassword); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityResetPassword: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityResetPassword: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
    
        /// <summary>
        /// Reset password 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> SecurityResetPasswordAsync (string userName, VirtoCommercePlatformWebModelSecurityResetPasswordInfo resetPassword)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling SecurityResetPassword");
            // verify the required parameter 'resetPassword' is set
            if (resetPassword == null) throw new ApiException(400, "Missing required parameter 'resetPassword' when calling SecurityResetPassword");
            
    
            var path_ = "/api/platform/security/users/{userName}/resetpassword";
    
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
            if (userName != null) pathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(resetPassword); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityResetPassword: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
        
        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetAllSettings ()
        {
            
    
            var path_ = "/api/platform/settings";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetAllSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetAllSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelSettingsSetting>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>));
        }
    
        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetAllSettingsAsync ()
        {
            
    
            var path_ = "/api/platform/settings";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetAllSettings: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelSettingsSetting>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>));
        }
        
        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <param name="settings"></param> 
        /// <returns></returns>            
        public void SettingUpdate (List<VirtoCommercePlatformWebModelSettingsSetting> settings)
        {
            
            // verify the required parameter 'settings' is set
            if (settings == null) throw new ApiException(400, "Missing required parameter 'settings' when calling SettingUpdate");
            
    
            var path_ = "/api/platform/settings";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(settings); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingUpdate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update settings values 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task SettingUpdateAsync (List<VirtoCommercePlatformWebModelSettingsSetting> settings)
        {
            // verify the required parameter 'settings' is set
            if (settings == null) throw new ApiException(400, "Missing required parameter 'settings' when calling SettingUpdate");
            
    
            var path_ = "/api/platform/settings";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(settings); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingUpdate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <param name="id">Module ID.</param> 
        /// <returns></returns>            
        public List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetModuleSettings (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SettingGetModuleSettings");
            
    
            var path_ = "/api/platform/settings/modules/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetModuleSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetModuleSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelSettingsSetting>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>));
        }
    
        /// <summary>
        /// Get settings registered by specific module 
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetModuleSettingsAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SettingGetModuleSettings");
            
    
            var path_ = "/api/platform/settings/modules/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetModuleSettings: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelSettingsSetting>) ApiClient.Deserialize(response, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>));
        }
        
        /// <summary>
        /// Get non-array setting value by name 
        /// </summary>
        /// <param name="name">Setting system name.</param> 
        /// <returns>InlineResponse200</returns>            
        public InlineResponse200 SettingGetValue (string name)
        {
            
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling SettingGetValue");
            
    
            var path_ = "/api/platform/settings/value/{name}";
    
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
            if (name != null) pathParams.Add("name", ApiClient.ParameterToString(name)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetValue: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetValue: " + response.ErrorMessage, response.ErrorMessage);
    
            return (InlineResponse200) ApiClient.Deserialize(response, typeof(InlineResponse200));
        }
    
        /// <summary>
        /// Get non-array setting value by name 
        /// </summary>
        /// <param name="name">Setting system name.</param>
        /// <returns>InlineResponse200</returns>
        public async System.Threading.Tasks.Task<InlineResponse200> SettingGetValueAsync (string name)
        {
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling SettingGetValue");
            
    
            var path_ = "/api/platform/settings/value/{name}";
    
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
            if (name != null) pathParams.Add("name", ApiClient.ParameterToString(name)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetValue: " + response.Content, response.Content);

            return (InlineResponse200) ApiClient.Deserialize(response, typeof(InlineResponse200));
        }
        
        /// <summary>
        /// Get array setting values by name 
        /// </summary>
        /// <param name="name">Setting system name.</param> 
        /// <returns></returns>            
        public List<Object> SettingGetArray (string name)
        {
            
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling SettingGetArray");
            
    
            var path_ = "/api/platform/settings/values/{name}";
    
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
            if (name != null) pathParams.Add("name", ApiClient.ParameterToString(name)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetArray: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetArray: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<Object>) ApiClient.Deserialize(response, typeof(List<Object>));
        }
    
        /// <summary>
        /// Get array setting values by name 
        /// </summary>
        /// <param name="name">Setting system name.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<Object>> SettingGetArrayAsync (string name)
        {
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling SettingGetArray");
            
    
            var path_ = "/api/platform/settings/values/{name}";
    
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
            if (name != null) pathParams.Add("name", ApiClient.ParameterToString(name)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetArray: " + response.Content, response.Content);

            return (List<Object>) ApiClient.Deserialize(response, typeof(List<Object>));
        }
        
        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <param name="id">Setting system name.</param> 
        /// <returns>VirtoCommercePlatformWebModelSettingsSetting</returns>            
        public VirtoCommercePlatformWebModelSettingsSetting SettingGetSetting (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SettingGetSetting");
            
    
            var path_ = "/api/platform/settings/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetSetting: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetSetting: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelSettingsSetting) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelSettingsSetting));
        }
    
        /// <summary>
        /// Get setting details by name 
        /// </summary>
        /// <param name="id">Setting system name.</param>
        /// <returns>VirtoCommercePlatformWebModelSettingsSetting</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelSettingsSetting> SettingGetSettingAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SettingGetSetting");
            
    
            var path_ = "/api/platform/settings/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetSetting: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelSettingsSetting) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelSettingsSetting));
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="user"></param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>            
        public VirtoCommercePlatformCoreSecuritySecurityResult FrontEndSecurityCreate (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling FrontEndSecurityCreate");
            
    
            var path_ = "/api/security/frontend/user";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(user); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityCreate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="user"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> FrontEndSecurityCreateAsync (VirtoCommercePlatformCoreSecurityApplicationUserExtended user)
        {
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling FrontEndSecurityCreate");
            
    
            var path_ = "/api/security/frontend/user";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(user); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityCreate: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>            
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended FrontEndSecurityGetUserById (string userId)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling FrontEndSecurityGetUserById");
            
    
            var path_ = "/api/security/frontend/user/id/{userId}";
    
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
            if (userId != null) pathParams.Add("userId", ApiClient.ParameterToString(userId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGetUserById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGetUserById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> FrontEndSecurityGetUserByIdAsync (string userId)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling FrontEndSecurityGetUserById");
            
    
            var path_ = "/api/security/frontend/user/id/{userId}";
    
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
            if (userId != null) pathParams.Add("userId", ApiClient.ParameterToString(userId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGetUserById: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="loginProvider"></param> 
        /// <param name="providerKey"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>            
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended FrontEndSecurityGetUserByLogin (string loginProvider, string providerKey)
        {
            
            // verify the required parameter 'loginProvider' is set
            if (loginProvider == null) throw new ApiException(400, "Missing required parameter 'loginProvider' when calling FrontEndSecurityGetUserByLogin");
            
            // verify the required parameter 'providerKey' is set
            if (providerKey == null) throw new ApiException(400, "Missing required parameter 'providerKey' when calling FrontEndSecurityGetUserByLogin");
            
    
            var path_ = "/api/security/frontend/user/login";
    
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
            
            if (loginProvider != null) queryParams.Add("loginProvider", ApiClient.ParameterToString(loginProvider)); // query parameter
            if (providerKey != null) queryParams.Add("providerKey", ApiClient.ParameterToString(providerKey)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGetUserByLogin: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGetUserByLogin: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> FrontEndSecurityGetUserByLoginAsync (string loginProvider, string providerKey)
        {
            // verify the required parameter 'loginProvider' is set
            if (loginProvider == null) throw new ApiException(400, "Missing required parameter 'loginProvider' when calling FrontEndSecurityGetUserByLogin");
            // verify the required parameter 'providerKey' is set
            if (providerKey == null) throw new ApiException(400, "Missing required parameter 'providerKey' when calling FrontEndSecurityGetUserByLogin");
            
    
            var path_ = "/api/security/frontend/user/login";
    
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
            
            if (loginProvider != null) queryParams.Add("loginProvider", ApiClient.ParameterToString(loginProvider)); // query parameter
            if (providerKey != null) queryParams.Add("providerKey", ApiClient.ParameterToString(providerKey)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGetUserByLogin: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userName"></param> 
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>            
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended FrontEndSecurityGetUserByName (string userName)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling FrontEndSecurityGetUserByName");
            
    
            var path_ = "/api/security/frontend/user/name/{userName}";
    
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
            if (userName != null) pathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGetUserByName: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGetUserByName: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> FrontEndSecurityGetUserByNameAsync (string userName)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling FrontEndSecurityGetUserByName");
            
    
            var path_ = "/api/security/frontend/user/name/{userName}";
    
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
            if (userName != null) pathParams.Add("userName", ApiClient.ParameterToString(userName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGetUserByName: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended));
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userId"></param> 
        /// <param name="token"></param> 
        /// <param name="newPassword"></param> 
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>            
        public VirtoCommercePlatformCoreSecuritySecurityResult FrontEndSecurityResetPassword (string userId, string token, string newPassword)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling FrontEndSecurityResetPassword");
            
            // verify the required parameter 'token' is set
            if (token == null) throw new ApiException(400, "Missing required parameter 'token' when calling FrontEndSecurityResetPassword");
            
            // verify the required parameter 'newPassword' is set
            if (newPassword == null) throw new ApiException(400, "Missing required parameter 'newPassword' when calling FrontEndSecurityResetPassword");
            
    
            var path_ = "/api/security/frontend/user/password/reset";
    
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
            
            if (userId != null) queryParams.Add("userId", ApiClient.ParameterToString(userId)); // query parameter
            if (token != null) queryParams.Add("token", ApiClient.ParameterToString(token)); // query parameter
            if (newPassword != null) queryParams.Add("newPassword", ApiClient.ParameterToString(newPassword)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityResetPassword: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityResetPassword: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>VirtoCommercePlatformCoreSecuritySecurityResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecuritySecurityResult> FrontEndSecurityResetPasswordAsync (string userId, string token, string newPassword)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling FrontEndSecurityResetPassword");
            // verify the required parameter 'token' is set
            if (token == null) throw new ApiException(400, "Missing required parameter 'token' when calling FrontEndSecurityResetPassword");
            // verify the required parameter 'newPassword' is set
            if (newPassword == null) throw new ApiException(400, "Missing required parameter 'newPassword' when calling FrontEndSecurityResetPassword");
            
    
            var path_ = "/api/security/frontend/user/password/reset";
    
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
            
            if (userId != null) queryParams.Add("userId", ApiClient.ParameterToString(userId)); // query parameter
            if (token != null) queryParams.Add("token", ApiClient.ParameterToString(token)); // query parameter
            if (newPassword != null) queryParams.Add("newPassword", ApiClient.ParameterToString(newPassword)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityResetPassword: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformCoreSecuritySecurityResult));
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userId"></param> 
        /// <param name="storeName"></param> 
        /// <param name="callbackUrl"></param> 
        /// <returns></returns>            
        public void FrontEndSecurityGenerateResetPasswordToken (string userId, string storeName, string callbackUrl)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling FrontEndSecurityGenerateResetPasswordToken");
            
            // verify the required parameter 'storeName' is set
            if (storeName == null) throw new ApiException(400, "Missing required parameter 'storeName' when calling FrontEndSecurityGenerateResetPasswordToken");
            
            // verify the required parameter 'callbackUrl' is set
            if (callbackUrl == null) throw new ApiException(400, "Missing required parameter 'callbackUrl' when calling FrontEndSecurityGenerateResetPasswordToken");
            
    
            var path_ = "/api/security/frontend/user/password/resettoken";
    
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
            
            if (userId != null) queryParams.Add("userId", ApiClient.ParameterToString(userId)); // query parameter
            if (storeName != null) queryParams.Add("storeName", ApiClient.ParameterToString(storeName)); // query parameter
            if (callbackUrl != null) queryParams.Add("callbackUrl", ApiClient.ParameterToString(callbackUrl)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGenerateResetPasswordToken: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGenerateResetPasswordToken: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task FrontEndSecurityGenerateResetPasswordTokenAsync (string userId, string storeName, string callbackUrl)
        {
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling FrontEndSecurityGenerateResetPasswordToken");
            // verify the required parameter 'storeName' is set
            if (storeName == null) throw new ApiException(400, "Missing required parameter 'storeName' when calling FrontEndSecurityGenerateResetPasswordToken");
            // verify the required parameter 'callbackUrl' is set
            if (callbackUrl == null) throw new ApiException(400, "Missing required parameter 'callbackUrl' when calling FrontEndSecurityGenerateResetPasswordToken");
            
    
            var path_ = "/api/security/frontend/user/password/resettoken";
    
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
            
            if (userId != null) queryParams.Add("userId", ApiClient.ParameterToString(userId)); // query parameter
            if (storeName != null) queryParams.Add("storeName", ApiClient.ParameterToString(storeName)); // query parameter
            if (callbackUrl != null) queryParams.Add("callbackUrl", ApiClient.ParameterToString(callbackUrl)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityGenerateResetPasswordToken: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userName"></param> 
        /// <param name="password"></param> 
        /// <param name="isPersistent"></param> 
        /// <returns>VirtoCommercePlatformWebModelSecuritySignInResult</returns>            
        public VirtoCommercePlatformWebModelSecuritySignInResult FrontEndSecurityPasswordSignIn (string userName, string password, bool? isPersistent)
        {
            
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling FrontEndSecurityPasswordSignIn");
            
            // verify the required parameter 'password' is set
            if (password == null) throw new ApiException(400, "Missing required parameter 'password' when calling FrontEndSecurityPasswordSignIn");
            
            // verify the required parameter 'isPersistent' is set
            if (isPersistent == null) throw new ApiException(400, "Missing required parameter 'isPersistent' when calling FrontEndSecurityPasswordSignIn");
            
    
            var path_ = "/api/security/frontend/user/signin";
    
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
            
            if (userName != null) queryParams.Add("userName", ApiClient.ParameterToString(userName)); // query parameter
            if (password != null) queryParams.Add("password", ApiClient.ParameterToString(password)); // query parameter
            if (isPersistent != null) queryParams.Add("isPersistent", ApiClient.ParameterToString(isPersistent)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityPasswordSignIn: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityPasswordSignIn: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelSecuritySignInResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelSecuritySignInResult));
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <returns>VirtoCommercePlatformWebModelSecuritySignInResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelSecuritySignInResult> FrontEndSecurityPasswordSignInAsync (string userName, string password, bool? isPersistent)
        {
            // verify the required parameter 'userName' is set
            if (userName == null) throw new ApiException(400, "Missing required parameter 'userName' when calling FrontEndSecurityPasswordSignIn");
            // verify the required parameter 'password' is set
            if (password == null) throw new ApiException(400, "Missing required parameter 'password' when calling FrontEndSecurityPasswordSignIn");
            // verify the required parameter 'isPersistent' is set
            if (isPersistent == null) throw new ApiException(400, "Missing required parameter 'isPersistent' when calling FrontEndSecurityPasswordSignIn");
            
    
            var path_ = "/api/security/frontend/user/signin";
    
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
            
            if (userName != null) queryParams.Add("userName", ApiClient.ParameterToString(userName)); // query parameter
            if (password != null) queryParams.Add("password", ApiClient.ParameterToString(password)); // query parameter
            if (isPersistent != null) queryParams.Add("isPersistent", ApiClient.ParameterToString(isPersistent)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling FrontEndSecurityPasswordSignIn: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelSecuritySignInResult) ApiClient.Deserialize(response, typeof(VirtoCommercePlatformWebModelSecuritySignInResult));
        }
        
    }
    
}
