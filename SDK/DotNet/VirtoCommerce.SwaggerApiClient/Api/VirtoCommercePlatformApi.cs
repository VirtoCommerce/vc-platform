using System;
using System.IO;
using System.Collections.Generic;
using RestSharp;
using VirtoCommerce.SwaggerApiClient.Client;
using VirtoCommerce.SwaggerApiClient.Model;


namespace VirtoCommerce.SwaggerApiClient.Api
{
    
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IVirtoCommercePlatformApi
    {
        
        /// <summary>
        /// Deletes blobs by they key.
        /// </summary>
        /// <remarks>
        /// Delete blob by key
        /// </remarks>
        /// <param name="blobKey">blob key.</param>
        /// <returns></returns>
        void AssetsDelete (string blobKey);
  
        /// <summary>
        /// Deletes blobs by they key.
        /// </summary>
        /// <remarks>
        /// Delete blob by key
        /// </remarks>
        /// <param name="blobKey">blob key.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task AssetsDeleteAsync (string blobKey);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder"></param>
        /// <param name="url"></param>
        /// <returns>VirtoCommercePlatformWebModelAssetBlobInfo</returns>
        VirtoCommercePlatformWebModelAssetBlobInfo AssetsUploadAssetFromUrl (string folder, string url);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder"></param>
        /// <param name="url"></param>
        /// <returns>VirtoCommercePlatformWebModelAssetBlobInfo</returns>
        System.Threading.Tasks.Task<VirtoCommercePlatformWebModelAssetBlobInfo> AssetsUploadAssetFromUrlAsync (string folder, string url);
        
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
        /// Method returns notification properties, that defined in notification class, this proprties used in notification template.
        /// </remarks>
        /// <param name="type">Notification type</param>
        /// <returns></returns>
        List<VirtoCommercePlatformCoreNotificationsNotificationParameter> NotificationsGetTestingParameters (string type);
  
        /// <summary>
        /// Get testing parameters
        /// </summary>
        /// <remarks>
        /// Method returns notification properties, that defined in notification class, this proprties used in notification template.
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
        /// <returns>Object</returns>
        Object SettingGetValue (string name);
  
        /// <summary>
        /// Get non-array setting value by name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="name">Setting system name.</param>
        /// <returns>Object</returns>
        System.Threading.Tasks.Task<Object> SettingGetValueAsync (string name);
        
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
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class VirtoCommercePlatformApi : IVirtoCommercePlatformApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommercePlatformApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public VirtoCommercePlatformApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommercePlatformApi"/> class.
        /// </summary>
        /// <returns></returns>
        public VirtoCommercePlatformApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
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
        /// Deletes blobs by they key. Delete blob by key
        /// </summary>
        /// <param name="blobKey">blob key.</param> 
        /// <returns></returns>            
        public void AssetsDelete (string blobKey)
        {
            
            // verify the required parameter 'blobKey' is set
            if (blobKey == null) throw new ApiException(400, "Missing required parameter 'blobKey' when calling AssetsDelete");
            
    
            var path = "/api/platform/assets";
    
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
            
            if (blobKey != null) queryParams.Add("blobKey", ApiClient.ParameterToString(blobKey)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes blobs by they key. Delete blob by key
        /// </summary>
        /// <param name="blobKey">blob key.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task AssetsDeleteAsync (string blobKey)
        {
            // verify the required parameter 'blobKey' is set
            if (blobKey == null) throw new ApiException(400, "Missing required parameter 'blobKey' when calling AssetsDelete");
            
    
            var path = "/api/platform/assets";
    
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
            
            if (blobKey != null) queryParams.Add("blobKey", ApiClient.ParameterToString(blobKey)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsDelete: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="folder"></param> 
        /// <param name="url"></param> 
        /// <returns>VirtoCommercePlatformWebModelAssetBlobInfo</returns>            
        public VirtoCommercePlatformWebModelAssetBlobInfo AssetsUploadAssetFromUrl (string folder, string url)
        {
            
            // verify the required parameter 'folder' is set
            if (folder == null) throw new ApiException(400, "Missing required parameter 'folder' when calling AssetsUploadAssetFromUrl");
            
            // verify the required parameter 'url' is set
            if (url == null) throw new ApiException(400, "Missing required parameter 'url' when calling AssetsUploadAssetFromUrl");
            
    
            var path = "/api/platform/assets/{folder}";
    
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
            if (folder != null) pathParams.Add("folder", ApiClient.ParameterToString(folder)); // path parameter
            
            if (url != null) queryParams.Add("url", ApiClient.ParameterToString(url)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsUploadAssetFromUrl: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsUploadAssetFromUrl: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelAssetBlobInfo) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelAssetBlobInfo), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="url"></param>
        /// <returns>VirtoCommercePlatformWebModelAssetBlobInfo</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelAssetBlobInfo> AssetsUploadAssetFromUrlAsync (string folder, string url)
        {
            // verify the required parameter 'folder' is set
            if (folder == null) throw new ApiException(400, "Missing required parameter 'folder' when calling AssetsUploadAssetFromUrl");
            // verify the required parameter 'url' is set
            if (url == null) throw new ApiException(400, "Missing required parameter 'url' when calling AssetsUploadAssetFromUrl");
            
    
            var path = "/api/platform/assets/{folder}";
    
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
            if (folder != null) pathParams.Add("folder", ApiClient.ParameterToString(folder)); // path parameter
            
            if (url != null) queryParams.Add("url", ApiClient.ParameterToString(url)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AssetsUploadAssetFromUrl: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelAssetBlobInfo) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelAssetBlobInfo), response.Headers);
        }
        
        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <returns></returns>            
        public List<string> DynamicPropertiesGetObjectTypes ()
        {
            
    
            var path = "/api/platform/dynamic/types";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
        /// <summary>
        /// Get object types which support dynamic properties 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<string>> DynamicPropertiesGetObjectTypesAsync ()
        {
            
    
            var path = "/api/platform/dynamic/types";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetObjectTypes: " + response.Content, response.Content);

            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetProperties: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetProperties: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>), response.Headers);
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetProperties: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty>), response.Headers);
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesCreateProperty: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesCreateProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty), response.Headers);
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesCreateProperty: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreDynamicPropertiesDynamicProperty), response.Headers);
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>), response.Headers);
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DynamicPropertiesGetDictionaryItems: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>), response.Headers);
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/jobs/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling JobsGetStatus: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling JobsGetStatus: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelJobsJob) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelJobsJob), response.Headers);
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
            
    
            var path = "/api/platform/jobs/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling JobsGetStatus: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelJobsJob) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelJobsJob), response.Headers);
        }
        
        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesGetModules ()
        {
            
    
            var path = "/api/platform/modules";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModules: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModules: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>), response.Headers);
        }
    
        /// <summary>
        /// Get installed modules 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>> ModulesGetModulesAsync ()
        {
            
    
            var path = "/api/platform/modules";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModules: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformWebModelPackagingModuleDescriptor>), response.Headers);
        }
        
        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>            
        public VirtoCommercePlatformWebModelPackagingModuleDescriptor ModulesUpload ()
        {
            
    
            var path = "/api/platform/modules";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpload: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpload: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelPackagingModuleDescriptor) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor), response.Headers);
        }
    
        /// <summary>
        /// Upload module package for installation or update 
        /// </summary>
        /// <returns>VirtoCommercePlatformWebModelPackagingModuleDescriptor</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformWebModelPackagingModuleDescriptor> ModulesUploadAsync ()
        {
            
    
            var path = "/api/platform/modules";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpload: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelPackagingModuleDescriptor) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor), response.Headers);
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
            
    
            var path = "/api/platform/modules/install";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesInstallModule: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesInstallModule: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification), response.Headers);
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
            
    
            var path = "/api/platform/modules/install";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesInstallModule: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification), response.Headers);
        }
        
        /// <summary>
        /// Restart web application 
        /// </summary>
        /// <returns></returns>            
        public void ModulesRestart ()
        {
            
    
            var path = "/api/platform/modules/restart";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/modules/restart";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/modules/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModuleById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModuleById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelPackagingModuleDescriptor) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor), response.Headers);
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
            
    
            var path = "/api/platform/modules/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesGetModuleById: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelPackagingModuleDescriptor) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelPackagingModuleDescriptor), response.Headers);
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
            
    
            var path = "/api/platform/modules/{id}/uninstall";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUninstallModule: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUninstallModule: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification), response.Headers);
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
            
    
            var path = "/api/platform/modules/{id}/uninstall";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUninstallModule: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification), response.Headers);
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
            
    
            var path = "/api/platform/modules/{id}/update";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpdateModule: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpdateModule: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification), response.Headers);
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
            
    
            var path = "/api/platform/modules/{id}/update";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ModulesUpdateModule: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelPackagingModulePushNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelPackagingModulePushNotification), response.Headers);
        }
        
        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommercePlatformWebModelNotificationsNotification> NotificationsGetNotifications ()
        {
            
    
            var path = "/api/platform/notification";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotifications: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotifications: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelNotificationsNotification>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformWebModelNotificationsNotification>), response.Headers);
        }
    
        /// <summary>
        /// Get all registered notification types Get all registered notification types in platform
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelNotificationsNotification>> NotificationsGetNotificationsAsync ()
        {
            
    
            var path = "/api/platform/notification";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotifications: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelNotificationsNotification>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformWebModelNotificationsNotification>), response.Headers);
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
            
    
            var path = "/api/platform/notification/journal/{objectId}/{objectTypeId}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationJournal: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationJournal: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult), response.Headers);
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
            
    
            var path = "/api/platform/notification/journal/{objectId}/{objectTypeId}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationJournal: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelNotificationsSearchNotificationsResult), response.Headers);
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
            
    
            var path = "/api/platform/notification/notification/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotification: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotification: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelNotificationsNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelNotificationsNotification), response.Headers);
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
            
    
            var path = "/api/platform/notification/notification/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotification: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelNotificationsNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelNotificationsNotification), response.Headers);
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
            
    
            var path = "/api/platform/notification/stopnotifications";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/notification/stopnotifications";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/notification/template";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/notification/template";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/notification/template/rendernotificationcontent";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsRenderNotificationContent: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsRenderNotificationContent: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult), response.Headers);
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
            
    
            var path = "/api/platform/notification/template/rendernotificationcontent";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsRenderNotificationContent: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelNotificationsRenderNotificationContentResult), response.Headers);
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
            
    
            var path = "/api/platform/notification/template/sendnotification";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsSendNotification: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsSendNotification: " + response.ErrorMessage, response.ErrorMessage);
    
            return (string) ApiClient.Deserialize(response.Content, typeof(string), response.Headers);
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
            
    
            var path = "/api/platform/notification/template/sendnotification";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsSendNotification: " + response.Content, response.Content);

            return (string) ApiClient.Deserialize(response.Content, typeof(string), response.Headers);
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
            
    
            var path = "/api/platform/notification/template/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/notification/template/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsDeleteNotificationTemplate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this proprties used in notification template.
        /// </summary>
        /// <param name="type">Notification type</param> 
        /// <returns></returns>            
        public List<VirtoCommercePlatformCoreNotificationsNotificationParameter> NotificationsGetTestingParameters (string type)
        {
            
            // verify the required parameter 'type' is set
            if (type == null) throw new ApiException(400, "Missing required parameter 'type' when calling NotificationsGetTestingParameters");
            
    
            var path = "/api/platform/notification/template/{type}/getTestingParameters";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetTestingParameters: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetTestingParameters: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformCoreNotificationsNotificationParameter>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformCoreNotificationsNotificationParameter>), response.Headers);
        }
    
        /// <summary>
        /// Get testing parameters Method returns notification properties, that defined in notification class, this proprties used in notification template.
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreNotificationsNotificationParameter>> NotificationsGetTestingParametersAsync (string type)
        {
            // verify the required parameter 'type' is set
            if (type == null) throw new ApiException(400, "Missing required parameter 'type' when calling NotificationsGetTestingParameters");
            
    
            var path = "/api/platform/notification/template/{type}/getTestingParameters";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetTestingParameters: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformCoreNotificationsNotificationParameter>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformCoreNotificationsNotificationParameter>), response.Headers);
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
            
    
            var path = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplates: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplates: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>), response.Headers);
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
            
    
            var path = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplates: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformWebModelNotificationsNotificationTemplate>), response.Headers);
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
            
    
            var path = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}/{language}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelNotificationsNotificationTemplate) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelNotificationsNotificationTemplate), response.Headers);
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
            
    
            var path = "/api/platform/notification/template/{type}/{objectId}/{objectTypeId}/{language}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationsGetNotificationTemplate: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelNotificationsNotificationTemplate) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelNotificationsNotificationTemplate), response.Headers);
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
            
    
            var path = "/api/platform/pushnotifications";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationSearch: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult), response.Headers);
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
            
    
            var path = "/api/platform/pushnotifications";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationSearch: " + response.Content, response.Content);

            return (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult), response.Headers);
        }
        
        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>            
        public VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult PushNotificationMarkAllAsRead ()
        {
            
    
            var path = "/api/platform/pushnotifications/markAllAsRead";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationMarkAllAsRead: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationMarkAllAsRead: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult), response.Headers);
        }
    
        /// <summary>
        /// Mark all notifications as read 
        /// </summary>
        /// <returns>VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult> PushNotificationMarkAllAsReadAsync ()
        {
            
    
            var path = "/api/platform/pushnotifications/markAllAsRead";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PushNotificationMarkAllAsRead: " + response.Content, response.Content);

            return (VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCorePushNotificationsPushNotificationSearchResult), response.Headers);
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
            
    
            var path = "/api/platform/security/apiaccounts/new";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGenerateNewApiAccount: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGenerateNewApiAccount: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApiAccount) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityApiAccount), response.Headers);
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
            
    
            var path = "/api/platform/security/apiaccounts/new";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGenerateNewApiAccount: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApiAccount) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityApiAccount), response.Headers);
        }
        
        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>            
        public VirtoCommercePlatformCoreSecurityApplicationUserExtended SecurityGetCurrentUser ()
        {
            
    
            var path = "/api/platform/security/currentuser";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetCurrentUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetCurrentUser: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended), response.Headers);
        }
    
        /// <summary>
        /// Get current user details 
        /// </summary>
        /// <returns>VirtoCommercePlatformCoreSecurityApplicationUserExtended</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityGetCurrentUserAsync ()
        {
            
    
            var path = "/api/platform/security/currentuser";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetCurrentUser: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended), response.Headers);
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
            
    
            var path = "/api/platform/security/login";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityLogin: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityLogin: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended), response.Headers);
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
            
    
            var path = "/api/platform/security/login";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityLogin: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended), response.Headers);
        }
        
        /// <summary>
        /// Sign out 
        /// </summary>
        /// <returns></returns>            
        public void SecurityLogout ()
        {
            
    
            var path = "/api/platform/security/logout";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/security/logout";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/security/permissions";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetPermissions: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetPermissions: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformCoreSecurityPermission>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformCoreSecurityPermission>), response.Headers);
        }
    
        /// <summary>
        /// Get all registered permissions 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformCoreSecurityPermission>> SecurityGetPermissionsAsync ()
        {
            
    
            var path = "/api/platform/security/permissions";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetPermissions: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformCoreSecurityPermission>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformCoreSecurityPermission>), response.Headers);
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
            
    
            var path = "/api/platform/security/roles";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchRoles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchRoles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityRoleSearchResponse) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityRoleSearchResponse), response.Headers);
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
            
    
            var path = "/api/platform/security/roles";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchRoles: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityRoleSearchResponse) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityRoleSearchResponse), response.Headers);
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
            
    
            var path = "/api/platform/security/roles";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateRole: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateRole: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityRole) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityRole), response.Headers);
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
            
    
            var path = "/api/platform/security/roles";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateRole: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityRole) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityRole), response.Headers);
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
            
    
            var path = "/api/platform/security/roles";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/security/roles";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/security/roles/{roleId}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetRole: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetRole: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityRole) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityRole), response.Headers);
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
            
    
            var path = "/api/platform/security/roles/{roleId}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetRole: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityRole) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityRole), response.Headers);
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
            
    
            var path = "/api/platform/security/users";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchUsersAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchUsersAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityUserSearchResponse) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityUserSearchResponse), response.Headers);
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
            
    
            var path = "/api/platform/security/users";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecuritySearchUsersAsync: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityUserSearchResponse) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityUserSearchResponse), response.Headers);
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
            
    
            var path = "/api/platform/security/users";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecuritySecurityResult), response.Headers);
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
            
    
            var path = "/api/platform/security/users";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityUpdateAsync: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecuritySecurityResult), response.Headers);
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
            
    
            var path = "/api/platform/security/users";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/security/users";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/security/users/create";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityCreateAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityCreateAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecuritySecurityResult), response.Headers);
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
            
    
            var path = "/api/platform/security/users/create";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityCreateAsync: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecuritySecurityResult), response.Headers);
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
            
    
            var path = "/api/platform/security/users/{userName}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetUserByName: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetUserByName: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended), response.Headers);
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
            
    
            var path = "/api/platform/security/users/{userName}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityGetUserByName: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecurityApplicationUserExtended) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecurityApplicationUserExtended), response.Headers);
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
            
    
            var path = "/api/platform/security/users/{userName}/changepassword";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityChangePassword: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityChangePassword: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecuritySecurityResult), response.Headers);
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
            
    
            var path = "/api/platform/security/users/{userName}/changepassword";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityChangePassword: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecuritySecurityResult), response.Headers);
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
            
    
            var path = "/api/platform/security/users/{userName}/resetpassword";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityResetPassword: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityResetPassword: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecuritySecurityResult), response.Headers);
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
            
    
            var path = "/api/platform/security/users/{userName}/resetpassword";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SecurityResetPassword: " + response.Content, response.Content);

            return (VirtoCommercePlatformCoreSecuritySecurityResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformCoreSecuritySecurityResult), response.Headers);
        }
        
        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommercePlatformWebModelSettingsSetting> SettingGetAllSettings ()
        {
            
    
            var path = "/api/platform/settings";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetAllSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetAllSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelSettingsSetting>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>), response.Headers);
        }
    
        /// <summary>
        /// Get all settings 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePlatformWebModelSettingsSetting>> SettingGetAllSettingsAsync ()
        {
            
    
            var path = "/api/platform/settings";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetAllSettings: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelSettingsSetting>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>), response.Headers);
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
            
    
            var path = "/api/platform/settings";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/platform/settings";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
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
            
    
            var path = "/api/platform/settings/modules/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetModuleSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetModuleSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePlatformWebModelSettingsSetting>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>), response.Headers);
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
            
    
            var path = "/api/platform/settings/modules/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetModuleSettings: " + response.Content, response.Content);

            return (List<VirtoCommercePlatformWebModelSettingsSetting>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePlatformWebModelSettingsSetting>), response.Headers);
        }
        
        /// <summary>
        /// Get non-array setting value by name 
        /// </summary>
        /// <param name="name">Setting system name.</param> 
        /// <returns>Object</returns>            
        public Object SettingGetValue (string name)
        {
            
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling SettingGetValue");
            
    
            var path = "/api/platform/settings/value/{name}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetValue: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetValue: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Object) ApiClient.Deserialize(response.Content, typeof(Object), response.Headers);
        }
    
        /// <summary>
        /// Get non-array setting value by name 
        /// </summary>
        /// <param name="name">Setting system name.</param>
        /// <returns>Object</returns>
        public async System.Threading.Tasks.Task<Object> SettingGetValueAsync (string name)
        {
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling SettingGetValue");
            
    
            var path = "/api/platform/settings/value/{name}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetValue: " + response.Content, response.Content);

            return (Object) ApiClient.Deserialize(response.Content, typeof(Object), response.Headers);
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
            
    
            var path = "/api/platform/settings/values/{name}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetArray: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetArray: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<Object>) ApiClient.Deserialize(response.Content, typeof(List<Object>), response.Headers);
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
            
    
            var path = "/api/platform/settings/values/{name}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetArray: " + response.Content, response.Content);

            return (List<Object>) ApiClient.Deserialize(response.Content, typeof(List<Object>), response.Headers);
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
            
    
            var path = "/api/platform/settings/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetSetting: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetSetting: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePlatformWebModelSettingsSetting) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelSettingsSetting), response.Headers);
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
            
    
            var path = "/api/platform/settings/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SettingGetSetting: " + response.Content, response.Content);

            return (VirtoCommercePlatformWebModelSettingsSetting) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePlatformWebModelSettingsSetting), response.Headers);
        }
        
    }
    
}
