#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using VirtoCommerce.Web.Convertors;
using VirtoCommerce.Web.Models.Cms;
using VirtoCommerce.Web.Views.Contents;
using VirtoCommerce.Web.Views.Engines.Liquid;

#endregion

namespace VirtoCommerce.Web.Services
{
    public class PagesService
    {
        #region Public Methods and Operators
        public Page GetPage(SiteContext context, string handle)
        {
            var service = GetService(context);

            if (!handle.StartsWith("/"))
                handle = "/" + handle;

            var item = service.GetContentItem(handle);

            if (item == null)
            {
                return null;
            }

            return item.AsPageWebModel();
        }

        public IEnumerable<Page> GetCollection(SiteContext context, string collectioName)
        {
            var service = GetService(context);

            var items = service.GetCollectionContentItems(collectioName);

            if (items == null)
            {
                return null;
            }

            return items.Select(x=>x.AsPageWebModel());
        }

        public IEnumerable<Blog> GetBlogs(SiteContext context)
        {

            var service = GetService(context);
            var items = service.GetCollectionContentItems("blogs");

            if (items == null)
            {
                return null;
            }

            // segment posts by blogs
            var segments = items.Select(x => x.FileName.Split('/')[2]).Distinct();

            var blogs = new List<Blog>();
            foreach(var segment in segments)
            {
                var prefix = string.Format("/blogs/{0}", segment);
                var articles = items.Where(x=>x.FileName.StartsWith(prefix)).Select(x => x.AsArticleWebModel());
                var blog = new Blog() { Id = segment, Handle = segment, Url = prefix, Title = segment, Articles = articles.ToArray() };
                blogs.Add(blog);
            }

            return blogs;
        }

        #endregion

        public PublishingService GetService(SiteContext context)
        {
            var filesPath = HostingEnvironment.MapPath(String.Format("~/App_Data/Pages/{0}/{1}", context.StoreId, context.Language));
            var service = new PublishingService(filesPath, new[] { new LiquidTemplateEngine(filesPath) });
            return service;
        }
    }
}