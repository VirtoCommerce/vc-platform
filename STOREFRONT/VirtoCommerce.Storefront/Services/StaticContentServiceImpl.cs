using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CacheManager.Core;
using MarkdownDeep;
using VirtoCommerce.LiquidThemeEngine;
using VirtoCommerce.LiquidThemeEngine.Converters;
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
        private readonly Func<string, ContentItem> _contentItemFactory;

     
        [CLSCompliant(false)]
        public StaticContentServiceImpl(string baseLocalPath, Markdown markdownRender, ILiquidThemeEngine liquidEngine,
                                        ICacheManager<object> cacheManager, Func<WorkContext> workContextFactory,
                                        Func<IStorefrontUrlBuilder> urlBuilderFactory, Func<string, ContentItem> contentItemFactory)
        {
            _baseLocalPath = baseLocalPath;
            _markdownRender = markdownRender;
            _liquidEngine = liquidEngine;
            _fileSystemWatcher = MonitorContentFileSystemChanges();
            _cacheManager = cacheManager;
            _workContextFactory = workContextFactory;
            _urlBuilderFactory = urlBuilderFactory;
            _contentItemFactory = contentItemFactory;
        }

        #region IStaticContentService Members

        public IEnumerable<ContentItem> LoadStoreStaticContent(Store store)
        {
            var retVal = new List<ContentItem>();
            var totalCount = 0;
            var baseStorePath = _baseLocalPath + "\\" + store.Id + "\\";
            var localSearchPath = baseStorePath;
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
                var config = _liquidEngine.GetSettings();

                //Search files by requested search pattern
                var files = Directory.GetFiles(localSearchPath, searchPattern, SearchOption.AllDirectories)
                                             .Where(x => _extensions.Any(y => x.EndsWith(y)))
                                             .Select(x => x.Replace("\\\\", "\\"));

                //each content file  has a name pattern {name}.{language?}.{ext}
                var localizedFiles = files.Select(x => new LocalizedFileInfo(x));

                totalCount = localizedFiles.Count();
                foreach (var localizedFile in localizedFiles.OrderBy(x => x.Name))
                {
                    var relativePath = localizedFile.LocalPath.Replace(baseStorePath, string.Empty).Replace("\\", "/");

                    var contentItem = _contentItemFactory(relativePath);
                    if (contentItem != null)
                    {
                        if (contentItem.Name == null)
                        {
                            contentItem.Name = localizedFile.Name;
                        }
                        contentItem.Language = localizedFile.Language;

                        contentItem.RelativePath = relativePath;
                        contentItem.FileName = Path.GetFileName(relativePath);
                        contentItem.LocalPath = localizedFile.LocalPath;
                    
                        LoadAndRenderContentItem(contentItem);

                        retVal.Add(contentItem);
                    }
                }
            }

            return retVal.ToArray();
        }

        #endregion

        private void LoadAndRenderContentItem(ContentItem contentItem)
        {
            var fileInfo = new FileInfo(contentItem.LocalPath);

            contentItem.CreatedDate = fileInfo.CreationTimeUtc;

            //Load raw content with metadata
            var content = File.ReadAllText(contentItem.LocalPath);
            IDictionary<string, IEnumerable<string>> metaHeaders = null;
            IDictionary themeSettings = null;
            try
            {
                metaHeaders = ReadYamlHeader(content);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Failed to read yaml header from \"{0}\"", contentItem.RelativePath), ex);
            }

            content = RemoveYamlHeader(content);

            var workContext = _workContextFactory();
            if (workContext != null)
            {
                var shopifyContext = workContext.ToShopifyModel(_urlBuilderFactory());
                var parameters = shopifyContext.ToLiquid() as Dictionary<string, object>;
              
                themeSettings = _liquidEngine.GetSettings();
                parameters.Add("settings", themeSettings);
                //Render content by liquid engine
                content = _liquidEngine.RenderTemplate(content, parameters);
            }

            //Render markdown content
            if (string.Equals(Path.GetExtension(contentItem.LocalPath), ".md", StringComparison.InvariantCultureIgnoreCase))
            {
                content = _markdownRender.Transform(content);
            }


            contentItem.LoadContent(content, metaHeaders, themeSettings);
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
                Language = Language.InvariantLanguage;
                LocalPath = filePath;
                var parts = Path.GetFileName(filePath).Split('.');
                Name = parts.FirstOrDefault();
                if (parts.Count() == 3)
                {
                    Language = new Language(parts[1]);
                }
            }
            public string Name { get; private set; }
            public Language Language { get; private set; }
            public string LocalPath { get; private set; }
        }
    }
}