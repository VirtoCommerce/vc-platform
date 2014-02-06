using System.Text.RegularExpressions;
using System.Web.Optimization;

namespace VirtoCommerce.Web.Virto.Helpers.MVC
{
    public class CssMinifyFix : CssMinify
    {
        public override void Process(BundleContext context, BundleResponse response)
        {
            base.Process(context,response);

            //Google chrome does not recognise media queries without spaces netween and
            //http://i-skool.co.uk/net/mvc/mvc-bundling-and-minifcation-issues-with-google-chrome/
            response.Content = Regex.Replace(response.Content, "(\\)and( )?\\()", ") and (");
        }
    }
}