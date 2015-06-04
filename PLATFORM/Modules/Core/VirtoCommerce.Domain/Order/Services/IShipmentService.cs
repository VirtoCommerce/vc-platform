using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;

namespace VirtoCommerce.Domain.Order.Services
{
	public interface IShipmentService
	{
		Shipment GetById(string id);
		Shipment Create(Shipment shipment);
		void Update(Shipment[] shipments);
		void Delete(string[] ids);
	}
}
