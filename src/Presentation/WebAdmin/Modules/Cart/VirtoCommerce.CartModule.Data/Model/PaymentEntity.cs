using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CartModule.Data.Model
{
	public class PaymentEntity : Entity
	{
		public PaymentEntity()
		{
			Addresses = new NullCollection<AddressEntity>();
		}

		[StringLength(128)]
		public string OuterId { get; set; }
		[Required]
		[StringLength(64)]
		public string Currency { get; set; }
		[StringLength(64)]
		public string GatewayCode { get; set; }
		[Column(TypeName = "Money")]
		public decimal Amount { get; set; }
		[StringLength(1024)]
		public string Purpose { get; set; }

		public virtual ShoppingCartEntity ShoppingCart { get; set; }
		public string ShoppingCartId { get; set; }

		public virtual ObservableCollection<AddressEntity> Addresses { get; set; }
	}
}
