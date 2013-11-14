using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public class RefusedConfirmation : Confirmation
	{
		public bool Refused { get; set; }
	}
}
