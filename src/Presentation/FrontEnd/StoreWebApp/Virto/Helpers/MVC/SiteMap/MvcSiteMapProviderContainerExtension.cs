using Microsoft.Practices.Unity;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Web.Compilation;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Script.Serialization;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Hosting;
using System.Web.Mvc;
using VirtoCommerce.Web.Client.Extensions;

namespace VirtoCommerce.Web.Virto.Helpers.MVC.SiteMap
{
    public class MvcSiteMapProviderContainerExtension
        : UnityContainerExtension
    {
        protected override void Initialize()
        {
            const bool enableLocalization = true;
            string absoluteFileName = HostingEnvironment.MapPath("~/Store.sitemap");
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);
            const bool visibilityAffectsDescendants = true;
            const bool useTitleIfDescriptionNotProvided = true;

            const bool securityTrimmingEnabled = false;
            var includeAssembliesForScan = new[] { "VirtoCommerce.Web" };

            var currentAssembly = GetType().Assembly;
            var siteMapProviderAssembly = typeof(SiteMaps).Assembly;
            var clientAssembly = typeof (NamedSiteMapNodeVisibilityProvider).Assembly;
            var allAssemblies = new[] { currentAssembly, siteMapProviderAssembly, clientAssembly };
            var excludeTypes = new[] {
                typeof(IRouteValueDictionaryFactory),
                typeof(ISiteMapCacheKeyGenerator),
            };
            var multipleImplementationTypes = new[] {
                typeof(ISiteMapNodeUrlResolver),
                typeof(ISiteMapNodeVisibilityProvider),
                typeof(IDynamicNodeProvider)
            };



// Matching type name (I[TypeName] = [TypeName]) or matching type name + suffix Adapter (I[TypeName] = [TypeName]Adapter)
// and not decorated with the [ExcludeFromAutoRegistrationAttribute].
            CommonConventions.RegisterDefaultConventions(
                (interfaceType, implementationType) => Container.RegisterType(interfaceType, implementationType, new ContainerControlledLifetimeManager()),
                new[] { siteMapProviderAssembly },
                allAssemblies,
                excludeTypes,
                string.Empty);

// Multiple implementations of strategy based extension points (and not decorated with [ExcludeFromAutoRegistrationAttribute]).
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => Container.RegisterType(interfaceType, implementationType, implementationType.Name, new ContainerControlledLifetimeManager()),
                multipleImplementationTypes,
                allAssemblies,
                excludeTypes,
                string.Empty);

// TODO: Find a better way to inject an array constructor

//Register custom route value dictionary factory
            Container.RegisterType<IRouteValueDictionaryFactory, SeoRouteValueDictionaryFactory>(new InjectionConstructor(
                new ResolvedParameter<IRequestCache>(),
                new ResolvedParameter<IReservedAttributeNameProvider>(),
                new ResolvedParameter<IJsonToDictionaryDeserializer>()));

//Register custom sitemap cache key generator
            Container.RegisterType<ISiteMapCacheKeyGenerator, SiteMapStoreCacheKeyGenerator>(new InjectionConstructor(
                new ResolvedParameter<IMvcContextFactory>()));

// Url Resolvers
            Container.RegisterType<ISiteMapNodeUrlResolverStrategy, SiteMapNodeUrlResolverStrategy>(new InjectionConstructor(
                new ResolvedArrayParameter<ISiteMapNodeUrlResolver>(Container.ResolveAll<ISiteMapNodeUrlResolver>().ToArray())
                ));

// Visibility Providers
            Container.RegisterType<ISiteMapNodeVisibilityProviderStrategy, SiteMapNodeVisibilityProviderStrategy>(new InjectionConstructor(
                new ResolvedArrayParameter<ISiteMapNodeVisibilityProvider>(Container.ResolveAll<ISiteMapNodeVisibilityProvider>().ToArray()),
                new InjectionParameter<string>(typeof(NamedSiteMapNodeVisibilityProvider).ShortAssemblyQualifiedName())
                ));

// Dynamic Node Providers
            Container.RegisterType<IDynamicNodeProviderStrategy, DynamicNodeProviderStrategy>(new InjectionConstructor(
                new ResolvedArrayParameter<IDynamicNodeProvider>(Container.ResolveAll<IDynamicNodeProvider>().ToArray())
                ));


// Pass in the global controllerBuilder reference
            Container.RegisterInstance<ControllerBuilder>(ControllerBuilder.Current);

