using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.ManagementClient.Import.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Implementations
{
	public class ImportJobProgressViewModel : ViewModelBase, IImportJobProgressViewModel
	{
		#region privates
		public ImportEntity InnerItem { get; set; }
		private readonly IImportService _importService;
		
		private readonly BackgroundWorker worker;
		private int currentProgress;
		private Timer _syncTimer;
		#endregion

		public ImportJobProgressViewModel(IImportService importService, ImportEntity jobEntity)
        {
			InnerItem = jobEntity;

			_importService = importService;
			var task = new Task(() => _importService.RunImportJob(jobEntity.ImportJob.ImportJobId, jobEntity.SourceFile));
			task.Start();

			worker = new BackgroundWorker();
			worker.DoWork += DoWork;
			worker.ProgressChanged += ProgressChanged;

			Start(10);
        }

		private bool _isImported;
		public bool IsValid
		{
			get
			{
				return _isImported;
			}
			set
			{
				_isImported = value;
				OnPropertyChanged();
				OnPropertyChanged("InProgress");
			}
		}

		public bool InProgress { get { return !IsValid; } }

		/// <summary>
		/// starts new timer
		/// </summary>
		/// <param name="PeriodInMilliSeconds">period in seconds to execute handler</param>
		public void Start(int PeriodInMilliSeconds)
		{
			_syncTimer = new Timer(SyncTimerTick, null, 0, PeriodInMilliSeconds );
		}

		public void Stop()
		{
			IsValid = true;
			_syncTimer.Dispose();
		}

		private void SyncTimerTick(object sender)
		{
			if (!worker.IsBusy)
				worker.RunWorkerAsync();
		}
		
		public int CurrentProgress
		{
			get 
			{ 
				return currentProgress; 
			}

			private set
			{
				if (currentProgress != value)
				{
					currentProgress = value;
					OnPropertyChanged();
				}
			}
		}

		private ImportResult _result;
		public ImportResult Result
		{
			get
			{
				return _result;
			}
			set
			{
				_result = value;
				OnPropertyChanged();
				OnPropertyChanged("Processed");
				OnPropertyChanged("Errors");
			}
		}

		public int Processed
		{
			get
			{
				return Result != null ? Result.ProcessedRecordsCount + Result.ErrorsCount : 0;
			}
		}

		public string Errors
		{
			get
			{
				if (Result != null && Result.ErrorsCount > 0)
				{
					return Result.Errors.Cast<object>().Where(val => val != null).Aggregate(string.Empty, (current, val) => current + (val.ToString() + Environment.NewLine));
				}
				return string.Empty;
			}
		}

		private void DoWork(object sender, DoWorkEventArgs e)
		{
			Result = _importService.GetImportResult(InnerItem.ImportJob.ImportJobId);
			
			worker.WorkerReportsProgress = true;
			if (Result != null)
			{
				if (Result.IsStarted)
				{
					if (Result.IsRunning)
					{
						var progress = Convert.ToInt16(Result.CurrentProgress * 100 / Result.Length);
						worker.ReportProgress(progress);
					}
					else
					{
						worker.ReportProgress(100);
						Stop();
					}
				}
				else
				{
					worker.ReportProgress(0);
				}

			}
			
			worker.WorkerReportsProgress = false;
		}

		private void ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			CurrentProgress = e.ProgressPercentage;
		}
	}
}
