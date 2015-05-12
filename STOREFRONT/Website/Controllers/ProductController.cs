#region
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
            var product = await this.Service.GetProductAsync(SiteContext.Current, item);
            this.Context.Set("Product", product);

            if(product == null)
                throw new HttpException(404, "Product not found");

            //this.Context.Set("current_page", page);
            return this.View("product");
        }

        public async Task<ActionResult> ProductByCodeAsync(string item)
        {
            var product = await this.Service.GetProductAsync(SiteContext.Current, item);
            this.Context.Set("Product", product);

            if (product == null)
                throw new HttpException(404, "Product not found");

            //this.Context.Set("current_page", page);
            return this.View("product");
        }

        public async Task<ActionResult> ProductByKeywordAsync(string item)
        {
            var product = await this.Service.GetProductByKeywordAsync(SiteContext.Current, item) ?? await this.Service.GetProductAsync(SiteContext.Current, item);

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
            this.Context.Set("Collection", await this.Service.GetCollectionAsync(SiteContext.Current, collection));

            var product = await this.Service.GetProductAsync(SiteContext.Current, handle);
            this.Context.Set("Product", product);

            if (product == null)
                throw new HttpException(404, "Product not found");

            this.Context.Set("current_page", page);
            return this.View("product");
        }

        [Route("products/{handle}.js")]
        public async Task<ActionResult> ProductJsonAsync(string handle)
        {
            var product = await this.Service.GetProductAsync(SiteContext.Current, handle);
            return this.Json(product, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}