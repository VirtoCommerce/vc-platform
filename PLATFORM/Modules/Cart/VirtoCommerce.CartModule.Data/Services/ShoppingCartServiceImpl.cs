using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Converters;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.Infrastructure;


namespace VirtoCommerce.CartModule.Data.Services
{
	public class ShoppingCartServiceImpl : ServiceBase, IShoppingCartService
	{
		private const string _workflowName = "CartRecalculate";
		private Func<ICartRepository> _repositoryFactory;
		private readonly IEventPublisher<CartChangeEvent> _eventPublisher;
		private readonly IItemService _productService;
        private readonly IDynamicPropertyService _dynamicPropertyService;

        public ShoppingCartServiceImpl(Func<ICartRepository> repositoryFactory, IEventPublisher<CartChangeEvent> eventPublisher, IItemService productService, IDynamicPropertyService dynamicPropertyService)
		{
			_repositoryFactory = repositoryFactory;
			_eventPublisher = eventPublisher;
			_productService = productService;
            _dynamicPropertyService = dynamicPropertyService;
        }

		#region IShoppingCartService Members

		public ShoppingCart GetById(string cartId)
		{
			ShoppingCart retVal = null;
			using (var repository = _repositoryFactory())
			{
				var entity = repository.GetShoppingCartById(cartId);
				if (entity != null)
				{
					retVal = entity.ToCoreModel();

					var productIds = retVal.Items.Select(x => x.ProductId).ToArray();
					var products = _productService.GetByIds(productIds, Domain.Catalog.Model.ItemResponseGroup.ItemInfo);
					foreach (var lineItem in retVal.Items)
					{
						var product = products.FirstOrDefault(x => x.Id == lineItem.ProductId);
						if (product != null)
						{
							lineItem.Product = product;
						}
					}

					_eventPublisher.Publish(new CartChangeEvent(Platform.Core.Common.EntryState.Unchanged, retVal, retVal));
				}
			}

            if (retVal != null)
            {
                _dynamicPropertyService.LoadDynamicPropertyValues(retVal);
            }

            return retVal;
		}

		public ShoppingCart Create(ShoppingCart cart)
		{
            var pkMap = new PrimaryKeyResolvingMap();
            //Do business logic on temporary  order object
            _eventPublisher.Publish(new CartChangeEvent(Platform.Core.Common.EntryState.Added, null, cart));

			var entity = cart.ToDataModel(pkMap);
			ShoppingCart retVal = null;
			using (var repository = _repositoryFactory())
			{
				repository.Add(entity);
				CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }

            retVal = GetById(entity.Id);
			return retVal;
		}

		public void Update(ShoppingCart[] carts)
		{
			var changedCarts = new List<ShoppingCart>();
            var pkMap = new PrimaryKeyResolvingMap();

            using (var repository = _repositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
			{
				foreach (var changedCart in carts)
				{
					var origCart = GetById(changedCart.Id);

                    var productIds = changedCart.Items.Select(x => x.ProductId).ToArray();
                    var products = _productService.GetByIds(productIds, Domain.Catalog.Model.ItemResponseGroup.ItemInfo);
                    foreach (var lineItem in changedCart.Items)
                    {
                        var product = products.FirstOrDefault(x => x.Id == lineItem.ProductId);
                        if (product != null)
                        {
                            lineItem.Product = product;
                        }
                    }

					_eventPublisher.Publish(new CartChangeEvent(Platform.Core.Common.EntryState.Modified, origCart, changedCart));

					var sourceCartEntity = changedCart.ToDataModel(pkMap);
					var targetCartEntity = repository.GetShoppingCartById(changedCart.Id);
					if (targetCartEntity == null)
					{
						throw new NullReferenceException("targetCartEntity");
					}

					changeTracker.Attach(targetCartEntity);
					sourceCartEntity.Patch(targetCartEntity);
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }

            //Save dynamic properties for carts and all nested objects
            foreach(var cart in carts)
            {
                _dynamicPropertyService.SaveDynamicPropertyValues(cart);
            }
        }

        public void Delete(string[] cartIds)
		{
			using (var repository = _repositoryFactory())
			{
				foreach (var id in cartIds)
				{
					var cart = GetById(id);
                   
                    _eventPublisher.Publish(new CartChangeEvent(Platform.Core.Common.EntryState.Deleted, cart, cart));

                    _dynamicPropertyService.DeleteDynamicPropertyValues(cart);

                    var entity = repository.GetShoppingCartById(id);
					if (entity != null)
					{
						repository.Remove(entity);
					}
				}
				repository.UnitOfWork.Commit();
			}
		}

        #endregion

    }
}
