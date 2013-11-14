using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization
{
	public interface IVirtualList
	{
		int TotalItemsCount { get; }
		int PageSize { get; set; }
		QueuedBackgroundWorkerState LoadingState { get; }
		Exception LastLoadingError { get; }
		event EventHandler LoadingStateChanged;
		void RetryLoading();
	}

}
