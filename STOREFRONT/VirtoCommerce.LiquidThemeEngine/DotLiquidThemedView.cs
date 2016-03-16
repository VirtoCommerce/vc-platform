using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DotLiquid;
using VirtoCommerce.LiquidThemeEngine.Converters;
using VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine
{
    public class DotLiquidThemedView : IView
    {
        private readonly ShopifyLiquidThemeEngine _themeAdaptor;
        private readonly string _viewName;
        private readonly string _masterViewName;

        public DotLiquidThemedView(ShopifyLiquidThemeEngine themeAdaptor, string viewName, string masterViewName)
        {
            if (themeAdaptor == null)
                throw new ArgumentNullException("themeAdaptor");

            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException("viewName");

            _themeAdaptor = themeAdaptor;
            _viewName = viewName;
            _masterViewName = masterViewName;
        }

        #region IView members
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            if (viewContext == null)
                throw new ArgumentNullException("viewContext");

            var shopifyContext = _themeAdaptor.WorkContext.ToShopifyModel(_themeAdaptor.UrlBuilder);
            //Set current template
            shopifyContext.Template = _viewName;

            var formErrors = new FormErrors(viewContext.ViewData.ModelState);
            if (shopifyContext.Form == null)
            {
                //Set single Form object with errors for shopify compilance
                shopifyContext.Form = new Form();
                shopifyContext.Form.PostedSuccessfully = !String.Equals(viewContext.HttpContext.Request.HttpMethod, "GET", StringComparison.InvariantCultureIgnoreCase);
                if (formErrors.Any())
                {
                    shopifyContext.Form.Errors = formErrors;
                    shopifyContext.Form.PostedSuccessfully = false;
                }
            }

            // Copy data from the view context over to DotLiquid
            var parameters = shopifyContext.ToLiquid() as Dictionary<string, object>;

            //Add settings to context
            parameters.Add("settings", _themeAdaptor.GetSettings());

            foreach (var item in viewContext.ViewData)
            {
                parameters.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);
            }
            foreach (var item in viewContext.TempData)
            {
                parameters.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);
            }

            var viewTemplate = _themeAdaptor.RenderTemplateByName(_viewName, parameters);

            // don't use layouts for partial views when masterViewName is not specified
            if (_masterViewName != null)
            {
                var masterViewName = _masterViewName;
                object layoutFromTemplate;
                if (parameters.TryGetValue("layout", out layoutFromTemplate))
                {
                    masterViewName = layoutFromTemplate.ToString();
                }
                //if layout specified need render with master page
                if (!String.IsNullOrEmpty(masterViewName))
                {
                    var headerTemplate = _themeAdaptor.RenderTemplateByName("content_header", parameters);

                    //add special placeholder 'content_for_layout' to content it will be replaced in master page by main content
                    parameters.Add("content_for_layout", viewTemplate);
                    parameters.Add("content_for_header", headerTemplate);
                    viewTemplate = _themeAdaptor.RenderTemplateByName(masterViewName, parameters);
                }
            }

            writer.Write(viewTemplate);

        }
        #endregion

    }
}
