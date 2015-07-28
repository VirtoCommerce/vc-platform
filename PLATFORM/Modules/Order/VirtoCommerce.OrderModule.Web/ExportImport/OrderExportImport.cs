using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.OrderModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<CustomerOrder> CustomerOrders { get; set; }
    }

    public abstract class ExportImportBase
    {
        private string _notifyPattern;
        private int _counter;
        private int _notifyMinSize = 1;
        protected Action<ExportImportProgressInfo> ProgressCallback;
        private ExportImportProgressInfo _progressInfo = new ExportImportProgressInfo();

        protected void InitProgressNotifier(string notifyPattern, int totalCount, int notifyMinSize)
        {
            _counter = 0;
            _notifyMinSize = notifyMinSize;
            _notifyPattern = notifyPattern;
            _progressInfo = new ExportImportProgressInfo() { TotalCount = totalCount };
        }

        protected void Notify(int count = 1)
        {
            _counter += count;
            if (ProgressCallback != null && _progressInfo != null)
            {
                _progressInfo.ProcessedCount = _counter;
                _progressInfo.Description = string.Format(_notifyPattern, _progressInfo.ProcessedCount, _progressInfo.TotalCount);
                ProgressCallback(_progressInfo);
            }
        }

        protected void NotifyText(string description)
        {
            if (ProgressCallback != null)
            {
                ProgressCallback(new ExportImportProgressInfo { Description = "loading data..." });
            }
        }

        protected void SetTotalCount(int totalCount)
        {
            if (_progressInfo != null)
            {
                _progressInfo.TotalCount = totalCount;
            }
        }

        protected IEnumerable<Q> Converter<T, Q>(IEnumerable<T> source, Func<T, Q> converterFunc)
        {
            var result = new List<Q>();
            if (source != null)
            {
                var sourceArray = source.ToArray();
                SetTotalCount(sourceArray.Count());
                for (var i = 0; i < sourceArray.Count(); i += _notifyMinSize)
                {
                    var part = sourceArray.Skip(i).Take(_notifyMinSize).Select(converterFunc).ToArray();
                    result.AddRange(part);
                    Notify(part.Count());
                }
            }
            return result;
        }

    }

    public sealed class OrderExportImport : ExportImportBase
    {
        private readonly ICustomerOrderSearchService _customerOrderSearchService;
        private readonly ICustomerOrderService _customerOrderService;

        public OrderExportImport(ICustomerOrderSearchService customerOrderSearchService, ICustomerOrderService customerOrderService)
        {
            _customerOrderSearchService = customerOrderSearchService;
            _customerOrderService = customerOrderService;
        }

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            ProgressCallback = progressCallback;
            var backupObject = GetBackupObject(); 
            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            ProgressCallback = progressCallback;
            var originalObject = GetBackupObject();
            var backupObject = backupStream.DeserializeJson<BackupObject>();

            UpdateOrders(originalObject.CustomerOrders, backupObject.CustomerOrders);
        }

        private void UpdateOrders(ICollection<CustomerOrder> original, ICollection<CustomerOrder> backup)
        {
            var toUpdate = new List<CustomerOrder>();

            backup.CompareTo(original, EqualityComparer<Operation>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        toUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        _customerOrderService.Create(x);
                        break;
                }
            });
            _customerOrderService.Update(toUpdate.ToArray());
        }

        private BackupObject GetBackupObject()
        {
            NotifyText("loading data...");

            const CustomerOrderResponseGroup responseGroup = CustomerOrderResponseGroup.WithAddresses | CustomerOrderResponseGroup.WithItems
                | CustomerOrderResponseGroup.WithShipments | CustomerOrderResponseGroup.WithInPayments;

            var searchResponse = _customerOrderSearchService.Search(new SearchCriteria { Count = int.MaxValue });
            
            InitProgressNotifier("{0} of {1} orders loaded", 0, 20);
            var customerOrders = Converter(searchResponse.CustomerOrders, (x) => _customerOrderService.GetById(x.Id, responseGroup));
            
            return new BackupObject { CustomerOrders = customerOrders.ToArray()};
        }

    }
}