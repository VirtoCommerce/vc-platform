using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Data.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
	public class PlatformRepositoryImpl : EFRepositoryBase, IPlatformRepository
	{
		public PlatformRepositoryImpl()
		{
			Database.SetInitializer<PlatformRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		public PlatformRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{
			Database.SetInitializer<PlatformRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			#region Settings
			modelBuilder.Entity<SettingEntity>().HasKey(x => x.Id).Property(x => x.Id)
												.HasColumnName("SettingId");
				

			modelBuilder.Entity<SettingEntity>().ToTable("Setting");
			#endregion

			#region SettingValue
			modelBuilder.Entity<SettingValueEntity>().HasKey(x => x.Id)
					    .Property(x => x.Id).HasColumnName("SettingValueId");

			modelBuilder.Entity<SettingValueEntity>().HasRequired(x => x.Setting)
									   .WithMany(x => x.SettingValues)
									   .HasForeignKey(x => x.SettingId);

			modelBuilder.Entity<SettingValueEntity>().ToTable("SettingValue");
			#endregion
		
		}

		#region IPlatformRepository Members

		public IQueryable<SettingEntity> Settings
		{
			get { return GetAsQueryable<SettingEntity>(); }
		}

		#endregion
	}
}
