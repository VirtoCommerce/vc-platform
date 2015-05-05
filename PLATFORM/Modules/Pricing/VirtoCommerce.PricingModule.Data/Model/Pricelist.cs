using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.PricingModule.Data.Model
{
	public class Pricelist : AuditableEntity
	{
		public Pricelist()
		{
			Prices = new ObservableCollection<Price>();
		}

		[Required]
		[StringLength(128)]
		public string Name { get; set; }
		
		[StringLength(512)]
		public string Description { get; set; }
	
		[Required]
		[StringLength(64)]
		public string Currency { get; set; }


		#region Navigation Properties

		public virtual ObservableCollection<Price> Prices { get; set; }
	
		#endregion
	}
}
