using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoSoftware.CommerceFoundation.Data.Inventories
{
	public class SqlInventoryDatabaseInitializer : VersionDatabaseInitializer<EFInventoryRepository>
	{
		protected override string ModelId
		{
			get { return "Inventory"; }
		}

		protected override void Seed(EFInventoryRepository context)
		{
			base.Seed(context);
		}

	}
}
