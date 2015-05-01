#region
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("")]
    public class ProductController : BaseController
    {
        #region Public Methods and Operators
        //[Route("products/{item}")]
        public async Task<ActionResult> ProductAsync(string item)
        {
            var product = await this.Service.GetProductAsync(item);
            this.Context.Set("Product", product);

            if(product == null)
                throw new HttpException(404, "Product not found");

            //this.Context.Set("current_page", page);
            return this.View("product");
        }

        public async Task<ActionResult> ProductByKeywordAsync(string item)
        {
            var product = await this.Service.GetProductByKeywordAsync(item);
            this.Context.Set("Product", product);

            if (product == null)
                throw new HttpException(404, "Product not found");

            return this.View("product");
        }

        //[Route("collections/{collection}/products/{handle}")]
        public async Task<ActionResult> ProductInCollectionAsync(string collection, string handle, int page = 1)
        {
            this.Context.Set("Collection", await this.Service.GetCollectionAsync(collection));

            var product = await this.Service.GetProductAsync(handle);
            this.Context.Set("Product", product);

            if (product == null)
                throw new HttpException(404, "Product not found");

            this.Context.Set("current_page", page);
            return this.View("product");
        }

        [Route("products/{handle}.js")]
        public async Task<ActionResult> ProductJsonAsync(string handle)
        {
            var product = await this.Service.GetProductAsync(handle);
            return this.Json(product, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}