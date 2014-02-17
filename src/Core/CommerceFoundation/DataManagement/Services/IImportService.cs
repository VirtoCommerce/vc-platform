using System.ServiceModel;
using VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.Foundation.Importing.Services
{
	[ServiceContract]
	public interface IImportService
	{
		[OperationContract]
		void RunImportJob(string jobId, string csvFileName);
		
		[OperationContract]
		ImportResult GetImportResult(string jobId);

		[OperationContract]
		string[] GetCsvColumns(string csvFileName, string columnDelimiter);

		[OperationContract]
		string GetCsvColumnsAutomatically(string csvFileName);
		
		[OperationContract]
		string[] GetEntityImporters();

		[OperationContract]
		string[] GetSystemPropertyNames(string entityImporterName);

		[OperationContract]
		bool Exists(string csvFileName);
	}
}
