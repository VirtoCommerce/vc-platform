using DotLiquid;
using System.Linq;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;
using Tag = VirtoCommerce.Web.Models.Tagging.Tag;

namespace VirtoCommerce.Web.Models.Cms
{
    public class Blog : Drop
    {
        private ItemCollection<Tag> _allTags;
        private ItemCollection<Article> _Articles;

        public ItemCollection<Tag> AllTags
        {
            get
            {
                return this._allTags;
            }
            set
            {
                this._allTags = value;
            }
        }

        public ItemCollection<Tag> Tags
        {
            get
            {
                return this._allTags;
            }
            set
            {
                this._allTags = value;
            }
        }

        public ItemCollection<Article> Articles
        {
            get
            {
                var pageSize = this.Context == null ? 20 : this.Context["paginate.page_size"].ToInt(20);
                var skip = this.Context == null ? 0 : this.Context["paginate.current_offset"].ToInt();

                if(AllArticles != null && AllArticles.Count() > 0)
                {
                    _Articles = new ItemCollection<Article>(AllArticles.Skip(skip).Take(pageSize).ToArray());
                    _Articles.TotalCount = AllArticles.Count();
                }

                return _Articles;
            }
        }

        public Article[] AllArticles { get; set; }

        public int ArticlesCount
        {
            get
            {
                return this.AllArticles == null ? 0 : this.AllArticles.Length;
            }
        }

        public bool CommentsEnabled { get; set; }

        public string Handle { get; set; }
        
        public string Id { get; set; }

        public bool Moderated { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
    }
}