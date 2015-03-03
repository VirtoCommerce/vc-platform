using System;
using VirtoCommerce.Foundation.Frameworks;

namespace CommerceFoundation.UI.FunctionalTests.TestHelpers
{
	public class DSRepositoryFactory<TRepository, TDSClient, TFactory> : IRepositoryFactory<TRepository>
		where TRepository : IRepository
		where TDSClient : IRepository
		where TFactory : IFactory, new()
	{
		private readonly Uri _serviceUri;

		public DSRepositoryFactory(Uri serviceUri)
		{
			_serviceUri = serviceUri;
		}

		public TRepository GetRepositoryInstance()
		{
			var retVal = (TRepository)Activator.CreateInstance(typeof(TDSClient), new object[] { _serviceUri, new TFactory(), null });
			return retVal;
		}
	}
	
	//public class DSRepositoryFactory<TRepository, TFactory> : IRepositoryFactory<TRepository> 
	//	where TRepository : IRepository
	//	where TFactory : IFactory, new()
	//{
	//	private readonly Uri _serviceUri;
	//	private readonly TFactory _factory;

	//	public DSRepositoryFactory(Uri serviceUri)
	//	{
	//		_serviceUri = serviceUri;
	//	}

	//	public TRepository GetRepositoryInstance()
	//	{
	//		var retVal = (TRepository)Activator.CreateInstance(typeof(TRepository), new object[] { _serviceUri, new TFactory(), null });
	//		return retVal;
	//	}
	//}
}
