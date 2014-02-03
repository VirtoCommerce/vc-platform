using System.Collections.Generic;
using VirtoCommerce.Foundation.Reporting.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Reporting.ViewModel.Interfaces
{
    public interface IReportingHomeViewModel : IViewModel
    {
        IEnumerable<ReportFolder> ReportFolder { get; }
        bool AllowReportManage { get; }
    }
}
