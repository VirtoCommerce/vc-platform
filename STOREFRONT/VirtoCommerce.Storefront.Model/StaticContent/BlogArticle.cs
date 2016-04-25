using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.StaticContent
{
    /// <summary>
    /// TODO: Comments and author user info
    /// </summary>
    public class BlogArticle : ContentItem
    {
        private static string _excerpToken = "<!--excerpt-->";

        public override string Type { get { return "post"; } }

        public string Excerpt { get; set; }

        public string BlogName { get; set; }

        public string ImageUrl { get; set; }

        public override void LoadContent(string content, IDictionary<string, IEnumerable<string>> metaInfoMap, IDictionary themeSettings)
        {
            var parts = content.Split(new[] { _excerpToken }, StringSplitOptions.None);
            if(parts.Count() > 1)
            {
                Excerpt = parts[0];
                content.Replace(_excerpToken, String.Empty);
            }
            base.LoadContent(content, metaInfoMap, themeSettings);
            //TODO: load image from meta info            
        }
    }
}
