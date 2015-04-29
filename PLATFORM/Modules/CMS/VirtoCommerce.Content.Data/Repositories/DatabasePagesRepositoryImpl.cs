using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Converters;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Content.Data.Repositories
{
	public class DatabasePagesRepositoryImpl : EFRepositoryBase, IPagesRepository
	{
		#region

		public DatabasePagesRepositoryImpl()
		{
			Database.SetInitializer<DatabasePagesRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		public DatabasePagesRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{
			Database.SetInitializer<DatabasePagesRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			#region Content Items
			modelBuilder.Entity<Page>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<Page>().ToTable("ContentPage");

			#endregion
		}

		#endregion

		#region Collections

		public IQueryable<Page> Pages
		{
			get { return GetAsQueryable<Page>(); }
		}

		#endregion

		#region IPagesRepository Methods

		public Page GetPage(string path)
		{
			return Pages.FirstOrDefault(p => p.Path == path);
		}

		public IEnumerable<Page> GetPages(string path)
		{
			return Pages.Where(p => p.Path.StartsWith(path));
		}

		public void SavePage(string path, Page page)
		{
			var existingItem = Pages.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				existingItem.Language = page.Language;
				existingItem.Content = page.Content;
				existingItem.ModifiedDate = DateTime.UtcNow;
			}
			else
			{
				existingItem = page;
			}
			AddOrUpdate(existingItem);
			UnitOfWork.Commit();
		}

		public void DeletePage(string path)
		{
			var existingItem = Pages.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				Remove(existingItem);
				UnitOfWork.Commit();
			}
		}

		#endregion
	}
}
