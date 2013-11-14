using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
	[DataContract]
	[EntitySet("SystemJobs")]
	[DataServiceKey("SystemJobId")]
	public class SystemJob: StorageEntity
	{
		public SystemJob()
		{
			_systemJobId = GenerateNewKey();
		}

		private string _systemJobId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string SystemJobId
		{
			get { return _systemJobId; }
			set { SetValue(ref _systemJobId, () => SystemJobId, value); }
		}

		private string _name;
		[DataMember]
		[StringLength(128)]
		public string Name
		{
			get { return _name; }
			set { SetValue(ref _name, () => Name, value); }
		}

		private string _description;
		[DataMember]
		[StringLength(256)]
		public string Description
		{
			get { return _description; }
			set { SetValue(ref _description, () => Description, value); }
		}

		#region not trackable fields
		private string _jobClassType;
		[Required]
		[DataMember]
		[StringLength(1024)]
		public string JobClassType
		{
			get
			{
				return _jobClassType;
			}
			set
			{
				SetValue(ref _jobClassType, () => JobClassType, value);
			}
		}

		private string _jobAssemblyPath;
		[DataMember]
		[StringLength(1024)]
		public string JobAssemblyPath
		{
			get
			{
				return _jobAssemblyPath;
			}
			set
			{
				SetValue(ref _jobAssemblyPath, () => JobClassType, value);
			}
		}

		private bool _loadFromGac;
		[Required]
		[DataMember]
		public bool LoadFromGac
		{
			get
			{
				return _loadFromGac;
			}
			set
			{
				SetValue(ref _loadFromGac, () => LoadFromGac, value);
			}
		}
		#endregion

		private bool _isEnabled;
		/// <summary>
		/// Gets or sets a value indicating whether this instance is locale dependant. If true, the locale must be specified for the values.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is locale dependant; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool IsEnabled
		{
			get
			{
				return _isEnabled;
			}
			set
			{
				SetValue(ref _isEnabled, () => IsEnabled, value);
			}
		}

		/// <summary>
		/// Job execution schedule in CRON format
		/// </summary>
		private string _schedule;
		[DataMember]
		[StringLength(64)]
		public string Schedule
		{
			get
			{
				return _schedule;
			}
			set
			{
				SetValue(ref _schedule, () => Schedule, value);
			}
		}

		private int _priority;
		[DataMember]
		public int Priority
		{
			get
			{
				return _priority;
			}
			set
			{
				SetValue(ref _priority, () => Priority, value);
			}
		}

		/// <summary>
		/// Job execution period in seconds
		/// </summary>
		private int _period;
		[DataMember]
		public int Period
		{
			get
			{
				return _period;
			}
			set
			{
				SetValue(ref _period, () => Period, value);
			}
		}

		/// <summary>
		/// Last execution time.
		/// </summary>
		private DateTime? _lastExecuted;
		[DataMember]
		public DateTime? LastExecuted
		{
			get
			{
				return _lastExecuted;
			}
			set
			{
				SetValue(ref _lastExecuted, () => LastExecuted, value);
			}
		}

		/// <summary>
		/// Next execution time.
		/// </summary>
		private DateTime? _nextExecute;
		[DataMember]
		public DateTime? NextExecute
		{
			get
			{
				return _nextExecute;
			}
			set
			{
				SetValue(ref _nextExecute, () => NextExecute, value);
			}
		}

		private bool _allowMultipleInstances = true;
		[DataMember]
		public bool AllowMultipleInstances
		{
			get { return _allowMultipleInstances; }
			set { SetValue(ref _allowMultipleInstances, () => AllowMultipleInstances, value); }
		}

		#region Navigation Properties

		ObservableCollection<JobParameter> _jobParameters;
		[DataMember]
		public virtual ObservableCollection<JobParameter> JobParameters
		{
			get
			{
				return _jobParameters ?? (_jobParameters = new ObservableCollection<JobParameter>());
			}
		}

		ObservableCollection<SystemJobLogEntry> _logEntries;
		[DataMember]
		public virtual ObservableCollection<SystemJobLogEntry> LogEntries
		{
			get
			{
				return _logEntries ?? (_logEntries = new ObservableCollection<SystemJobLogEntry>());
			}
		}

		ObservableCollection<TaskSchedule> _taskSchedules;
		[DataMember]
		public virtual ObservableCollection<TaskSchedule> TaskSchedules
		{
			get
			{
				return _taskSchedules ?? (_taskSchedules = new ObservableCollection<TaskSchedule>());
			}
		}

		#endregion
	}
}
