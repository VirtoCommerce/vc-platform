using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using IActionSettingsSerialiser = VirtoCommerce.Web.Client.Caching.Interfaces.IActionSettingsSerialiser;
using IDonutHoleFiller = VirtoCommerce.Web.Client.Caching.Interfaces.IDonutHoleFiller;

namespace VirtoCommerce.Web.Client.Caching
{
    public class DonutHoleFiller : IDonutHoleFiller
    {
        private static readonly Regex DonutHoles = new Regex("<!--Donut#(.*?)#-->(.*?)<!--EndDonut-->", RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly IActionSettingsSerialiser _actionSettingsSerialiser;

        public DonutHoleFiller(IActionSettingsSerialiser actionSettingsSerialiser)
        {
            if (actionSettingsSerialiser == null)
            {
                throw new ArgumentNullException("actionSettingsSerialiser");
            }

            _actionSettingsSerialiser = actionSettingsSerialiser;
        }

        public string RemoveDonutHoleWrappers(string content, ControllerContext filterContext, OutputCacheOptions options)
        {
            //if (
            //    filterContext.IsChildAction &&
            //    (options & OutputCacheOptions.ReplaceDonutsInChildActions) != OutputCacheOptions.ReplaceDonutsInChildActions)
            //{
            //    return content;
            //}

            return DonutHoles.Replace(content, match => match.Groups[2].Value);
        }

        public string ReplaceDonutHoleContent(string content, ControllerContext filterContext, OutputCacheOptions options)
        {
            //if (
            //    filterContext.IsChildAction &&
            //    (options & OutputCacheOptions.ReplaceDonutsInChildActions) != OutputCacheOptions.ReplaceDonutsInChildActions)
            //{
            //    return content;
            //}

            return DonutHoles.Replace(content, match =>
            {
                var actionSettings = _actionSettingsSerialiser.Deserialise(match.Groups[1].Value);

                return InvokeAction(
                    filterContext.Controller,
                    actionSettings.ActionName,
                    actionSettings.ControllerName,
                    actionSettings.RouteValues
                );
            });
        }

        private static string InvokeAction(ControllerBase controller, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            var viewContext = new ViewContext(
                controller.ControllerContext,
                new WebFormView(controller.ControllerContext, "tmp"),
                controller.ViewData,
                controller.TempData,
                TextWriter.Null
            );

            var htmlHelper = new HtmlHelper(viewContext, new ViewPage());

            return htmlHelper.Action(actionName, controllerName, routeValues).ToString();
        }
    }
}