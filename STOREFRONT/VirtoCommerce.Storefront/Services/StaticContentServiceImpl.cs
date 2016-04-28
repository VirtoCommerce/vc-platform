using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CacheManager.Core;
using MarkdownSharp;
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
        private readonly ILocalCacheManager _cacheManager;
        private readonly Func<WorkContext> _workContextFactory;
        private readonly Func<IStorefrontUrlBuilder> _urlBuilderFactory;
        private readonly Func<string, ContentItem> _contentItemFactory;
        private readonly IContentBlobProvider _contentBlobProvider;

        [CLSCompliant(false)]
        public StaticContentServiceImpl(Markdown markdownRender, ILiquidThemeEngine liquidEngine,
                                        ILocalCacheManager cacheManager, Func<WorkContext> workContextFactory,
                                        Func<IStorefrontUrlBuilder> urlBuilderFactory, Func<string, ContentItem> contentItemFactory,
                                        IContentBlobProvider contentBlobProvider)
        {
            _markdownRender = markdownRender;
            _liquidEngine = liquidEngine;
            
            _cacheManager = cacheManager;
            _workContextFactory = workContextFactory;
            _urlBuilderFactory = urlBuilderFactory;
            _contentItemFactory = contentItemFactory;
            _contentBlobProvider = contentBlobProvider;

            //Observe content changes to invalidate cache if changes occur
            _contentBlobProvider.Changed += (sender, args) =>
            {
                _cacheManager.Clear();
            };
            _contentBlobProvider.Renamed += (sender, args) =>
            {
                _cacheManager.Clear();
            };
        }

        #region IStaticContentService Members

        public IEnumerable<ContentItem> LoadStoreStaticContent(Store store)
        {
            var retVal = new List<ContentItem>();
            var baseStoreContentPath = "/" + store.Id;
            var searchPattern = "*.*";

            if (_contentBlobProvider.PathExists(baseStoreContentPath))
            {
                var config = _liquidEngine.GetSettings();

                //Search files by requested search pattern
                var contentBlobs = _contentBlobProvider.Search(baseStoreContentPath, searchPattern, true)
                                             .Where(x => _extensions.Any(y => x.EndsWith(y)))
                                             .Select(x => x.Replace("\\\\", "\\"));

                //each content file  has a name pattern {name}.{language?}.{ext}
                var localizedBlobs = contentBlobs.Select(x => new LocalizedBlobInfo(x));

                foreach (var localizedBlob in localizedBlobs.OrderBy(x => x.Name))
                {
                    var blobRelativePath = "/" + localizedBlob.Path.TrimStart('/');
                    var contentItem = _contentItemFactory(blobRelativePath);
                    if (contentItem != null)
                    {
                        if (contentItem.Name == null)
                        {
                            contentItem.Name = localizedBlob.Name;
                        }
                        contentItem.Language = localizedBlob.Language;
                        contentItem.FileName = Path.GetFileName(blobRelativePath);
                        contentItem.StoragePath = "/" + blobRelativePath.Replace(baseStoreContentPath + "/", string.Empty).TrimStart('/');

                        LoadAndRenderContentItem(blobRelativePath, contentItem);

                        retVal.Add(contentItem);
                    }
                }
            }

            return retVal.ToArray();
        }

        #endregion

        private void LoadAndRenderContentItem(string contentPath, ContentItem contentItem)
        {
            string content = null;
            using (var stream = _contentBlobProvider.OpenRead(contentPath))
        {
            //Load raw content with metadata
                content = stream.ReadToString();
            }
            IDictionary<string, IEnumerable<string>> metaHeaders = null;
            IDictionary themeSettings = null;
            try
            {
                metaHeaders = ReadYamlHeader(content);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Failed to read yaml header from \"{0}\"", contentItem.StoragePath), ex);
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
            if (string.Equals(Path.GetExtension(contentItem.StoragePath), ".md", StringComparison.InvariantCultureIgnoreCase))
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


        //each content file  has a name pattern {name}.{language?}.{ext}
        private class LocalizedBlobInfo
        {
            public LocalizedBlobInfo(string blobPath)
            {
                Language = Language.InvariantLanguage;
                Path = blobPath;
                var parts = System.IO.Path.GetFileName(blobPath).Split('.');
                Name = parts.FirstOrDefault();
                if (parts.Count() == 3)
                {
                    try
                    {
                        Language = new Language(parts[1]);
                    }
                    catch(Exception)
                    {
                        Language = Language.InvariantLanguage;
                    }
                }
            }
            public string Name { get; private set; }
            public Language Language { get; private set; }
            public string Path { get; private set; }
        }
    }
}