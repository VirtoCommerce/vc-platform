#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DotLiquid;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Search;
using VirtoCommerce.Web.Convertors;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Services;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;
using VirtoCommerce.ApiClient.DataContracts.Marketing;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Collection : Drop, ILoadSlice
    {
        private int _allProductsCount;
        private ItemCollection<Tag> _allTags;
        private ItemCollection<Product> _products;
        private bool _productsLoaded;

        #region Public Properties
        public int AllProductsCount
        {
            get
            {
                this.LoadProducts();
                return this._allProductsCount;
            }
            set
            {
                this._allProductsCount = value;
            }
        }

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

        public ItemCollection<string> AllTypes { get; set; }

        public ItemCollection<string> AllVendors { get; set; }

        public string CurrentType { get; set; }

        public string CurrentVendor { get; set; }

        public string DefaultSortBy { get; set; }

        public string Description { get; set; }

        public string Handle { get; set; }

        public string Id { get; set; }

        public string Outline { get; set; }

        public Image Image { get; set; }

        public IEnumerable<SeoKeyword> Keywords { get; set; }

        public string NextProduct { get; set; }

        public IEnumerable<Collection> Parents { get; set; }

        public string PreviousProduct { get; set; }

        public ItemCollection<Product> Products
        {
            get
            {
                this.LoadProducts();
                return this._products;
            }
            set
            {
                this._products = value;
            }
        }

        public int ProductsCount
        {
            get
            {
                return this.Products == null ? 0 : this.Products.Size;
            }
        }

        public string SortBy { get; set; }

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

        public string TemplateSuffix { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
        #endregion

        #region Public Methods and Operators
        public void LoadSlice(int from, int? to)
        {
            var pageSize = to == null ? 50 : to - from;

            var tags = this.Context["current_tags"] as string[];

            var filters = new Dictionary<string, string[]>();
            if (tags != null && tags.Any())
            {
                var tagsArray =
                    tags.Select(t => t.Split(new[] { '_' })).Select(x => new Tuple<string, string>(x[0], x[1]));

                foreach (var tagsGroup in tagsArray.GroupBy(x => x.Item1))
                {
                    filters.Add(tagsGroup.Key, tagsGroup.Select(g => g.Item2).ToArray());
                }
            }

            var service = new CommerceService();
            var context = SiteContext.Current;

            var sortProperty = String.Empty;
            var sortDirection = "ascending";

            var sort = string.IsNullOrEmpty(this.SortBy) ? this.DefaultSortBy : this.SortBy;
            if (sort.Equals("manual"))
            {
                sortProperty = "position";
            }
            else if (sort.Equals("best-selling"))
            {
                sortProperty = "position";
            }
            else
            {
                var sortArray = sort.Split(new[] { '-' });
                if (sortArray.Length > 1)
                {
                    sortProperty = sortArray[0];
                    sortDirection = sortArray[1];
                }
            }

            var searchQuery = new BrowseQuery() { SortProperty = sortProperty, SortDirection = sortDirection, Filters = filters, Skip = from, Take = pageSize.Value, Outline = this.Id };
            var response =
                Task.Run(() => service.SearchAsync<Product>(context, 
                    searchQuery, this, responseGroups: ItemResponseGroups.ItemSmall | ItemResponseGroups.Variations)).Result;

            // populate tags with facets returned
            if (response.Facets != null && response.Facets.Any())
            {
                var values = response.Facets.SelectMany(f => f.Values.Select(v => v.AsWebModel(f.Field)));
                this.Tags = new ItemCollection<Tag>(values.ToArray());
            }

            this.Products = response;
        }

        public override string ToString()
        {
            return this.Url;
        }
        #endregion

        #region Methods
        private void LoadProducts()
        {
            if (this._productsLoaded)
            {
                return;
            }

            this._productsLoaded = true;

            var pageSize = this.Context == null ? 20 : this.Context["paginate.page_size"].ToInt(20);
            var skip = this.Context == null ? 0 : this.Context["paginate.current_offset"].ToInt();

            this.LoadSlice(skip, pageSize + skip);
        }
        #endregion
    }
}