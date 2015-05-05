using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class Weight : ValueObject<Weight>
	{
		public string Unit { get; set; }
		public decimal Value { get; set; }

	}
}
