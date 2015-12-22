using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using Omu.ValueInjecter;
using VirtoCommerce.OrderModule.Data.Model;
using cartCoreModel = VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class ShipmentPackageConverter
	{
		public static ShipmentPackage ToCoreModel(this ShipmentPackageEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new ShipmentPackage();
			retVal.InjectFrom(entity);

			if (entity.Items != null)
			{
				retVal.Items = entity.Items.Select(x=>x.ToCoreModel()).ToList();
			}
			return retVal;
		}


		public static ShipmentPackageEntity ToDataModel(this ShipmentPackage package, CustomerOrderEntity orderEntity, PrimaryKeyResolvingMap pkMap)
		{
			if (package == null)
				throw new ArgumentNullException("package");

			var retVal = new ShipmentPackageEntity();
            pkMap.AddPair(package, retVal);
            retVal.InjectFrom(package);

			if(package.Items != null)
			{
				retVal.Items = new ObservableCollection<ShipmentItemEntity>(package.Items.Select(x => x.ToDataModel(orderEntity, pkMap)));
			}

			return retVal;
		}

		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this ShipmentPackageEntity source, ShipmentPackageEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");


			var patchInjectionPolicy = new PatchInjection<ShipmentPackageEntity>(x => x.PackageType, x => x.Weight, x => x.ShipmentId, x => x.Height, x => x.Length,
																			  x => x.Width, x => x.MeasureUnit, x => x.WeightUnit);

			if (!source.Items.IsNullCollection())
			{
				source.Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}

			target.InjectFrom(patchInjectionPolicy, source);
		}

	}
}
