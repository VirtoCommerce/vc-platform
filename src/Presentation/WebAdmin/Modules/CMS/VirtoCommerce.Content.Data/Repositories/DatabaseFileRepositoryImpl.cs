namespace VirtoCommerce.Content.Data.Repositories
{
	#region

	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using System.Linq;
	using VirtoCommerce.Content.Data.Models;
	using VirtoCommerce.Foundation.Data;
	using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

	#endregion

	public class DatabaseFileRepositoryImpl : EFRepositoryBase, IFileRepository
	{
		#region

		public DatabaseFileRepositoryImpl()
		{
			Database.SetInitializer<DatabaseFileRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		public DatabaseFileRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, null, interceptors)
		{
			Database.SetInitializer<DatabaseFileRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			#region Content Items
			modelBuilder.Entity<ContentItem>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<ContentItem>().ToTable("ContentItem");

			#endregion
			#region Themes
			modelBuilder.Entity<Theme>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<Theme>().ToTable("ContentTheme");
			#endregion

		}

		#endregion

		#region Public Methods and Operators

		public IQueryable<ContentItem> ContentItems
		{
			get { return GetAsQueryable<ContentItem>(); }
		}

		public IQueryable<Theme> Themes
		{
			get { return GetAsQueryable<Theme>(); }
		}

		#endregion

		public ContentItem GetContentItem(string path)
		{
			ContentItem retVal = null;
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);

			if (existingItem != null)
			{
				retVal = existingItem;
			}

			return retVal;
		}

		public IEnumerable<Theme> GetThemes(string storePath)
		{
			var path = string.Format("{0}/", storePath);

			var items = Themes.Where(c => c.ThemePath.StartsWith(storePath));

			return items;
		}

		public IEnumerable<ContentItem> GetContentItems(string path, bool loadContent = false)
		{
			return ContentItems.Where(i => i.Path.Contains(path));
		}

		public void SaveContentItem(string path, ContentItem item)
		{
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				existingItem.Content = item.Content;
				Update(existingItem);
			}
			else
			{
				item.Path = path;
				Add(item);
			}

			UnitOfWork.Commit();
		}

		public void DeleteContentItem(string path)
		{
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				Remove(existingItem);
			}

			UnitOfWork.Commit();
		}
	}
}