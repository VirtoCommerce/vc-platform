#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
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
        private readonly ITemplateParser _parser = null;
        #region Constructors and Destructors
        public DotLiquidView(ControllerContext controllerContext, IViewLocator locator, ITemplateParser parser, ViewLocationResult partialPath)
            : this(controllerContext, locator, parser, partialPath, null)
        {
        }

        public DotLiquidView(ControllerContext controllerContext, IViewLocator locator, ITemplateParser parser, ViewLocationResult viewResult, ViewLocationResult masterViewResult)
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
            _parser = parser;
            this.ViewResult = viewResult;
            this.MasterViewResult = masterViewResult;
        }
        #endregion

        #region Public Properties
        public ViewLocationResult MasterViewResult { get; protected set; }

        public ViewLocationResult ViewResult { get; protected set; }
        #endregion

        #region Public Methods and Operators
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

            var template = _parser.Parse(this.ViewResult);

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
                var masterTemplate = _parser.Parse(layoutPath);
                var headerTemplate = _parser.Parse(_locator.LocatePartialView("content_header"));
                var renderedHeaderContents = headerTemplate.RenderWithTracing(renderParams);

                renderParams.LocalVariables.Add("content_for_layout", renderedContents);
                renderParams.LocalVariables.Add("content_for_header", renderedHeaderContents);
                masterTemplate.RenderWithTracing(writer, renderParams);
            }
        }
        #endregion
    }
}