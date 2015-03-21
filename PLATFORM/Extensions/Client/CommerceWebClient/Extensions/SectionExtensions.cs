using System;
using System.Web.WebPages;

namespace VirtoCommerce.Web.Client.Extensions
{
    public static class SectionExtensions
    {
        private static readonly object O = new object();

        public static HelperResult RenderSection(this WebPageBase page, string sectionName, Func<object, HelperResult> defaultContent)
        {
            return page.IsSectionDefined(sectionName) ? page.RenderSection(sectionName) : defaultContent(O);
        }


        public static HelperResult RedefineSection(this WebPageBase page, string sectionName, Func<object, HelperResult> defaultContent = null)
        {
            if (page.IsSectionDefined(sectionName))
            {
                page.DefineSection(sectionName, () => page.Write(page.RenderSection(sectionName)));
            }
            else if (defaultContent != null)
            {
                page.DefineSection(sectionName, () => page.Write(defaultContent(O)));
            }
            return new HelperResult(_ => { });
        }
    }
}
