using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;

namespace VirtoCommerce.Scheduling.Azure
{
    using VirtoCommerce.Foundation.Data.Azure.Common;

    /// <summary>
    /// Class JobScheduler.
    /// </summary>
    public class JobScheduler
    {
        /// <summary>
        /// Class CatalogedTask.
        /// </summary>
	    private class CatalogedTask : Task
	    {
            /// <summary>
            /// The cancellation token source
            /// </summary>
			public readonly CancellationTokenSource CancellationTokenSource;
            /// <summary>
            /// Initializes a new instance of the <see cref="CatalogedTask"/> class.
            /// </summary>
            /// <param name="action">The action.</param>
            /// <param name="cancellationTokenSource">The cancellation token source.</param>
			public CatalogedTask(Action action, CancellationTokenSource cancellationTokenSource)
				: base(action, cancellationTokenSource.Token)
			{
				CancellationTokenSource = cancellationTokenSource;
			}

            /// <summary>
            /// Gets or sets the system job identifier.
            /// </summary>
            /// <value>The system job identifier.</value>
		    public string SystemJobId { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether [allow multi instance].
            /// </summary>
            /// <value><c>true</c> if [allow multi instance]; otherwise, <c>false</c>.</value>
			public bool AllowMultiInstance { get; set; }
	    }

