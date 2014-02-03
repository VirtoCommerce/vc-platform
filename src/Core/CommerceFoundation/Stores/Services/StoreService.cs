using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;


namespace VirtoCommerce.Foundation.Stores.Services
{
	[UnityInstanceProviderServiceBehaviorAttribute]
	public partial class StoreService : IStoreService
	{
		protected IStoreRepository StoreRepository;
		protected IFulfillmentCenterRepository FulfillmentCenterRepository;

		public StoreService(IStoreRepository repository)
		{
			StoreRepository = repository;
			CurrentDateTime = DateTime.Now;
		}

		public DateTime CurrentDateTime
		{
			get;
			set;
		}

        public IQueryable<Store> Stores
        {
            get
            {
                return StoreRepository.Stores;
            }
        } 
	}
}
