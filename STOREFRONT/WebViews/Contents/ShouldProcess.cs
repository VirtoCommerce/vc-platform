using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Web.Views.Contents
{
    public static class ShouldProcess
    {
        public static bool Archive(ContentItem post)
        {
            return IsPublished(post) && IsInThePast(post);
        }

        public static bool Category(ContentItem post)
        {
            return IsPublished(post) && IsInThePast(post);
        }

        public static bool Posts(ContentItem post)
        {
            return IsPublished(post) && IsInThePast(post);
        }

        private static bool IsInThePast(ContentItem post)
        {
            return post.Date < DateTime.Now;
        }

        private static bool IsPublished(ContentItem post)
        {
            return post.Published == Published.True;
        }
    }
}
