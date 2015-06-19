using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Web.Model;
using VirtoCommerce.StoreModule.Web.Model.Notifications;
using PaymentMethod = VirtoCommerce.Domain.Payment.Model.PaymentMethod;
using SeoInfo = VirtoCommerce.Domain.Store.Model.SeoInfo;
using ShippingMethod = VirtoCommerce.Domain.Shipping.Model.ShippingMethod;
using Store = VirtoCommerce.Domain.Store.Model.Store;

namespace VirtoCommerce.StoreModule.Web.BackgroundJobs
{
    public class BackupStoreJob
    {
        #region Constants

        private const string _stopeXmlName = "Store.xml";
        private const string _storeLanguagesXmlName = "StoreLanguages.xml";
        private const string _storeCurrenciesXmlName = "StoreCurrencies.xml";
        private const string _paymentMethodXmlNamePrefix = "PaymentMethod-";
        private const string _shippingMethodXmlNamePrefix = "ShippingMethod-";
        private const string _storeSeoXmlName = "StoreSeo.xml";
        private const string _settingsXmlName = "Settings.xml";

        #endregion

        #region Dependency properties

        private readonly IStoreService _storeService;
        private readonly INotifier _notifier;
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly IBlobUrlResolver _blobUrlResolver;
        private readonly ISettingsManager _settingsManager;

        #endregion

        #region Constructors

        internal BackupStoreJob()
        {

        }

        public BackupStoreJob(IStoreService storeService, ISettingsManager settingsManager,
                                INotifier notifier, IBlobStorageProvider blobProvider, IBlobUrlResolver blobUrlResolver)
        {
            _storeService = storeService;
            _settingsManager = settingsManager;
            _notifier = notifier;
            _blobStorageProvider = blobProvider;
            _blobUrlResolver = blobUrlResolver;
        }

        #endregion

        public virtual void DoExport(StoreExportConfiguration exportConfiguration, ExportNotification notification)
        {
            //Notification
            notification.Description = "loading ...";
            _notifier.Upsert(notification);

            try
            {
                var store = _storeService.GetById(exportConfiguration.StoreId);
                var settings = new List<SettingEntry>();
                var backup = new Backup(_blobStorageProvider, _blobUrlResolver);

                // Add objects to backup
                backup.Add(_stopeXmlName, store);
                
                // Add collections of the same types elements
                if (!exportConfiguration.IsDisableLanguages)
                {
                    backup.Add(_storeLanguagesXmlName, store.Languages.ToArray());
                }
                if (!exportConfiguration.IsDisableCurrencies)
                {
                    backup.Add(_storeCurrenciesXmlName, store.Currencies.ToArray());
                }
                if (!exportConfiguration.IsDisableSeo)
                {
                    backup.Add(_storeSeoXmlName, store.SeoInfos.ToArray());
                }
                
                // Add collections of different types elements
                if (!exportConfiguration.IsDisablePamentMethods)
                {
                    foreach (var paymentMethod in store.PaymentMethods.Where(x => x.IsActive))
                    {
                        //don't export Settings because security
                        backup.Add(string.Format("{0}{1}.xml", _paymentMethodXmlNamePrefix, paymentMethod.Code),
                            paymentMethod);
                    }
                }
                if (!exportConfiguration.IsDisableShipmentMethods)
                {
                    foreach (var shippingMethod in store.ShippingMethods.Where(x => x.IsActive))
                    {
                        backup.Add(string.Format("{0}{1}.xml", _shippingMethodXmlNamePrefix, shippingMethod.Code),
                            shippingMethod);
                        settings.AddRange(shippingMethod.Settings);
                    }
                }

                settings.AddRange(store.Settings);
                backup.Add(_settingsXmlName, settings.ToArray());

                // Create backup file
                var zipUrl = backup.Save("Store-" + (store.Name ?? store.Id) + ".zip");
                
                notification.DownloadUrl = zipUrl;
                notification.Description = "Export finished";
            }
            catch (Exception ex)
            {
                notification.Description = "Export error";
                notification.ErrorCount++;
                notification.Errors.Add(ex.ToString());
            }
            finally
            {
                notification.Finished = DateTime.UtcNow;
                _notifier.Upsert(notification);
            }


        }

        public virtual void DoImport(StoreImportConfiguration importConfiguration, ImportNotification notification)
        {
            if (string.IsNullOrEmpty(importConfiguration.FileUrl))
            {
                throw new Exception("FileUrl is null or empty. Can't import.");
            }
            //Notification
            notification.Description = "loading ...";
            _notifier.Upsert(notification);

            try
            {
                var backup = new Backup(_blobStorageProvider, null);
                backup.OpenZip(importConfiguration.FileUrl);

                // Extract objects from backup
                var store = backup.LoadFromFile<Store>(_stopeXmlName);
                if (store != null)
                {
                    store.Languages = backup.LoadFromFile<string[]>(_storeLanguagesXmlName);
                    store.Currencies = backup.LoadFromFile<CurrencyCodes[]>(_storeCurrenciesXmlName);
                    store.SeoInfos = backup.LoadFromFile<SeoInfo[]>(_storeSeoXmlName);
                    store.PaymentMethods = backup.LoadFromFiles<PaymentMethod>(_paymentMethodXmlNamePrefix).ToArray();
                    store.ShippingMethods = backup.LoadFromFiles<ShippingMethod>(_shippingMethodXmlNamePrefix).ToArray();
                    var settings = backup.LoadFromFile<SettingEntry[]>(_settingsXmlName);

                    var storeSettings = settings.Where(x => x.ObjectId == store.Id).ToArray();
                    
                    // Clear ids of collections to prevent dublicate ids 
                    //todo check payment and shipping modules
                    if (store.PaymentMethods != null)
                    {
                        store.PaymentMethods.ForEach(x => x.Id = null);
                    }
                    if (store.ShippingMethods != null)
                    {
                        store.ShippingMethods.ForEach(x => x.Id = null);
                    }
                    if (store.SeoInfos != null)
                    {
                        store.SeoInfos.ForEach(x => x.Id = null);
                    }


                    if (!string.IsNullOrEmpty(importConfiguration.NewStoreName))
                    {
                        store.Name = importConfiguration.NewStoreName;
                    }
                    if (!string.IsNullOrEmpty(importConfiguration.NewStoreId))
                    {
                        store.Id = importConfiguration.NewStoreId;
                    }
                    if (_storeService.GetById(store.Id) != null)
                    {
                        //todo change generation code
                        store.Id = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture).GenerateSlug();
                    }
                    SaveStore(store);

                    storeSettings.ForEach(x => x.ObjectId = store.Id);
                    _settingsManager.SaveSettings(storeSettings);
                }

                backup.CloseZip();
            }
            catch (Exception ex)
            {
                notification.Description = "Export error";
                notification.ErrorCount++;
                notification.Errors.Add(ex.ToString());
            }
            finally
            {
                notification.Finished = DateTime.UtcNow;
                notification.Description = "Import finished"
                    + (notification.Errors.Any() ? " with errors" : " successfully");
                _notifier.Upsert(notification);
            }


        }

        #region Private Methods

        private void SaveStore(Store store)
        {
            var isNew = store.IsTransient() || true;
            //For new store try to find them by id
            //if (store.IsTransient() && !String.IsNullOrEmpty(store.Id))
            //{
            //	isNew = _storeService.GetById(store.Id) != null;
            //}

            if (isNew)
            {
                var newStore = _storeService.Create(store);
                store.Id = newStore.Id;
            }
            else
            {
                _storeService.Update(new[] { store });
            }
        }

        #endregion
    }
}