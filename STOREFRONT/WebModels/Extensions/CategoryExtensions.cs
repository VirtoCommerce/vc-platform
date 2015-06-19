#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Web.Models;

#endregion

namespace VirtoCommerce.Web.Extensions
{
    public static class CategoryExtensions
    {
        #region Public Methods and Operators
        /// <summary>
        ///     Builds the outline for category.
        ///     Key - categoryId
        ///     Value - semantic url
        /// </summary>
        /// <param name="collection">The category.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static Dictionary<string, string> BuildOutline(this Collection collection, string language = null)
        {
            var segments = new Dictionary<string, string>();

            //first add parents
            if (collection.Parents != null)
            {
                foreach (var parent in collection.Parents)
                {
                    SeoKeyword keyword = null;
                    if (parent.Keywords != null)
                    {
                        keyword = parent.Keywords.SeoKeyword(language);
                    }

                    segments.Add(parent.Id, keyword != null ? keyword.Keyword : parent.Handle);
                }
            }

            //Finally add category itself
            {
                var keyword = collection.Keywords != null ? collection.Keywords.SeoKeyword(language) : null;
                segments.Add(collection.Id, keyword != null ? keyword.Keyword : collection.Handle);
            }

            return segments;
        }

        public static string BuildSearchOutline(this Collection collection, string sep = "/")
        {
            var segments = new List<string>();

            //first add parents
            if (collection.Parents != null)
            {
                segments.AddRange(collection.Parents.Select(parent => parent.Id));
            }

            //Finally add category itself
            segments.Add(collection.Id);

            return String.Join(sep, segments);
        }

        public static Dictionary<string, string> BuildTitleOutline(this Collection collection)
        {
            var segments = new Dictionary<string, string>();

            //first add parents
            if (collection.Parents != null)
            {
                foreach (var parent in collection.Parents)
                {
                    segments.Add(parent.Id, parent.Title);
                }
            }

            //Finally add category itself
            {
                segments.Add(collection.Id, collection.Title);
            }

            return segments;
        }
        #endregion
    }
}