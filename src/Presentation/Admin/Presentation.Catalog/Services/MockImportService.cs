using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtoSoftware.CommerceFoundation.Catalogs.Repositories;
using VirtoSoftware.CommerceFoundation.Data.Catalogs;
using VirtoSoftware.CommerceFoundation.Frameworks;
using VirtoSoftware.CommerceFoundation.Frameworks.Csv;
using VirtoSoftware.CommerceFoundation.Importing.Model;
using VirtoSoftware.CommerceFoundation.Importing.Repositories;
using VirtoSoftware.CommerceFoundation.Importing.Services;

namespace Presentation.Catalog.Services
{
    public class MockImportService : IImportService
    {
		private IImportRepository _importJobRepository;
		private ICatalogRepository _catalogRepository;

		private List<IEntityImporter> _entityImporters;
		private Dictionary<string, ImportResult> _importResults;

		#region .ctor

		public MockImportService(IImportRepository importJobRepository)
		{
			_importJobRepository = importJobRepository;
			_catalogRepository = null;

			_entityImporters = new List<IEntityImporter>();
			_entityImporters.Add(new ItemImporter());
			_entityImporters.Add(new PriceImporter());
			_entityImporters.Add(new AssociationImporter());
			_entityImporters.Add(new CategoryImporter());

			_importResults = new Dictionary<string, ImportResult>();
		}

		#endregion

		#region IImportService

		public string[] GetEntityImporters()
		{
			return _entityImporters.Select(i => i.Name).ToArray();
		}

		public string[] GetPropertySets(string entityImporterName)
		{
			return _entityImporters.First(i => i.Name == entityImporterName).PropertySets;
		}

		public string[] GetSystemPropertyNames(string entityImporterName)
		{
			return _entityImporters.First(i => i.Name == entityImporterName).SystemPropertyNames;
		}

		public string[] GetCustomPropertyNames(string entityImporterName, string propertySetId)
		{
			return _entityImporters.First(i => i.Name == entityImporterName).GetCustomPropertyNames(propertySetId);
		}

		public string[] GetCsvColumns(string csvFileName, char columnDelimiter)
		{
			using (var data = GetCsvContent(csvFileName))
			{
				CsvReader csvReader = new CsvReader(data, columnDelimiter);
				return csvReader.ReadRow();
			}
		}

		public StreamReader CsvContent(string fileName)
		{
			//var rep = _container.Resolve<IAssetRepository>();
			//var folderItem = rep.GetFolderItemById(fileName);
			//MemoryStream theMemStream = new MemoryStream();
			//theMemStream.Write(folderItem.SmallData, 0, folderItem.SmallData.Length);
			//theMemStream.Position = 0;
			return new StreamReader(fileName);
		}

		public void RunImportJob(string jobId, string csvFileName)
		{
			ImportJob job = ImportJobs.First(x => x.ImportJobId == jobId);

			if (job != null)
			{
				ImportResult result = new ImportResult();
				_importResults[jobId] = result;



				using (var data = GetCsvContent(csvFileName))
				{
					result.Length = data.BaseStream.Length;
					CsvReader csvReader = new CsvReader(data, job.ColumnDelimiter);
					try
					{
						result.IsRunning = true;
						result.Started = DateTime.Now;

						Import(job, csvReader, result);
					}
					finally
					{
						result.Stopped = DateTime.Now;
						result.IsRunning = false;
					}
				}
			}
		}

		public ImportResult GetImportResult(string jobId)
		{
			ImportResult result = null;

			if (_importResults.ContainsKey(jobId))
				result = _importResults[jobId];

			return result;
		}

		#endregion

		#region IImportRepository

		public IQueryable<ImportJob> ImportJobs
		{
			get { return _importJobRepository.ImportJobs; }
		}
		
		public IQueryable<ColumnMapping> ColumnMappings
		{
			get { return _importJobRepository.ColumnMappings; }
		}

		public StreamReader GetCsvContent(string fileName)
		{
			return _importJobRepository.CsvContent(fileName);
		}

		#endregion


		private void Import(ImportJob job, CsvReader reader, ImportResult result)
		{
			
			Dictionary<string, int> csvNames = GetCsvNamesAndIndexes(reader);

			ColumnMapping columnMapping = ColumnMappings.First(x => x.ColumnMappingId == job.ColumnMappingId);
			IEntityImporter importer = _entityImporters.FirstOrDefault(i => i.Name == job.EntityImporter);

			while (true)
			{
				try
				{
					string[] csvValues = reader.ReadRow();

					if (csvValues == null)
						break;

					var systemValues = MapColumns(columnMapping.SystemPropertiesMap, csvNames, csvValues);					
					var customValues = MapColumns(columnMapping.CustomPropertiesMap, csvNames, csvValues);

					importer.Import(job.ContainerId, columnMapping.PropertySetId, systemValues, customValues, _catalogRepository);
					
					result.CurrentProgress = reader.CurrentPosition;
					result.ProcessedRecordsCount++;
				}
				catch(Exception e)
				{
					result.ErrorsCount++;
					if (result.Errors == null)
						result.Errors = new List<string>();
					result.Errors.Add(e.Message);

					//check if errors amount reached the allowed errors limit if yes do not save made changes.
					if (result.ErrorsCount >= job.MaxErrorsCount)
					{
						_catalogRepository.UnitOfWork.RollbackChanges();
						break;
					}
				}
			}

			if (result.ErrorsCount < job.MaxErrorsCount)
			{
				_catalogRepository.UnitOfWork.Commit();
			}
			
		}

		private static Dictionary<string, int> GetCsvNamesAndIndexes(CsvReader reader)
		{
			string[] csvNames = reader.ReadRow();

			Dictionary<string, int> csvNamesAndIndexes = new Dictionary<string, int>();

			for (int i = 0; i < csvNames.Length; i++)
				csvNamesAndIndexes.Add(csvNames[i], i);

			return csvNamesAndIndexes;
		}

		private static ImportItem[] MapColumns(IEnumerable<MappingItem> mappingItems, Dictionary<string, int> csvNamesAndIndexes, string[] csvValues)
		{
			List<ImportItem> csvNamesAndValues = new List<ImportItem>();

			if (mappingItems != null)
			{
				foreach (var mappingItem in mappingItems)
				{
					if (mappingItem.CsvColumnName != null && csvNamesAndIndexes.ContainsKey(mappingItem.CsvColumnName))
					{
						int columnIndex = csvNamesAndIndexes[mappingItem.CsvColumnName];

						ImportItem importItem = new ImportItem();

						importItem.Name = mappingItem.EntityColumnName;
						importItem.Value = csvValues[columnIndex];

						csvNamesAndValues.Add(importItem);
					}
				}
			}

			return csvNamesAndValues.ToArray();
		}
            
    }
}
