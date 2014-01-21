using System.Collections.Generic;
using System.Data;
using System.IO;
using System.ServiceModel;
using VirtoCommerce.Foundation.Reporting.Model;

namespace VirtoCommerce.Foundation.Reporting.Services
{
    [ServiceContract]
    public interface IReportingService
    {
        [OperationContract]
        Stream GetReportFile(string reportFileName);

        [OperationContract]
        DataSet GetReportData(string reportFileName, IDictionary<string, object> parameters = null);

        [OperationContract]
        IEnumerable<ReportInfo> GetReportsList(string folder = "");

        [OperationContract]
        IEnumerable<ReportFolder> GetReportsFolders();
    }
}
