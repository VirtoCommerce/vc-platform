namespace VirtoCommerce.Foundation.Data.Azure.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class AzureLogHelper
    {
        /// <summary>
        /// Truncates the diagnostics.
        /// </summary>
        /// <param name="storageAccount">The storage account.</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="finishDateTime">The finish date time.</param>
        /// <param name="stepFunciton">The step funciton.</param>
        /// <param name="traceSource">The trace source.</param>
		[CLSCompliant(false)]
		public static void TruncateDiagnostics(CloudStorageAccount storageAccount,
			DateTime startDateTime, DateTime finishDateTime, Func<DateTime, DateTime> stepFunciton, TraceSource traceSource)
		{

			var itemsRemoved = 0;
			try
			{
				var cloudTable = storageAccount.CreateCloudTableClient().GetTableReference("WADLogsTable");

				var query = new TableQuery();
				var dt = startDateTime;
				var prevDay = dt.Day;
				var errorsCount = 0;
				while (true)
				{
				
					dt = stepFunciton(dt);
					if (dt>finishDateTime)
						break;
					if (prevDay!=dt.Day)
						traceSource.TraceEvent(TraceEventType.Error, 0, string.Format("PLEASE REMOVE TruncateDiagnostics day={0} , removed={1}", dt, itemsRemoved));
					prevDay = dt.Day;
					var l = dt.Ticks;
					var partitionKey =  "0" + l;
					query.FilterString = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.LessThan, partitionKey);
					query.Select(new string[] {});
					try
					{
						var items = cloudTable.ExecuteQuery(query).ToList();
						itemsRemoved += items.Count;
						const int chunkSize = 100;
						var chunkedList = new List<List<DynamicTableEntity>>();
						int index = 0;
						while (index < items.Count)
						{
							var count = items.Count - index > chunkSize ? chunkSize : items.Count - index;
							chunkedList.Add(items.GetRange(index, count));
							index += chunkSize;
						}
						foreach (var chunk in chunkedList)
						{
							var batches = new Dictionary<string, TableBatchOperation>();
							foreach (var entity in chunk)
							{
								var tableOperation = TableOperation.Delete(entity);
								if (batches.ContainsKey(entity.PartitionKey))
									batches[entity.PartitionKey].Add(tableOperation);
								else
									batches.Add(entity.PartitionKey, new TableBatchOperation { tableOperation });
							}

							foreach (var batch in batches.Values)
								cloudTable.ExecuteBatch(batch);
						}
						errorsCount = 0;
					}
					catch (Exception ex)
					{
						errorsCount++;
						traceSource.TraceEvent(TraceEventType.Error, 0, "ERROR ATTEMPT " + errorsCount+" "+ex);
						if (errorsCount > 10)
							throw;
						Thread.Sleep(60 * 100);
					}
					

				}


			}
			catch (Exception ex)
			{
				traceSource.TraceEvent(TraceEventType.Error, 0, ex.ToString());
				
			}
			if (itemsRemoved == 0)
				traceSource.TraceEvent(TraceEventType.Error, 0, "PLEASE REMOVE TruncateDiagnostics");
		}

//        const string LogStartTime = "StartTime";
//        const string LogEndTime = "EndTime";

//        public static LogEntry[] GetLogEntries(string[] tables, DateTime startTimeOfSearch, DateTime endTimeOfSearch)
//        {
//            // Add extra time to allow for delayed indexes, azure has a delay of 5 min in current release, make it 15 to prevent from loosing the indexes
//            startTimeOfSearch = startTimeOfSearch.AddMinutes(-15);

//            CloudBlobClient blobClient = AzureConfiguration.Instance.AzureStorageAccount.CreateCloudBlobClient();

//            List<ICloudBlob> blobList = ListLogFiles(blobClient, "table", startTimeOfSearch.ToUniversalTime(), endTimeOfSearch.ToUniversalTime());

//            List<LogEntry> entries = new List<LogEntry>();

//            int index = 0;
//            foreach (ICloudBlob blob in blobList)
//            {
//                using (Stream stream = blob.OpenRead())
//                {
//                    using (StreamReader reader = new StreamReader(stream))
//                    {
//                        string logEntry = String.Empty;
//                        while ((logEntry = reader.ReadLine()) != null)
//                        {
//                            Console.WriteLine(logEntry);
//                            LogEntry entry = new LogEntry();
//                            string[] logEntryStringArray = logEntry.Split(new char[] { ';' });

//                            DateTime date = DateTime.Parse(logEntryStringArray[1]);
//                            string operation = logEntryStringArray[2];
//                            string status = logEntryStringArray[3];

//                            if (!status.Equals("Success", StringComparison.OrdinalIgnoreCase))
//                                continue;

//                            if (operation.Equals("DeleteEntity", StringComparison.OrdinalIgnoreCase))
//                            {
//                                entry.Op = LogOperation.Deleted;
//                            }
//                            else if (operation.Equals("InsertEntity", StringComparison.OrdinalIgnoreCase))
//                            {
//                                entry.Op = LogOperation.Added;
//                            }
//                            else if (operation.Equals("UpdateEntity", StringComparison.OrdinalIgnoreCase) || operation.Equals("MergeEntity", StringComparison.OrdinalIgnoreCase))
//                            {
//                                entry.Op = LogOperation.Updated;
//                            }
//                            else
//                            {
//                                continue;
//                            }

