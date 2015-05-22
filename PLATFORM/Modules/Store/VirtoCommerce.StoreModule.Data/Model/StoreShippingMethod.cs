using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.StoreModule.Data.Model
{
	public class StoreShippingMethod : Entity
	{
		[Required]
		[StringLength(128)]
		public string Code { get; set; }

		public int Priority { get; set; }

		[StringLength(128)]
		public string Name { get; set; }

		public string Description { get; set; }

		[StringLength(2048)]
		public string LogoUrl { get; set; }

		public bool IsActive { get; set; }


		#region Navigation Properties

		[Required]
		[StringLength(128)]
		public string StoreId { get; set; }

		public Store Store { get; set; }

		#endregion
	}
}
