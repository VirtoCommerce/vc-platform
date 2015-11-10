using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DotLiquid;
using DotLiquid.FileSystems;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model;

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
            _masterViewName = String.IsNullOrEmpty(masterViewName) ? _themeAdaptor.MasterViewName : masterViewName;
        }

        #region IView members
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            if (viewContext == null)
                throw new ArgumentNullException("viewContext");

            var shopifyContext = _themeAdaptor.WorkContext as ShopifyThemeWorkContext;

            var formErrors = new FormErrors(viewContext.ViewData.ModelState);
            //Set single Form object with errors for shopify compilance
            shopifyContext.Form = new Form();
            if(formErrors.Any())
            {
                shopifyContext.Form.Errors = formErrors;
                shopifyContext.Form.PostedSuccessfully = false;
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
                   
            //if layout specified need render with master page
            if (!String.IsNullOrEmpty(_masterViewName))
            {
                //add sepcial placeholder 'content_for_layout' to content it will be replaced in master page by main content
                parameters.Add("content_for_layout", viewTemplate);
                viewTemplate = _themeAdaptor.RenderTemplateByName(_masterViewName, parameters);
            }
            writer.Write(viewTemplate);

        }
        #endregion

    }
}
