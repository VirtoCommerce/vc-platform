using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/metafield
    /// The metafields object allows you to store additional information for products, collections, orders, blogs, pages and your shop. You can output metafields on your storefront using Liquid.
    /// </summary>
    public class MetafieldsCollection : Dictionary<string, object>
    {
        #region Constructors and Destructors
        public MetafieldsCollection(string scope, IDictionary<string, object> collection)
            : base(collection)
        {
            this.Namespace = scope;
        }

        public MetafieldsCollection(string scope, ICollection<SettingEntry> settings)
        {
            this.Namespace = scope;
            if (settings != null)
            {
                foreach (var setting in settings)
                {
                    if (setting.IsArray)
                    {
                        this.Add(setting.Name, setting.ArrayValues);
                    }
                    else
                    {
                        this.Add(setting.Name, setting.Value);
                    }
                }
            }
        }

        public MetafieldsCollection(string scope, Storefront.Model.Language language, ICollection<DynamicProperty> dynamicProperties)
        {
            this.Namespace = scope;
            if (dynamicProperties != null)
            {
                foreach (var dynamicProperty in dynamicProperties)
                {
                    if (dynamicProperty.IsDictionary || dynamicProperty.IsArray)
                    {
                        this.Add(dynamicProperty.Name, dynamicProperty.Values.GetLocalizedStringsForLanguage(language).Select(x => x.Value));
                    }
                    else
                    {
                        this.Add(dynamicProperty.Name, dynamicProperty.Values.GetLocalizedStringsForLanguage(language).Select(x => x.Value).FirstOrDefault());
                    }
                }
            }
        }

        public MetafieldsCollection(string scope, ICollection<CatalogProperty> catalogProperties)
        {
            this.Namespace = scope;
            if (catalogProperties != null)
            {
                foreach (var property in catalogProperties)
                {
                    //TODO: Add support multi-values
                    this.Add(property.Name, property.Value);
                }
            }
        }
        #endregion

        #region Public Properties
        public string Namespace { get; set; }
        #endregion
    }

    public class MetaFieldNamespacesCollection : ItemCollection<MetafieldsCollection>
    {
        #region Constructors and Destructors
        public MetaFieldNamespacesCollection(IEnumerable<MetafieldsCollection> collections)
            : base(collections)
        {
        }

        public MetaFieldNamespacesCollection(MetafieldsCollection collection)
            : base(new[] { collection })
        {
        }
        #endregion

        #region Public Methods and Operators
        public override object BeforeMethod(string method)
        {
            var result = this.SingleOrDefault(x => x.Namespace == method);
            return result;
        }
        #endregion

        #region Public Indexers
        public MetafieldsCollection this[string name]
        {
            get
            {
                var result = this.SingleOrDefault(x => x.Namespace == name);
                return result;
            }
        }

        #endregion

    }
}
