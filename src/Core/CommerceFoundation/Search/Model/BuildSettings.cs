using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Search.Model
{
	[DataContract]
	[EntitySet("BuildSettings")]
	[DataServiceKey("BuildSettingId")]
	public class BuildSettings : StorageEntity
	{
		private string _BuildSettingId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string BuildSettingId
		{
			get
			{
				return _BuildSettingId;
			}
			set
			{
				SetValue(ref _BuildSettingId, () => this.BuildSettingId, value);
			}
		}

		private string _DocumentType;
		[Required]
		[StringLength(64)]
		[DataMember]
		public string DocumentType
		{
			get
			{
				return _DocumentType;
			}
			set
			{
				SetValue(ref _DocumentType, () => this.DocumentType, value);
			}
		}

		private string _Scope;
		[Required]
		[StringLength(64)]
		[DataMember]
		public string Scope
		{
			get
			{
				return _Scope;
			}
			set
			{
				SetValue(ref _Scope, () => this.Scope, value);
			}
		}

		private DateTime _LastBuildDate;
		/// <summary>
		/// Gets or sets the last build date.
		/// </summary>
		/// <value>
		/// The last build date.
		/// </value>
		[Required]
		[DataMember]
		public System.DateTime LastBuildDate
		{
			get
			{
				return _LastBuildDate;
			}
			set
			{
				SetValue(ref _LastBuildDate, () => this.LastBuildDate, value);
			}
		}

		private int _Status;
		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[Required]
		[DataMember]
		public int Status
		{
			get
			{
				return _Status;
			}
			set
			{
				SetValue(ref _Status, () => this.Status, value);
			}
		}

        /*
		private string _Indexer;
		/// <summary>
		/// Gets or sets the indexer.
		/// </summary>
		/// <value>
		/// The indexer.
		/// </value>
		[Required]
		[StringLength(128)]
		[DataMember]
		public string Indexer
		{
			get
			{
				return _Indexer;
			}
			set
			{
				SetValue(ref _Indexer, () => this.Indexer, value);
			}
		}
         * */

		/// <summary>
		/// Initializes a new instance of the <see cref="BuildSettings"/> class.
		/// </summary>
		public BuildSettings()
		{
			_BuildSettingId = GenerateNewKey();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BuildSettings"/> class.
		/// </summary>
		/// <param name="scope">The scope.</param>
		/// <param name="documentType">Type of the document.</param>
		public BuildSettings(string scope, string documentType)
		{
			_BuildSettingId = GenerateNewKey();
			this.DocumentType = documentType;
			this.Scope = scope;
		}
	}
}
