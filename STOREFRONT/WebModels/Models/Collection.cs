﻿#region
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
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Services;
using VirtoCommerce.Web.Models.Tagging;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;
using VirtoCommerce.ApiClient.DataContracts.Marketing;
using VirtoCommerce.Web.Caching;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Collection : Drop
    {
        private int _allProductsCount;
        private TagCollection _allTags;
        private ItemCollection<Product> _products;
        private bool _productsLoaded;

        public Collection()
        {
            DefaultSortBy = "manual";
        }

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

        public TagCollection AllTags
        {
            get
            {
                this.LoadProducts();
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

        public TagCollection Tags
        {
            get
            {
                return this.AllTags;
            }
            set
            {
                this.AllTags = value;
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

            var tags = this.Context["current_tags"] as SelectedTagCollection;

            var filters = new Dictionary<string, string[]>();
            if (tags != null && tags.Any())
            {
                // split tags to field=value using "_", if there is no "_", then simply add them to "tags"=values
                var tagsMultiArray = tags.Select(t => t.Split(new[] { '_' }));

                var tagsArray = new List<Tuple<string, string>>();

                // add tags that have "_"
                tagsArray.AddRange(tagsMultiArray.Where(x=>x.Length > 1).Select(x => new Tuple<string, string>(x[0], x[1])));

                // add the rest that don't have "_" as tags, will sort them out on the server api
                tagsArray.AddRange(tagsMultiArray.Where(x => x.Length == 1).Select(x => new Tuple<string, string>("tags", x[0])));

                foreach (var tagsGroup in tagsArray.GroupBy(x => x.Item1))
                {
                    filters.Add(tagsGroup.Key, tagsGroup.Select(g => g.Item2).ToArray());
                }
            }

            var service = CommerceService.Create();
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

            var searchQuery = new BrowseQuery() { SortProperty = sortProperty, SortDirection = sortDirection, Filters = filters, Skip = from, Take = pageSize.Value, Outline = Id == "All" ? "" : this.BuildSearchOutline() };
            var searchCacheKey = CacheKey.Create("Collection.LoadSlice", searchQuery.ToString());
            var response = Task.Run(() => context.CacheManager.GetAsync(searchCacheKey, TimeSpan.FromMinutes(5), async () =>
            {
                return await Task.Run(() => service.SearchAsync<Product>(context,
                 searchQuery, this, responseGroups: ItemResponseGroups.ItemSmall | ItemResponseGroups.Variations));
            })).Result;

            
            // populate tags with facets returned
            if (response.Facets != null && response.Facets.Any())
            {
                var values = response.Facets.Where(x=>x.Values != null).SelectMany(f => f.Values.Select(v => v.AsWebModel(f.Field))).ToArray();
                this.Tags = new TagCollection(values);
            }

            this.AllProductsCount = response.TotalCount;

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