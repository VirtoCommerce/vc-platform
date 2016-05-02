using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.LinkList.Services;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class MenuLinkListServiceImpl : IMenuLinkListService
    {
        private readonly ICMSContentModuleApi _cmsApi;
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly IStorefrontUrlBuilder _urlBuilder;
        public MenuLinkListServiceImpl(ICMSContentModuleApi cmsApi, ICatalogSearchService catalogSearchService, IStorefrontUrlBuilder urlBuilder)
        {
            _cmsApi = cmsApi;
            _catalogSearchService = catalogSearchService;
            _urlBuilder = urlBuilder;
        }

        #region ILinkListService Members
        public async Task<MenuLinkList[]> LoadAllStoreLinkListsAsync(string storeId)
        {
            var retVal = new List<MenuLinkList>();
            var linkLists = await _cmsApi.MenuGetListsAsync(storeId);
            if (linkLists != null)
            {
                retVal.AddRange(linkLists.Select(x => x.ToWebModel()));
                var allMenuLinks = retVal.SelectMany(x => x.MenuLinks);
                var productLinks = allMenuLinks.OfType<ProductMenuLink>();
                var categoryLinks = allMenuLinks.OfType<CategoryMenuLink>();
                Task<Product[]> productsLoadingTask = null;
                Task<Category[]> categoriesLoadingTask = null;

                //Parallel loading associated objects
                var productIds = productLinks.Select(x => x.AssociatedObjectId).ToArray();
                if (productIds.Any())
                {
                    productsLoadingTask = _catalogSearchService.GetProductsAsync(productIds, ItemResponseGroup.ItemSmall | ItemResponseGroup.Seo);
                }
                var categoriesIds = categoryLinks.Select(x => x.AssociatedObjectId).ToArray();
                if (categoriesIds.Any())
                {
                    categoriesLoadingTask = _catalogSearchService.GetCategoriesAsync(categoriesIds, CategoryResponseGroup.Info | CategoryResponseGroup.WithImages | CategoryResponseGroup.WithSeo);
                }
                //Populate link by associated product
                if (productsLoadingTask != null)
                {
                    var products = await productsLoadingTask;
                    foreach (var productLink in productLinks)
                    {
                        productLink.Product = products.FirstOrDefault(x => x.Id == productLink.AssociatedObjectId);
                    }
                }
                //Populate link by associated category
                if (categoriesLoadingTask != null)
                {
                    var categories = await categoriesLoadingTask;
                    foreach (var categoryLink in categoryLinks)
                    {
                        categoryLink.Category = categories.FirstOrDefault(x => x.Id == categoryLink.AssociatedObjectId);
                    }
                }

            }
            return retVal.ToArray();
        }

    
        #endregion


    }
}