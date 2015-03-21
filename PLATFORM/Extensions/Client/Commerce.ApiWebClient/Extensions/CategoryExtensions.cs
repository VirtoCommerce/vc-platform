using System.Collections.Generic;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiWebClient.Extensions
{
    public static class CategoryExtensions
    {


        /// <summary>
        /// Builds the outline for category.
        /// Key - categoryId
        /// Value - semantic url
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static Dictionary<string,string> BuildOutline(this Category category, string language = null)
        {
            var segments = new Dictionary<string,string>();

            //first add parents
            if (category.Parents != null)
            {
                foreach (var parent in category.Parents)
                {
                    SeoKeyword keyword = null;
                    if (parent.SeoKeywords != null)
                    {
                        keyword = parent.SeoKeywords.SeoKeyword(language);         
                    }

                    segments.Add(parent.Id, keyword != null ? keyword.Keyword : parent.Id);
                }
            }

            //Finally add category itself
            {
                var keyword = category.SeoKeywords != null ? category.SeoKeywords.SeoKeyword(language) : null;
                segments.Add(category.Id,keyword != null ? keyword.Keyword : category.Id);
            }

            
            return segments;
        }

        public static Dictionary<string, string> BuildTitleOutline(this Category category)
        {
            var segments = new Dictionary<string, string>();

            //first add parents
            if (category.Parents != null)
            {
                foreach (var parent in category.Parents)
                {
                    segments.Add(parent.Id, parent.Name);
                }
            }

            //Finally add category itself
            {
                segments.Add(category.Id, category.Name);
            }


            return segments;
        }
    }
}
