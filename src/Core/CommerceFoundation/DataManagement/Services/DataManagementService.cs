#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks.Csv;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.DataManagement.Model;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.AppConfig.Model;

#endregion

namespace VirtoCommerce.Foundation.DataManagement.Services
{
	[UnityInstanceProviderServiceBehaviorAttribute]
	public class DataManagementService : IDataManagementService
	{
		#region Const

		private const string Comma = ",";
		private const string Tab = "\t";
		private const string Semicolon = ";";

		#endregion

		#region Dependencies

		private readonly IRepositoryFactory<IImportRepository> _importJobRepositoryFactory;
		private readonly IRepositoryFactory<ICatalogRepository> _catalogRepositoryFactory;
		private readonly IRepositoryFactory<IOrderRepository> _orderRepositoryFactory;
		private readonly IAssetService _assetProvider;
		private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;

		#endregion

		#region privates

		private readonly List<IEntityImporter> _entityImporters;
		private readonly List<OperationStatus> _operationsList;

		#endregion

		#region Constructor

		public DataManagementService(
			IRepositoryFactory<IImportRepository> importRepositoryFactory, 			
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory,
			IRepositoryFactory<IOrderRepository> orderRepositoryFactory,
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
			IAssetService blobProvider
			)
		{
			_orderRepositoryFactory = orderRepositoryFactory;
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_importJobRepositoryFactory = importRepositoryFactory;
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
			_assetProvider = blobProvider;

			_entityImporters = new List<IEntityImporter>
				{
					new ItemImporter() { Name = "Product"},
					new ItemImporter() { Name = "Sku"},
					new ItemImporter() { Name = "Bundle"},
					new ItemImporter() { Name = "DynamicKit"},
					new ItemImporter() { Name = "Package"},
					new PriceImporter(_catalogRepositoryFactory.GetRepositoryInstance()),
					new AssociationImporter(_catalogRepositoryFactory.GetRepositoryInstance()),
					new RelationImporter(_catalogRepositoryFactory.GetRepositoryInstance()),
					new CategoryImporter(),
					new LocalizationImporter(),
					new TaxValueImporter(),
					new ItemAssetImporter(),
					new TaxCategoryImporter(),
					new JurisdictionImporter(),
					new JurisdictionGroupImporter(),
					new SeoImporter()
				};

			_operationsList = new List<OperationStatus>();
		}
		
		#endregion

		#region IDataManagementService
		
		public string GetImportFileDelimiter(string assetPath)
		{
			var _delimiter = string.Empty;

			string[] columns;

			using (var data = GetCsvContent(assetPath))
			{
				var csvReader = new CsvReader(data, Comma);
				columns = csvReader.ReadRow();
				_delimiter = Comma;
			}

			using (var data = GetCsvContent(assetPath))
			{
				var csvReader = new CsvReader(data, Semicolon);
				var temp = csvReader.ReadRow();
				if ((temp != null && columns!= null) && (temp.Count() > columns.Count()))
				{
					columns = temp;
					_delimiter = Semicolon;
				}
			}

			using (var data = GetCsvContent(assetPath))
			{
				var csvReader = new CsvReader(data, Tab);
				var temp = csvReader.ReadRow();
				if ((temp != null && columns!= null) && (temp.Count() > columns.Count()))
				{
					_delimiter = Tab;
				}
			}

			return _delimiter;
		}

		public string[] GetImportFileColumns(string assetPath, string columnDelimiter = null)
		{
			if (string.IsNullOrEmpty(columnDelimiter))
				columnDelimiter = GetImportFileDelimiter(assetPath);
			using (var data = GetCsvContent(assetPath))
			{
				var csvReader = new CsvReader(data, columnDelimiter);
				return csvReader.ReadRow();
			}
		}
				
		public string ImportData(string jobId, string assetPath)
		{
			var job = _importJobRepositoryFactory.GetRepositoryInstance().ImportJobs.Expand((x) => x.PropertiesMap).Where(x => x.ImportJobId == jobId).SingleOrDefault();

			if (job != null)
			{
				var operationStatus = new OperationStatus();
				_operationsList.Add(operationStatus);
				
				operationStatus.OperationState = OperationState.Initiated;

				using (var data = GetCsvContent(assetPath))
				{
					operationStatus.Length = data.BaseStream.CanSeek ? data.BaseStream.Length : 100000;
					var csvReader = new CsvReader(data, job.ColumnDelimiter);
					try
					{
						operationStatus.OperationState = OperationState.InProgress;
						operationStatus.Started = DateTime.UtcNow;

						Task.Run(() => Import(job, csvReader, operationStatus));
					}
					finally
					{
						operationStatus.Stopped = DateTime.UtcNow;
						operationStatus.OperationState = OperationState.Finished;
					}
				}

				return operationStatus.OperationId;
			}

			return null;
		}

