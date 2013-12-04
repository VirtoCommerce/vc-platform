using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
	public static class InputBindings
	{
		private static InputBindingCollection _inputBindings;
		private static Stack<KeyBinding> _stash;
		private static int _currentVM;
		private static ActionBinding[] _entriesAll;

		//static InputBindings()
		//{
		//}

		//public InputBindings(InputBindingCollection bindingsOwner)
		//{
		//	_inputBindings = bindingsOwner;
		//	_stash = new Stack<KeyBinding>();
		//}

		public static void Initialize(InputBindingCollection inputBindings, ActionBinding[] entries)
		{
			_inputBindings = inputBindings;
			_entriesAll = entries;

			_stash = new Stack<KeyBinding>();
		}

		public static void RegisterCommands(IEnumerable<ActionBinding> entriesSupported, int objectID)
		{
			if (_inputBindings == null || _entriesAll == null)
				return;

			if (_currentVM != objectID)
			{
				_currentVM = objectID;
				DeregisterCommands();

				_entriesAll.Where(x => entriesSupported.Any(y => y.Name == x.Name))
					.ToList().ForEach(x =>
						{
							var supportedItem = entriesSupported.First(y => y.Name == x.Name);
							foreach (var gesture in x.Gestures)
							{
								var binding = new KeyBinding(supportedItem.Command, gesture);

								_stash.Push(binding);
								_inputBindings.Add(binding);
							}
						});
			}
		}

		private static void DeregisterCommands()
		{
			foreach (var keyBinding in _stash)
				_inputBindings.Remove(keyBinding);
		}
	}
}