#region
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Web.Caching;
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("")]
    public class ProductController : StoreControllerBase
    {
        #region Public Methods and Operators
        [Route("products/{item}")]
        public async Task<ActionResult> ProductAsync(string item)
        {
            var product = await InnerGetProductAsync(item);

            this.Context.Set("Product", product);

            if(product == null)
                throw new HttpException(404, "Product not found");

            //this.Context.Set("current_page", page);
            return this.View("product");
        }

        public async Task<ActionResult> ProductByCodeAsync(string item)
        {
            var product = await InnerGetProductAsync(item);

            this.Context.Set("Product", product);

            if (product == null)
                throw new HttpException(404, "Product not found");

            //this.Context.Set("current_page", page);
            return this.View("product");
        }

        public async Task<ActionResult> ProductByKeywordAsync(string category, string item)
        {
            var productCollectionCacheKey = CacheKey.Create("product-collection", category, item);
            var collection = await base.Context.CacheManager.GetAsync(productCollectionCacheKey, TimeSpan.FromHours(1), async () =>
            {
                return await this.Service.GetCollectionByKeywordAsync(SiteContext.Current, category) ?? await this.Service.GetCollectionAsync(SiteContext.Current, category);
            });
            this.Context.Set("Collection", collection);

            var productCacheKey = CacheKey.Create("product-by-keyword", category, item);
            var product = await base.Context.CacheManager.GetAsync(productCacheKey, TimeSpan.FromHours(1), async () =>
            {
                return await this.Service.GetProductByKeywordAsync(SiteContext.Current, item, collection) ?? await this.Service.GetProductAsync(SiteContext.Current, item, collection);
            });


            if (product != null)
            {
                var keyword = product.Keywords.SeoKeyword();
                SetPageMeta(keyword);
            }

            this.Context.Set("Product", product);

            if (product == null)
                throw new HttpException(404, "Product not found");

            return this.View("product");
        }

        //[Route("collections/{collection}/products/{handle}")]
        public async Task<ActionResult> ProductInCollectionAsync(string collection, string handle, int page = 1)
        {
            var parentCollectionCacheKey = CacheKey.Create("product-collection", collection, handle, page.ToString());
            var parentCollection = await base.Context.CacheManager.GetAsync(parentCollectionCacheKey, TimeSpan.FromHours(1), async () =>
            {
                return await this.Service.GetCollectionByKeywordAsync(SiteContext.Current, collection) ?? await this.Service.GetCollectionAsync(SiteContext.Current, collection);
            });
            this.Context.Set("Collection", parentCollection);

            var productCacheKey = CacheKey.Create("product-in-collection", handle, collection);
            var product = await base.Context.CacheManager.GetAsync(productCacheKey, TimeSpan.FromHours(1), async () =>
            {
                return await this.Service.GetProductAsync(SiteContext.Current, handle, parentCollection);
            });
            this.Context.Set("Product", product);

            if (product == null)
                throw new HttpException(404, "Product not found");

            this.Context.Set("current_page", page);
            return this.View("product");
        }

        [Route("products/{handle}.js")]
        public async Task<ActionResult> ProductJsonAsync(string handle)
        {
            var product = await InnerGetProductAsync(handle);
     
            return this.Json(product, JsonRequestBehavior.AllowGet);
        }
        #endregion

        private async Task<Product> InnerGetProductAsync(string handle)
        {
            var productCacheKey = CacheKey.Create("product", handle);
            var product = await base.Context.CacheManager.GetAsync(productCacheKey, TimeSpan.FromHours(1), async () =>
            {
                return await this.Service.GetProductAsync(SiteContext.Current, handle);
            });

            return product;
      
        }

    



    }
}