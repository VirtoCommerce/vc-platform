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
			var backupObject = GetBackupObject(progressCallback); 
            backupObject.SerializeJson(backupStream);
        }

		public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
			var originalObject = GetBackupObject(progressCallback);
            var backupObject = backupStream.DeserializeJson<BackupObject>();

			var progressInfo = new ExportImportProgressInfo();
			progressInfo.Description = String.Format("{0} orders importing", backupObject.CustomerOrders.Count());
			progressCallback(progressInfo);

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

		private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
        {
			var retVal = new BackupObject();
			var progressInfo = new ExportImportProgressInfo();
            const CustomerOrderResponseGroup responseGroup = CustomerOrderResponseGroup.WithAddresses | CustomerOrderResponseGroup.WithItems
                | CustomerOrderResponseGroup.WithShipments | CustomerOrderResponseGroup.WithInPayments;

            var searchResponse = _customerOrderSearchService.Search(new SearchCriteria { Count = int.MaxValue });

			progressInfo.Description = String.Format("{0} orders loading", searchResponse.CustomerOrders.Count());
			progressCallback(progressInfo);

            retVal.CustomerOrders = searchResponse.CustomerOrders.Select((x) => _customerOrderService.GetById(x.Id, responseGroup)).ToList();
            
            return retVal;
        }

    }
}