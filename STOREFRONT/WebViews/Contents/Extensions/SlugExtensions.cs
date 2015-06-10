using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VirtoCommerce.Web.Views.Contents.Extensions
{
    public static class SlugExtensions
    {
        private static readonly SortedList<int, Func<string, ContentItem, string>> UrlFormatParser = new SortedList
            <int, Func<string, ContentItem, string>>
        {
            {0, DayFull},
            {1, DayAbbreviated},
            {2, Day},
            {3, MonthFull},
            {4, MonthAbbreviated},
            {5, Month},
            {6, YearFull},
            {7, Year},
            {8, Slug},
            {9, Category},
            {10, Author}
        };

        public static string ToUrlSlug(this string value)
        {
            //First to lower case
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Replace ampersand
            value = value.Replace("&", "and");

            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_\.]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        public static string AppendSlashIfNecessary(this string url)
        {
            if (url.IsFileUrl()) // URL is to a file
            {
                return url;
            }
            else // URL is to a directory
            {
                return url + "/";
            }
        }

        public static bool IsFileUrl(this string Url)
        {
            return Url.Contains('/') && Url.Split('/').Last().Contains('.');
        }

        public static void SetPostUrl(this IEnumerable<ContentItem> posts, SiteStaticContentContext settings)
        {
            foreach (var postHeader in posts)
            {
                var urlFormat = "/" + postHeader.Url.Trim('/');
                if (postHeader.Published == Published.Draft)
                {
                    urlFormat = "/drafts" + urlFormat;
                }

                postHeader.Url = "/" + urlFormat.TrimStart('/');
            }
        }

        private static string DayFull(string url, ContentItem post)
        {
            return url.Replace("dddd", post.Date.ToString("dddd"));
        }

        private static string DayAbbreviated(string url, ContentItem post)
        {
            return url.Replace("ddd", post.Date.ToString("ddd"));
        }

        private static string Day(string url, ContentItem post)
        {
            return url.Replace("dd", post.Date.ToString("dd"));
        }

        private static string Month(string url, ContentItem post)
        {
            return url.Replace("MM", post.Date.ToString("MM"));
        }

        private static string MonthAbbreviated(string url, ContentItem post)
        {
            return url.Replace("MMM", post.Date.ToString("MMM"));
        }

        private static string MonthFull(string url, ContentItem post)
        {
            return url.Replace("MMMM", post.Date.ToString("MMMM"));
        }

        private static string YearFull(string url, ContentItem post)
        {
            return url.Replace("yyyy", post.Date.ToString("yyyy"));
        }

        private static string Year(string url, ContentItem post)
        {
            return url.Replace("yy", post.Date.ToString("yy"));
        }

        private static string Slug(string url, ContentItem post)
        {
            return url.Replace("{slug}", post.Url).Replace("slug", post.Url);
        }

        private static string Category(string url, ContentItem post)
        {
            return url.Replace("{category}", post.Categories.FirstOrDefault());
        }

        private static string Author(string url, ContentItem post)
        {
            return url.Replace("{author}", post.Author);
        }
    }
}
