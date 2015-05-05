using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class PaymentMethod  : ValueObject<PaymentMethod>
	{
		public string GatewayCode { get; set; }
		public string Name { get; set; }
		public string IconUrl { get; set; }
	}
}
