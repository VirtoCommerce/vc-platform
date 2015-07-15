using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Data.ExportImport;

namespace VirtoCommerce.OrderModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<Operation> Orders { get; set; }
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

            var responce = _customerOrderSearchService.Search(new SearchCriteria{Count = int.MaxValue});
            var orderIds = responce.CustomerOrders.Select(x => x.Id);
            const CustomerOrderResponseGroup filter = CustomerOrderResponseGroup.WithAddresses | CustomerOrderResponseGroup.WithItems
                | CustomerOrderResponseGroup.WithShipments | CustomerOrderResponseGroup.WithInPayments;
            
            var backupObject = new BackupObject
            {
                Orders = orderIds.Select(id => _customerOrderService.GetById(id, filter)).ToArray(),
            };

            backupStream.JsonSerializationObject(backupObject, progressCallback, prodgressInfo);
        }

    }
}