            Container.RegisterType<IControllerTypeResolverFactory, ControllerTypeResolverFactory>(new InjectionConstructor(
                new List<string>(),
                new ResolvedParameter<IControllerBuilder>(),
                new ResolvedParameter<IBuildManager>()));

// Configure Security

// IMPORTANT: Must give arrays of object a name in Unity in order for it to resolve them.
            Container.RegisterType<IAclModule, AuthorizeAttributeAclModule>("authorizeAttribute");
            Container.RegisterType<IAclModule, XmlRolesAclModule>("xmlRoles");
            Container.RegisterType<IAclModule, CompositeAclModule>(new InjectionConstructor(new ResolvedArrayParameter<IAclModule>(
                new ResolvedParameter<IAclModule>("authorizeAttribute"),
                new ResolvedParameter<IAclModule>("xmlRoles"))));

            Container.RegisterInstance<ObjectCache>(MemoryCache.Default);
            Container.RegisterType(typeof(ICacheProvider<>), typeof(RuntimeCacheProvider<>));
            Container.RegisterType<ICacheDependency, RuntimeFileCacheDependency>(
                "cacheDependency", new InjectionConstructor(absoluteFileName));

            Container.RegisterType<ICacheDetails, CacheDetails>("cacheDetails",
                new InjectionConstructor(absoluteCacheExpiration, TimeSpan.MinValue, new ResolvedParameter<ICacheDependency>("cacheDependency")));

// Configure the visitors
            Container.RegisterType<ISiteMapNodeVisitor, UrlResolvingSiteMapNodeVisitor>();

// Prepare for the sitemap node providers
            Container.RegisterType<IXmlSource, FileXmlSource>("file1XmlSource", new InjectionConstructor(absoluteFileName));
            Container.RegisterType<IReservedAttributeNameProvider, ReservedAttributeNameProvider>(new InjectionConstructor(new List<string>()));

// IMPORTANT: Must give arrays of object a name in Unity in order for it to resolve them.
// Register the sitemap node providers
            Container.RegisterInstance<ISiteMapNodeProvider>("xmlSiteMapNodeProvider1",
                Container.Resolve<XmlSiteMapNodeProviderFactory>().Create(Container.Resolve<IXmlSource>("file1XmlSource")), new ContainerControlledLifetimeManager());
            Container.RegisterInstance<ISiteMapNodeProvider>("reflectionSiteMapNodeProvider1",
                Container.Resolve<ReflectionSiteMapNodeProviderFactory>().Create(includeAssembliesForScan), new ContainerControlledLifetimeManager());
            Container.RegisterType<ISiteMapNodeProvider, CompositeSiteMapNodeProvider>(new InjectionConstructor(new ResolvedArrayParameter<ISiteMapNodeProvider>(
                new ResolvedParameter<ISiteMapNodeProvider>("xmlSiteMapNodeProvider1"),
                new ResolvedParameter<ISiteMapNodeProvider>("reflectionSiteMapNodeProvider1"))));

// Configure the builders
            Container.RegisterType<ISiteMapBuilder, SiteMapBuilder>();

// Configure the builder sets
            Container.RegisterType<ISiteMapBuilderSet, SiteMapBuilderSet>("builderSet1",
                new InjectionConstructor(
                    "default",
                    securityTrimmingEnabled,
                    enableLocalization,
                    visibilityAffectsDescendants,
                    useTitleIfDescriptionNotProvided,
                    new ResolvedParameter<ISiteMapBuilder>(),
                    new ResolvedParameter<ICacheDetails>("cacheDetails")));

            Container.RegisterType<ISiteMapBuilderSetStrategy, SiteMapBuilderSetStrategy>(new InjectionConstructor(
                new ResolvedArrayParameter<ISiteMapBuilderSet>(new ResolvedParameter<ISiteMapBuilderSet>("builderSet1"))));
        }
    }
}
