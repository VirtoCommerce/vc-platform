using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.Web.Models
{
    public class ItemModel
    {
        public ItemModel(CatalogItem catalogItem)
        {
            CatalogItem = catalogItem;
        }
        public CatalogItem CatalogItem { get; set; }

        public PriceModel Price { get; set; }

        public string DisplayName
        {
            get
            {
                var retVal = CatalogItem.Name;
                if (Properties.ContainsKey("Title"))
                {
                    retVal = this["Title"][0];
                }

                return retVal;
            }
        }

        public bool IsNew
        {
            get
            {
                return CatalogItem.StartDate.AddMonths(1) >= DateTime.UtcNow;
            }
        }

        public bool IsSale
        {
            get
            {
                return Price != null && Price.Type == PriceType.Sale;
            }
        }

        #region Properties

        private IDictionary<string, string[]> _properties = new Dictionary<string, string[]>();

        public string[] this[string name]
        {
            get
            {
                return _properties[name];
            }
            set
            {
                if (_properties.ContainsKey(name))
                {
                    _properties[name] = value;
                }
                else
                {
                    _properties.Add(name, value);
                }
            }
        }

        public IDictionary<string, string[]> Properties
        {
            get
            {
                return _properties;
            }
            set { _properties = value; }
        }

        #endregion
    }

    public class EditorialReviewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string ReviewType { get; set; }

    }

    public class CategoryPathModel
    {
        private string _url;

        public string Url
        {
            get { return _url; }
            set
            {
                Category = value.Split(new[] { '/' }).Last();
                _url = value;
            }
        }

        public string Category { get; set; }
    }
}