		public string ExportData(IList<EntityType> exportEntityTypes, string assetPath, IDictionary<string, object> parameters)
		{
			var operationStatus = new OperationStatus();
			operationStatus.OperationState = OperationState.Initiated;
			
			try
			{				
				_operationsList.Add(operationStatus);
				var tasks  = new List<Task>();
				foreach (var entityType in exportEntityTypes)
				{
					operationStatus.OperationState = OperationState.InProgress;
					var tempFile = Path.GetTempFileName();
					switch (entityType)
					{
						case EntityType.Localization:
							var sourceLang = parameters["SourceLanguage"];
							var targetLang = parameters["TargetLanguage"];
							if (sourceLang == null || targetLang == null)
							{
								operationStatus.Errors.Add("Source and target languages should be provided");
								operationStatus.OperationState = OperationState.Finished;
							}
							tasks.Add(Task.Factory.StartNew(() => ExportLocalization(sourceLang.ToString(), targetLang.ToString(), operationStatus)));
							break;

						case EntityType.Catalog:
							var catalogId = parameters["CatalogId"];
							if (catalogId == null)
							{
								operationStatus.Errors.Add("Catalog id must be provided");
								operationStatus.OperationState = OperationState.Finished;
							}
							tasks.Add(Task.Factory.StartNew(() => ExportCatalog(catalogId.ToString(), operationStatus)));
							break;
					}					
				}

				Task.WaitAll(tasks.ToArray());
				operationStatus.OperationState = OperationState.Finished;
			}
			catch (Exception ex)
			{
				//var finalStatus = new StatusMessage
				//{
				//	ShortText = string.Format("Error occured during localization export to '{0}'.", filePath),
				//	StatusMessageId = id,
				//	State = StatusMessageState.Error,
				//	Details = ex.Message
				//};
			}

			return operationStatus.OperationId;
		}

		public OperationStatus GetOperationStatus(string operationId)
		{
			return _operationsList.FirstOrDefault(op => op.OperationId.Equals(operationId));
		}

		#endregion

		#region Private methods

		private void ExportCatalog(string catalogId, OperationStatus currentOperation)
		{

		}

		private void ExportLocalization(string sourceLanguage, string targetLanguage, OperationStatus currentOperation)
		{
			var tempFile = Path.GetTempFileName();

			using (var textWriter = File.CreateText(tempFile))
			{
				var csvWriter = new CsvWriter(textWriter, ",");
				csvWriter.WriteRow(new List<string> { "Name", sourceLanguage, string.Format("LanguageCode ({0})", targetLanguage), string.Format("Value - {0}", targetLanguage) },
								   false);
				
				using (var repository = _appConfigRepositoryFactory.GetRepositoryInstance())
				{
					if (repository != null)
					{
						var names = repository.Localizations.ToList().Select(x => x.Name).Distinct();
						var translateItems =
							repository.Localizations.Where(x => x.LanguageCode == targetLanguage).ToDictionary(x => x.Name);
						var originalItems =
							repository.Localizations.Where(x => x.LanguageCode == sourceLanguage).ToDictionary(x => x.Name);

						foreach (var name in names)
						{
							var isTranslateExists = translateItems.ContainsKey(name);
							//if ((IsUntranslatedOnly && !isTranslateExists) || !IsUntranslatedOnly)
							{
								var original = originalItems.ContainsKey(name)
													? originalItems[name]
													: new Localization();
								var translated = isTranslateExists
														? translateItems[name]
														: new Localization()
															{
																Name = original.Name,
																LanguageCode = targetLanguage,
																Category = original.Category
															};

								csvWriter.WriteRow(new string[] {name, original.Value, translated.Value}, true);
							}
						}
						
						
						//var statusUpdate = new StatusMessage
						//{
						//	Details = string.Format("Exported {0} of {1}.", item.index, itemsCount),
						//	StatusMessageId = id
						//};
					}
				}
			}

			Upload(tempFile);
		}

		private void Upload(string tempFilePath)
		{
			using (var info = new UploadStreamInfo())
			using (var fileStream = File.OpenRead(tempFilePath))
			{
				info.FileName = tempFilePath;
				info.FileByteStream = fileStream;
				info.Length = fileStream.Length;
				_assetProvider.Upload(info);
			}
		}

		private StreamReader GetCsvContent(string fileName)
		{
			var stream = _assetProvider.OpenReadOnly(fileName);
			if (stream.CanSeek)
				stream.Seek(0, SeekOrigin.Begin);
			return new StreamReader(stream);
		}
		
