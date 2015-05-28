#region
using System.Web.Mvc;

#endregion

namespace VirtoCommerce.Web.Models.Helpers
{
    public class ExtensionPartialView : PartialViewResult
    {
        #region Constructors and Destructors
        public ExtensionPartialView(string viewName, object model)
        {
            this.ViewData = new ViewDataDictionary(model);
            this.ViewName = viewName;
        }
        #endregion

        #region Public Methods and Operators
        public override void ExecuteResult(ControllerContext context)
        {
            var filePath = this.ViewName;
            var extIndex = filePath.LastIndexOf('.');
            if (extIndex > -1)
            {
                var fileExt = filePath.Substring(extIndex);
                if (!string.IsNullOrEmpty(fileExt) && ExtensionMapper.Contains(fileExt))
                {
                    context.HttpContext.Response.ContentType = ExtensionMapper.GetContentType(fileExt);
                }
            }

            base.ExecuteResult(context);
        }
        #endregion
    }
}