using System;
using System.Linq;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Order.Model.Builders
{
	public sealed class ShipmentBuilder
	{
		private readonly Shipment innerItem;
		private readonly OrderForm _orderForm;
		private readonly IOrderEntityFactory _entityFactory;
		private readonly IRepositoryFactory<IPricelistRepository> _pricelistRepository;
		private readonly PriceListClient _priceListClient;

		public ShipmentBuilder(Shipment item, OrderForm orderForm, IOrderEntityFactory entityFactory, IRepositoryFactory<IPricelistRepository> pricelistRepository, PriceListClient priceListClient)
		{
			innerItem = item;
			_orderForm = orderForm;
			_entityFactory = entityFactory;
			_pricelistRepository = pricelistRepository;
			_priceListClient = priceListClient;
		}

		public ShipmentBuilder AddShipmentItem(Item catalogItem, decimal quantity)
		{
			if (catalogItem == null)
			{
				throw new ArgumentNullException("catalogItem");
			}

			var shipmentItem = innerItem.ShipmentItems.FirstOrDefault(x => x.LineItem.CatalogItemId == catalogItem.ItemId);

			if (shipmentItem == null)
			{
				shipmentItem = CreateEntity<ShipmentItem>();
				var newLineItem = CatalogItem2LineItem(catalogItem, quantity);
				_orderForm.LineItems.Add(newLineItem);

				shipmentItem.LineItem = newLineItem;
				shipmentItem.LineItemId = newLineItem.LineItemId; // this line shouldn't be necessary. But it is.
				innerItem.ShipmentItems.Add(shipmentItem);
			}
			else
			{
				shipmentItem.LineItem.Quantity += quantity;
			}
			shipmentItem.Quantity += quantity;

			return this;
		}

		//public ShipmentBuilder RemoveShipmentItem(ShipmentItem item, decimal Quantity)
		//{
		//}

		private IPricelistRepository _PriceListRepository;
		public IPricelistRepository PriceListRepository
		{
			get { return _PriceListRepository ?? (_PriceListRepository = _pricelistRepository.GetRepositoryInstance()); }
		}

		private LineItem CatalogItem2LineItem(Item item, decimal quantity)
		{
			var retVal = CreateEntity<LineItem>();
			retVal.DisplayName = item.Name;
			retVal.OrderFormId = innerItem.OrderFormId;
			retVal.ParentCatalogItemId = null;
			retVal.CatalogItemId = item.ItemId;
			retVal.CatalogItemCode = item.Code;
			retVal.Catalog = item.CatalogId;
			var client = _priceListClient;
			var priceLists = client.GetPriceListStack(item.CatalogId, _orderForm.OrderGroup.BillingCurrency, null, false);
			var price = PriceListRepository.FindLowestPriceDirect(priceLists, item.ItemId, quantity);
			if (price != null)
			{
				retVal.ListPrice = price.Sale ?? price.List;
				retVal.PlacedPrice = price.Sale ?? price.List;
			}

			retVal.MaxQuantity = item.MaxQuantity;
			retVal.MinQuantity = item.MinQuantity;
			retVal.Quantity = quantity;

			return retVal;
		}

		private T CreateEntity<T>()
		{
			return (T)_entityFactory.CreateEntityForType(_entityFactory.GetEntityTypeStringName(typeof(T)));
		}
	}
}
