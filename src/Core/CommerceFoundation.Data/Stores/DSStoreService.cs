using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Stores
{
    [JsonSupportBehavior]
	public class DSStoreService : DServiceBase<EFStoreRepository>
	{
	}
}
