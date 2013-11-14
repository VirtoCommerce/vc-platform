using System;
using System.Windows;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors.Common
{
	public sealed class AttachedBehavior
	{
		public static AttachedBehavior Register(Func<DependencyObject, IBehavior> behaviorFactory)
		{
			return new AttachedBehavior(RegisterNextProperty(), behaviorFactory);
		}

		private static DependencyProperty RegisterNextProperty()
		{
			return DependencyProperty.RegisterAttached(GetNextPropertyName(), typeof(IBehavior), typeof(AttachedBehavior));
		}

		private static string GetNextPropertyName()
		{
			return "_" + Guid.NewGuid().ToString("N");
		}

		private readonly DependencyProperty _property;
		private readonly Func<DependencyObject, IBehavior> _behaviorFactory;

		internal AttachedBehavior(DependencyProperty property, Func<DependencyObject, IBehavior> behaviorFactory)
		{
			_property = property;
			_behaviorFactory = behaviorFactory;
		}

		public void Update(DependencyObject host)
		{
			var behavior = (IBehavior) host.GetValue(_property);

			if(behavior == null)
			{
				TryCreateBehavior(host);
			}
			else
			{
				UpdateBehavior(host, behavior);
			}
		}

		private void TryCreateBehavior(DependencyObject host)
		{
			var behavior = _behaviorFactory(host);

			if(behavior.IsApplicable())
			{
				behavior.Attach();

				host.SetValue(_property, behavior);

				behavior.Update();
			}
		}

		private void UpdateBehavior(DependencyObject host, IBehavior behavior)
		{
			if(behavior.IsApplicable())
			{
				behavior.Update();
			}
			else
			{
				host.ClearValue(_property);

				behavior.Detach();
			}
		}
	}
}