//                            string objectKey = logEntryStringArray[12];
//                            string[] objectKeyStringArray = logEntryStringArray[12].Substring(2, logEntryStringArray[12].Length - 3).Split(new char[] { '/' });
//                            string tableName = objectKeyStringArray[1];

//                            if (!tables.Contains(tableName.ToLower()))
//                                continue;

//                            string partitionKey = objectKeyStringArray[2];

//                            string rowKey = objectKeyStringArray.Length > 3 ? objectKeyStringArray[3] : null;

//                            entry.Key = rowKey;
//                            entry.Index = index;
//                            entry.Timestamp = date;

//                            entries.Add(entry);
//                            index++;
//                        }
//                    }
//                }
//            }

//            // Now lets trim the results so it doesn't include duplicates
//            var groupedEntries = from e in entries group e by new {e.Key, e.Op} into g select g;

//            List<LogEntry> groupedLogs = new List<LogEntry>();

//            foreach (var groupEntry in groupedEntries)
//            {
//                LogEntry entry = new LogEntry();

//                int max = (from g in groupEntry select g.Index).Max();
//                var e = (from g in groupEntry where g.Index == max select g).FirstOrDefault();
//                groupedLogs.Add(e);
//            }

//            return groupedLogs.OrderBy(x=>x.Index).ToArray();
//        }


//        /// <summary>
//        /// Given service name, start time for search and end time for search, creates a prefix that can be used
//        /// to efficiently get a list of logs that may match the search criteria
//        /// </summary>
//        /// <param name="service"></param>
//        /// <param name="startTime"></param>
//        /// <param name="endTime"></param>
//        /// <returns></returns>
//        static string GetSearchPrefix(string service, DateTime startTime, DateTime endTime)
//        {
//            StringBuilder prefix = new StringBuilder("$logs/");

//            prefix.AppendFormat("{0}/", service);

//            // if year is same then add the year
//            if (startTime.Year == endTime.Year)
//            {
//                prefix.AppendFormat("{0}/", startTime.Year);
//            }
//            else
//            {
//                return prefix.ToString();
//            }

//            // if month is same then add the month
//            if (startTime.Month == endTime.Month)
//            {
//                prefix.AppendFormat("{0:D2}/", startTime.Month);
//            }
//            else
//            {
//                return prefix.ToString();
//            }

//            // if day is same then add the day
//            if (startTime.Day == endTime.Day)
//            {
//                prefix.AppendFormat("{0:D2}/", startTime.Day);
//            }
//            else
//            {
//                return prefix.ToString();
//            }

//            // if hour is same then add the hour
//            if (startTime.Hour == endTime.Hour)
//            {
//                prefix.AppendFormat("{0:D2}00", startTime.Hour);
//            }

//            return prefix.ToString();
//        }

//        /// <summary>
//        /// Given a service, start time, end time, provide list of log files
//        /// </summary>
//        /// <param name="blobClient"></param>
//        /// <param name="serviceName">The name of the service interested in</param>
//        /// <param name="startTimeForSearch">Start time for the search</param>
//        /// <param name="endTimeForSearch">End time for the search</param>
//        /// <returns></returns>
//        public static List<ICloudBlob> ListLogFiles(CloudBlobClient blobClient, string serviceName, DateTime startTimeForSearch, DateTime endTimeForSearch)
//        {
//            List<ICloudBlob> selectedLogs = new List<ICloudBlob>();

//            // form the prefix to search. Based on the common parts in start and end time, this prefix is formed
//            string prefix = GetSearchPrefix(serviceName, startTimeForSearch, endTimeForSearch);

//            Console.WriteLine("Prefix used for log listing = {0}", prefix);

//            // List the blobs using the prefix
//            IEnumerable<IListBlobItem> blobs = blobClient.ListBlobs(
//                prefix, true, BlobListingDetails.Metadata);


//            // iterate through each blob and figure the start and end times in the metadata
//            foreach (IListBlobItem item in blobs)
//            {
//                var log = item as ICloudBlob;
//                if (log != null)
//                {
//                    // we will exclude the file if the file does not have log entries in the interested time range.
//                    DateTime startTime = DateTime.Parse(log.Metadata[LogStartTime]).ToUniversalTime();
//                    DateTime endTime = DateTime.Parse(log.Metadata[LogEndTime]).ToUniversalTime();

//                    bool exclude = (startTime > endTimeForSearch || endTime < startTimeForSearch);

//                    Console.WriteLine("{0} Log {1} Start={2:U} End={3:U}.",
//                        exclude ? "Ignoring" : "Selected",
//                        log.Uri.AbsoluteUri,
//                        startTime,
//                        endTime);

//                    if (!exclude)
//                    {
//                        selectedLogs.Add(log);
//                    }
//                }
//            }

//            return selectedLogs;
//        }
 	}
}
