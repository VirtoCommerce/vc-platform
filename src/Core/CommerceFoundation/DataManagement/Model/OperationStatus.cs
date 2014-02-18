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

		private OperationState _operationState;
		public OperationState OperationState 
		{ 
			get
			{
				if (_operationState == null)
					_operationState = OperationState.Initiated;
				return _operationState;
			}
			internal set
			{
				_operationState = value;
			}
		}
		public long Processed { get; internal set; }
		public long Length { get; internal set; }
		public long CurrentProgress { get; internal set; }

		private IList _errors;
		public IList Errors 
		{ 
			get
			{
				if (_errors == null)
					_errors = new List<string>();
				return _errors;
			}
			internal set
			{
				_errors = value;
			}
		}

		public DateTime? Started { get; internal set; }
		public DateTime? Stopped { get; internal set; }
	}
}
