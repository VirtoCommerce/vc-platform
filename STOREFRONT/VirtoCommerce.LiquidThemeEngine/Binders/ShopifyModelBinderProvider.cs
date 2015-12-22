using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using shopifyModel = VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine.Binders
{
    /// <summary>
    /// Shopify model binders convert Shopify form fields with bad names to VirtoCommerce model properties.
    /// </summary>
    public class ShopifyModelBinderProvider : IModelBinderProvider
    {
        private static readonly Dictionary<Type, IModelBinder> _binders = new Dictionary<Type, IModelBinder>
        {
            { typeof(Login), new LoginModelBinder() },
            { typeof(Register), new RegisterModelBinder() },
            { typeof(ResetPassword), new ResetPasswordModelBinder() },
            { typeof(shopifyModel.Address), new AddressModelBinder() },
        };

        public IModelBinder GetBinder(Type modelType)
        {
            if (_binders.ContainsKey(modelType))
                return _binders[modelType];

            return null;
        }
    }
}
