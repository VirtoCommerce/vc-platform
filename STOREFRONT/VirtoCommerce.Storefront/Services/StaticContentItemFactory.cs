using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.StaticContent;

namespace VirtoCommerce.Storefront.Services
{
    public static class StaticContentItemFactory
    {
        private static readonly Regex _blogMatchRegex = new Regex(@"blogs/(?<blog>[^\/]*)\/([^\/]*)\.[^\.]+$", RegexOptions.Compiled);

        public static ContentItem GetContentItemFromPath(string path)
        {
            ContentItem retVal = null;
            if (!string.IsNullOrEmpty(path))
            {
                //Blog
                var blogMatch = _blogMatchRegex.Match(path);
                if (blogMatch.Success)
                {
                    var blogName = blogMatch.Groups["blog"].Value;
                    var fileName = Path.GetFileNameWithoutExtension(path);
                    if (fileName.EqualsInvariant(blogName) || fileName.EqualsInvariant("default"))
                    {
                        retVal = new Blog()
                        {
                            Name = blogName,
                        };
                    }
                    else
                    {
                        retVal = new BlogArticle()
                        {
                            BlogName = blogName
                        };
                    }
                }
                else
                {
                    retVal = new ContentPage();
                }
            }
                
            return retVal;
        }
    }
}