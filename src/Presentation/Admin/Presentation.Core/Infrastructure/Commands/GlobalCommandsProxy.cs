using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Commands
{
	public class GlobalCommandsProxy
	{
		private static CompositeCommand _cancelOrderAllCommand = new CompositeCommand(true);
		public virtual CompositeCommand CancelAllCommand
		{
			get
			{
				return _cancelOrderAllCommand;
			}
		}

		private static CompositeCommand _saveChangesAllCommand = new CompositeCommand(true);
		public virtual CompositeCommand SaveChangesAllCommand
		{
			get
			{
				return _saveChangesAllCommand;
			}
		}
	}
}
