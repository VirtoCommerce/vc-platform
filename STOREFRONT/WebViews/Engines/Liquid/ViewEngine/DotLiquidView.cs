#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using DotLiquid;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Util;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine
{
    public class DotLiquidView : IView
    {
        private readonly IViewLocator _locator = null;
        #region Constructors and Destructors
        public DotLiquidView(ControllerContext controllerContext, IViewLocator locator, ViewLocationResult partialPath)
            : this(controllerContext, locator, partialPath, null)
        {
        }

        public DotLiquidView(ControllerContext controllerContext, IViewLocator locator, ViewLocationResult viewResult, ViewLocationResult masterViewResult)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (viewResult == null)
            {
                throw new ArgumentNullException("viewPath");
            }

            _locator = locator;
            this.ViewResult = viewResult;
            this.MasterViewResult = masterViewResult;
        }
        #endregion

        #region Public Properties
        public ViewLocationResult MasterViewResult { get; protected set; }

        public ViewLocationResult ViewResult { get; protected set; }
        #endregion

        #region Public Methods and Operators
        public static Template GetTemplateFromFile(ViewLocationResult path)
        {
            var contextKey = "vc-cms-file-" + path.Location;
            var value = HttpRuntime.Cache.Get(contextKey);

            if (value != null)
            {
                return value as Template;
            }

            if (path.Contents == null)
                return null;

            var contents = path.Contents.Invoke().ReadToEnd();
            var template = Template.Parse(contents);

            HttpRuntime.Cache.Insert(contextKey, template, new CacheDependency(new[] { path.Location}));

            return template;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            // Copy data from the view context over to DotLiquid
            var localVars = new Hash();

            if (viewContext.ViewData.Model != null)
            {
                var model = viewContext.ViewData.Model;
                if (model is ILiquidizable)
                {
                    model = ((ILiquidizable)model).ToLiquid();
                }

                if (model is Hash)
                {
                    localVars = model as Hash;
                }
                else if (model is IDictionary<string, object>)
                {
                    var modelDictionary = model as IDictionary<string, object>;
                    foreach (var item in modelDictionary.Keys)
                    {
                        localVars.Add(item, modelDictionary[item]);
                    }
                }
            }

            foreach (var item in viewContext.ViewData)
            {
                localVars.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);
            }

            foreach (var item in viewContext.TempData)
            {
                localVars.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);
            }

            var renderParams = new RenderParameters { LocalVariables = Hash.FromDictionary(localVars) };

            //var fileContents = rawContentItem.Content;
            var template = GetTemplateFromFile(this.ViewResult);

            if (this.MasterViewResult == null)
            {
                template.RenderWithTracing(writer, renderParams);
            }
            else // add master
            {
                var renderedContents = template.RenderWithTracing(renderParams);

                // read layout from context
                var layout = template.Registers["layout"].ToNullOrString();

                var layoutPath = layout == null
                    ? this.MasterViewResult
                    : _locator.LocateView(layout);

                // render master with contents
                var masterTemplate = GetTemplateFromFile(layoutPath);
                var headerTemplate = GetTemplateFromFile(_locator.LocatePartialView("content_header"));
                var renderedHeaderContents = headerTemplate.RenderWithTracing(renderParams);

                renderParams.LocalVariables.Add("content_for_layout", renderedContents);
                renderParams.LocalVariables.Add("content_for_header", renderedHeaderContents);
                masterTemplate.RenderWithTracing(writer, renderParams);
            }
        }
        #endregion
    }
}