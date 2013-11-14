using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[EntitySet("ShoppingCarts")]
	public class ShoppingCart : OrderGroup
	{
	}
}
