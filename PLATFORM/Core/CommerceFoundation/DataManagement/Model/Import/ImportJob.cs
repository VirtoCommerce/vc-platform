using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;
namespace VirtoCommerce.Foundation.Importing.Model
{
	[DataContract]
	[EntitySet("ImportJobs")]
	[DataServiceKey("ImportJobId")]
	public class ImportJob: StorageEntity
	{
		public ImportJob()
        {
			_ImportJobId = GenerateNewKey();
        }

		private string _ImportJobId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string ImportJobId
		{
			get
			{
				return _ImportJobId;
			}
			set
			{
				SetValue(ref _ImportJobId, () => this.ImportJobId, value);
			}
		}

		private string _Name;
		[Required(ErrorMessage = "Field 'Name' is required.")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				SetValue(ref _Name, () => this.Name, value);
			}
		}
		
		/// <summary>
		/// Catalog Id
		/// </summary>
		private string _catalogId;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string CatalogId
		{
			get
			{
				return _catalogId;
			}
			set
			{
				SetValue(ref _catalogId, () => this.CatalogId, value);
			}
		}

		/// <summary>
		/// Template file path
		/// </summary>
		private string _TemplatePath;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
		public string TemplatePath
		{
			get
			{
				return _TemplatePath;
			}
			set
			{
				SetValue(ref _TemplatePath, () => this.TemplatePath, value);
			}
		}
		
		/// <summary>
		/// Maximum errors amount before import should stop
		/// </summary>
		private int _MaxErrorsCount = 0;
		[DataMember]
		public int MaxErrorsCount 
		{
			get
			{
				return _MaxErrorsCount;
			}
			set
			{
				SetValue(ref _MaxErrorsCount, () => this.MaxErrorsCount, value);
			}
		}

		/// <summary>
		/// Set import step. If every second entry should be imported set this property value to 2. Default every entry is imported.
		/// </summary>
		private int _importStep = 1;
		[DataMember]
		public int ImportStep
		{
			get
			{
				return _importStep;
			}
			set
			{
				SetValue(ref _importStep, () => this.ImportStep, value);
			}
		}

		/// <summary>
		/// Set quantity to be imported. Default import all.
		/// </summary>
		private int _importCount;
		[DataMember]
		public int ImportCount
		{
			get
			{
				return _importCount;
			}
			set
			{
				SetValue(ref _importCount, () => this.ImportCount, value);
			}
		}

		/// <summary>
		/// Line number to start import from
		/// </summary>
		private int _startIndex;
		[DataMember]
		public int StartIndex
		{
			get
			{
				return _startIndex;
			}
			set
			{
				SetValue(ref _startIndex, () => this.StartIndex, value);
			}
		}

		/// <summary>
		/// Comma, Semicolon, Tab or other
		/// </summary>
		private string _ColumnDelimiter;
		[DataMember]
		[StringLength(8, ErrorMessage = "Only 8 characters allowed.")]
		public string ColumnDelimiter
		{
			get
			{
				return _ColumnDelimiter;
			}
			set
			{
				SetValue(ref _ColumnDelimiter, () => this.ColumnDelimiter, value);
			}
		}
		
		private string _EntityImporter;
		[Required]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		[DataMember]
		public string EntityImporter // product, sku, bundle, package, dynamickit, category, association, price, customer, inventory, itemrelation
		{
			get
			{
				return _EntityImporter;
			}
			set
			{
				SetValue(ref _EntityImporter, () => this.EntityImporter, value);
			}
		}


		private string _PropertySetId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string PropertySetId
		{
			get
			{
				return _PropertySetId;
			}
			set
			{
				SetValue(ref _PropertySetId, () => this.PropertySetId, value);
			}
		}


		ObservableCollection<MappingItem> _PropertiesMap = null;
		[DataMember]
		public virtual ObservableCollection<MappingItem> PropertiesMap
		{
			get
			{
				if (_PropertiesMap == null)
					_PropertiesMap = new ObservableCollection<MappingItem>();

				return _PropertiesMap;
			}
			set
			{
				_PropertiesMap = value;
			}
		}
	}
}
