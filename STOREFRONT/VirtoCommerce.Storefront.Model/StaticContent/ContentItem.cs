using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.StaticContent
{
    public abstract class ContentItem
    {
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

        public Language Language { get; set; }
        
        /// <summary>
        /// Content file name without extension
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Storage path in file system
        /// </summary>
        public string LocalPath { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// Liquid layout from store theme used as master page for page rendering. If its null will be used default layout.
        /// </summary>
        public string Layout { get; set; }

        public string FileName { get; set; }

        public string RelativePath { get; set; }

        public virtual void LoadContent(string content, IDictionary<string, IEnumerable<string>> metaInfoMap)
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

            Content = content;
            if (Title == null)
            {
                Title = Name;
            }
        }

    }
}
