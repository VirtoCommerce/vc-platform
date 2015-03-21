namespace VirtoCommerce.Foundation.Data.Azure.CQRS
{
    using System;
    using System.Collections.Generic;

    using Microsoft.WindowsAzure.ServiceRuntime;

    using VirtoCommerce.Foundation.Frameworks.CQRS.Engines;

    using System.Threading;
    using System.Threading.Tasks;

    public abstract class AzureEngineRole : RoleEntryPoint
	{
		/// <summary>
		/// Implement in the inheriting class to configure the bus host.
		/// </summary>
		/// <returns></returns>
		protected abstract EngineHost BuildHost();
		protected abstract IEnumerable<string> QueueNames { get; }

		protected event Action<EngineHost> WhenEngineStarts;

		private EngineHost _host;
		private readonly CancellationTokenSource _source = new CancellationTokenSource();
		private Task _task;

		public override bool OnStart()
		{
			this._host = this.BuildHost();
			return base.OnStart();
		}

		public override void Run()
		{
			this._task = this._host.Start(this._source.Token, this.QueueNames);
			if (this.WhenEngineStarts != null)
			{
				this.WhenEngineStarts(this._host);
			}
			this._source.Token.WaitHandle.WaitOne();
		}

		public string InstanceName
		{
			get
			{
				return string.Format("{0}/{1}",
					RoleEnvironment.CurrentRoleInstance.Role.Name,
					RoleEnvironment.CurrentRoleInstance.Id);
			}
		}

		public override void OnStop()
		{
			this._source.Cancel(true);

			this._task.Wait(TimeSpan.FromSeconds(10));
			this._host.Dispose();

			base.OnStop();
		}
	}
}
