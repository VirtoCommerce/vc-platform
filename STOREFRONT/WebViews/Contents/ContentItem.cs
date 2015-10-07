#region
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace VirtoCommerce.Web.Views.Contents
{
    public class ContentItem
    {
        //public Series Series { get; set; }

        #region Fields
        private string excerpt;
        private string content;
        #endregion

        #region Constructors and Destructors
        public ContentItem()
        {
            this.Categories = new List<string>();
            this.Tags = new List<string>();
        }
        #endregion

        #region Public Properties
        public string Author { get; set; }

        public List<string> Categories { get; set; }

        /// <summary>
        /// Contains both excerpt and body
        /// </summary>
        public string FullContent
        {
            get;
            set;
        }

        /// <summary>
        /// Only contains body without excerpt
        /// </summary>
        public string Content {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.content))
                {
                    return this.content;
                }

                if(this.FullContent.Contains("<!--excerpt-->"))
                {
                    return this.FullContent.Split(new[] { "<!--excerpt-->" }, StringSplitOptions.None)[1];
                }

                return this.FullContent;
            }
            set
            {
                this.content = value;
            }
        }

        public string ContentExcerpt
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.excerpt))
                {
                    return this.excerpt;
                }

                return this.FullContent.Split(new[] { "<!--excerpt-->" }, StringSplitOptions.None)[0];
            }
            set
            {
                this.excerpt = value;
            }
        }

        public DateTime Date { get; set; }

        public string Email { get; set; }

        public string FileName { get; set; }

        public string Keywords { get; set; }

        public string Layout { get; set; }

        public string MetaDescription { get; set; }

        public IDictionary<string, dynamic> Settings { get; set; }

        public Published Published { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public List<string> Tags { get; set; }
        #endregion

        #region Public Methods and Operators
        public void SetHeaderSettings(IDictionary<string, object> settings)
        {
            foreach (var setting in settings)
            {
                switch (setting.Key.ToLower())
                {
                    case "categories":
                    case "category":
                        {
                            if (setting.Value is IEnumerable<string>)
                            {
                                Categories.AddRange((IEnumerable<string>)setting.Value);
                            }
                            else
                            {
                                Categories.Add((string)setting.Value);
                            }

                            break;
                        }
                    case "title":
                    {
                        this.Title = (string)setting.Value;
                        break;
                    }
                    case "layout":
                    {
                        this.Layout = (string)setting.Value;
                        break;
                    }
                    case "author":
                    {
                        this.Author = (string)setting.Value;
                        break;
                    }
                    case "email":
                    {
                        this.Email = (string)setting.Value;
                        break;
                    }
                    case "published":
                    {
                        Published published;
                        Enum.TryParse((string)setting.Value, true, out published);
                        Published = published;
                        break;
                    }
                    case "series":
                    {
                        //Series = (Series)setting.Value;
                        break;
                    }
                    case "metadescription":
                    {
                        this.MetaDescription = (string)setting.Value;
                        break;
                    }
                    case "tags":
                    {
                            if (setting.Value is IEnumerable<string>)
                            {
                                Tags.AddRange((IEnumerable<string>)setting.Value);
                            }
                            else
                            {
                                Tags.Add((string)setting.Value);
                            }

                        break;
                    }
                }
            }
        }
        #endregion
    }
}