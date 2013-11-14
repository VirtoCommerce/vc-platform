using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presentation.Core.Infrastructure;
using Presentation.Security.Model;

namespace VirtoCommerce.Presentation
{
	public interface IShellViewModel : IViewModel
	{
		IAuthenticationContext AuthContext { get; }
	}
}
