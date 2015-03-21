using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class ImportResult
	{
		public bool IsRunning { get; set; }
		public int ProcessedRecordsCount { get; set; }
		public long Length { get; set; }
		public long CurrentProgress { get; set; }
		public int ErrorsCount { get; set; }
		public ICollection<string> Errors { get; set; }
		public DateTime? Started { get; set; }
		public DateTime? Stopped { get; set; }
		public bool IsCancelled { get; set; }
		public bool IsStarted { get { return Started != null; } }
		public bool IsFinished { get { return Stopped != null; } }
	}
}
