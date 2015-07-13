using System;
using System.Collections.Generic;
using System.IO;
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


    public sealed class OrderExportImport : JsonExportImport
    {
        private readonly ICustomerOrderSearchService _customerOrderSearchService;
        public OrderExportImport(ICustomerOrderSearchService customerOrderSearchService)
        {
            _customerOrderSearchService = customerOrderSearchService;
        }

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var responce = _customerOrderSearchService.Search(new SearchCriteria());
            var backupObject = new BackupObject
            {
                Orders = responce.CustomerOrders.ToArray(),
            };

            Save(backupStream, backupObject, progressCallback, prodgressInfo);
        }

    }
}