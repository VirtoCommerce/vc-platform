using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Xml;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

internal class MvcSiteMapProviderConfig
{
    public static void Register(IDependencyResolver container)
    {
// Setup global sitemap loader
        MvcSiteMapProvider.SiteMaps.Loader = container.GetService<ISiteMapLoader>();

// Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider
        var validator = container.GetService<ISiteMapXmlValidator>();
        validator.ValidateXml(HostingEnvironment.MapPath("~/Store.sitemap"));

// Register the Sitemaps routes for search engines
        XmlSiteMapController.RegisterRoutes(RouteTable.Routes);
    }
}
