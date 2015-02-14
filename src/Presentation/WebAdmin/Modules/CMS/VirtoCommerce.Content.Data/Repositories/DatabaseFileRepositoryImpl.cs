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
				existingItem.Content = item.Content;
				Update(existingItem);
				UnitOfWork.Commit();
			}
			else
			{
				var steps = path.Split('/');
				for(int i = steps.Length; i > 0; i--)
				{
					var subPath = string.Join("/", SubArray(steps, i));
					existingItem = ContentItems.FirstOrDefault(p => p.Path == subPath);
					if (existingItem != null)
					{
						for (int j = i + 1; j < steps.Length; j++)
						{
							var addedItem = new ContentItem
							{
								Name = steps[j - 1],
								Path = string.Join("/", SubArray(steps, j)),
								ContentType = ContentType.Directory,
								CreatedDate = DateTime.UtcNow,
								Id = Guid.NewGuid().ToString()
							};

							existingItem.ChildContentItems.Add(addedItem);
							Update(existingItem);
							UnitOfWork.Commit();

							existingItem = addedItem;
						}

						existingItem.ChildContentItems.Add(item);
						Update(existingItem);
						UnitOfWork.Commit();

						break;
					}
				}
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

		private string[] SubArray(string[] data, int indexStep)
		{
			string[] result = new string[indexStep];
			Array.Copy(data, 0, result, 0, indexStep);
			return result;
		}
	}
}