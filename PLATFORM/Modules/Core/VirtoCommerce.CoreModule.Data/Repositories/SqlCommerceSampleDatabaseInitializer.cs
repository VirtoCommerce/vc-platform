using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using VirtoCommerce.CoreModule.Data.Model;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;
using coreModel = VirtoCommerce.Domain.Store.Model;

namespace VirtoCommerce.CoreModule.Data.Repositories
{
	public class SqlCommerceSampleDatabaseInitializer : SetupDatabaseInitializer<CommerceRepositoryImpl, VirtoCommerce.CoreModule.Data.Migrations.Configuration>
	{
		private readonly bool _reduced;

		readonly string[] _files =
		{
			"SeoUrlKeyword.sql"
		
		};
		public SqlCommerceSampleDatabaseInitializer(bool reduced = false)
		{
			_reduced = reduced;
		}

		protected override void Seed(CommerceRepositoryImpl context)
		{
			CreateFulfillmentCenter(context);
			ExcecuteSqlScripts(context);
			base.Seed(context);
		}

		private void ExcecuteSqlScripts(CommerceRepositoryImpl context)
		{
			foreach (var file in _files)
			{
				ExecuteSqlScriptFile(context, file, "Commerce");
			}
		}

		public static void CreateFulfillmentCenter(CommerceRepositoryImpl context)
		{
			var center = new FulfillmentCenter
				{
					Id = "vendor-fulfillment",
					Name = "Vendor Fulfillment Center",
					PickDelay = 30,
					Line1 = "1232 Wilshire Blvd",
					MaxReleasesPerPickBatch = 20,
					PostalCode = "90234",
					StateProvince = "California",
					City = "Los Angeles",
					CountryName = "United States",
					CountryCode = "USA",
					DaytimePhoneNumber = "3232323232"
				};

			context.Add(center);
			context.UnitOfWork.Commit();
		}

	
	}
}
