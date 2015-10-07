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

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/products")]
    public class CatalogModuleProductsController : CatalogBaseController
    {
        private readonly IItemService _itemsService;
        private readonly IPropertyService _propertyService;
        private readonly IBlobUrlResolver _blobUrlResolver;
        private readonly ICatalogService _catalogService;
        private readonly ISkuGenerator _skuGenerator;

        public CatalogModuleProductsController(IItemService itemsService, IPropertyService propertyService, IBlobUrlResolver blobUrlResolver, 
                                               ICatalogService catalogService, ISkuGenerator skuGenerator, ISecurityService securityService, IPermissionScopeService permissionScopeService)
            :base(securityService, permissionScopeService)
        {
            _itemsService = itemsService;
            _propertyService = propertyService;
            _blobUrlResolver = blobUrlResolver;
            _catalogService = catalogService;
            _skuGenerator = skuGenerator;
        }


        /// <summary>
        /// Gets item by id.
        /// </summary>
        /// <param name="id">Item id.</param>
        [HttpGet]
        [ResponseType(typeof(webModel.Product))]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var item = _itemsService.GetById(id, coreModel.ItemResponseGroup.ItemLarge);
            if (item == null)
            {
                return NotFound();
            }

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Read, item);

            var properties = GetAllCatalogProperies(item.CatalogId, item.CategoryId);
            var retVal = item.ToWebModel(_blobUrlResolver, properties);
            retVal.SecurityScopes = base.GetObjectPermissionScopeStrings(item);
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

			};

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, retVal.ToModuleModel(_blobUrlResolver));


            if (catalogId != null)
			{
				var properites = GetAllCatalogProperies(catalogId, categoryId);
				retVal.Properties = properites.Select(x => x.ToWebModel()).ToList();

				foreach (var property in retVal.Properties)
				{
					property.Values = new List<webModel.PropertyValue>();
					property.IsManageable = true;
					property.IsReadOnly = property.Type != coreModel.PropertyType.Product && property.Type != coreModel.PropertyType.Variation;
				}
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


            var properties = GetAllCatalogProperies(product.CatalogId, product.CategoryId);
            var mainWebProduct = product.ToWebModel(_blobUrlResolver, properties);

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
                //Need reset value ids
                foreach (var val in property.Values.ToArray())
                {
                    val.Id = null;
                }

                // Mark variation property as required
                if (property.Type == coreModel.PropertyType.Variation)
                {
                    property.Required = true;
                }

                property.IsManageable = true;
            }


            newVariation.Code = _skuGenerator.GenerateSku(newVariation.ToModuleModel(null));
            return Ok(newVariation);
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

        private coreModel.Property[] GetAllCatalogProperies(string catalogId, string categoryId)
        {
            if (catalogId == null)
                throw new ArgumentNullException("catalogId");

            coreModel.Property[] retVal = null;
            if (!String.IsNullOrEmpty(categoryId))
            {
                retVal = _propertyService.GetCategoryProperties(categoryId);
            }
            else
            {
                retVal = _propertyService.GetCatalogProperties(catalogId);
            }
            return retVal;
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