using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Tracing;
using Microsoft.Owin;
using Owin;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Web;

[assembly: OwinStartup(typeof(SignalRStartup))]
namespace VirtoCommerce.Web
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration();
            // Any connection or hub wire up and configuration should go here
#if DEBUG
            hubConfiguration.EnableDetailedErrors = true;
#endif
            app.MapSignalR(hubConfiguration);

            GlobalHost.DependencyResolver.Register(typeof(IHubDescriptorProvider), () => new CustomHubDescriptorProvider(GlobalHost.DependencyResolver));
        }
    }

    #region Fix for issue https://github.com/SignalR/SignalR/issues/558
    public class CustomHubDescriptorProvider : IHubDescriptorProvider
    {
        private readonly Lazy<IDictionary<string, HubDescriptor>> _hubs;
        private readonly Lazy<IAssemblyLocator> _locator;
        private readonly TraceSource _trace;

        public CustomHubDescriptorProvider(IDependencyResolver resolver)
        {
            _locator = new Lazy<IAssemblyLocator>(resolver.Resolve<IAssemblyLocator>);
            _hubs = new Lazy<IDictionary<string, HubDescriptor>>(BuildHubsCache);


            var traceManager = resolver.Resolve<ITraceManager>();
            _trace =
                traceManager["SignalR." + typeof(ReflectedHubDescriptorProvider).Name];
        }

        public IList<HubDescriptor> GetHubs()
        {
            return _hubs.Value
                .Select(kv => kv.Value)
                .Distinct()
                .ToList();
        }

        public bool TryGetHub(string hubName, out HubDescriptor descriptor)
        {
            return _hubs.Value.TryGetValue(hubName, out descriptor);
        }

        protected IDictionary<string, HubDescriptor> BuildHubsCache()
        {
            // Getting all IHub-implementing types that apply
            var types = _locator.Value.GetAssemblies()
                .SelectMany(GetTypesSafe)
                .Where(IsHubType);

            // Building cache entries for each descriptor
            // Each descriptor is stored in dictionary under a key
            // that is it's name or the name provided by an attribute
            var hubDescriptors = types
                .Select(type => new HubDescriptor
                {
                    NameSpecified = (type.GetHubAttributeName() != null),
                    Name = type.GetHubName(),
                    HubType = type
                });

            var cacheEntries = new Dictionary<string, HubDescriptor>(StringComparer.OrdinalIgnoreCase);

            foreach (var descriptor in hubDescriptors)
            {
                HubDescriptor oldDescriptor;
                if (!cacheEntries.TryGetValue(descriptor.Name, out oldDescriptor))
                {
                    cacheEntries[descriptor.Name] = descriptor;
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format(CultureInfo.CurrentCulture,
                            "Two Hubs must not share the same name. '{0}' and '{1}' both share the name '{2}'",
                            oldDescriptor.HubType.AssemblyQualifiedName,
                            descriptor.HubType.AssemblyQualifiedName,
                            descriptor.Name));
                }
            }

            return cacheEntries;
        }

        private static bool IsHubType(Type type)
        {
            try
            {
                return typeof(IHub).IsAssignableFrom(type) &&
                       !type.IsAbstract &&
                       (type.Attributes.HasFlag(TypeAttributes.Public) ||
                        type.Attributes.HasFlag(TypeAttributes.NestedPublic));
            }
            catch
            {
                return false;
            }
        }

        private IEnumerable<Type> GetTypesSafe(Assembly a)
        {
            try
            {
                return a.GetLoadableTypes();
            }
            catch (Exception ex)
            {
                _trace.TraceWarning("None of the classes from assembly '{0}' could be loaded when searching for Hubs. [{1}]",
                                   a.FullName,
                                   a.Location,
                                   ex.GetType().Name,
                                   ex.Message);
                return Enumerable.Empty<Type>();
            }
        }
    }

    internal static class HubTypeExtensions
    {
        internal static string GetHubName(this Type type)
        {
            if (!typeof(IHub).IsAssignableFrom(type))
            {
                return null;
            }

            return GetHubAttributeName(type) ?? type.Name;
        }

        internal static string GetHubAttributeName(this Type type)
        {
            if (!typeof(IHub).IsAssignableFrom(type))
            {
                return null;
            }

            // We can still return null if there is no attribute name
            return ReflectionHelper.GetAttributeValue<HubNameAttribute, string>(type, attr => attr.HubName);
        }
    }

    #endregion
}