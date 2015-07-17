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


    public sealed class OrderExportImport
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
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = GetBackupObject(); 
            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.DeserializeJson<BackupObject>();
            var originalObject = GetBackupObject();

            UpdateStores(originalObject.CustomerOrders, backupObject.CustomerOrders);
        }

        private void UpdateStores(ICollection<CustomerOrder> original, ICollection<CustomerOrder> backup)
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
            var responce = _customerOrderSearchService.Search(new SearchCriteria { Count = int.MaxValue });
            var orderIds = responce.CustomerOrders.Select(x => x.Id);
            const CustomerOrderResponseGroup filter = CustomerOrderResponseGroup.WithAddresses | CustomerOrderResponseGroup.WithItems
                | CustomerOrderResponseGroup.WithShipments | CustomerOrderResponseGroup.WithInPayments;

            return new BackupObject
            {
                CustomerOrders = orderIds.Select(id => _customerOrderService.GetById(id, filter)).ToArray(),
            };
        }

    }
}