//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VirtoCommerce.Content.Menu.Data.Repositories;
//using VirtoCommerce.Content.Menu.Data.Services;
//using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
//using Xunit;

//namespace VirtoCommerce.Content.Tests.MenuTests
//{
//	public class MenuServiceTests
//	{
//		private MenuServiceImpl GetRepository()
//		{
//			var repository = new DatabaseMenuRepositoryImpl("VirtoCommerce", new AuditableInterceptor(),
//															   new EntityPrimaryKeyGeneratorInterceptor());

//			var service = new MenuServiceImpl(repository);

//			return service;
//		}

//		//[Fact]
//		//public 
//	}
//}
