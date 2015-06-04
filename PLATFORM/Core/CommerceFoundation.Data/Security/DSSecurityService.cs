using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Security
{
    [JsonSupportBehavior]
	public class DSSecurityService : DServiceBase<EFSecurityRepository>
	{
	}
}
