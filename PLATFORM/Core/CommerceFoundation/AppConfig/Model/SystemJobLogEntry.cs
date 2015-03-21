using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
	[DataContract]
	[EntitySet("SystemJobLogEntries")]
	[DataServiceKey("SystemJobLogEntryId")]
	public class SystemJobLogEntry: StorageEntity
	{
		public SystemJobLogEntry()
		{
			_systemJobLogEntryId = GenerateNewKey();
		}

		private string _systemJobLogEntryId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string SystemJobLogEntryId
		{
			get { return _systemJobLogEntryId; }
			set { SetValue(ref _systemJobLogEntryId, () => SystemJobLogEntryId, value); }
		}

		private DateTime _startTime;
		[DataMember]
		public DateTime StartTime
		{
			get { return _startTime; }
			set { SetValue(ref _startTime, () => StartTime, value); }
		}

		private DateTime? _endTime;
		[DataMember]
		public DateTime? EndTime
		{
			get { return _endTime; }
			set { SetValue(ref _endTime, () => EndTime, value); }
		}

		private string _message;
		[DataMember]
		public string Message
		{
			get { return _message; }
			set { SetValue(ref _message, () => Message, value); }
		}

		// value of EntryLevel enumeration (Fatal, Error, Info, Debug etc). Lookup EntryLevel enumeration
		private int _entryLevel;
		[DataMember]
		public int EntryLevel
		{
			get { return _entryLevel; }
			set { SetValue(ref _entryLevel, () => EntryLevel, value); }
		}

		private string _instance;
		[DataMember]
		public string Instance
		{
			get { return _instance; }
			set { SetValue(ref _instance, () => Instance, value); }
		}

		private string _taskScheduleId;
		[StringLength(128)]
		[DataMember]
		public string TaskScheduleId
		{
			get { return _taskScheduleId; }
			set { SetValue(ref _taskScheduleId, () => TaskScheduleId, value); }
		}

		private bool _multipleInstance;
		[DataMember]
		public bool MultipleInstance
		{
			get { return _multipleInstance; }
			set { SetValue(ref _multipleInstance, () => MultipleInstance, value); }
		}

		#region Navigation Properties

		private string _systemJobId;
		[StringLength(128)]
		[Required]
		[DataMember]
		public string SystemJobId
		{
			get
			{
				return _systemJobId;
			}
			set
			{
				SetValue(ref _systemJobId, () => SystemJobId, value);
			}
		}

		[DataMember]
		[ForeignKey("SystemJobId")]
		[Parent]
		public virtual SystemJob SystemJob { get; set; }

		#endregion
	}
}
