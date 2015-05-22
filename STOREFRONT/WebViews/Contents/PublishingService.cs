#region
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Web;
using System.Web.Caching;
using MarkdownDeep;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;

#endregion

namespace VirtoCommerce.Web.Views.Contents
{
    public class PublishingService
    {
        #region Static Fields
        private static readonly Markdown Markdown = new Markdown();
        #endregion

        #region Fields
        private readonly SiteStaticContentContext Context;

        private readonly FileSystem _fileSystem;

        private readonly ITemplateEngine[] _templateEngines;

        private Dictionary<string, object> _Config;
        #endregion

        #region Constructors and Destructors
        public PublishingService(string sourceFolder, ITemplateEngine[] templateEngines)
        {
            this._templateEngines = templateEngines;
            this._fileSystem = new FileSystem();

            // Now lets build the context
            this.Context = this.BuildSiteContext(sourceFolder);
        }
        #endregion

        #region Public Methods and Operators
        public ContentItem[] GetCollectionContentItems(string collectioName)
        {
            return
                this.Context.Collections.Where(col => col.Key.Equals(collectioName, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Value)
                    .SingleOrDefault();
        }

        public ContentItem GetContentItem(string name)
        {
            return
                this.Context.Collections.SelectMany(pages => pages.Value)
                    .SingleOrDefault(page => page.Url.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region Methods
        private SiteStaticContentContext BuildSiteContext(string sourceFolder)
        {
            var contextKey = "vc-no-cms";
            var value = HttpRuntime.Cache.Get(contextKey);

            if (value != null)
            {
                return value as SiteStaticContentContext;
            }

            this._Config = new Dictionary<string, object>();
            var configPath = Path.Combine(sourceFolder, "config.yml");
            if (this._fileSystem.File.Exists(configPath))
            {
                this._Config =
                    (Dictionary<string, object>)this._fileSystem.File.ReadAllText(configPath).YamlHeader(true);
            }

            var context = new SiteStaticContentContext() { SourceFolder = sourceFolder, Config = this._Config };

            var collections = new Dictionary<string, ContentItem[]>();

            // list all directories
            var collectionFolders =
                this._fileSystem.Directory.GetDirectories(sourceFolder, "*", SearchOption.TopDirectoryOnly)
                    .Where(name => !name.EndsWith("includes", StringComparison.OrdinalIgnoreCase));

            // now for each directory get a list of content items
            foreach (var collectionFolder in collectionFolders)
            {
                var items =
                    this.GetCollectionContentItemsInternal(context, collectionFolder)
                        .Where(item => item != null)
                        .ToArray();

                var collectionName = this.GetPageTitle(collectionFolder);
                collections.Add(collectionName, items);
            }

            // populate collection object
            context.Collections = collections;

            // add to cache
            var allDirectories = this._fileSystem.Directory.GetDirectories(
                sourceFolder,
                "*",
                SearchOption.AllDirectories);
            HttpRuntime.Cache.Insert(contextKey, context, new CacheDependency(allDirectories));

            // return context
            return context;
        }

        private ContentItem CreateContentItem(
            SiteStaticContentContext context,
            string path,
            Dictionary<string, object> config)
        {
            // 1: Read raw contents and meta data. Determine contents format and read it into Contents property, create RawContentItem.
            var rawItem = this.CreateRawItem(path);

            // 2: Use convert engines to get html contents and create ContentItem object
            foreach (var templateEngine in this._templateEngines)
            {
                if (templateEngine.CanProcess(rawItem.ContentType, "html"))
                {
                    var content = templateEngine.Process(rawItem.Content, rawItem.Settings);
                    var page = new ContentItem { Content = content };

                    page.SetHeaderSettings(rawItem.Settings);
                    page.Settings = rawItem.Settings;
                    page.Url = rawItem.Settings.ContainsKey("permalink")
                        ? rawItem.Settings["permalink"]
                        : this.EvaluateLink(context, path);
                    return page;
                }
            }

            return null;
        }

        private RawContentItem CreateRawItem(string file)
        {
            var contents = this.SafeReadContents(file);
            var header = contents.YamlHeader();

            var page = new RawContentItem { Content = this.RenderContent(file, contents, header) };

            page.Settings = header;

            return page;
        }

        private string EvaluateLink(SiteStaticContentContext context, string path)
        {
            var directory = Path.GetDirectoryName(path);
            var relativePath = directory.Replace(context.SourceFolder, string.Empty);
            var fileExtension = Path.GetExtension(path);

            var htmlExtensions = new[] { ".markdown", ".mdown", ".mkdn", ".mkd", ".md", ".textile" };

            if (htmlExtensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase))
            {
                fileExtension = "";
            }

            var link = relativePath.Replace('\\', '/').TrimStart('/') + "/" + this.GetPageTitle(path) + fileExtension;
            if (!link.StartsWith("/"))
            {
                link = "/" + link;
            }
            return link;
        }

        /// <summary>
        /// Loads all content items in the certain collection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="collectionFolder"></param>
        /// <returns></returns>
        private IEnumerable<ContentItem> GetCollectionContentItemsInternal(
            SiteStaticContentContext context,
            string collectionFolder)
        {
            var items = new List<ContentItem>();
            if (this._fileSystem.Directory.Exists(collectionFolder))
            {
                items.AddRange(
                    this._fileSystem.Directory.GetFiles(collectionFolder, "*.*", SearchOption.AllDirectories)
                        .Select(file => this.CreateContentItem(context, file, this._Config))
                        .Where(post => post != null));
            }

            return items;
        }

        private string GetPageTitle(string file)
        {
            return Path.GetFileNameWithoutExtension(file);
        }

        private string RenderContent(string file, string contents, IDictionary<string, object> header)
        {
            string html;
            try
            {
                var contentsWithoutHeader = contents.ExcludeHeader();
                html = string.Equals(Path.GetExtension(file), ".md", StringComparison.InvariantCultureIgnoreCase)
                    ? Markdown.Transform(contentsWithoutHeader)
                    : contentsWithoutHeader;

                //html = contentTransformers.Aggregate(html, (current, contentTransformer) => contentTransformer.Transform(current));
            }
            catch (Exception)
            {
                //Tracing.Info(String.Format("Error ({0}) converting {1}", e.Message, file));
                //Tracing.Debug(e.ToString());
                html = String.Format("<p><b>Error converting markdown</b></p><pre>{0}</pre>", contents);
            }
            return html;
        }

        private string SafeReadContents(string file)
        {
            try
            {
                return this._fileSystem.File.ReadAllText(file);
            }
            catch (IOException)
            {
                var fileInfo = this._fileSystem.FileInfo.FromFileName(file);
                var tempFile = Path.Combine(Path.GetTempPath(), fileInfo.Name);
                try
                {
                    fileInfo.CopyTo(tempFile, true);
                    return this._fileSystem.File.ReadAllText(tempFile);
                }
                finally
                {
                    if (this._fileSystem.File.Exists(tempFile))
                    {
                        this._fileSystem.File.Delete(tempFile);
                    }
                }
            }
        }
        #endregion
    }
}