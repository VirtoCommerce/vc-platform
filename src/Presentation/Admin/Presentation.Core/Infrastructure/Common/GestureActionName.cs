using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
	public enum GestureActionName
	{
		add,
		delete,
		refresh,
		save,
		close,
		navigateback,
		find
	}

	public struct ActionBinding
	{
		public GestureActionName Name;
		public DelegateCommandBase Command;
		public object CommandParameter;
		public KeyGesture[] Gestures;
	}
}
