using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using VirtoCommerce.Foundation.Frameworks.CQRS.Events;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Engines
{
	public sealed class EngineHost : IDisposable
	{
		readonly ISystemObserver _observer;
		readonly IEnumerable<IEngineProcess> _serverProcesses;

		public EngineHost(ISystemObserver observer,	IEnumerable<IEngineProcess> serverProcesses)
		{
			_serverProcesses = serverProcesses;
			_observer = observer;
		}

		public Task Start(CancellationToken token, IEnumerable<string> queues)
		{
			var tasks = _serverProcesses.Select(p => p.Start(token, queues)).ToArray();
			_observer.Notify(new HostStarted());

			return Task.Factory.ContinueWhenAll(tasks, t => _observer.Notify(new HostStopped()));
		}

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}
