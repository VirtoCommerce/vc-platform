using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Inventories.Model;


namespace VirtoCommerce.Foundation.Inventories.Factories
{
	public class InventoryEntityFactory : FactoryBase, IInventoryEntityFactory
    {
        public InventoryEntityFactory()
		{
            RegisterStorageType(typeof(Inventory), "Inventory");
		}
    }
}
