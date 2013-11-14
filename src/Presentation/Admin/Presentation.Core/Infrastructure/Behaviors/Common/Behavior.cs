using System;
using System.Windows;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors.Common
{
	public abstract class Behavior<THost> : IBehavior where THost : DependencyObject
	{
		private readonly WeakReference _hostReference;

		protected Behavior(DependencyObject host)
		{
			_hostReference = new WeakReference(host);
		}

		#region IBehavior

		public bool IsApplicable()
		{
			var host = GetHost();

			return host != null && IsApplicable(host);
		}

		public void Attach()
		{
			var host = GetHost();

			if(host != null)
			{
				Attach(host);
			}
		}

		public void Detach()
		{
			var host = GetHost();

			if(host != null)
			{
				Detach(host);
			}
		}

		public void Update()
		{
			var host = GetHost();

			if(host != null)
			{
				Update(host);
			}
		}
		#endregion

		private THost GetHost()
		{
			return (THost) _hostReference.Target;
		}

		protected virtual bool IsApplicable(THost host)
		{
			return true;
		}

		protected virtual void Attach(THost host)
		{}

		protected virtual void Detach(THost host)
		{}

		protected abstract void Update(THost host);

		protected void TryUpdate(Action<THost> update)
		{
			var host = GetHost();

			if(host != null)
			{
				update(host);
			}
		}
	}
}