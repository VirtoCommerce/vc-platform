using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.ExportImport;

namespace VirtoCommerce.StoreModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<Store> Stores { get; set; }
    }

    public sealed class StoreExportImport
    {
        private readonly IStoreService _storeService;
        private readonly IPaymentMethodsService _paymentMethodsService;
        private readonly IShippingService _shippingService;

        public StoreExportImport(IStoreService storeService, IPaymentMethodsService paymentMethodsService, IShippingService shippingService)
        {
            _storeService = storeService;
            _paymentMethodsService = paymentMethodsService;
            _shippingService = shippingService;
        }

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = new BackupObject { Stores = _storeService.GetStoreList().Where(x => x.Name == "Test").ToArray() };
            backupObject.Stores.ForEach(x => x.PaymentMethods = x.PaymentMethods.Where(s => s.IsActive).ToList());
            backupObject.Stores.ForEach(x => x.ShippingMethods = x.ShippingMethods.Where(s => s.IsActive).ToList());

            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.DeserializeJson<BackupObject>();
            var originalStoreIds = _storeService.GetStoreList().Select(x => x.Id).ToArray();
            
            var toAdd = backupObject.Stores.Where(x => !originalStoreIds.Contains(x.Id)).ToArray();
            var toUpdate = backupObject.Stores.Where(x => originalStoreIds.Contains(x.Id)).ToArray();
            var toRemove = originalStoreIds.Where(x => backupObject.Stores.All(s => s.Id != x)).ToArray();

            //_storeService.Delete(toRemove);
            _storeService.Update(toUpdate);
            foreach (var store in toAdd)
            {
                _storeService.Create(store);
            }
        }

        //private void UpdateSeoInfos(ICollection<SeoInfo> originalSeoInfos, ICollection<SeoInfo> importedSeoInfos)
        //{
        //    Parallel.ForEach(importedSeoInfos, importedSeoInfo =>
        //    {
        //        var originalSeoInfo = originalSeoInfos.FirstOrDefault(x => x.LanguageCode == importedSeoInfo.LanguageCode);
        //        if (originalSeoInfo != null)
        //        {
        //            importedSeoInfo.Id = originalSeoInfo.Id;
        //        }
        //    });
        //}

        //private void UpdateShipmentMethods(ICollection<ShippingMethod> originalMethods, ICollection<ShippingMethod> importedMethods)
        //{
        //    // Exclude not installed methods
        //    var installedMethodCodes =_shippingService.GetAllShippingMethods().Select(x => x.Code);
        //    importedMethods = importedMethods.Where(x => installedMethodCodes.Contains(x.Code)).ToArray();

        //    // update settings
        //    Parallel.ForEach(importedMethods, importedMethod =>
        //    {
        //        var originalMethod = originalMethods.FirstOrDefault(x => x.Code == importedMethod.Code);
        //        if (originalMethod != null)
        //        {
        //            importedMethod.Id = originalMethod.Id;
        //            UpdateSettings(originalMethod.Settings, importedMethod.Settings);
        //        }
        //    });

        //    // remove old
        //    //foreach (var originalMethod in originalMethods)
        //    //{
        //    //    var importedMethod = importedMethods.FirstOrDefault(x => x.Code == originalMethod.Code);
        //    //    if (importedMethod == null)
        //    //    {
        //    //        originalMethod.IsActive = false;
        //    //        importedMethods.Add(originalMethod);
        //    //    }
        //    //}
        //}

        //private void UpdatePaymentMethods(ICollection<PaymentMethod> originalMethods, ICollection<PaymentMethod> importedMethods)
        //{
        //    // Exclude not installed methods
        //    var installedMethodCodes = _paymentMethodsService.GetAllPaymentMethods().Select(x => x.Code);
        //    importedMethods = importedMethods.Where(x => installedMethodCodes.Contains(x.Code)).ToArray();

        //    // update settings
        //    Parallel.ForEach(importedMethods, importedMethod =>
        //    {
        //        var originalMethod = originalMethods.FirstOrDefault(x => x.Code == importedMethod.Code);
        //        if (originalMethod != null)
        //        {
        //            importedMethod.Id = originalMethod.Id;
        //            UpdateSettings(originalMethod.Settings, importedMethod.Settings);
        //        }
        //    });
        //}

        //private void UpdateSettings(ICollection<SettingEntry> originalSettings, ICollection<SettingEntry> importedSettings)
        //{
        //    Parallel.ForEach(importedSettings, importedSetting =>
        //    {
        //        var originalSetting = originalSettings.FirstOrDefault(x => x.Name == importedSetting.Name);
        //        importedSetting.ObjectId = originalSetting != null ? originalSetting.ObjectId : null;
        //    });
        //}

    }
}