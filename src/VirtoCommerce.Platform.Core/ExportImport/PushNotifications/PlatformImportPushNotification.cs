namespace VirtoCommerce.Platform.Core.ExportImport.PushNotifications
{
    public class PlatformImportPushNotification : PlatformExportImportPushNotification
    {
        public PlatformImportPushNotification()
           : base(null)
        {
        }
        public PlatformImportPushNotification(string creator)
            : base(creator)
        {
        }

    }
}
