using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.MerchandisingModule.Model;
using f = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.MerchandisingModule.Data.Adaptors
{
    using VirtoCommerce.Foundation.Assets.Services;

    public static class CatalogItemAdaptor
    {
        public static CatalogItem AsCatalogItem(this f.Item item, IAssetUrl resolver = null)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var ret = new CatalogItem {Id = item.ItemId, Catalog = item.CatalogId};
            ret.InjectFrom(item);

            #region Load Assets
            var assets = item.ItemAssets.ToArray();

            //load images
            var images = new List<ItemImage>();
            foreach (var asset in assets.Where(x => x.AssetType.Equals("image", StringComparison.OrdinalIgnoreCase)).OrderBy(x=>x.SortOrder))
            {
                var image = new ItemImage
                {
                    Name = asset.GroupName,
                    Src = asset.AssetId,
                    Id = asset.AssetId
                };

                if (resolver != null)
                {
                    image.Src = resolver.ResolveUrl(asset.AssetId);
                    image.ThumbSrc = resolver.ResolveUrl(asset.AssetId, thumb:true);
                }

                images.Add(image);
            }

            if (images.Count > 0)
            {
                ret.Images = images.ToArray();
            }

            #endregion

            #region Properties
            foreach (var prop in item.ItemPropertyValues)
            {
                if (ret.Properties == null)
                {
                    ret.Properties = new PropertyDictionary();
                }

                ret.Properties[prop.Name] = prop.ToString();
            }
            #endregion

            return ret;
        }

        public static Product AsProduct(this f.Product item, IAssetUrl resolver = null)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var catalogItem = (item as f.Item).AsCatalogItem(resolver);

            var ret = new Product();
            ret.InjectFrom(catalogItem);
            return ret;
        }

        #region Model to Entity Mapping

        public static f.Item Map(this CatalogItem item, f.Item dbItem)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            dbItem.InjectFrom(item);

            if (!String.IsNullOrEmpty(item.Id))
            {
                dbItem.ItemId = item.Id;
            }

            if (!String.IsNullOrEmpty(item.Catalog))
            {
                dbItem.CatalogId = item.Catalog;
            }

            #region Properties
            // Process attributes, with the following logic:
            // if attribute collection
            //      is null = do nothing
            //      has some elements = add those elements to the collection or update existing ones by the key (items will be added only if meta data already exists?)
            //      empty collection = remove all elements from db

            if (item.Properties == null) // do nothing
            {
            }
            else if (item.Properties.Count == 0) // remove everything
            {
                dbItem.ItemPropertyValues.Clear();
            }
            else // merge
            {
                foreach (var attributeKey in item.Properties.Keys)
                {
                    var exitingValue =
                        dbItem.ItemPropertyValues.SingleOrDefault(x => x.Name.Equals(attributeKey, StringComparison.OrdinalIgnoreCase));

                    if (exitingValue != null) // value already exists, simply update it
                    {
                        exitingValue.ShortTextValue = item.Properties[attributeKey].ToString();
                    }
                    else // Since it is an API, we'll allow adding non defined items as strings, or we can add better logic to determing basic types
                    {
                        dbItem.ItemPropertyValues.Add(new f.ItemPropertyValue() { ItemId = dbItem.ItemId, Name = attributeKey, ShortTextValue = item.Properties[attributeKey].ToString(), ValueType = f.PropertyValueType.ShortString.GetHashCode() });
                    }
                }
            }

            #endregion

            return dbItem;
            
        }

        public static f.Product Map(this Product item, f.Product dbProduct)
        {
            ((CatalogItem)item).Map(dbProduct);
            return dbProduct;
        }
        #endregion
    }
}
