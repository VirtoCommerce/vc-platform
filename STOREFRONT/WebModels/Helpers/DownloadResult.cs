#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

#endregion

namespace VirtoCommerce.Web.Models.Helpers
{
    /// <summary>
    ///     Class DownloadResult.
    /// </summary>
    public class DownloadResult : ActionResult
    {
        #region Constructors and Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DownloadResult" /> class.
        /// </summary>
        public DownloadResult()
        {
            this.FallbackPaths = new Dictionary<string, string>
                                 {
                                     {
                                         "image/jpeg",
                                         "~/Content/themes/default/images/blank.png"
                                     },
                                     {
                                         "image/png",
                                         "~/Content/themes/default/images/blank.png"
                                     },
                                     {
                                         "image/bmp",
                                         "~/Content/themes/default/images/blank.png"
                                     },
                                     {
                                         "image/gif",
                                         "~/Content/themes/default/images/blank.png"
                                     },
                                 };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DownloadResult" /> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        public DownloadResult(string virtualPath)
            : this()
        {
            this.VirtualPath = virtualPath;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }

        /// <summary>
        ///     Gets the fallback paths.
        /// </summary>
        /// <value>The fallback paths.</value>
        public Dictionary<string, string> FallbackPaths { get; private set; }

        /// <summary>
        ///     Gets or sets the name of the file download.
        /// </summary>
        /// <value>The name of the file download.</value>
        public string FileDownloadName { get; set; }

        /// <summary>
        ///     Gets or sets the virtual base path.
        /// </summary>
        /// <value>The virtual base path.</value>
        public string VirtualBasePath { get; set; }

        /// <summary>
        ///     Gets or sets the virtual path.
        /// </summary>
        /// <value>The virtual path.</value>
        public string VirtualPath { get; set; }
        #endregion

        #region Public Methods and Operators
        /// <summary>
        ///     Enables processing of the result of an action method by a custom type that inherits from the
        ///     <see cref="T:System.Web.Mvc.ActionResult" /> class.
        /// </summary>
        /// <param name="context">
        ///     The context in which the result is executed. The context information includes the controller,
        ///     HTTP content, request context, and route data.
        /// </param>
        public override void ExecuteResult(ControllerContext context)
        {
            var filePath = String.IsNullOrEmpty(this.VirtualBasePath)
                ? context.HttpContext.Request.MapPath(this.VirtualPath)
                : context.HttpContext.Request.MapPath(this.VirtualPath, this.VirtualBasePath, false);

            if (!String.IsNullOrEmpty(this.FileDownloadName))
            {
                context.HttpContext.Response.AddHeader(
                    "content-disposition",
                    "attachment; filename=" + this.FileDownloadName);
            }

            if (!String.IsNullOrEmpty(this.ContentType))
            {
                context.HttpContext.Response.ContentType = this.ContentType;
            }
            else
            {
                var extIndex = filePath.LastIndexOf('.');
                if (extIndex > -1)
                {
                    var fileExt = filePath.Substring(extIndex);
                    if (!string.IsNullOrEmpty(fileExt) && ExtensionMapper.Contains(fileExt))
                    {
                        context.HttpContext.Response.ContentType = ExtensionMapper.GetContentType(fileExt);
                    }
                }
            }

            try
            {
                context.HttpContext.Response.TransmitFile(filePath);
                context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.HttpContext.Response.Cache.SetMaxAge(TimeSpan.FromHours(1));
            }
            catch (FileNotFoundException)
            {
                if (this.FallbackPaths.ContainsKey(context.HttpContext.Response.ContentType))
                {
                    filePath =
                        context.HttpContext.Request.MapPath(
                            this.FallbackPaths[context.HttpContext.Response.ContentType]);
                    context.HttpContext.Response.TransmitFile(filePath);
                }
            }
            catch (DirectoryNotFoundException)
            {
                if (this.FallbackPaths.ContainsKey(context.HttpContext.Response.ContentType))
                {
                    filePath =
                        context.HttpContext.Request.MapPath(
                            this.FallbackPaths[context.HttpContext.Response.ContentType]);
                    context.HttpContext.Response.TransmitFile(filePath);
                }
            }
        }
        #endregion
    }
}