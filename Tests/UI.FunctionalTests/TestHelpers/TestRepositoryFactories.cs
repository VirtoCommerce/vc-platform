using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Frameworks;

namespace CommerceFoundation.UI.FunctionalTests.TestHelpers
{
	public class TestAppConfigRepositoryFactory<T> : FunctionalTestBase, IRepositoryFactory<T> where T : IAppConfigRepository
	{
		private readonly TestDataService _service = null;

		public TestAppConfigRepositoryFactory(TestDataService service)
	    {
	        _service = service;
	    }

	    public T GetRepositoryInstance()
		{
			IAppConfigRepository retVal = new DSAppConfigClient(_service.ServiceUri, new AppConfigEntityFactory(), null);
			return (T) retVal;
		}
	}

	public class TestCatalogRepositoryFactory<T> : FunctionalTestBase, IRepositoryFactory<T> where T : ICatalogRepository
	{
		private readonly TestDataService _service = null;

		public TestCatalogRepositoryFactory(TestDataService service)
		{
			_service = service;
		}

		public T GetRepositoryInstance()
		{
			ICatalogRepository retVal = new DSCatalogClient(_service.ServiceUri, new CatalogEntityFactory(), null);
			return (T)retVal;
		}

	}

	public class TestPricelistRepositoryFactory<T> : FunctionalTestBase, IRepositoryFactory<T> where T : IPricelistRepository
	{
		private readonly TestDataService _service = null;

		public TestPricelistRepositoryFactory(TestDataService service)
		{
			_service = service;
		}

		public T GetRepositoryInstance()
		{
			IPricelistRepository retVal = new DSCatalogClient(_service.ServiceUri, new CatalogEntityFactory(), null);
			return (T)retVal;
		}
	}


}
