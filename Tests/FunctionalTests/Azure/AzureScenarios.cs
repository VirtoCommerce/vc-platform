using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FunctionalTests.TestHelpers;
using Microsoft.WindowsAzure.Storage;
using VirtoCommerce.Foundation.Data.Common;
using VirtoCommerce.Scheduling;
using VirtoCommerce.Scheduling.Azure;
using Xunit;
using Xunit.Sdk;

namespace FunctionalTests.Azure
{
    using VirtoCommerce.Foundation.Data.Azure.Common;

    public class AzureScenarios : FunctionalTestBase
    {
        [Fact]
        public void Can_create_local_vhd()
        {
            /*
            CloudDrive.InitializeCache(localCache.RootPath.TrimEnd('\\'), localCache.MaximumSizeInMegabytes - 50);

            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var client = storageAccount.CreateCloudBlobClient();

            var roleId = RoleEnvironment.CurrentRoleInstance.Id;
            Log(String.Format("Role ID {0}", roleId), "Information");
            var containerAddress = ContainerNameFromRoleId(roleId);
            Log(String.Format("Container {0}", containerAddress), "Information");
            var drives = client.GetContainerReference(containerAddress);

            Log("Creating drives", "Information");
            try
            {
                drives.CreateIfNotExist();
            }
            catch
            {
                Log("Failed to create drive", "Information");
            };

            try
            {
                _elasticStorageDrive = storageAccount.CreateCloudDrive(String.Format("{0}/{1}", containerAddress, "ElasticStorage.vhd"));
            }
            catch (Exception ex)
            {
                Log(String.Format("{0}:{1}", ex.Message, ex.StackTrace), "ERROR");
                throw;
            }
            Log(String.Format("ElasticStorage.vhd {0}", containerAddress), "Information");
            */
        }

        [Fact]
        public void Can_detect_azure_runtime()
        {
            var isValid = AzureCommonHelper.IsAzureEnvironment();
        }


		[Fact(Skip = "took to long")]
		public void Scheduler5MinutesTest()
		{
			JobScheduler jobScheduler=null;
			try
			{
				string m = Helper.FormatTrace("Staring the Scheduler", "SchedulerWorkerRole", "Run");
				var traceSource = new TraceSource("VirtoCommerce.ScheduleService.Trace");
				traceSource.TraceEvent(TraceEventType.Information, 0, m);
				var connectionString = ConfigurationManager.ConnectionStrings["VirtoCommerce_AzureStorage"].ConnectionString;
			
				CloudStorageAccount storageAccount;
				CloudStorageAccount.TryParse(connectionString, out storageAccount);
				var schedulerDbContext = new TestISchedulerDbContext();
				jobScheduler = new JobScheduler(
					schedulerDbContext,
					name => new TestISchedulerDbContext.TestJob(),
					"test-context",
					traceSource,
					() => storageAccount,
					new Settings("test")
					);

				Task.Run(
					() => Parallel.Invoke(
						jobScheduler.JobsManagerProcess,
						jobScheduler.SchedulerProcess));

				Thread.Sleep((5 * 60 * 1000) + 15 * 1000);


				//disable jobs, check that disablinng works - there are no tasks
				var jobs = schedulerDbContext.GetSystemJobs();
				jobs.ForEach(j => j.IsEnabled = false);
				Thread.Sleep((1 * 60 * 1000));

				Assert.Equal(schedulerDbContext.TaskSchedules.Count, 0);

				Assert.Equal(schedulerDbContext.Count, 12);
				Assert.Equal(schedulerDbContext.Error, 0);
			}
			finally
			{
				if (jobScheduler!=null)
				jobScheduler.Stop();
			}
		}
    }
}
