using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtoCommerce.Foundation.DataManagement.Model
{
	public class OperationStatus
	{
		private string _operationId;
		public string OperationId
		{
			get
			{
				if (string.IsNullOrEmpty(_operationId))
					_operationId = Guid.NewGuid().ToString();
				return _operationId;
			}
		}

		public OperationState OperationState { get; internal set; }
		public long Processed { get; internal set; }
		public long Length { get; internal set; }
		public long CurrentProgress { get; internal set; }

		private IList _errors;
		public IList Errors 
		{ 
			get { return _errors ?? (_errors = new List<string>()); }
			internal set
			{
				_errors = value;
			}
		}

		public DateTime? Started { get; internal set; }
		public DateTime? Stopped { get; internal set; }
	}
}
