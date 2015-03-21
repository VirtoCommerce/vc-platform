using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace VirtoCommerce.Foundation.Frameworks.CQRS
{
	public interface IEngineProcess : IDisposable
	{
		/// <summary>
		/// Is executed by the engine on initialization phase.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Creates and starts a long-running task, given the cancellation token to stop it.
		/// </summary>
		/// <param name="token">The cancellation token.</param>
		/// <returns>Long-running task instance</returns>
		Task Start(CancellationToken token, IEnumerable<string> queues);
	}
}
