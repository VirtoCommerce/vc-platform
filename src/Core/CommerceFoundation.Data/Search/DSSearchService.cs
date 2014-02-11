using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services;
using VirtoCommerce.Foundation.Search.Factories;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Search
{
    [JsonSupportBehavior]
	public class DSSearchService : DServiceBase<EFSearchRepository>
    {
    }
}
