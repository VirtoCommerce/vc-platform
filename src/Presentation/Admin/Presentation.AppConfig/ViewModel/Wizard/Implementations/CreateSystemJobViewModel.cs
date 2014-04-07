using System.Collections.Generic;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.SystemJobs.Implementations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Implementations
{
	public class CreateSystemJobViewModel : WizardContainerStepsViewModel, ICreateSystemJobViewModel
	{
		public CreateSystemJobViewModel(IViewModelsFactory<ISystemJobOverviewStepViewModel> overviewVmFactory, IViewModelsFactory<ISystemJobParametersStepViewModel> parametersVmFactory, SystemJob item)
		{
			RegisterStep(overviewVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item)));
			RegisterStep(parametersVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item)));
		}
    }

	public class SystemJobOverviewStepViewModel : SystemJobEditViewModel, ISystemJobOverviewStepViewModel
	{
        public SystemJobOverviewStepViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, IViewModelsFactory<IAddParameterViewModel> vmFactory, SystemJob item)
            : base(repositoryFactory, entityFactory, vmFactory, item)
		{
		}

		public override bool IsValid
		{
			get
			{
				bool result = InnerItem != null && !string.IsNullOrEmpty(InnerItem.Name)
					&& !string.IsNullOrEmpty(InnerItem.JobClassType);
				return result;
			}
		}

		public override string Description
		{
			get
			{
				return "Enter System job general information.".Localize();
			}
		}
	}

	public class SystemJobParametersStepViewModel : SystemJobEditViewModel, ISystemJobParametersStepViewModel
	{
        public SystemJobParametersStepViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, IViewModelsFactory<IAddParameterViewModel> vmFactory, SystemJob item)
            : base(repositoryFactory, entityFactory, vmFactory, item)
		{
		}

		public override bool IsValid
		{
			get
			{
				return true;
			}
		}

		public override string Description
		{
			get
			{
				return "Enter System job parameters.".Localize();
			}
		}
	}

	//public class SystemJobScheduleStepViewModel : SystemJobEditViewModel, ISystemJobScheduleStepViewModel
	//{
	//	public SystemJobScheduleStepViewModel(IAppConfigEntityFactory entityFactory, IAddParameterViewModel parameterVM)
	//		: base(entityFactory, parameterVM, null, false)
	//	{
	//		public override bool IsValid
	//		{
	//			get
	//			{
	//				return true;
	//			}
	//		}

	//		public override string Description
	//		{
	//			get
	//			{
	//				return "Setup System job schedule.";
	//			}
	//		}
	//	}
	//}

	#region commented out class to use for cron enabled scheduling in future releases
	/*public class SystemJobScheduleStepViewModel : CreateSystemJobContainerStepsViewModel, ISystemJobScheduleStepViewModel
	{
		public SystemJobScheduleStepViewModel()
			: base()
		{
		}

		public void InitilizeStep()
		{
			daysOfWeek.Clear();
			StartDate = null;
			StartTime = null;
			IsRecurring = false;
			UpdateDaysOfWeek();
		}

		private void UpdateDaysOfWeek()
		{
			OnPropertyChanged("DaySunday");
			OnPropertyChanged("DayMonday");
			OnPropertyChanged("DayThursday");
			OnPropertyChanged("DayWednesday");
			OnPropertyChanged("DayTuesday");
			OnPropertyChanged("DayFriday");
			OnPropertyChanged("DaySaturday");
		}


		public string[] RecurrancePeriods
		{
			get
			{
				return new string[] { "Daily", "Weekly", "Monthly", "Yearly" };
			}
		}

		public bool IsWeekly
		{
			get
			{
				return IsRecurring && SelectedRecurrancePeriod == "Weekly";
			}
		}

		#region DaysOfWeek properties

		private List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();

		private bool _daySunday;
		public bool DaySunday
		{
			get
			{
				return _daySunday;
			}
			set
			{
				_daySunday = value;
				if (value && !daysOfWeek.Contains(DayOfWeek.Sunday))
					daysOfWeek.Add(DayOfWeek.Sunday);
				else if (!value)
					daysOfWeek.Remove(DayOfWeek.Sunday);
				OnPropertyChanged();
				UpdateScheduleCronValue();
			}
		}

		private bool _dayMonday;
		public bool DayMonday
		{
			get
			{
				return _dayMonday;
			}
			set
			{
				_dayMonday = value;
				if (value && !daysOfWeek.Contains(DayOfWeek.Monday))
					daysOfWeek.Add(DayOfWeek.Monday);
				else if (!value)
					daysOfWeek.Remove(DayOfWeek.Monday);
				OnPropertyChanged();
				UpdateScheduleCronValue();
			}
		}

		private bool _dayTuesday;
		public bool DayTuesday
		{
			get
			{
				return _dayTuesday;
			}
			set
			{
				_dayTuesday = value;
				if (value && !daysOfWeek.Contains(DayOfWeek.Tuesday))
					daysOfWeek.Add(DayOfWeek.Tuesday);
				else if (!value)
					daysOfWeek.Remove(DayOfWeek.Tuesday);
				OnPropertyChanged();
				UpdateScheduleCronValue();
			}
		}

		private bool _dayWednesday;
		public bool DayWednesday
		{
			get
			{
				return _dayWednesday;
			}
			set
			{
				_dayWednesday = value;
				if (value && !daysOfWeek.Contains(DayOfWeek.Wednesday))
					daysOfWeek.Add(DayOfWeek.Wednesday);
				else if (!value)
					daysOfWeek.Remove(DayOfWeek.Wednesday);
				OnPropertyChanged();
				UpdateScheduleCronValue();
			}
		}

		private bool _dayThursday;
		public bool DayThursday
		{
			get
			{
				return _dayThursday;
			}
			set
			{
				_dayThursday = value;
				if (value && !daysOfWeek.Contains(DayOfWeek.Thursday))
					daysOfWeek.Add(DayOfWeek.Thursday);
				else if (!value)
					daysOfWeek.Remove(DayOfWeek.Thursday);
				OnPropertyChanged();
				UpdateScheduleCronValue();
			}
		}

		private bool _dayFriday;
		public bool DayFriday
		{
			get
			{
				return _dayFriday;
			}
			set
			{
				_dayFriday = value;
				if (value && !daysOfWeek.Contains(DayOfWeek.Friday))
					daysOfWeek.Add(DayOfWeek.Friday);
				else if (!value)
					daysOfWeek.Remove(DayOfWeek.Friday);
				OnPropertyChanged();
				UpdateScheduleCronValue();
			}
		}

		private bool _daySaturday;
		public bool DaySaturday
		{
			get
			{
				return _daySaturday;
			}
			set
			{
				_daySaturday = value;
				if (value && !daysOfWeek.Contains(DayOfWeek.Saturday))
					daysOfWeek.Add(DayOfWeek.Saturday);
				else if (!value)
					daysOfWeek.Remove(DayOfWeek.Saturday);
				OnPropertyChanged();
				UpdateScheduleCronValue();
			}
		}

		#endregion

		private bool _isRecurring;
		public bool IsRecurring
		{
			get
			{
				return _isRecurring;
			}
			set
			{
				_isRecurring = value;
				SelectedRecurrancePeriod = RecurrancePeriods[0];
				SetCurrentDayOfWeek();
				OnPropertyChanged();
			}
		}

		private int _repeatPeriod;
		public int RepeatPeriod
		{
			get
			{
				return _repeatPeriod;
			}
			set
			{
				_repeatPeriod = value;
				OnPropertyChanged();
				UpdateScheduleCronValue();
			}
		}

		private DateTime? _startDate;
		public DateTime? StartDate
		{
			get
			{
				return _startDate ?? DateTime.Now;
			}
			set
			{
				_startDate = value;
				OnPropertyChanged();
				UpdateScheduleCronValue();
			}
		}

		private DateTime? _startTime;
		public DateTime? StartTime
		{
			get
			{
				return _startTime ?? DateTime.Now;
			}
			set
			{
				_startTime = value;
				OnPropertyChanged();
				UpdateScheduleCronValue();
			}
		}

		private string _recurrancePeriod;
		public string SelectedRecurrancePeriod
		{
			get
			{
				return _recurrancePeriod;
			}
			set
			{
				_recurrancePeriod = value;
				OnPropertyChanged();
				OnPropertyChanged("PeriodText");
				OnPropertyChanged("IsWeekly");
				UpdateScheduleCronValue();
			}
		}

		public string PeriodText
		{
			get
			{
				switch (SelectedRecurrancePeriod)
				{
					case "Daily":
						return "day(s)";
					case "Weekly":
						return "week(s)";
					case "Monthly":
						return "month(s)";
					default:
						return "year(s)";
				}
			}
		}

		private void SetCurrentDayOfWeek()
		{
			switch (StartDate.Value.DayOfWeek)
			{
				case DayOfWeek.Sunday:
					DaySunday = true;
					break;
				case DayOfWeek.Monday:
					DayMonday = true;
					break;
				case DayOfWeek.Tuesday:
					DayTuesday = true;
					break;
				case DayOfWeek.Wednesday:
					DayWednesday = true;
					break;
				case DayOfWeek.Thursday:
					DayThursday = true;
					break;
				case DayOfWeek.Friday:
					DayFriday = true;
					break;
				case DayOfWeek.Saturday:
					DaySaturday = true;
					break;

			}
		}
		
		private void UpdateScheduleCronValue()
		{
			if (IsRecurring)
			{
				switch (SelectedRecurrancePeriod)
				{
					case "Daily":
						var dsb = CronScheduleBuilder
							.DailyAtHourAndMinute(StartTime.Value.Hour, StartTime.Value.Minute);

						var dtrig = (ICronTrigger)TriggerBuilder
							.Create()
							.WithSchedule(dsb)
							.Build();

						InnerItem.Schedule = dtrig.CronExpressionString;
						break;
					case "Weekly":
						if (daysOfWeek.Count > 0)
						{
							var wsb = CronScheduleBuilder
								.AtHourAndMinuteOnGivenDaysOfWeek(StartTime.Value.Hour, StartTime.Value.Minute, daysOfWeek.ToArray());
							//.WeeklyOnDayAndHourAndMinute(StartDate.Value.DayOfWeek, StartTime.Value.Hour, StartTime.Value.Minute);

							var wtrig = (ICronTrigger)TriggerBuilder
								.Create()
								.WithSchedule(wsb)
								.Build();

							InnerItem.Schedule = wtrig.CronExpressionString;
						}
						else
						{
							InnerItem.Schedule = string.Format("{0} {1} {2} {3} {4}",
								StartTime.Value.Minute,
								StartTime.Value.Hour,
								StartDate.Value.Day,
								StartDate.Value.Month,
								StartDate.Value.Year);
						}
						break;
					case "Monthly":
						var msb = CronScheduleBuilder
							.MonthlyOnDayAndHourAndMinute(StartDate.Value.Day, StartTime.Value.Hour, StartTime.Value.Minute);

						var mtrig = (ICronTrigger)TriggerBuilder
							.Create()
							.WithSchedule(msb)
							.Build();

						InnerItem.Schedule = mtrig.CronExpressionString;
						break;
					default:
						var sb = CronScheduleBuilder
							.WeeklyOnDayAndHourAndMinute(DayOfWeek.Monday, 12, 0);

						var trig = (ICronTrigger)TriggerBuilder
							.Create()
							.WithSchedule(sb)
							.Build();

						InnerItem.Schedule = trig.CronExpressionString;
						break;
				}
			}
			else
			{
				InnerItem.Schedule = string.Format("{0} {1} {2} {3} {4}",
					StartTime.Value.Minute,
					StartTime.Value.Hour,
					StartDate.Value.Day,
					StartDate.Value.Month,
					StartDate.Value.Year);
			}
			OnPropertyChanged("InnerItem");
		}
	}*/
	#endregion
}
