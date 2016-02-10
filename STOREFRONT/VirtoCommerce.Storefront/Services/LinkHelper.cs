using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VirtoCommerce.Storefront.Model.StaticContent;

namespace VirtoCommerce.Storefront.Services
{
    public sealed class LinkHelper
    {
        private static readonly Regex TimestampAndTitleFromPathRegex = new Regex(
           string.Format(@"{0}(?:(?<timestamp>\d+-\d+-\d+)-)?(?<title>[^{0}]*)\.[^\.]+$",
               Regex.Escape(Path.DirectorySeparatorChar.ToString())),
           RegexOptions.Compiled);

        private static readonly Regex TimestampAndTitleAndLanguageFromPathRegex = new Regex(
   string.Format(@"{0}(?:(?<timestamp>\d+-\d+-\d+)-)?(?<title>[^{0}]*)\.(?<language>[A-z]{{2}}-[A-z]{{2}})\.[^\.]+$",
       Regex.Escape(Path.DirectorySeparatorChar.ToString())),
   RegexOptions.Compiled);


        private static readonly Regex CategoryRegex = new Regex(@":category(\d*)", RegexOptions.Compiled);

        private static readonly Regex SlashesRegex = new Regex(@"/{1,}", RegexOptions.Compiled);

        private static readonly string[] HtmlExtensions = new[] { ".markdown", ".mdown", ".mkdn", ".mkd", ".md", ".textile", ".cshtml" };

        private static readonly Dictionary<string, string> BuiltInPermalinks = new Dictionary<string, string>
        {
            { "date", ":folder/:categories/:year/:month/:day/:title" },
            { "pretty", ":folder/:categories/:year/:month/:day/:title/" },
            { "ordinal", ":folder/:categories/:year/:y_day/:title" },
            { "none", ":folder/:categories/:title" },
        };

        // http://jekyllrb.com/docs/permalinks/
        public string EvaluatePermalink(string permalink, ContentItem page)
        {
            if (BuiltInPermalinks.ContainsKey(permalink))
            {
                permalink = BuiltInPermalinks[permalink];
            }

            var removeLeadingSlash = true;
            if (permalink.StartsWith("/"))
                removeLeadingSlash = false;

                var date = page.PublishedDate ?? page.CreatedDate;

            permalink = permalink.Replace(":folder", Path.GetDirectoryName(page.RelativePath).Replace("\\", "/"));

            if (!String.IsNullOrEmpty(page.Category))
                permalink = permalink.Replace(":categories", page.Category);
            else
                permalink = permalink.Replace(":categories", string.Join("/", page.Categories.ToArray()));

            permalink = permalink.Replace(":dashcategories", string.Join("-", page.Categories.ToArray()));
            permalink = permalink.Replace(":year", date.Year.ToString(CultureInfo.InvariantCulture));
            permalink = permalink.Replace(":month", date.ToString("MM"));
            permalink = permalink.Replace(":day", date.ToString("dd"));
            permalink = permalink.Replace(":title", GetTitle(page.LocalPath));
            permalink = permalink.Replace(":y_day", date.DayOfYear.ToString("000"));
            permalink = permalink.Replace(":short_year", date.ToString("yy"));
            permalink = permalink.Replace(":i_month", date.Month.ToString());
            permalink = permalink.Replace(":i_day", date.Day.ToString());

            if (permalink.Contains(":category"))
            {
                var matches = CategoryRegex.Matches(permalink);
                if (matches != null && matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        var replacementValue = string.Empty;
                        int categoryIndex;
                        if (match.Success)
                        {
                            if (int.TryParse(match.Groups[1].Value, out categoryIndex) && categoryIndex > 0)
                            {
                                replacementValue = page.Categories.Skip(categoryIndex - 1).FirstOrDefault();
                            }
                            else if (page.Categories.Any())
                            {
                                replacementValue = page.Categories.First();
                            }
                        }

                        permalink = permalink.Replace(match.Value, replacementValue);
                    }
                }
            }

            permalink = SlashesRegex.Replace(permalink, "/");

            if(removeLeadingSlash)
                permalink = permalink.TrimStart('/');

            return permalink;
        }

        public string GetTitle(string file)
        {
            // try extracting title when language is specified, if null or empty continue without a language
            var title = TimestampAndTitleAndLanguageFromPathRegex.Match(file).Groups["title"].Value;

            if (string.IsNullOrEmpty(title))
                title = TimestampAndTitleFromPathRegex.Match(file).Groups["title"].Value;

            return title;
        }

        private string GetPageTitle(string file)
        {
            return Path.GetFileNameWithoutExtension(file);
        }
    }
}