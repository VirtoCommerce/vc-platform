using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Web.Model.Notifications;

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

        public virtual void DoExport(string storeId, ExportNotification notification)
        {
            //Notification
            notification.Description = "loading ...";
            _notifier.Upsert(notification);

            try
            {
                var store = _storeService.GetById(storeId);
                var settings = new List<SettingEntry>();
                var backup = new Backup(_blobStorageProvider, _blobUrlResolver);

                // Add objects to backup
                backup.Add(_stopeXmlName, store);
                backup.Add(_storeLanguagesXmlName, store.Languages.ToList());
                backup.Add(_storeCurrenciesXmlName, store.Currencies.ToList());
                foreach (var paymentMethod in store.PaymentMethods.Where(x => x.IsActive))
                {
                    //don't export Settings because security
                    backup.Add(string.Format("{0}{1}.xml", _paymentMethodXmlNamePrefix, paymentMethod.Code), paymentMethod);
                }

                foreach (var shippingMethod in store.ShippingMethods.Where(x => x.IsActive))
                {
                    backup.Add(string.Format("{0}{1}.xml", _shippingMethodXmlNamePrefix, shippingMethod.Code), shippingMethod);
                    settings.AddRange(shippingMethod.Settings);
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

        public virtual void DoImport(string fileUrl, ImportNotification notification)
        {
            //Notification
            notification.Description = "loading ...";
            _notifier.Upsert(notification);

            try
            {
                var backup = new Backup(_blobStorageProvider, null);
                backup.OpenZip(fileUrl);

                // Extract objects from backup
                var store = backup.GetZipObject<Store>(_stopeXmlName);
                if (store != null)
                {
                    store.Languages = backup.GetZipObject<string[]>(_storeLanguagesXmlName);
                    store.Currencies = backup.GetZipObject<CurrencyCodes[]>(_storeCurrenciesXmlName);
                    var paymentMethods = backup.GetZipObjects<PaymentMethod>(_paymentMethodXmlNamePrefix).ToArray();
                    var shippingMethods = backup.GetZipObjects<ShippingMethod>(_shippingMethodXmlNamePrefix).ToArray();
                    var settings = backup.GetZipObject<SettingEntry[]>(_settingsXmlName);

                    var storeSettings = settings.Where(x => x.ObjectId == store.Id).ToArray();

                    // Clear ids 
                    // todo check payment and shipping modules
                    paymentMethods.ForEach(x => x.Id = null);
                    shippingMethods.ForEach(x => x.Id = null);

                    store.PaymentMethods = paymentMethods;
                    store.ShippingMethods = shippingMethods;

                    // to test only
                    store.Id = "test";
                    store.Name = "Test";

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