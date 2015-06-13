#region
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Web;
using System.Web.Caching;
using MarkdownDeep;
using VirtoCommerce.Web.Views.Contents.Extensions;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;

#endregion

namespace VirtoCommerce.Web.Views.Contents
{
    public class PublishingService
    {
        #region Static Fields
        private static readonly Markdown _markdown = new Markdown();
        #endregion

        #region Fields
        private readonly SiteStaticContentContext _context;

        private readonly FileSystem _fileSystem;

        private readonly ITemplateEngine[] _templateEngines;

        #endregion

        #region Constructors and Destructors
        public PublishingService(string sourceFolder, ITemplateEngine[] templateEngines)
        {
            this._templateEngines = templateEngines;
            this._fileSystem = new FileSystem();

            // Now lets build the context
            this._context = this.BuildSiteContext(sourceFolder);
        }
        #endregion

        #region Public Methods and Operators
        public ContentItem[] GetCollectionContentItems(string collectioName)
        {
            return
                this._context.Collections.Where(col => col.Key.Equals(collectioName, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Value)
                    .SingleOrDefault();
        }

        public ContentItem GetContentItem(string name)
        {
            return
                this._context.Collections.SelectMany(pages => pages.Value)
                    .SingleOrDefault(page => page.Url.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region Methods
        private SiteStaticContentContext BuildSiteContext(string sourceFolder)
        {
            var contextKey = "vc-no-cms" + sourceFolder;
            var value = HttpRuntime.Cache.Get(contextKey);

            if (value != null)
            {
                return value as SiteStaticContentContext;
            }

            var context = CreateStaticContext(sourceFolder);

            var collections = new Dictionary<string, ContentItem[]>();

            if (this._fileSystem.Directory.Exists(sourceFolder))
            {
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

                // set URLs
                var allItems = collections.SelectMany(x => x.Value).ToList();
                allItems.SetPostUrl(context);

                // populate collection object
                context.Collections = collections;

                // add to cache
                var allDirectories = this._fileSystem.Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories);
                HttpRuntime.Cache.Insert(contextKey, context, new CacheDependency(allDirectories));
            }
            else
            {
                context.Collections = collections;
            }

            // return context
            return context;
        }

        private ContentItem CreateContentItem(
            SiteStaticContentContext context,
            string path)
        {
            // 1: Read raw contents and meta data. Determine contents format and read it into Contents property, create RawContentItem.
            var rawItem = this.CreateRawItem(path);

            /// if a 'date' property is found in markdown file header, that date will be used instead of the date in the file name
            var date = DateTime.Now;
            if (rawItem.Settings.ContainsKey("date"))
                DateTime.TryParse((string)rawItem.Settings["date"], out date);

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
                    page.Date = date;
                    return page;
                }
            }

            return null;
        }

        private SiteStaticContentContext CreateStaticContext(string sourceFolder)
        {
            var config = new Dictionary<string, object>();
            var configPath = Path.Combine(sourceFolder, "config.yml");
            if (this._fileSystem.File.Exists(configPath))
            {
                config =
                    (Dictionary<string, object>)this._fileSystem.File.ReadAllText(configPath).YamlHeader(true);
            }

            var context = new SiteStaticContentContext() { SourceFolder = sourceFolder, Config = config };

            return context;
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
            var extensions = new HashSet<string>(new[] { ".md", ".markdown", ".html" }, StringComparer.OrdinalIgnoreCase);
            var items = new List<ContentItem>();
            if (_fileSystem.Directory.Exists(collectionFolder))
            {
                var files = _fileSystem.DirectoryInfo.FromDirectoryName(collectionFolder).GetFiles("*", SearchOption.AllDirectories)
                    .Where(x => extensions.Contains(x.Extension));
                items.AddRange(
                    files.Select(file => CreateContentItem(context, file.FullName))
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
                    ? _markdown.Transform(contentsWithoutHeader)
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