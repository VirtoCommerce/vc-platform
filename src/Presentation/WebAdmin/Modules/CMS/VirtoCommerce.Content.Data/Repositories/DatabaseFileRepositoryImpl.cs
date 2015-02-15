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
		}

		#endregion

		#region Public Methods and Operators

		public IQueryable<ContentItem> ContentItems
		{
			get { return GetAsQueryable<ContentItem>(); }
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

		public Theme[] GetThemes(string storePath)
		{
			var path = string.Format("{0}/", storePath);

			var items = ContentItems.Where(c => c.Path.StartsWith(storePath)).ToArray();

			return items.Select(c => c.Path.Split('/')[1])
				.Distinct()
				.Select(c => new Theme { Name = c, ThemePath = string.Format("{0}/{1}", storePath, c) })
				.ToArray();
		}

		public ContentItem[] GetContentItems(string path)
		{
			return ContentItems.Where(i => i.Path.Contains(path)).ToArray();
		}

		public void SaveContentItem(ContentItem item)
		{
			var path = item.Path;
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				existingItem.Content = item.Content;
				Update(existingItem);
			}
			else
			{
				Add(item);
			}

			UnitOfWork.Commit();
		}

		public void DeleteContentItem(ContentItem item)
		{
			var path = item.Path;
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				Remove(existingItem);
			}

			UnitOfWork.Commit();
		}
	}
}