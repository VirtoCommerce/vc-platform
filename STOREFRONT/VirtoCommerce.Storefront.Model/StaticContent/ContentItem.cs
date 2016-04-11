using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.StaticContent
{
    public abstract class ContentItem : IHasLanguage
    {
        private static readonly Regex _timestampAndTitleFromPathRegex = new Regex(string.Format(@"{0}(?:(?<timestamp>\d+-\d+-\d+)-)?(?<title>[^{0}]*)\.[^\.]+$", Regex.Escape("/")), RegexOptions.Compiled);
        private static readonly Regex _timestampAndTitleAndLanguageFromPathRegex = new Regex(string.Format(@"{0}(?:(?<timestamp>\d+-\d+-\d+)-)?(?<title>[^{0}]*)\.(?<language>[A-z]{{2}}-[A-z]{{2}})\.[^\.]+$", Regex.Escape("/")), RegexOptions.Compiled);
        private static readonly Regex _categoryRegex = new Regex(@":category(\d*)", RegexOptions.Compiled);
        private static readonly Regex _slashesRegex = new Regex(@"/{1,}", RegexOptions.Compiled);
        private static readonly string[] _htmlExtensions = new[] { ".markdown", ".mdown", ".mkdn", ".mkd", ".md", ".textile", ".cshtml" };

        private static readonly Dictionary<string, string> _builtInPermalinks = new Dictionary<string, string>
        {
            { "date", ":folder/:categories/:year/:month/:day/:title" },
            { "pretty", ":folder/:categories/:year/:month/:day/:title/" },
            { "ordinal", ":folder/:categories/:year/:y_day/:title" },
            { "none", ":folder/:categories/:title" },
        };

        public ContentItem()
        {
            Tags = new List<string>();
            Categories = new List<string>();
        }

        public virtual string Type { get { return "page"; } }

        public string Author { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// Relative content url
        /// </summary>
        public string Url { get; set; }

        public string Permalink { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Categories { get; set; }

        public string Category { get; set; }

        public ContentPublicationStatus PublicationStatus { get; set; }

        /// <summary>
        /// Content file name without extension
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Relative storage path in storage system (/blogs/page1)
        /// </summary>
        public string StoragePath { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// Liquid layout from store theme used as master page for page rendering. If its null will be used default layout.
        /// </summary>
        public string Layout { get; set; }

        public string FileName { get; set; }


        #region IHasLanguage Members
        public Language Language { get; set; }
        #endregion

        public virtual void LoadContent(string content, IDictionary<string, IEnumerable<string>> metaInfoMap, IDictionary themeSettings)
        {
            if (metaInfoMap != null)
            {
                foreach (var setting in metaInfoMap)
                {
                    var settingValue = setting.Value.FirstOrDefault();
                    switch (setting.Key.ToLower())
                    {
                        case "permalink":
                            this.Permalink = settingValue;
                            break;

                        case "title":
                            this.Title = settingValue;
                            break;

                        case "author":
                            this.Author = settingValue;
                            break;

                        case "published":
                            this.PublicationStatus = EnumUtility.SafeParse<ContentPublicationStatus>(settingValue, ContentPublicationStatus.Draft);
                            break;

                        case "tags":
                            this.Tags = setting.Value.ToList();
                            break;

                        case "categories":
                            this.Categories = setting.Value.ToList();
                            break;

                        case "category":
                            this.Category = settingValue;
                            break;


                        case "layout":
                            this.Layout = settingValue;
                            break;
                    }
                }
            }

            //Try to get permalink template from theme settings
            if (string.IsNullOrEmpty(Permalink) && themeSettings != null && themeSettings.Contains("permalink"))
            {
                Permalink = themeSettings["permalink"] as string;
            }

            if (string.IsNullOrEmpty(Permalink))
            {
                Permalink = "none";
            }

            Url = EvaluateUrlFromPermalink(Permalink);

            Content = content;
            if (Title == null)
            {
                Title = Name;
            }
        }


        // http://jekyllrb.com/docs/permalinks/
        protected virtual string EvaluateUrlFromPermalink(string permalink)
        {
            if (_builtInPermalinks.ContainsKey(permalink))
            {
                permalink = _builtInPermalinks[permalink];
            }

            var removeLeadingSlash = true;
            if (permalink.StartsWith("/"))
                removeLeadingSlash = false;

            var date = PublishedDate ?? CreatedDate;

            permalink = permalink.Replace(":folder", Path.GetDirectoryName(StoragePath).Replace("\\", "/"));

            if (!String.IsNullOrEmpty(Category))
                permalink = permalink.Replace(":categories", Category);
            else
                permalink = permalink.Replace(":categories", string.Join("/", Categories.ToArray()));

            permalink = permalink.Replace(":dashcategories", string.Join("-", Categories.ToArray()));
            permalink = permalink.Replace(":year", date.Year.ToString(CultureInfo.InvariantCulture));
            permalink = permalink.Replace(":month", date.ToString("MM"));
            permalink = permalink.Replace(":day", date.ToString("dd"));
            permalink = permalink.Replace(":title", GetTitle(StoragePath));
            permalink = permalink.Replace(":y_day", date.DayOfYear.ToString("000"));
            permalink = permalink.Replace(":short_year", date.ToString("yy"));
            permalink = permalink.Replace(":i_month", date.Month.ToString());
            permalink = permalink.Replace(":i_day", date.Day.ToString());

            if (permalink.Contains(":category"))
            {
                var matches = _categoryRegex.Matches(permalink);
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
                                replacementValue = Categories.Skip(categoryIndex - 1).FirstOrDefault();
                            }
                            else if (Categories.Any())
                            {
                                replacementValue = Categories.First();
                            }
                        }

                        permalink = permalink.Replace(match.Value, replacementValue);
                    }
                }
            }

            permalink = _slashesRegex.Replace(permalink, "/");

            if (removeLeadingSlash)
                permalink = permalink.TrimStart('/');

            return permalink;
        }

        private static string GetTitle(string file)
        {
            // try extracting title when language is specified, if null or empty continue without a language
            var title = _timestampAndTitleAndLanguageFromPathRegex.Match(file).Groups["title"].Value;

            if (string.IsNullOrEmpty(title))
                title = _timestampAndTitleFromPathRegex.Match(file).Groups["title"].Value;

            return title;
        }

        public override string ToString()
        {
            return Url ?? Name;
        }
    }
}
