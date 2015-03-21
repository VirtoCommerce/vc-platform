/* Copyright (C) 2012 by Matt Brailsford

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#region
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.FileSystems;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Util;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.FileSystems
{
    public abstract class VirtualPathProviderFileSystem : IFileSystem
    {
        #region Fields
        protected IEnumerable<string> _viewLocations;
        #endregion

        #region Constructors and Destructors
        protected VirtualPathProviderFileSystem(IEnumerable<string> viewLocations)
        {
            this._viewLocations = viewLocations;
        }
        #endregion

        #region Public Properties
        public abstract string FileNameFormat { get; }
        #endregion

        #region Public Methods and Operators
        public string ReadTemplateFile(Context context, string templateName)
        {
            var templatePath = (string)context[templateName];

            if (templatePath == null || !Regex.IsMatch(templatePath, @"[a-zA-Z0-9]+$"))
            {
                throw new FileSystemException("Error - Illegal template name '{0}'", templatePath);
            }

            var checkedLocations = new List<string>();
            var viewPath = string.Empty;
            var viewFound = false;

            foreach (var fullPath in
                this._viewLocations.Select(
                    viewLocation => Path.Combine(viewLocation, string.Format(this.FileNameFormat, templatePath))))
            {
                if (HostingEnvironment.VirtualPathProvider.FileExists(fullPath))
                {
                    viewPath = fullPath;
                    viewFound = true;
                    break;
                }

                checkedLocations.Add(fullPath);
            }

            if (!viewFound)
            {
                throw new FileSystemException(
                    "Error - No such template. Looked in the following locations:<br />{0}",
                    string.Join("<br />", checkedLocations));
            }

            return VirtualPathProviderHelper.Load(viewPath);
        }
        #endregion
    }
}