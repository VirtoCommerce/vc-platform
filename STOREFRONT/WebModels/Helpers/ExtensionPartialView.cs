#region
using System.Web.Mvc;

#endregion

namespace VirtoCommerce.Web.Models.Helpers
{

    #region
    #endregion

    public class ExtensionPartialView : PartialViewResult
    {
        #region Constructors and Destructors
        public ExtensionPartialView(object model)
        {
            this.ViewData = new ViewDataDictionary(model);
        }
        #endregion

        #region Public Methods and Operators
        public override void ExecuteResult(ControllerContext context)
        {
            var filePath = this.ViewData.Model.ToString();
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