        /// <summary>
        /// The _job constructor
        /// </summary>
	    private readonly Func<Type,IJobActivity> _jobConstructor;
        /// <summary>
        /// The _cloud storage account
        /// </summary>
	    private readonly CloudStorageAccount _cloudStorageAccount;
        /// <summary>
        /// The _cloud context
        /// </summary>
		private readonly string _cloudContext;
        /// <summary>
        /// The _trace source
        /// </summary>
		private readonly TraceSource _traceSource;
        /// <summary>
        /// The _settings
        /// </summary>
	    private readonly Settings _settings;
        /// <summary>
        /// The _scheduler database context
        /// </summary>
	    private readonly ISchedulerDbContext _schedulerDbContext;
        /// <summary>
        /// Initializes a new instance of the <see cref="JobScheduler"/> class.
        /// </summary>
        /// <param name="cloudContext">The cloud context.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="storageAccountFactory">The storage account factory.</param>
        /// <param name="settings">The settings.</param>
		public JobScheduler(string cloudContext, TraceSource traceSource, Func<CloudStorageAccount> storageAccountFactory, Settings settings)
		{
			var container = Bootstrapper.Initialize();
			Func<IAppConfigRepository> repositoryFactory = () => container.Resolve<IAppConfigRepository>();
			_jobConstructor = type=>(IJobActivity)container.Resolve(type, null);
			_settings = settings;
			_cloudContext = cloudContext;
			_cloudStorageAccount = storageAccountFactory();
			_traceSource = traceSource;
			_schedulerDbContext = new SchedulerDbContext(repositoryFactory);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="JobScheduler"/> class.
        /// </summary>
        /// <param name="schedulerDbContext">The scheduler database context.</param>
        /// <param name="jobConstructor">The job constructor.</param>
        /// <param name="cloudContext">The cloud context.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="storageAccountFactory">The storage account factory.</param>
        /// <param name="settings">The settings.</param>
		public JobScheduler(ISchedulerDbContext schedulerDbContext, Func<Type,IJobActivity> jobConstructor, string cloudContext, TraceSource traceSource, Func<CloudStorageAccount> storageAccountFactory, Settings settings)
		{
			_jobConstructor = jobConstructor;
			_settings = settings;
			_cloudContext = cloudContext;
			_cloudStorageAccount = storageAccountFactory();
			_traceSource = traceSource;
			_schedulerDbContext = schedulerDbContext;
		}

        /// <summary>
        /// Schedulers the process. Get alarmed tasks, lock blob, send message to queue, remove task, add new
        /// </summary>
	    public void SchedulerProcess()
	    {
		    try
		    {
				while (true) // try to be scheduler
				{
					if (_isStoped)
						break;

					string queueNameForLog = "";

					var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
					var cloudBlobContainer = cloudBlobClient.GetContainerReference(_settings.GetContainerName());
					cloudBlobContainer.CreateIfNotExists();
					var blockBlobName = _settings.GetBlockBlobName();
					var blockBlobReference = cloudBlobContainer.GetBlockBlobReference(blockBlobName);

					if (!blockBlobReference.Exists())
					{
					    blockBlobReference.Create();
					    /* sasha: the following create blob of type page, which is incorrect
						var b = cloudBlobContainer.GetPageBlobReference(blockBlobName);
						b.Create(0);
						blockBlobReference = cloudBlobContainer.GetBlockBlobReference(blockBlobName);
                         * */
					}

					// Try to get lease.  If succeeds, then you are the scheduler
					var proposedLeaseId = Settings.GetLeaseId(Guid.NewGuid().ToString());
					string leaseId = null;
					try
					{
						leaseId = blockBlobReference.AcquireLease(TimeSpan.FromSeconds(30), proposedLeaseId);
						if (!String.IsNullOrEmpty(leaseId))
						{
							var alarmedTasks = _schedulerDbContext.GetSingletonTasksWithAlarms();
							foreach (var t in alarmedTasks)
							{
								var taskScheduleId = t.TaskScheduleId;
								var systemJobId = t.SystemJobId;
								try
								{
									var queueClient = _cloudStorageAccount.CreateCloudQueueClient();
									var queueName = queueNameForLog = _settings.GetQueueName(systemJobId);
									var queue = queueClient.GetQueueReference(queueName);

									queue.CreateIfNotExists();

									var currentMessage = queue.GetMessage();
									if (currentMessage == null)
									{
										queue.AddMessage(new CloudQueueMessage(taskScheduleId));
									}
									else
									{
										currentMessage.SetMessageContent(taskScheduleId);
										queue.UpdateMessage(currentMessage, TimeSpan.FromSeconds(0.0),
											MessageUpdateFields.Content | MessageUpdateFields.Visibility);
									}
								}
								catch (Exception ex)
								{
									_traceSource.TraceEvent(TraceEventType.Error, 0,
										String.Format("Problem creating a queue named \"{0}\"" + ex, queueNameForLog));
								}
								finally
								{
									_schedulerDbContext.UpdateTaskSchedule(systemJobId);
								}
							}
						}
					}
					catch (Exception ex)
					{
					    if (!(ex is StorageException && ex.Message.Contains("(409) Conflict")))
					    {
					        _traceSource.TraceEvent(
					            TraceEventType.Error, 0, String.Format("Problem creating a queue named \"{0}\"" + ex, queueNameForLog));
					    }
					    else
					    {
                            _traceSource.TraceEvent(
                                TraceEventType.Error, 0, String.Format("Problem with queue named \"{0}\"" + ex, queueNameForLog));

					    }
					}
					finally
					{
						try
						{
							if (leaseId != null)
								blockBlobReference.ReleaseLease(AccessCondition.GenerateLeaseCondition(leaseId));
						}
						catch (Exception ex)
						{
							_traceSource.TraceEvent(TraceEventType.Error, 0, String.Format("Error releasing lease {0}" + ex, leaseId));
						}
					}
					Thread.Sleep(Settings.RunSchedulerWakeupFrequency); // was 1 min , changed by rp to 30 sec
				}
		    }
		    catch (Exception ex)
		    {
				_traceSource.TraceEvent(TraceEventType.Error, 0, Helper.FormatException(ex, "JobScheduler", "SchedulerProcess"));
			    throw;
		    }

	    }

        /// <summary>
        /// Job manager process.
        /// </summary>
	    public void JobsManagerProcess()
        {
            try
            {
				// start "never ended" service process, at the end of "single iteration" pause it for 30 sec
				var tasks = new List<CatalogedTask>();
				while (true)  
                {
					if (_isStoped)
						break;
	                try
	                {
						// 1. remove already finished tasks
						//var canceledTasks = tasks.Where(i => i.IsCompleted).ToList();
						tasks.RemoveAll(i => i.IsCompleted);
						// 1b. remove azure queues for removed tasks
						//canceledTasks.ForEach(t => RemoveUnusedQueues(t.SystemJobId, _cloudStorageAccount, _traceSource));

						// 2. find new tasks by comparison with SystemJob (what is absent?)
						var addedJobs = new List<SystemJob>();
						var removedTaskCandidates = new List<CatalogedTask>();
						
						//var addedMultiInstanceSystemJobs = new List<SystemJob>();

						var systemJobs = _schedulerDbContext.GetSystemJobs();

                        _traceSource.TraceEvent(TraceEventType.Verbose, 0, Helper.FormatTrace(String.Format("Iterating {0} Jobs", systemJobs.Count), "JobScheduler", "SchedulerProcess-ApartmentIteration"));

						foreach (var sj in systemJobs)
						{
                            var te = tasks.SingleOrDefault(t => sj.SystemJobId == t.SystemJobId);

							if (sj.IsEnabled && sj.Period > 0)
							{

							    if (te == null)
							    {
							        addedJobs.Add(sj);
							    }
							    else
							    {
							        if (te.AllowMultiInstance != sj.AllowMultipleInstances)
							        {
							            removedTaskCandidates.Add(te);
							        }
							    }
							}
							else 
							{
							    if (te != null)
							    {
							        removedTaskCandidates.Add(te);
							    }
							}
						}

						// initiate cancel in removed
						removedTaskCandidates.ForEach(t => t.CancellationTokenSource.Cancel());

						// 3. Create  jobActivities (pop up them from container), register them in scheduler in DB and launch "listeners"
		                #region get new job, create task, start listeners (of queue)
		                var singletonTasks = new List<CatalogedTask>();

		                foreach (var j in addedJobs)
		                {
                            var systemJob = j;
			                // create listener
                            var systemJobId = systemJob.SystemJobId;

                            var multiInstance = systemJob.AllowMultipleInstances;
			                var cancellationTokenSource = new CancellationTokenSource();

		                    var t1 = new CatalogedTask(
                                () => ListenerProcess(_cloudStorageAccount, _jobConstructor, _cloudContext, () => systemJob,
									() => cancellationTokenSource.IsCancellationRequested), cancellationTokenSource)
			                {
				                SystemJobId = systemJobId,
                                AllowMultiInstance = systemJob.AllowMultipleInstances
			                };
							
							
			                singletonTasks.Add(t1);
			                t1.Start();

							// create task, SchedulerProcess will handle it (create queue, etc.) 
							_schedulerDbContext.PrepareTaskSchedule(systemJobId, multiInstance);
		                }
		                #endregion

		                var newTasks = (IEnumerable<CatalogedTask>) singletonTasks;
						tasks.AddRange(newTasks);
	                }
	                catch (Exception ex)
	                {
						_traceSource.TraceEvent(TraceEventType.Error, 0, Helper.FormatException(ex, "JobScheduler", "SchedulerProcess-ApartmentIteration"));
	                }
					Thread.Sleep(Settings.RunJobsManagerWakeupFrequency); // was 1 min , changed by rp to 30 sec
                }
            }
            catch (Exception ex)
            {
				_traceSource.TraceEvent(TraceEventType.Error, 0, Helper.FormatException(ex, "JobScheduler", "JobsManagerProcess"));
	            throw;
            }
        }


        /// <summary>
        /// Listens the process.
        /// </summary>
        /// <param name="cloudStorageAccount">The cloud storage account.</param>
        /// <param name="jobConstructor">The job constructor.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="getSystemJob">The get system job.</param>
        /// <param name="getIsCancellationRequested">The get is cancellation requested.</param>
        private void ListenerProcess(CloudStorageAccount cloudStorageAccount, Func<Type, IJobActivity> jobConstructor,  
			string instance, Func<SystemJob> getSystemJob, Func<bool> getIsCancellationRequested)
	    {
		    try
		    {
                var job = getSystemJob();

                var systemJobId = job.SystemJobId;
                var multiInstance = job.AllowMultipleInstances;
                var type = Type.GetType(job.JobClassType);
                Debug.Assert(type != null, "Can't find job with class name " + job.JobClassType);
		        var jobParameters = job.JobParameters.ToDictionary(k => k.Name.ToLowerInvariant(), v => v.Value);

				string prevTaskScheduleId = null;
				while (true) // actually we could finish it in case of configuration changed and task become disabled
				{
					if (_isStoped)
						break;
					try
					{
						var isDisabled = getIsCancellationRequested(); //_schedulerDbContext.IsDisabled(systemJobId, multiInstance);
						if (isDisabled)
							break;

						var queueReference =
							cloudStorageAccount.CreateCloudQueueClient().GetQueueReference(_settings.GetQueueName(systemJobId));
						queueReference.CreateIfNotExists();

						var message = multiInstance
							? queueReference.PeekMessage()
							: queueReference.GetMessage(TimeSpan.FromMinutes(30) /* Invisibility Timeout*/);
						string taskScheduleId = null;
						if (message != null)
						{
							taskScheduleId = message.AsString;
							Guid tmp;
							var test = Guid.TryParse(taskScheduleId, out tmp);
							Debug.Assert(test);

							if (!multiInstance)
							{
								try
								{
									queueReference.DeleteMessage(message); // remove to ensure that other machines cannot execute task
								}
								catch (Exception ex)
								{
									_traceSource.TraceEvent(TraceEventType.Error, 0, ex.ToString());
								}
							}
						}

						if (taskScheduleId != null && prevTaskScheduleId != taskScheduleId)
						{
							// EXECUTE THE JOB
							var startDateTime = DateTime.Now;
							Action<string> audit = errorMessage =>
								_schedulerDbContext.CreateSystemJobLogEntry(systemJobId, startDateTime, DateTime.Now,
									errorMessage, instance, taskScheduleId, multiInstance);
                            JobActivityTool.ControlledExecution(() => jobConstructor(type), _traceSource, audit, jobParameters);

							prevTaskScheduleId = taskScheduleId;
						}
					}
					catch (Exception ex)
					{
						_traceSource.TraceEvent(TraceEventType.Error, 0, ex.ToString());
						throw;
					}
					Thread.Sleep(Settings.QueueListenerWakeupFrequency);
				}
		    }
		    catch (Exception ex)
		    {
				_traceSource.TraceEvent(TraceEventType.Error, 0, Helper.FormatException(ex, "JobScheduler", "ListenerProcess"));
		    }
	    }

        //private void RemoveUnusedQueues(string systemJobId, CloudStorageAccount storageAccount, TraceSource traceSource)
        //{
        //    try
        //    {
        //        var queueClient = storageAccount.CreateCloudQueueClient();
        //        var queueName = _settings.GetQueueName(systemJobId);
        //        var queue = queueClient.GetQueueReference(queueName);
        //        queue.DeleteIfExists();
        //    }
        //    catch (Exception ex)
        //    {
        //        traceSource.TraceEvent(TraceEventType.Error, 0,
        //           "Problem removing unused blog with queue: " + ex);
        //    }
        //}

        /// <summary>
        /// The _is stoped
        /// </summary>
	    private bool _isStoped;// = false;
        /// <summary>
        /// Stops this instance.
        /// </summary>
	    public void Stop()
	    {
		    _isStoped = true;
	    }
    }
}