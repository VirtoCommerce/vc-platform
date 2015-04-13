using System;
using System.Linq;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.FileSystems;

namespace VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.FileSystems
{
    public class ThemeFileSystem : IFileSystem
    {
        private readonly IViewLocator _locater;

        public ThemeFileSystem(IViewLocator locater)
        {
            this._locater = locater;
        }

        #region Implementation of IFileSystem

        public string ReadTemplateFile(Context context, string templateName)
        {
            var templatePath = (string)context[templateName];

            if (templatePath == null || !Regex.IsMatch(templatePath, @"[a-zA-Z0-9]+$"))
            {
                throw new FileSystemException("Error - Illegal template name '{0}'", templatePath);
            }

            var foundView = this._locater.LocatePartialView(templatePath);

            if (foundView.Contents == null)
                return String.Format("not found {0}", templateName);

            return foundView.Contents.Invoke().ReadToEnd();
        }

        #endregion
    }
}