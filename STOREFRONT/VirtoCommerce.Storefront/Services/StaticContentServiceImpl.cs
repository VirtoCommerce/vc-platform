using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// <summary>
    /// Represent a search and rendering static content (pages and blogs)
    /// </summary>
    public class StaticContentServiceImpl : IStaticContentService
    {
        private static Regex _headerRegExp = new Regex(@"(?s:^---(.*?)---)");
        private static string[] _extensions = new[] { ".md", ".html" };
        private readonly Markdown _markdownRender;
        private readonly ILiquidThemeEngine _liquidEngine;
        private readonly string _baseLocalPath;
        private FileSystemWatcher _fileSystemWatcher;
        private readonly ICacheManager<object> _cacheManager;
        private readonly Func<WorkContext> _workContextFactory;
        private readonly Func<IStorefrontUrlBuilder> _urlBuilderFactory;
        private readonly LinkHelper _linkHelper;

        [CLSCompliant(false)]
        public StaticContentServiceImpl(string baseLocalPath, Markdown markdownRender, ILiquidThemeEngine liquidEngine,
                                        ICacheManager<object> cacheManager, Func<WorkContext> workContextFactory,
                                        Func<IStorefrontUrlBuilder> urlBuilderFactory)
        {
            _baseLocalPath = baseLocalPath;
            _markdownRender = markdownRender;
            _liquidEngine = liquidEngine;
            _fileSystemWatcher = MonitorContentFileSystemChanges();
            _cacheManager = cacheManager;
            _workContextFactory = workContextFactory;
            _urlBuilderFactory = urlBuilderFactory;
            _linkHelper = new LinkHelper();
        }

        #region IStaticContentService Members
        /// <summary>
        /// Search store static contents by path and for specified language
        /// </summary>
        /// <param name="url"></param>
        /// <param name="store"></param>
        /// <param name="language"></param>
        /// <param name="contentItemFactory"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ContentItem> LoadContentItemsByUrl(string url, Store store, Language language, Func<ContentItem> contentItemFactory, string[] excludingNames = null, int pageIndex = 1, int pageSize = 10, bool renderContent = true)
        {
            var retVal = new List<ContentItem>();
            var totalCount = 0;
            url = Uri.UnescapeDataString(url);
            //construct local path {base path}\{store}\{url}
            var baseStorePath = _baseLocalPath + "\\" + store.Id + "\\";
            var localSearchPath = baseStorePath + url.Replace('/', '\\');
            var isDirectorySearch = Directory.Exists(localSearchPath);
            var searchPattern = "*.*";
            if (!isDirectorySearch)
            {
                searchPattern = Path.GetFileNameWithoutExtension(localSearchPath) + ".*";
                //Get parent directory path
                localSearchPath = Path.GetDirectoryName(localSearchPath);
            }

            if (Directory.Exists(localSearchPath))
            {
                //Search files by requested search pattern
                var files = Directory.GetFiles(localSearchPath, searchPattern, SearchOption.AllDirectories)
                                             .Where(x => _extensions.Any(y => x.EndsWith(y)))
                                             .Select(x=>x.Replace("\\\\", "\\"));
                //Because can be exist files with same name but for different languages
                //need filter and leave only files for requested language in file extension or without it (default)
                //each content file  has a name pattern {name}.{language?}.{ext}
                var localizedFiles = files.Select(x => new LocalizedFileInfo(x))
                             .GroupBy(x => x.Name).Select(x => x.OrderByDescending(y => y.Language).FirstOrDefault(y => language.Equals(y.Language) || String.IsNullOrEmpty(y.Language)))
                             .Where(x => x != null);

                if(excludingNames != null && excludingNames.Any())
                {
                    localizedFiles = localizedFiles.Where(x => !excludingNames.Contains(x.Name.ToLowerInvariant()));
                }

                totalCount = localizedFiles.Count();
                foreach (var localizedFile in localizedFiles.OrderBy(x => x.Name).Skip((pageIndex - 1) * pageSize).Take(pageSize))
                {
                    var relativePath = localizedFile.LocalPath.Replace(baseStorePath, string.Empty);
                    var contentItem = contentItemFactory();
                    contentItem.Name = localizedFile.Name;
                    contentItem.Language = language;
                    contentItem.RelativePath = relativePath;
                    contentItem.FileName = Path.GetFileName(relativePath);
                    contentItem.LocalPath = localizedFile.LocalPath;

                    LoadAndRenderContentItem(contentItem, renderContent);

                    contentItem.Url = _linkHelper.EvaluatePermalink("none", contentItem); // TODO: replace with setting "permalink"


                    retVal.Add(contentItem);
                }
            }

            return new StaticPagedList<ContentItem>(retVal, pageIndex, pageSize, totalCount);
        }
        #endregion
         private void LoadAndRenderContentItem(ContentItem contentItem, bool renderContent)
        {
            var fileInfo = new FileInfo(contentItem.LocalPath);

            contentItem.CreatedDate = fileInfo.CreationTimeUtc;

            //Load raw content with metadata
            var content = File.ReadAllText(contentItem.LocalPath);
            var metaHeaders = ReadYamlHeader(content);
            content = RemoveYamlHeader(content);

            if (renderContent)
            {
                var workContext = _workContextFactory();
                if (workContext != null)
                {
                    var shopifyContext = workContext.ToShopifyModel(_urlBuilderFactory());
                    var parameters = shopifyContext.ToLiquid() as Dictionary<string, object>;
                    parameters.Add("settings", _liquidEngine.GetSettings());
                    //Render content by liquid engine
                    content = _liquidEngine.RenderTemplate(content, parameters);
                }

                //Render markdown content
                if (String.Equals(Path.GetExtension(contentItem.LocalPath), ".md", StringComparison.InvariantCultureIgnoreCase))
                {
                    content = _markdownRender.Transform(content);
                }
            }

            contentItem.LoadContent(content, metaHeaders);
        }

        private static string RemoveYamlHeader(string text)
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

        //each content file  has a name pattern {name}.{language?}.{ext}
        private class LocalizedFileInfo
        {
            public LocalizedFileInfo(string filePath)
            {
                LocalPath = filePath;
                var parts = Path.GetFileName(filePath).Split('.');
                Name = parts.FirstOrDefault();
                if (parts.Count() == 3)
                {
                    Language = parts[1];
                }
            }
            public string Name { get; private set; }
            public string Language { get; private set; }
            public string LocalPath { get; private set; }
        }
    }
}