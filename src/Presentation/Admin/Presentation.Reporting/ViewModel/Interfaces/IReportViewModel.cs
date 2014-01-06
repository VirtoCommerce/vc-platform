using System.Collections.Generic;
using System.Data;
using System.IO;
using VirtoCommerce.Foundation.Reporting.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Reporting.ViewModel.Interfaces
{
    public interface IReportViewModel : IViewModel
    {
        Stream ReportDefinition { get; }
        DataSet ReportDataSet { get; }
        IEnumerable<ReportInfo> GetReportsList();
        ReportInfo InnerItem { get; }
    }
}
