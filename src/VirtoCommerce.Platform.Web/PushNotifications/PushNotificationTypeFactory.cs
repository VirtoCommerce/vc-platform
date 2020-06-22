using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public static class PushNotificationTypeFactory
    {
        // TODO: move logic to reflection utility
        static readonly IDictionary<string, Type> _notificationTypes = (
                    from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                        // from domainAssembly in domainAssembly.GetExportedTypes()
                    from assemblyType in domainAssembly.GetTypes()
                    where typeof(PushNotification).IsAssignableFrom(assemblyType)
                    // alternative: where assemblyType.IsSubclassOf(typeof(B))
                    // alternative: && ! assemblyType.IsAbstract
                    select assemblyType).ToDictionary(x => x.Name);

        public static Type GetType(string notificationTypeName)
        {
            _notificationTypes.TryGetValue(notificationTypeName, out var result);
            return result;
        }

    }
}
