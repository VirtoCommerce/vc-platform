using System;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Catalogs.Services
{
	public interface IPriceListAssignmentEvaluationContext: IEvaluationContext
	{
		string CustomerId { get; set; }
        string CatalogId { get; set; }
        string Currency { get; set; }
		DateTime CurrentDate { get; set; }
	}
}