using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Order.Model
{
	public class ShipmentPackage : AuditableEntity, IHaveDimension
	{
		public string BarCode { get; set; }
		public string PackageType { get; set; }

		public ICollection<ShipmentItem> Items { get; set; }

		#region IHaveDimension Members
		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }
		#endregion
	}
}
