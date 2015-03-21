using System.Data.Entity;

namespace VirtoCommerce.Foundation.Data.Infrastructure
{
	public class DbMigrationContext
	{
		private static DbMigrationContext _dbMigrationContext;

		public string DatabaseName { get; set; }

		public static DbMigrationContext Current
		{
			get
			{
				if (_dbMigrationContext == null)
				{
					_dbMigrationContext = new DbMigrationContext();
				}
				return _dbMigrationContext;
			}
			
		}
	}
}
