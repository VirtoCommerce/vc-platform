using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.OrderModule.Model
{
	public class Shipment : AuditableEntityBase<Shipment>, INumbered
	{
		public Shipment(string number)
		{
			Number = number;
		}
		#region INumbered Members

		public string Number
		{
			get;
			set;
		}

		#endregion
	}
}
