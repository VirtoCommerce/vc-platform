using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Importing.Model
{
	[DataContract]
	[EntitySet("MappingItems")]
	[DataServiceKey("MappingItemId")]
	public class MappingItem: StorageEntity
	{
		public MappingItem()
        {
			_MappingItemId = GenerateNewKey();
        }

		private string _MappingItemId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string MappingItemId
		{
			get
			{
				return _MappingItemId;
			}
			set
			{
				SetValue(ref _MappingItemId, () => this.MappingItemId, value);
			}
		}

		private string _EntityColumnName;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string EntityColumnName
		{
			get
			{
				return _EntityColumnName;
			}
			set
			{
				SetValue(ref _EntityColumnName, () => this.EntityColumnName, value);
			}
		}

		private string _CsvColumnName;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
		public string CsvColumnName
		{
			get
			{
				return _CsvColumnName;
			}
			set
			{
				SetValue(ref _CsvColumnName, () => this.CsvColumnName, value);
			}
		}

		private bool _IsSystemProperty;
		[DataMember]
		public bool IsSystemProperty
		{
			get
			{
				return _IsSystemProperty;
			}
			set
			{
				SetValue(ref _IsSystemProperty, () => this.IsSystemProperty, value);
			}
		}

		private bool _isRequired;
		[DataMember]
		public bool IsRequired
		{
			get
			{
				return _isRequired;
			}
			set
			{
				SetValue(ref _isRequired, () => this.IsRequired, value);
			}
		}

		private string _CustomValue;
		[DataMember]
		public string CustomValue
		{
			get
			{
				return _CustomValue;
			}
			set
			{
				SetValue(ref _CustomValue, () => this.CustomValue, value);
			}
		}

		private string _displayName;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]		
		public string DisplayName
		{
			get
			{
				return _displayName;
			}
			set
			{
				SetValue(ref _displayName, () => this.DisplayName, value);
			}
		}

		private string _stringFormat;
		[DataMember]
		public string StringFormat
		{
			get
			{
				return _stringFormat;
			}
			set
			{
				SetValue(ref _stringFormat, () => this.StringFormat, value);
			}
		}

		private string _Locale;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Locale
		{
			get
			{
				return _Locale;
			}
			set
			{
				SetValue(ref _Locale, () => this.Locale, value);
			}
		}

		#region Navigation Properties

		private string _ImportJobId;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		[ForeignKey("ImportJob")]
		[Required]
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

		[DataMember]
		public virtual ImportJob ImportJob { get; set; }

		#endregion

	}
}
