using System;
using System.Data.Entity.Validation;
using System.Data.Services;
using System.Data.Services.Common;
using System.Text;
using VirtoCommerce.Foundation.Marketing.Factories;
using System.Linq;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Marketing
{
    [JsonSupportBehavior]
	public class DSDynamicContentService : DServiceBase<EFDynamicContentRepository>
	{
	}
}
