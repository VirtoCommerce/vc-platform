using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
	[DataContract]
	[EntitySet("JobParameters")]
	[DataServiceKey("JobParameterId")]
	public class JobParameter: StorageEntity
	{
		public JobParameter()
		{
			_jobParameterId = GenerateNewKey();
		}

		private string _jobParameterId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string JobParameterId
		{
			get { return _jobParameterId; }
			set { SetValue(ref _jobParameterId, () => JobParameterId, value); }
		}

		private string _name;
		[DataMember]
		[StringLength(128)]
		public string Name
		{
			get { return _name; }
			set { SetValue(ref _name, () => Name, value); }
		}

		private string _alias;
		[DataMember]
		[StringLength(128)]
		public string Alias
		{
			get { return _alias; }
			set { SetValue(ref _alias, () => Alias, value); }
		}

		private string _value;
		[DataMember]
		[StringLength(128)]
		public string Value
		{
			get { return _value; }
			set { SetValue(ref _value, () => Value, value); }
		}

		private bool _isRequired;
		[DataMember]
		public bool IsRequired
		{
			get { return _isRequired; }
			set { SetValue(ref _isRequired, () => IsRequired, value); }
		}

		#region Navigation Properties

		private string _SystemJobId;

		[StringLength(128)] 
		[Required]
		[DataMember]
		public string SystemJobId
		{
			get
			{
				return _SystemJobId;
			}
			set
			{
				SetValue(ref _SystemJobId, () => SystemJobId, value);
			}
		}

		[DataMember]
		[ForeignKey("SystemJobId")]
		[Parent]
		public virtual SystemJob SystemJob{get;set;}		

		#endregion
	}
}
