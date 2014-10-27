using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VirtoCommerce.Web.Client.Extensions
{
    public class ViewRenderer<T> where T : Controller, new()
    {

        public ControllerContext Context;
        public ViewRenderer(ControllerContext controllerContext = null)
        {
            // Create a known controller from HttpContext if no context is passed
            if (controllerContext == null)
            {
                if (HttpContext.Current != null)
                    controllerContext = CreateController().ControllerContext;
                else
                    throw new InvalidOperationException(
                        "ViewRenderer must run in the context of an ASP.NET " +
                        "Application and requires HttpContext.Current to be present.");
            }
            Context = controllerContext;
        }

        public string RenderTemplate(string template, object model)
        {
            var guid = Guid.NewGuid();
            var path = "~/Views/Shared/" + guid + ".cshtml";
            var fullPath = Context.HttpContext.Server.MapPath(path);

            try
            {
                using (var fs = File.Create(fullPath))
                {
                    using (var txtWriter = new StreamWriter(fs))
                    {
                        txtWriter.WriteLine(template);
                    }
                }

                return RenderViewToStringInternal(path, model);
            }
            catch
            {
                return template;
            }
            finally
            {
                File.Delete(fullPath);
            }
        }

        protected string RenderViewToStringInternal(string viewPath, object model, bool partial = true)
        {
            // first find the ViewEngine for this view
            var viewEngineResult = partial ? ViewEngines.Engines.FindPartialView(Context, viewPath) : 
                ViewEngines.Engines.FindView(Context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException();

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            Context.Controller.ViewData.Model = model;

            string result;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(Context, view,
                                            Context.Controller.ViewData,
                                            Context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }

        /// <summary>
        /// Creates an instance of an MVC controller from scratch 
        /// when no existing ControllerContext is present       
        /// </summary>
        /// <typeparam name="T">Type of the controller to create</typeparam>
        /// <returns>Controller Context for T</returns>
        /// <exception cref="InvalidOperationException">thrown if HttpContext not available</exception>
        private static T CreateController(RouteData routeData = null)
        {
            // create a disconnected controller instance
            var controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper;
            if (HttpContext.Current != null)
            {
                wrapper = new HttpContextWrapper(HttpContext.Current);
            }
            else
            {
                throw new InvalidOperationException(
                    "Can't create Controller Context if no active HttpContext instance is available.");
            }

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller", controller.GetType().Name
                                                            .ToLower()
                                                            .Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }
    }
}
