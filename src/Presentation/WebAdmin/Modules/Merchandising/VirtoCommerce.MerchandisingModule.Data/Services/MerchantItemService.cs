using System;
using System.Linq;
using System.Web.Http.OData;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.MerchandisingModule.Data.Adaptors;
using VirtoCommerce.MerchandisingModule.Model;
using VirtoCommerce.MerchandisingModule.Services;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.MerchandisingModule.Data.Services
{
    using System.IO;

    using Microsoft.Practices.ServiceLocation;

    using VirtoCommerce.Foundation.Assets.Repositories;
    using VirtoCommerce.Foundation.Assets.Services;

    public class MerchantItemService : IMerchantItemService
    {
        private readonly ICatalogRepository _repository;
        private IAssetService _assetService;

        private IAssetService AssetService
        {
            get
            {
                if (_assetService == null)
                {
                    _assetService = ServiceLocator.Current.GetInstance<IAssetService>();
                }

                return _assetService;
            }
        }

        public MerchantItemService(ICatalogRepository repository, IAssetService assetService = null)
        {
            _repository = repository;
            if (assetService != null)
            {
                _assetService = assetService;
            }
        }

        public void Create(string categoryId, Product product)
        {
            var dbProduct = CreateEntity(product);

            #region Primary Category
            dbProduct.CategoryItemRelations.Add(new foundation.CategoryItemRelation
            {
                CatalogId = product.Catalog,
                CategoryId = categoryId,
                ItemId = dbProduct.ItemId
            });
            #endregion

            #region Variations
            if (product.Variations != null && product.Variations.Length > 0)
            {
                foreach (var variation in product.Variations)
                {
                    var entity = CreateEntity(variation);

                    var entityRelation = new foundation.ItemRelation
                    {
                        RelationTypeId = foundation.ItemRelationType.Sku,
                        GroupName = "variation",
                        ChildItemId = entity.ItemId,
                        ParentItemId = dbProduct.ItemId,
                        Quantity = 1
                    };

                    _repository.Add(entityRelation);
                    _repository.Add(entity);
                }
            }
            #endregion

            #region Process Images
            if (product.Images != null && product.Images.Length > 0)
            {
                var index = 0;
                foreach (var image in product.Images)
                {
                    var fileName = String.Format("catalog/{0}/{1}", dbProduct.ItemId, image.Name);
                    var asset = new foundation.ItemAsset() { GroupName = image.Name, AssetType = "images", ItemId = dbProduct.ItemId, AssetId = fileName, SortOrder = index};
                    dbProduct.ItemAssets.Add(asset);
                    using (var info = new UploadStreamInfo())
                    {
                        info.FileName = fileName;
                        info.Length = image.Attachement.Length;
                        info.FileByteStream = new MemoryStream(image.Attachement);
                        AssetService.Upload(info);
                    }
                    index++;
                }
            }
            #endregion

            _repository.Add(dbProduct);
            _repository.UnitOfWork.Commit();
        }

        public void Update(Delta<Product> delta)
        {
            var productId = delta.GetEntity().Id;

            var dbProduct = ItemHelper.GetItems<foundation.Product>(_repository, new[] {productId}, ItemResponseGroups.ItemLarge).SingleOrDefault();
            
            if (dbProduct == null)
            {
                throw new ApplicationException();
            }

            var model = dbProduct.AsProduct();

            delta.Patch(model);

            // Need to update reference types too.
            foreach (var pn in delta.GetChangedPropertyNames())
            {
                Type t;
                if (delta.TryGetPropertyType(pn, out t) && t.IsClass)
                {
                    object v;
                    if (delta.TryGetPropertyValue(pn, out v))
                    {
                        model.GetType().GetProperty(pn).GetSetMethod().Invoke(model, new object[] { v });
                    }
                }
            }

            model.Map(dbProduct);
            _repository.UnitOfWork.Commit();
        }

        public void Delete(string id)
        {
            var dbProduct =
                ItemHelper.GetItems<foundation.Product>(_repository, new[] { id }).SingleOrDefault();

            _repository.Remove(dbProduct);
            _repository.UnitOfWork.Commit();
        }

        public void Create(string categoryId, string productId, ProductVariation variation)
        {
            throw new NotImplementedException();
        }

        public void Update(Delta<ProductVariation> delta)
        {
            throw new NotImplementedException();
        }

        #region Private Entity Methods
        private static foundation.Product CreateEntity(Product product)
        {
            var entity = new foundation.Product();
            CreatePopulate(entity);
            product.Map(entity);
            return entity;
        }

        private static foundation.Sku CreateEntity(ProductVariation variation)
        {
            var entity = new foundation.Sku();
            CreatePopulate(entity);
            variation.Map(entity);
            return entity;
        }

        private static void CreatePopulate(foundation.Item item)
        {
            item.IsActive = true;
            item.AvailabilityRule = (int)foundation.AvailabilityRule.Always;
            item.StartDate = DateTime.UtcNow;
            item.IsBuyable = true;
            item.MinQuantity = 1;
            item.MaxQuantity = 0;
            item.TrackInventory = false;
            item.Weight = 0;
        }
        #endregion

        public T GetItem<T>(string itemId, ItemResponseGroups responseGroup) where T : CatalogItem
        {
            var items = ItemHelper.GetItems<foundation.Product>(_repository, new [] { itemId }, responseGroup);
            if (items != null && items.Length > 0) return items[0].AsProduct() as T;
            return null;
        }
    }
}