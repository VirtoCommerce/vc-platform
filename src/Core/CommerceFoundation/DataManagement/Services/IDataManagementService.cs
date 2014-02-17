using System.Collections.Generic;
using System.ServiceModel;
using VirtoCommerce.Foundation.DataManagement.Model;
using VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.Foundation.DataManagement.Services
{
	[ServiceContract]
	public interface IDataManagementService
	{
		[OperationContract]
		string ImportData(string jobId, string assetPath);

		[OperationContract]
		string ExportData(IList<EntityType> exportEntityTypes, string assetPath, IDictionary<string, object> parameters);
		
		[OperationContract]
		OperationStatus GetOperationStatus(string operationId);

		[OperationContract]
		string GetImportFileDelimiter(string assetPath);

		[OperationContract]
		string[] GetImportFileColumns(string assetPath, string columnDelimiter);		
	}
}
