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
    [EntitySet("TaskSchedules")]
    [DataServiceKey("TaskScheduleId")]
    public class TaskSchedule: StorageEntity
    {
        public TaskSchedule()
		{
            _taskScheduleId = GenerateNewKey();
		}

        private string _taskScheduleId;
		[Key]
		[StringLength(128)]
		[DataMember]
        public string TaskScheduleId
		{
            get { return _taskScheduleId; }
            set { SetValue(ref _taskScheduleId, () => TaskScheduleId, value); }
		}

		private string _systemJobId;
		[DataMember]
		[StringLength(128)]
		[Required]
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
		[Parent]
		[ForeignKey("SystemJobId")]
		public virtual SystemJob SystemJob { get; set; }

        private int _frequency;
        [DataMember]
        public int Frequency
        {
            get { return _frequency; }
            set { SetValue(ref _frequency, () => Frequency, value); }
        }

        private DateTime _nextScheduledStartTime;
        [DataMember]
        public DateTime NextScheduledStartTime
        {
            get { return _nextScheduledStartTime; }
            set { SetValue(ref _nextScheduledStartTime, () => NextScheduledStartTime, value); }
        }
    }
}
