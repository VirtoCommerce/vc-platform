using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerceCMS.PagesModule.Web
{
	public class Module : IModule//, IDatabaseModule
	{
		private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void Initialize()
		{

		}

		#endregion



		#region IDatabaseModule Members

		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{

		}

		#endregion
	}
}
