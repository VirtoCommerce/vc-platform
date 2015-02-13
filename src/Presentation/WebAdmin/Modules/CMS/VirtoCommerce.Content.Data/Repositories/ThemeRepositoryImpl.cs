using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Foundation.Data;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Content.Data.Repositories
{
	public class ThemeRepositoryImpl : EFRepositoryBase, IThemeRepository
	{
		public ThemeRepositoryImpl()
		{
			Database.SetInitializer<ThemeRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		public ThemeRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, null, interceptors)
		{
			Database.SetInitializer<ThemeRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			#region ShoppingCart
			modelBuilder.Entity<ThemeStoreRelation>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<ThemeStoreRelation>().ToTable("content_ThemeStoreRelation");
			#endregion
		}

		public IQueryable<Models.ThemeStoreRelation> ThemeStoreRelations
		{
			get { return GetAsQueryable<Models.ThemeStoreRelation>() ; }
		}
	}
}
