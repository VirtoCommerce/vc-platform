using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using VirtoCommerce.Foundation.Reporting.Helpers;
using VirtoCommerce.Foundation.Reporting.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Reporting.ViewModel.Interfaces
{
    public interface IReportViewModel : IViewModel
    {
        Stream ReportDefinition { get; }
        RdlType ReportType { get; }
        DataSet GetReportDataSet(IDictionary<string, object> parameters = null);
        IEnumerable<ReportInfo> GetReportsList();
        ReportInfo InnerItem { get; }
        event EventHandler RefreshReport; 
        event EventHandler ClearParameters;
        void ReloadReportData(bool reloadFromDb = true);
        bool UIPrepared { get; set; }
    }
}
