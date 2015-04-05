﻿using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Assets.Services;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using googleModel = Google.Apis.ShoppingContent.v2.Data;

namespace GoogleShopping.MerchantModule.Web.Converters
{
    public static class ProductConverter
    {
        public static googleModel.Product ToGoogleModel(this moduleModel.CatalogProduct product, IAssetUrlResolver assetUrlResolver, moduleModel.Property[] properties = null)
        {
            var retVal = new googleModel.Product();
            retVal.InjectFrom(product);
            var langCode = product.Catalog.Languages.First().LanguageCode;

            retVal.Link = @"http://virtocommerce-test.azurewebsites.net/";

            retVal.OfferId = product.Id;
            retVal.Title = product.Name;
            retVal.Description = product.Reviews.Any() ? product.Reviews.First(x => x.LanguageCode == langCode).Content : product.Name;
            retVal.Link = @"http://virtocommerce-test.azurewebsites.net/";
            retVal.ImageLink = assetUrlResolver.GetAbsoluteUrl(product.Assets.First().Url).TrimStart('/');
            retVal.ContentLanguage = langCode;
            retVal.TargetCountry = "US";
            retVal.Channel = "online";
            retVal.Availability = "in stock";
            retVal.Condition = "new";
            retVal.GoogleProductCategory = "Media > Books";
            retVal.Gtin = "9780007350896";

            return retVal;
        }

        public static moduleModel.CatalogProduct ToModuleModel(this googleModel.Product product, IAssetUrlResolver assetUrlResolver)
        {
            var retVal = new moduleModel.CatalogProduct();
            retVal.InjectFrom(product);

            retVal.StartDate = DateTime.UtcNow;
            
            return retVal;
        }
    }
}