		private void Import(ImportJob job, CsvReader reader, OperationStatus currentOperation)
		{
			var skip = job.ImportStep;
			var count = job.ImportCount;
			var startIndex = job.StartIndex;

			var csvNames = GetCsvNamesAndIndexes(reader);
			if (csvNames.Count > 0)
			{
				var importer = _entityImporters.SingleOrDefault(i => i.Name == job.EntityImporter);

				if (importer != null)
				{
					IRepository rep = null;
					if (importer.Name == ImportEntityType.Localization.ToString() || importer.Name == ImportEntityType.Seo.ToString())
						rep = _appConfigRepositoryFactory.GetRepositoryInstance();

					if (rep == null)
						rep = IsTaxImport(importer.Name) ? _orderRepositoryFactory.GetRepositoryInstance() : (IRepository)_catalogRepositoryFactory.GetRepositoryInstance();

					var processed = 0;
					while (true)
					{
						try
						{
							var csvValues = reader.ReadRow();
							processed++;
							if (csvValues == null)
								break;

							if ((currentOperation.Processed < count || count <= 0) && processed >= startIndex && ((processed - startIndex) % skip == 0))
							{
								var systemValues = MapColumns(job.PropertiesMap.Where(prop => prop.IsSystemProperty), csvNames, csvValues, currentOperation);
								var customValues = MapColumns(job.PropertiesMap.Where(prop => !prop.IsSystemProperty), csvNames, csvValues, currentOperation);
								if (systemValues != null && customValues != null)
								{
									var res = importer.Import(job.CatalogId, job.PropertySetId, systemValues, customValues, rep);
									currentOperation.CurrentProgress = reader.CurrentPosition;

									if (string.IsNullOrEmpty(res))
									{
										rep.UnitOfWork.Commit();
										currentOperation.Processed++;
									}
									else
									{
										currentOperation.Errors.Add(string.Format("Row: {0}, Error: {1}", currentOperation.Processed + currentOperation.Errors.Count + 1, res));

										//check if errors amount reached the allowed errors limit if yes do not save made changes.
										if (currentOperation.Errors.Count >= job.MaxErrorsCount)
										{
											break;
										}
									}
								}
								else
								{
									//check if errors amount reached the allowed errors limit if yes do not save made changes.
									if (currentOperation.Errors != null && currentOperation.Errors.Count >= job.MaxErrorsCount)
									{
										break;
									}
								}
							}
						}
						catch (Exception e)
						{
							currentOperation.Errors.Add(e.Message + Environment.NewLine+e);

							//check if errors amount reached the allowed errors limit if yes do not save made changes.
							if (currentOperation.Errors.Count >= job.MaxErrorsCount)
							{
								break;
							}
						}
					}

					try
					{
						rep.UnitOfWork.Commit();
					}
					catch (Exception e)
					{
						currentOperation.Errors.Add(e.Message);
					}
				}
			}
			else
			{
				currentOperation.Errors.Add("File contains no rows");
			}
		}

		private static bool IsTaxImport(string entityImporter)
		{
			return entityImporter == ImportEntityType.Jurisdiction.ToString() || entityImporter == ImportEntityType.JurisdictionGroup.ToString() ||
			       entityImporter == ImportEntityType.TaxValue.ToString();
		}

		private static Dictionary<string, int> GetCsvNamesAndIndexes(CsvReader reader)
		{
			var csvNames = reader.ReadRow();

			var csvNamesAndIndexes = new Dictionary<string, int>();
			if (csvNames != null)
			{
				for (var i = 0; i < csvNames.Length; i++)
					csvNamesAndIndexes.Add(csvNames[i], i);
			}
			return csvNamesAndIndexes;
		}

		private static ImportItem[] MapColumns(IEnumerable<MappingItem> mappingItems, IReadOnlyDictionary<string, int> csvNamesAndIndexes, IList<string> csvValues, OperationStatus currentOperation)
		{
			var csvNamesAndValues = new List<ImportItem>();

			if (mappingItems != null)
			{
				foreach (var mappingItem in mappingItems)
				{					
					var importItem = new ImportItem {Name = mappingItem.EntityColumnName, Locale = mappingItem.Locale};
					if (mappingItem.CsvColumnName != null && csvNamesAndIndexes.ContainsKey(mappingItem.CsvColumnName))
					{
						var columnIndex = csvNamesAndIndexes[mappingItem.CsvColumnName];

						if (csvValues[columnIndex] != null)
							importItem.Value = csvValues[columnIndex];

						if (!string.IsNullOrEmpty(mappingItem.StringFormat))
							importItem.Value = string.Format(mappingItem.StringFormat, importItem.Value);

						if (mappingItem.IsRequired && string.IsNullOrEmpty(importItem.Value))
						{
							if (currentOperation.Errors == null)
								currentOperation.Errors = new List<string>();
							currentOperation.Errors.Add(string.Format("Row: {0}, Property: {1}, Error: {2}", currentOperation.Processed + currentOperation.Errors.Count + 1, mappingItem.DisplayName, "The value for required property not provided"));
							
							return null;
						}
					}
					else if (mappingItem.CustomValue != null)
					{
						importItem.Value = mappingItem.CustomValue;

						if (!string.IsNullOrEmpty(mappingItem.StringFormat))
							importItem.Value = string.Format(mappingItem.StringFormat, importItem.Value);
					}

					csvNamesAndValues.Add(importItem);
				}
			}

			return csvNamesAndValues.ToArray();
		}

		#endregion
	}
}
