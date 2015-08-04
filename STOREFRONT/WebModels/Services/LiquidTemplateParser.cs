using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using DotLiquid;
using VirtoCommerce.Web.Views.Engines.Liquid;

namespace VirtoCommerce.Web.Services
{
    public class LiquidTemplateParser : ITemplateParser
    {
        private readonly string _baseDirectoryPath;
        public LiquidTemplateParser(string baseDirectoryPath)
        {
            this._baseDirectoryPath = baseDirectoryPath;
        }

        #region Implementation of ITemplateParser

        public Template Parse(ViewLocationResult location)
        {
            var contextKey = "vc-cms-file-" + location.Location;
            var value = HttpRuntime.Cache.Get(contextKey);

            if (value != null)
            {
                return value as Template;
            }

            if (location.Contents == null)
                return null;

            var contents = location.Contents;
            var template = Template.Parse(contents);

            var path = HostingEnvironment.MapPath(_baseDirectoryPath);
            var allDirectories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);

            HttpRuntime.Cache.Insert(contextKey, template, new CacheDependency(allDirectories));

            return template;

        }
        #endregion
    }
}
