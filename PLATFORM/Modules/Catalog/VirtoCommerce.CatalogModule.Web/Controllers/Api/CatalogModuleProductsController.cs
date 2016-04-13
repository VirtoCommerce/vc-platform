using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Security;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.CatalogModule.Web.Security;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CatalogModule.Web.Binders;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/products")]
    public class CatalogModuleProductsController : CatalogBaseController
    {
        private readonly IItemService _itemsService;
        private readonly IBlobUrlResolver _blobUrlResolver;
        private readonly ICatalogService _catalogService;
        private readonly ICategoryService _categoryService;
        private readonly ISkuGenerator _skuGenerator;

        public CatalogModuleProductsController(IItemService itemsService, IBlobUrlResolver blobUrlResolver, ICatalogService catalogService, ICategoryService categoryService,
                                               ISkuGenerator skuGenerator, ISecurityService securityService, IPermissionScopeService permissionScopeService)
            : base(securityService, permissionScopeService)
        {
            _itemsService = itemsService;
            _categoryService = categoryService;
            _blobUrlResolver = blobUrlResolver;
            _catalogService = catalogService;
            _skuGenerator = skuGenerator;
        }


        /// <summary>
        /// Gets product by id.
        /// </summary>
        /// <param name="id">Item id.</param>
        ///<param name="respGroup">Response group.</param>
        [HttpGet]
        [ResponseType(typeof(webModel.Product))]
        [Route("{id}")]
        public IHttpActionResult GetProductById(string id, [FromUri] coreModel.ItemResponseGroup respGroup = coreModel.ItemResponseGroup.ItemLarge)
        {
            var item = _itemsService.GetById(id, respGroup);
            if (item == null)
            {
                return NotFound();
            }

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Read, item);

            var retVal = item.ToWebModel(_blobUrlResolver);

            retVal.SecurityScopes = base.GetObjectPermissionScopeStrings(item);
            return Ok(retVal);
        }

        /// <summary>
        /// Gets products by ids
        /// </summary>
        /// <param name="ids">Item ids</param>
        ///<param name="respGroup">Response group.</param>
        //Because Swagger generated API client passed arrays as joined string need parse query string by binder
        [HttpGet]
        [ResponseType(typeof(webModel.Product[]))]
        [Route("")]
        public IHttpActionResult GetProductByIds([ModelBinder(typeof(IdsStringArrayBinder))] string[] ids, [FromUri] coreModel.ItemResponseGroup respGroup = coreModel.ItemResponseGroup.ItemLarge)
        {
            var items = _itemsService.GetByIds(ids, respGroup);
            if (items == null)
            {
                return NotFound();
            }

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Read, items);

            var retVal = items.Select(x => x.ToWebModel(_blobUrlResolver)).ToArray();
            foreach (var product in retVal)
            {
                product.SecurityScopes = base.GetObjectPermissionScopeStrings(product);
            }
            return Ok(retVal);
        }

        /// <summary>
        /// Gets the template for a new product (outside of category).
        /// </summary>
        /// <remarks>Use when need to create item belonging to catalog directly.</remarks>
        /// <param name="catalogId">The catalog id.</param>
        [HttpGet]
        [ResponseType(typeof(webModel.Product))]
        [Route("~/api/catalog/{catalogId}/products/getnew")]
        public IHttpActionResult GetNewProductByCatalog(string catalogId)
        {
            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, new coreModel.Catalog { Id = catalogId });

            return GetNewProductByCatalogAndCategory(catalogId, null);
        }


        /// <summary>
        /// Gets the template for a new product (inside category).
        /// </summary>
        /// <remarks>Use when need to create item belonging to catalog category.</remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        [HttpGet]
        [ResponseType(typeof(webModel.Product))]
        [Route("~/api/catalog/{catalogId}/categories/{categoryId}/products/getnew")]
        public IHttpActionResult GetNewProductByCatalogAndCategory(string catalogId, string categoryId)
        {
            var retVal = new webModel.Product
            {
                CategoryId = categoryId,
                CatalogId = catalogId,
                IsActive = true,
                SeoInfos = new SeoInfo[0]
            };

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, retVal.ToModuleModel(_blobUrlResolver));

            if (catalogId != null)
            {
                var catalog = _catalogService.GetById(catalogId);
                retVal.Properties = catalog.Properties.Select(x => x.ToWebModel()).ToList();
            }

            if (categoryId != null)
            {
                var category = _categoryService.GetById(categoryId, Domain.Catalog.Model.CategoryResponseGroup.WithProperties);
                retVal.Properties = category.Properties.Select(x => x.ToWebModel()).ToList();
            }


            foreach (var property in retVal.Properties)
            {
                property.Values = new List<webModel.PropertyValue>();
                property.IsManageable = true;
                property.IsReadOnly = property.Type != coreModel.PropertyType.Product && property.Type != coreModel.PropertyType.Variation;
            }


            retVal.Code = _skuGenerator.GenerateSku(retVal.ToModuleModel(null));

            return Ok(retVal);
        }


        /// <summary>
        /// Gets the template for a new variation.
        /// </summary>
        /// <param name="productId">The parent product id.</param>
        [HttpGet]
        [ResponseType(typeof(webModel.Product))]
        [Route("{productId}/getnewvariation")]
        public IHttpActionResult GetNewVariation(string productId)
        {
            var product = _itemsService.GetById(productId, coreModel.ItemResponseGroup.ItemLarge);
            if (product == null)
            {
                return NotFound();
            }

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, product);

            var mainWebProduct = product.ToWebModel(_blobUrlResolver);

            var newVariation = new webModel.Product
            {
                Name = product.Name,
                CategoryId = product.CategoryId,
                CatalogId = product.CatalogId,
                TitularItemId = product.MainProductId ?? productId,
                Properties = mainWebProduct.Properties.Where(x => x.Type == coreModel.PropertyType.Variation).ToList(),
            };

            foreach (var property in newVariation.Properties)
            {
                // Mark variation property as required
                if (property.Type == coreModel.PropertyType.Variation)
                {
                    property.Required = true;
                    property.Values.Clear();
                }

                property.IsManageable = true;
            }


            newVariation.Code = _skuGenerator.GenerateSku(newVariation.ToModuleModel(null));
            return Ok(newVariation);
        }


        [HttpGet]
        [ResponseType(typeof(webModel.Product))]
        [Route("{productId}/clone")]
        public IHttpActionResult CloneProduct(string productId)
        {

            var product = _itemsService.GetById(productId, coreModel.ItemResponseGroup.ItemLarge);
            if (product == null)
            {
                return NotFound();
            }
            //Need reset all Id instead properties because its gets by inheritance
            var allEntities = product.GetFlatObjectsListWithInterface<IEntity>();
            foreach (var entity in allEntities)
            {
                var property = entity as coreModel.Property;
                if (property == null)
                {
                    entity.Id = null;
                }
            }
            product.Code = _skuGenerator.GenerateSku(product);
            product.SeoInfos.Clear();
            foreach (var variation in product.Variations)
            {
                variation.Code = _skuGenerator.GenerateSku(variation);
                variation.SeoInfos.Clear();
            }

            return Ok(product.ToWebModel(_blobUrlResolver));
        }

        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult Update(webModel.Product product)
        {
            var updatedProduct = UpdateProduct(product);
            if (updatedProduct != null)
            {
                return Ok(updatedProduct);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Deletes the specified items by id.
        /// </summary>
        /// <param name="ids">The items ids.</param>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult Delete([FromUri] string[] ids)
        {
            var products = _itemsService.GetByIds(ids, Domain.Catalog.Model.ItemResponseGroup.ItemInfo);
            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Delete, products);

            _itemsService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }


        private coreModel.CatalogProduct UpdateProduct(webModel.Product product)
        {
            var moduleProduct = product.ToModuleModel(_blobUrlResolver);
            if (moduleProduct.Id == null)
            {
                if (moduleProduct.SeoInfos == null || !moduleProduct.SeoInfos.Any())
                {
                    var slugUrl = GenerateProductDefaultSlugUrl(product);
                    if (!string.IsNullOrEmpty(slugUrl))
                    {
                        var catalog = _catalogService.GetById(product.CatalogId);
                        var defaultLanguageCode = catalog.Languages.First(x => x.IsDefault).LanguageCode;
                        var seoInfo = new SeoInfo
                        {
                            LanguageCode = defaultLanguageCode,
                            SemanticUrl = slugUrl
                        };
                        moduleProduct.SeoInfos = new SeoInfo[] { seoInfo };
                    }
                }

                base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, moduleProduct);
                return _itemsService.Create(moduleProduct);
            }
            else
            {
                base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Update, moduleProduct);
                _itemsService.Update(new[] { moduleProduct });
            }

            return null;
        }

        private string GenerateProductDefaultSlugUrl(webModel.Product product)
        {
            var retVal = new List<string>();
            retVal.Add(product.Name);
            if (product.Properties != null)
            {
                foreach (var property in product.Properties.Where(x => x.Type == coreModel.PropertyType.Variation && x.Values != null))
                {
                    retVal.AddRange(property.Values.Select(x => x.PropertyName + "-" + x.Value));
                }
            }
            return String.Join(" ", retVal).GenerateSlug();
        }
    }
}