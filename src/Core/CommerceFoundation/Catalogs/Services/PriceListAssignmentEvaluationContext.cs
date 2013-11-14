using System;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Catalogs.Services
{
	[DataContract]
	public sealed class PriceListAssignmentEvaluationContext : BaseEvaluationContext, IPriceListAssignmentEvaluationContext
	{
		#region IPriceListAssignementContext Members

		public string CustomerId { get; set; }		
		public DateTime CurrentDate { get; set; }
        public string CatalogId { get; set; }
        public string Currency { get; set; }

		#endregion
						
	}
}
