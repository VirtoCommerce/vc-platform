using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLiquid;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.Web.Models.Services;

namespace VirtoCommerce.Web.Models.Banners
{
    public class Banner : Drop
    {
        private IDictionary<string, string> _properties = new Dictionary<string, string>();

        public virtual string ContentType { get; set; }

        public virtual string Id { get; set; }

        public virtual bool IsMultilingual { get; set; }

        public virtual string Name { get; set; }

        public virtual IDictionary<string, string> Properties
        {
            get { return this._properties; }
            set { this._properties = value; }
        }

        #region Overrides of DropBase

        public override object BeforeMethod(string method)
        {
            return this._properties.ContainsKey(method) ? this._properties[method] : base.BeforeMethod(method);
        }

        #endregion
    }

    public class ProductWithImageAndPriceBanner : Banner
    {
        private Product _product;
        private bool _productLoaded ;

        public Product Product
        {
            get
            {
                this.LoadProduct();
                return this._product;
            }
            set
            {
                this._product = value;
            }
        }

        private void LoadProduct()
        {
            if (this._productLoaded)
            {
                return;
            }

            this._productLoaded = true;

            var service = new CommerceService();
            var context = SiteContext.Current;

            var response =
                Task.Run(() => service.GetProductAsync(context, this.Context["product_code"].ToString(), ItemResponseGroups.ItemSmall)).Result;

            Product = response;
        }
    }

    public class CategorySearchBanner : Banner
    {
        private bool _productsLoaded;
        private ItemCollection<Product> _products;

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

        public string Collection { get; set; }

        private void LoadProducts()
        {
            if (this._productsLoaded)
            {
                return;
            }

            //var pageSize = this.Context == null ? 20 : this.Context["paginate.page_size"].ToInt(20);

            this._productsLoaded = true;

            var service = new CommerceService();
            var context = SiteContext.Current;

            var searchQuery = new BrowseQuery() { Skip = 0, Take = 20 };
            var response =
                Task.Run(() => service.SearchAsync<Product>(context, searchQuery, responseGroups: ItemResponseGroups.ItemSmall | ItemResponseGroups.Variations)).Result;

            Products = response;
        }
    }
}