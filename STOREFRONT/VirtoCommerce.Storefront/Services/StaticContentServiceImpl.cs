using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using CacheManager.Core;
using MarkdownDeep;
using PagedList;
using VirtoCommerce.LiquidThemeEngine;
using VirtoCommerce.LiquidThemeEngine.Converters;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Model.StaticContent;
using YamlDotNet.RepresentationModel;

namespace VirtoCommerce.Storefront.Services
{
    public class StaticContentServiceImpl : IStaticContentService
    {
        private static  Regex _headerRegExp = new Regex(@"(?s:^---(.*?)---)");
        private static string[] _extensions = new[] { ".md", ".html" };
        private readonly Markdown _markdownRender;
        private readonly ShopifyLiquidThemeEngine _liquidEngine;
        private readonly string _baseLocalPath;
        private FileSystemWatcher _fileSystemWatcher;
        private readonly ICacheManager<object> _cacheManager;

        public StaticContentServiceImpl(string baseLocalPath, Markdown markdownRender, ShopifyLiquidThemeEngine liquidEngine, ICacheManager<object> cacheManager)
        {
            _baseLocalPath = baseLocalPath;
            _markdownRender = markdownRender;
            _liquidEngine = liquidEngine;
            _fileSystemWatcher = MonitorContentFileSystemChanges();
            _cacheManager = cacheManager;
        }

        #region IStaticContentService Members

        public IPagedList<ContentItem> LoadContentItemsByUrl(string url, Store store, Language language, Func<string, ContentItem> contentItemFactory, int pageIndex = 1, int pageSize = 10)
        {
            var retVal = new List<ContentItem>();
            var totalCount = 0;
            url = Uri.UnescapeDataString(url);
            var parts = url.Split('/');
            var localPath = _baseLocalPath + "\\" + store.Id + "\\" + String.Join("\\", parts);
            var searchPattern = "*." + language.CultureName + ".*";
            if (!Directory.Exists(localPath))
            {
                localPath = _baseLocalPath + "\\" + store.Id + "\\" + String.Join("\\", parts.Take(parts.Count() - 1));
                searchPattern = parts.Last() + "." + language.CultureName + ".*";
            }
            if (Directory.Exists(localPath))
            {
                var files = Directory.GetFiles(localPath, searchPattern, SearchOption.TopDirectoryOnly)
                                             .Where(x => _extensions.Any(y => x.EndsWith(y)));
                totalCount = files.Count();
                foreach (var file in files.OrderBy(x => x).Skip((pageIndex - 1) * pageSize).Take(pageSize))
                {
                    var contentItem = contentItemFactory(GetUrlFromPath(file, url));
                    LoadAndRenderContentItemFromFile(contentItem, file);
                    retVal.Add(contentItem);
                }
            }
            return new StaticPagedList<ContentItem>(retVal, pageIndex, pageSize, totalCount);
        }
        #endregion
     
        private void LoadAndRenderContentItemFromFile(ContentItem contentItem, string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            contentItem.CreatedDate = fileInfo.CreationTimeUtc;
            contentItem.LocalPath = filePath;

            //Load raw content with metadata
            var content = File.ReadAllText(filePath);
            var metaHeaders = ReadYamlHeader(content);
            content = ExcludeYamlHeader(content);


            var shopifyContext = _liquidEngine.WorkContext.ToShopifyModel(_liquidEngine.UrlBuilder);
            var parameters = shopifyContext.ToLiquid() as Dictionary<string, object>;
            parameters.Add("settings", _liquidEngine.GetSettings());
            //Render content by liquid engine
            content = _liquidEngine.RenderTemplate(content, parameters);

            //Render markdown content
            if (String.Equals(Path.GetExtension(filePath), ".md", StringComparison.InvariantCultureIgnoreCase))
            {
                content = _markdownRender.Transform(content);
            }


            contentItem.LoadContent(content, metaHeaders);
        }

        private string GetUrlFromPath(string filePath, string url)
        {
            var retVal = url;
            var fileName = "/" + Path.GetFileNameWithoutExtension(filePath).Split('.').First();
            if(!retVal.EndsWith(fileName))
            {
                retVal += fileName;
            }
            return Uri.EscapeUriString(retVal.TrimStart('/'));
        }

        private static string ExcludeYamlHeader(string text)
        {
            var retVal = text;
            var headerMatches = _headerRegExp.Matches(text);
            if (headerMatches.Count > 0)
            {
                retVal = text.Replace(headerMatches[0].Groups[0].Value, "").Trim();
            }
            return retVal;
        }

        private static IDictionary<string, IEnumerable<string>> ReadYamlHeader(string text)
        {
            var retVal = new Dictionary<string, IEnumerable<string>>();
            var headerMatches = _headerRegExp.Matches(text);
            if (headerMatches.Count == 0)
                return retVal;

            var input = new StringReader(headerMatches[0].Groups[1].Value);
            var yaml = new YamlStream();

            yaml.Load(input);

            if (yaml.Documents.Count > 0)
            {
                var root = yaml.Documents[0].RootNode;
                var collection = root as YamlMappingNode;
                if (collection != null)
                {
                    foreach (var entry in collection.Children)
                    {
                        var node = entry.Key as YamlScalarNode;
                        if (node != null)
                        {
                            retVal.Add(node.Value, GetYamlNodeValues(entry.Value));
                        }
                    }
                }
            }
            return retVal;
        }

        private static IEnumerable<string> GetYamlNodeValues(YamlNode value)
        {
            var retVal = new List<String>();
            var list = value as YamlSequenceNode;

            if (list != null)
            {
                foreach (var entry in list.Children)
                {
                    var node = entry as YamlScalarNode;
                    if (node != null)
                    {
                        retVal.Add(node.Value);
                    }
                }
            }
            else
            {
                retVal.Add(value.ToString());
            }

            return retVal;
        }

        private FileSystemWatcher MonitorContentFileSystemChanges()
        {
            var fileSystemWatcher = new FileSystemWatcher();

            if (Directory.Exists(_baseLocalPath))
            {
                fileSystemWatcher.Path = _baseLocalPath;
                fileSystemWatcher.IncludeSubdirectories = true;

                FileSystemEventHandler handler = (sender, args) =>
                {
                    _cacheManager.Clear();
                };
                RenamedEventHandler renamedHandler = (sender, args) =>
                {
                    _cacheManager.Clear();
                };
                var throttledHandler = handler.Throttle(TimeSpan.FromSeconds(5));
                // Add event handlers.
                fileSystemWatcher.Changed += throttledHandler;
                fileSystemWatcher.Created += throttledHandler;
                fileSystemWatcher.Deleted += throttledHandler;
                fileSystemWatcher.Renamed += renamedHandler;

                // Begin watching.
                fileSystemWatcher.EnableRaisingEvents = true;
            }
            return fileSystemWatcher;
        }

     
    }
}