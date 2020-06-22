using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public static class PushNotificationExtensions
    {
        public static void AddPushNotifications(this IServiceCollection services)
        {
            services.AddSingleton<IPushNotificationStorage, PushNotificationInMemoryStorage>();
            services.AddSingleton<IPushNotificationManager, PushNotificationManager>();

            RegisterTypesInAbstractFactory();
        }

        private static void RegisterTypesInAbstractFactory()
        {
            // TODO: move logic to reflection utility
            var notificationTypes = (
                    from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()                            
                    from assemblyType in domainAssembly.GetTypes()
                    where //typeof(PushNotification).IsAssignableFrom(assemblyType)
                    assemblyType.IsSubclassOf(typeof(PushNotification)) && !assemblyType.IsAbstract
                    select assemblyType).Except(new[] { typeof(PushNotification) }).ToArray();

            foreach(var type in notificationTypes)
            {
                AbstractTypeFactory<PushNotification>.RegisterType(type);
            }
        }
    }
}
