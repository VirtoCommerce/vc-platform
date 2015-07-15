using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.ExportImport;
using PaymentMethod = VirtoCommerce.Domain.Payment.Model.PaymentMethod;
using ShippingMethod = VirtoCommerce.Domain.Shipping.Model.ShippingMethod;
using Store = VirtoCommerce.Domain.Store.Model.Store;

namespace VirtoCommerce.StoreModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<Store> Stores { get; set; }
    }

    public sealed class StoreExportImport
    {
        private readonly IStoreService _storeService;

        public StoreExportImport(IStoreService storeService)
        {
            _storeService = storeService;
        }

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = new BackupObject { Stores = _storeService.GetStoreList().Where(x => x.Name == "Test").ToArray() };
            backupObject.Stores.ForEach(x => x.PaymentMethods = x.PaymentMethods.Where(s => s.IsActive).ToList());
            backupObject.Stores.ForEach(x => x.ShippingMethods = x.ShippingMethods.Where(s => s.IsActive).ToList());

            backupStream.JsonSerializationObject(backupObject, progressCallback, prodgressInfo);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var sync = new Object();
            var storesForUpdate = new List<Store>();

            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.JsonDeserializationObject<BackupObject>(progressCallback, prodgressInfo);
            foreach (var store in backupObject.Stores)
            {
                var originalStore = _storeService.GetById(store.Id);
                if (originalStore == null)
                {
                    _storeService.Create(store);
                }
                else
                {
                    //Catalog stay as is

                    //SeoInfos
                    UpdateSeoInfos(originalStore.SeoInfos, store.SeoInfos);

                    //ShippingMethods
                    UpdateShipmentMethods(originalStore.ShippingMethods, store.ShippingMethods);

                    //PaymentMethods
                    UpdatePaymentMethods(originalStore.PaymentMethods, store.PaymentMethods);

                    //Settings
                    UpdateSettings(originalStore.Settings, store.Settings);

                    //Fullfilments ??

                    originalStore.InjectFrom(store);
                    lock (sync)
                    {
                        storesForUpdate.Add(originalStore);
                    }
                }
            }
            if (storesForUpdate.Any())
            {
                _storeService.Update(storesForUpdate.ToArray());
            }
        }

        private void UpdateSeoInfos(ICollection<SeoInfo> originalSeoInfos, ICollection<SeoInfo> importedSeoInfos)
        {
            Parallel.ForEach(importedSeoInfos, importedSeoInfo =>
            {
                var originalSeoInfo = originalSeoInfos.FirstOrDefault(x => x.LanguageCode == importedSeoInfo.LanguageCode);
                if (originalSeoInfo != null)
                {
                    importedSeoInfo.Id = originalSeoInfo.Id;
                }
            });
        }

        private void UpdateShipmentMethods(ICollection<ShippingMethod> originalMethods, ICollection<ShippingMethod> importedMethods)
        {
            var sync = new Object();
            var itemForRemove = new List<ShippingMethod>();

            Parallel.ForEach(importedMethods, importedMethod =>
            {
                var originalMethod = originalMethods.FirstOrDefault(x => x.Code == importedMethod.Code);
                if (originalMethod != null)
                {
                    importedMethod.Id = originalMethod.Id;
                    UpdateSettings(originalMethod.Settings, importedMethod.Settings);
                }
                else
                {
                    //Method is not exists in original platform
                    lock (sync)
                    {
                        itemForRemove.Add(importedMethod);
                    }
                }
            });

            foreach (var item in itemForRemove)
            {
                importedMethods.Remove(item);
            }

            foreach (var originalMethod in originalMethods)
            {
                var importedMethod = importedMethods.FirstOrDefault(x => x.Code == originalMethod.Code);
                if (importedMethod == null)
                {
                    originalMethod.IsActive = false;
                    importedMethods.Add(originalMethod);
                }
            }
        }

        private void UpdatePaymentMethods(ICollection<PaymentMethod> originalMethods, ICollection<PaymentMethod> importedMethods)
        {
            var sync = new Object();
            var itemForRemove = new List<PaymentMethod>();

            Parallel.ForEach(importedMethods, importedMethod =>
            {
                var originalMethod = originalMethods.FirstOrDefault(x => x.Code == importedMethod.Code);
                if (originalMethod != null)
                {
                    importedMethod.Id = originalMethod.Id;
                    UpdateSettings(originalMethod.Settings, importedMethod.Settings);
                }
                else
                {
                    //Method is not exists in original platform
                    lock (sync)
                    {
                        itemForRemove.Add(importedMethod);
                    }
                }
            });

            foreach (var item in itemForRemove)
            {
                importedMethods.Remove(item);
            }
        }

        private void UpdateSettings(ICollection<SettingEntry> originalSettings, ICollection<SettingEntry> importedSettings)
        {
            Parallel.ForEach(importedSettings, importedSetting =>
            {
                var originalSetting = originalSettings.FirstOrDefault(x => x.Name == importedSetting.Name);
                importedSetting.ObjectId = originalSetting != null ? originalSetting.ObjectId : null;
            });
        }

    }
}