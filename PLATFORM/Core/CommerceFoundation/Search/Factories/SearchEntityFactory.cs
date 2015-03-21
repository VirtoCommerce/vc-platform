using System;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Search.Model;


namespace VirtoCommerce.Foundation.Search.Factories
{
	public class SearchEntityFactory : FactoryBase, ISearchEntityFactory
	{
        public SearchEntityFactory()
		{
			RegisterStorageType(typeof(BuildSettings), "BuildSettings");
		}
	}
}

