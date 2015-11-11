using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Binders
{
    public class ShopifyModelBinderProvider : IModelBinderProvider
    {
        private static Dictionary<Type, IModelBinder> _binders = new Dictionary<Type, IModelBinder>
        {
            { typeof(Login), new LoginModelBinder() },
            { typeof(Register), new RegisterModelBinder() },
        };

        public IModelBinder GetBinder(Type modelType)
        {
            if (_binders.ContainsKey(modelType))
                return _binders[modelType];

            return null;
        }
    }
}
