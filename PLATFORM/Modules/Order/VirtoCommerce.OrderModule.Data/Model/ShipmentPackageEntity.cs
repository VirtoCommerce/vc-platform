using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class ShipmentPackageEntity : AuditableEntity
	{
		public ShipmentPackageEntity()
		{
			Items = new NullCollection<ShipmentItemEntity>();
		}
		[StringLength(128)]
		public string BarCode { get; set; }
		[StringLength(64)]
		public string PackageType { get; set; }

		[StringLength(32)]
		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }
		[StringLength(32)]
		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }

		public virtual ShipmentEntity Shipment { get; set; }
		public string ShipmentId { get; set; }

		public virtual ObservableCollection<ShipmentItemEntity> Items { get; set; }
	}
}
