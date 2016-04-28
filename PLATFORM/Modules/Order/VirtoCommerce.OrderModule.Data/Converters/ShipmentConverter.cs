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
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Payment.Model;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class ShipmentConverter
	{
		public static Shipment ToCoreModel(this ShipmentEntity entity, IEnumerable<ShippingMethod> shippingMethods, IEnumerable<PaymentMethod> paymentMethods)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new Shipment();
			retVal.InjectFrom(entity);
			retVal.Currency = entity.Currency;

			if (entity.Addresses != null && entity.Addresses.Any())
			{
				retVal.DeliveryAddress = entity.Addresses.First().ToCoreModel();
			}
			if(entity.Discounts != null && entity.Discounts.Any())
			{
				retVal.Discount = entity.Discounts.First().ToCoreModel();
			}
			if (entity.Items != null)
			{
				retVal.Items = entity.Items.Select(x => x.ToCoreModel()).ToList();
			}
			if (entity.InPayments != null)
			{
				retVal.InPayments = entity.InPayments.Select(x => x.ToCoreModel(paymentMethods)).ToList();
			}
			if (entity.Packages != null)
			{
				retVal.Packages = entity.Packages.Select(x => x.ToCoreModel()).ToList();
			}
			retVal.TaxDetails = entity.TaxDetails.Select(x => x.ToCoreModel()).ToList();

            //Set shipment method for shipment by code
            if (shippingMethods != null)
            {
                retVal.ShippingMethod = shippingMethods.FirstOrDefault(x => String.Equals(x.Code, entity.ShipmentMethodCode, StringComparison.InvariantCultureIgnoreCase));
            }
         

            return retVal;
		}

		public static Shipment ToOrderCoreModel(this cartCoreModel.Shipment shipment)
		{
			var retVal = new Shipment();
			retVal.InjectFrom(shipment);
			retVal.Currency = shipment.Currency;
			retVal.Sum = shipment.Total;
			retVal.Tax = shipment.TaxTotal;
            retVal.DiscountAmount = shipment.DiscountTotal;
            retVal.Status = "New";
            if (shipment.DeliveryAddress != null)
			{
				retVal.DeliveryAddress = shipment.DeliveryAddress.ToCoreModel();
			}
			if(shipment.Items != null)
			{
				retVal.Items = shipment.Items.Select(x => x.ToOrderCoreModel()).ToList();
			}
            if (shipment.Discounts != null)
            {
                retVal.Discount = shipment.Discounts.Select(x => x.ToOrderCoreModel()).FirstOrDefault();
            }
            retVal.TaxDetails = shipment.TaxDetails;
			return retVal;
		}

		public static ShipmentEntity ToDataModel(this Shipment shipment, CustomerOrderEntity orderEntity, PrimaryKeyResolvingMap pkMap)
		{
			if (shipment == null)
				throw new ArgumentNullException("shipment");

			var retVal = new ShipmentEntity();
			retVal.InjectFrom(shipment);
            pkMap.AddPair(shipment, retVal);

            retVal.Currency = shipment.Currency.ToString();

		
			//Allow to empty address
			retVal.Addresses = new ObservableCollection<AddressEntity>();
			if (shipment.DeliveryAddress != null)
			{
				retVal.Addresses = new ObservableCollection<AddressEntity>(new AddressEntity[] { shipment.DeliveryAddress.ToDataModel() });
			}
			if(shipment.Items != null)
			{
				retVal.Items = new ObservableCollection<ShipmentItemEntity>(shipment.Items.Select(x=>x.ToDataModel(orderEntity, pkMap)));
			}
			if (shipment.Packages != null)
			{
				retVal.Packages = new ObservableCollection<ShipmentPackageEntity>(shipment.Packages.Select(x => x.ToDataModel(orderEntity, pkMap)));
			}
			if (shipment.TaxDetails != null)
			{
				retVal.TaxDetails = new ObservableCollection<TaxDetailEntity>();
				retVal.TaxDetails.AddRange(shipment.TaxDetails.Select(x => x.ToDataModel()));
			}
            if (shipment.Discount != null)
            {
                retVal.Discounts = new ObservableCollection<DiscountEntity>(new DiscountEntity[] { shipment.Discount.ToDataModel() });
            }
            return retVal;
		}

		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this ShipmentEntity source, ShipmentEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			source.Patch((OperationEntity)target);

            var patchInjectionPolicy = new PatchInjection<ShipmentEntity>(x => x.FulfillmentCenterId, x => x.OrganizationId, x => x.EmployeeId, x => x.Height, x => x.Length,
                                                                         x => x.Width, x => x.MeasureUnit, x => x.WeightUnit, x => x.Weight, x => x.TaxType, x => x.DiscountAmount);
            target.InjectFrom(patchInjectionPolicy, source);

			if (!source.InPayments.IsNullCollection())
			{
				source.InPayments.Patch(target.InPayments, (sourcePayment, targetPayment) => sourcePayment.Patch(targetPayment));
			}
			if(!source.Items.IsNullCollection())
			{
				source.Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}
			if (!source.Discounts.IsNullCollection())
			{
				source.Discounts.Patch(target.Discounts, new DiscountComparer(), (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
			}
			if (!source.Addresses.IsNullCollection())
			{
				source.Addresses.Patch(target.Addresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
			}
			if (!source.Packages.IsNullCollection())
			{
				source.Packages.Patch(target.Packages, (sourcePackage, targetPackage) => sourcePackage.Patch(targetPackage));
			}
			if (!source.TaxDetails.IsNullCollection())
			{
				var taxDetailComparer = AnonymousComparer.Create((TaxDetailEntity x) => x.Name);
				source.TaxDetails.Patch(target.TaxDetails, taxDetailComparer, (sourceTaxDetail, targetTaxDetail) => sourceTaxDetail.Patch(targetTaxDetail));
			}
		}
	}
}
