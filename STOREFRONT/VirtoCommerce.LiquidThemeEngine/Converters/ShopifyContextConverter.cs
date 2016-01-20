﻿using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using storefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ShopifyContextConverter
    {
        public static ShopifyThemeWorkContext ToShopifyModel(this storefrontModel.WorkContext workContext, IStorefrontUrlBuilder urlBuilder)
        {
            var result = new ShopifyThemeWorkContext();

            result.CurrentPage = 1;
            result.CountryOptionTags = string.Join("\r\n", workContext.AllCountries.OrderBy(c => c.Name).Select(c => c.ToOptionTag()));
            result.PageDescription = workContext.CurrentPageSeo != null ? workContext.CurrentPageSeo.MetaDescription : string.Empty;
            result.PageTitle = workContext.CurrentPageSeo != null ? workContext.CurrentPageSeo.Title : string.Empty;
            result.Shop = workContext.CurrentStore != null ? workContext.CurrentStore.ToShopifyModel(workContext) : null;
            result.Cart = workContext.CurrentCart != null ? workContext.CurrentCart.ToShopifyModel(workContext) : null;
            result.Product = workContext.CurrentProduct != null ? workContext.CurrentProduct.ToShopifyModel(workContext) : null;
            result.Customer = workContext.CurrentCustomer != null && workContext.CurrentCustomer.HasAccount ? workContext.CurrentCustomer.ToShopifyModel(workContext, urlBuilder) : null;
            result.AllStores = workContext.AllStores.Select(x => x.ToShopifyModel(workContext)).ToArray();

            result.CurrentCurrency = workContext.CurrentCurrency != null ? workContext.CurrentCurrency.ToShopifyModel() : null;
            result.CurrentLanguage = workContext.CurrentLanguage != null ? workContext.CurrentLanguage.ToShopifyModel() : null;

            if (workContext.CurrentProduct != null && workContext.CurrentProduct.Category != null)
            {
                result.Collection = workContext.CurrentProduct.Category.ToShopifyModel(workContext);
            }

            if (workContext.CurrentCatalogSearchCriteria != null && workContext.CurrentCatalogSearchCriteria.Terms.Any())
            {
                result.CurrentTags =
                    new TagCollection(
                        workContext.CurrentCatalogSearchCriteria.Terms.Select(t => t.ToShopifyModel()).ToList());
            }

            var searchResult = workContext.CurrentCatalogSearchResult;
            if (searchResult != null)
            {
                result.Collection = searchResult.ToShopifyModel(workContext);

                if (searchResult.Categories != null)
                {
                    result.Collections = new Collections(searchResult.Categories.Select(x => x.ToShopifyModel(workContext)));
                }
            }

            if (workContext.CurrentLinkLists != null)
            {
                result.Linklists = new Linklists(workContext.CurrentLinkLists.Select(x => x.ToShopifyModel()));
            }

            if (workContext.Order != null)
            {
                result.Order = workContext.Order.ToShopifyModel(urlBuilder);
            }

            result.PaymentFormHtml = workContext.PaymentFormHtml;

            if(workContext.CurrentPage != null)
            {
                result.Page = workContext.CurrentPage.ToShopifyModel();
            }

            if(workContext.CurrentBlog != null)
            {
                result.Blog = workContext.CurrentBlog.ToShopifyModel();
            }

            if(workContext.CurrentBlogArticle != null)
            {
                result.Article = workContext.CurrentBlogArticle.ToShopifyModel();
            }

            if (workContext.ContactUsForm != null)
            {
                result.Form = workContext.ContactUsForm.ToShopifyModel();
            }

            if (workContext.Login != null)
            {
                result.Form = workContext.Login.ToShopifyModel();
            }

            return result;
        }
    }
}
