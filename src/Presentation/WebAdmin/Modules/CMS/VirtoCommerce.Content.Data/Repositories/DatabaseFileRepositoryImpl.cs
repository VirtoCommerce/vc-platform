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
		private string _mainPath;

		#region

		public DatabaseFileRepositoryImpl(string mainPath)
		{
			Database.SetInitializer<DatabaseFileRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;

			_mainPath = mainPath;
		}

		public DatabaseFileRepositoryImpl(string nameOrConnectionString, string mainPath, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, null, interceptors)
		{
			Database.SetInitializer<DatabaseFileRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;

			_mainPath = mainPath;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			#region ShoppingCart
			modelBuilder.Entity<ContentItem>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<ContentItem>().HasOptional(x => x.ParentContentItem)
									   .WithMany(x => x.ChildContentItems)
									   .HasForeignKey(x => x.ParentContentItemId);

			modelBuilder.Entity<ContentItem>().ToTable("content_ContentItem");
			#endregion
		}

		#endregion

		#region Public Methods and Operators

		public System.Linq.IQueryable<ContentItem> ContentItems
		{
			get { return GetAsQueryable<ContentItem>(); }
		}

        #endregion

		public ContentItem GetContentItem(string themePath, string path)
		{
			ContentItem retVal = null;

			path = GetFullPath(themePath, path);
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);

			if(existingItem != null)
			{
				retVal = existingItem;
			}

			return retVal;
		}

		public ContentItem[] GetContentItems(string themePath, string path)
		{
			var items = new List<ContentItem>();

			path = GetFullPath(themePath, path);
			var existingItem = ContentItems.Include(p => p.ChildContentItems).FirstOrDefault(p => p.Path == path);
			if(existingItem != null)
			{
				items = existingItem.ChildContentItems.ToList();
			}

			return items.ToArray();
		}

		public void SaveContentItem(string themePath, ContentItem item)
		{
			var path = GetFullPath(themePath, item.Path);
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				Attach(existingItem);
			}
		}

		public void DeleteContentItem(string themePath, ContentItem item)
		{
			var path = GetFullPath(themePath, item.Path);
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				Remove(existingItem);
			}
		}

		private string GetFullPath(string themePath, string path)
		{
			return string.Format("{0}{1}{2}", _mainPath, themePath, path);
		}
	}
}