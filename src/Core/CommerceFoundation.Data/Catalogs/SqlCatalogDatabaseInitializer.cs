using System;
using System.IO;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Catalogs
{
	public class SqlCatalogDatabaseInitializer : SetupDatabaseInitializer<EFCatalogRepository, Migrations.Configuration>
	{
		private readonly string _location = string.Empty;

		public SqlCatalogDatabaseInitializer()
		{
		}

		public SqlCatalogDatabaseInitializer(string location)
		{
			_location = location;
		}

		public SqlCatalogDatabaseInitializer(string location, string connectionString)
			: base(connectionString)
		{
			_location = location;
		}


		protected override string ReadSql(string fileName, string modelName)
		{
			string path = string.Format("{0}/Catalogs/Data/{1}", _location, fileName);

			if (File.Exists(path))
				return File.ReadAllText(path);

			path = string.Format("{0}/Catalogs/Data/{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

			if (File.Exists(path))
				return File.ReadAllText(path);

			path = string.Format("{0}/bin/Catalogs/Data/{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

			if (File.Exists(path))
				return File.ReadAllText(path);

			path = string.Format("{0}/App_Data/Catalogs/Data/{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

			if (File.Exists(path))
				return File.ReadAllText(path);

			path = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

			if (File.Exists(path))
				return File.ReadAllText(path);

			return base.ReadSql(fileName, modelName);
		}
	}
}
