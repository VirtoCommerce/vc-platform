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

		public DatabaseFileRepositoryImpl(string nameOrConnectionString, string mainPath, params IInterceptor[] interceptors)
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

			modelBuilder.Entity<ContentItem>().HasOptional(x => x.ParentContentItem)
									   .WithMany(x => x.ChildContentItems)
									   .HasForeignKey(x => x.ParentContentItemId);

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

			path = path;
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);

			if(existingItem != null)
			{
				retVal = existingItem;
			}

			return retVal;
		}

		public ContentItem[] GetContentItems(string path)
		{
			var items = new List<ContentItem>();

			path = path;
			var existingItem = ContentItems.Include(p => p.ChildContentItems).FirstOrDefault(p => p.Path == path);
			if(existingItem != null)
			{
				items = existingItem.ChildContentItems.ToList();
			}

			return items.ToArray();
		}

		public void SaveContentItem(ContentItem item)
		{
			var path = item.Path;
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				Attach(existingItem);
			}
		}

		public void DeleteContentItem(ContentItem item)
		{
			var path = item.Path;
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				Remove(existingItem);
			}
		}
	}
}