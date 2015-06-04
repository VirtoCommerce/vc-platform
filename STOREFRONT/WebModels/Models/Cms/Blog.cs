using DotLiquid;

namespace VirtoCommerce.Web.Models.Cms
{
    public class Blog : Drop
    {
        private ItemCollection<Tag> _allTags;
        //private ItemCollection<Article> _articles;

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

        public Article[] Articles { get; set; }

        public int ArticlesCount
        {
            get
            {
                return this.Articles == null ? 0 : this.Articles.Length;
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