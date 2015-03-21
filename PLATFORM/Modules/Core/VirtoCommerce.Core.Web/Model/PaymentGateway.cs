using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CoreModule.Web.Model
{
	public class PaymentGateway
	{
		public string GatewayCode { get; set; }
		public string Description { get; set; }
		public string LogoUrl { get; set; }
	}
}
