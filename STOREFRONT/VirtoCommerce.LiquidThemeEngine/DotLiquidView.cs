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
using DotLiquid.ViewEngine.FileSystems;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine
{
    public class DotLiquidView : IView
    {
        private readonly ControllerContext _controllerContext;
        private readonly string _viewPath;
        private readonly string _masterPath;

        public DotLiquidView(ControllerContext controllerContext, string viewPath, string masterPath = null)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            if (string.IsNullOrEmpty(viewPath))
                throw new ArgumentNullException("viewPath");

            _controllerContext = controllerContext;

            _viewPath = viewPath;
            _masterPath = masterPath;
        }

        #region IView members
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            if (viewContext == null)
                throw new ArgumentNullException("viewContext");

            var themeFileSystem = Template.FileSystem as ShopifyThemeLiquidFileSystem;

            // Copy data from the view context over to DotLiquid
            var localVars = new Hash();
            var shopifyWorkContext = viewContext.ViewData.Model as WorkContext;
            if (shopifyWorkContext != null)
            {
                var settingDefaultValue = HttpContext.Current.Request.Path.Contains(".scss") ? "''" : null;
                //Add settings to context
                localVars.Add("settings", themeFileSystem.GetSettings(settingDefaultValue));
                //Todo: need convert our context to liquid context
            }

            foreach (var item in viewContext.ViewData)
                localVars.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);

            foreach (var item in viewContext.TempData)
                localVars.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);

            var renderParams = new RenderParameters
            {
                LocalVariables = Hash.FromDictionary(localVars)
            };

            if (themeFileSystem != null)
            {
                var viewContent = themeFileSystem.ReadTemplateByName(_viewPath);
                // Render requested template
                var template = Template.Parse(viewContent);

                //next need render master template if it defined manualy or in liquid view by special tag 'layout'
                var layout = (template.Registers["layout"] ?? String.Empty).ToString();
                if (String.IsNullOrEmpty(layout))
                {
                    layout = _masterPath;
                }
                //if layout specified need render with master page
                if (!String.IsNullOrEmpty(layout))
                {
                    var renderedTemplate = template.RenderWithTracing(renderParams);
                    var masterTemplate = Template.Parse(layout);
                    renderParams.LocalVariables.Add("content_for_layout", renderedTemplate);
                    masterTemplate.RenderWithTracing(writer, renderParams);
                }
                else
                {
                    template.RenderWithTracing(writer, renderParams);
                }
            }

        } 
        #endregion

    }
}
