namespace VirtoCommerce.Content.Data.Repositories
{
	#region

	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using System.Linq;
	using System.Threading.Tasks;
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

		public async Task<ContentItem> GetContentItem(string path)
		{
			ContentItem retVal = null;
			var existingItem = await ContentItems.FirstOrDefaultAsync(p => p.Path == path);

			if (existingItem != null)
			{
				retVal = existingItem;
			}

			return retVal;
		}

		public Task<IEnumerable<Theme>> GetThemes(string storePath)
		{
			var path = string.Format("{0}/", storePath);

			var items = Themes.Where(c => c.ThemePath.StartsWith(storePath));

			return Task.FromResult(items.AsEnumerable());
		}

		public Task<IEnumerable<ContentItem>> GetContentItems(string path, GetThemeAssetsCriteria criteria)
		{
			if (criteria.LastUpdateDate.HasValue)
			{
				return Task.FromResult(ContentItems.Where(i => i.Path.Contains(path) && i.ModifiedDate.HasValue ? criteria.LastUpdateDate.Value < i.ModifiedDate.Value : criteria.LastUpdateDate.Value < i.CreatedDate).AsEnumerable());
			}
			else
			{
				return Task.FromResult(ContentItems.Where(i => i.Path.Contains(path)).AsEnumerable());
			}
		}

		public Task<bool> SaveContentItem(string path, ContentItem item)
		{
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				existingItem.ByteContent = item.ByteContent;
				Update(existingItem);
			}
			else
			{
				item.Path = path;
				Add(item);
			}

			var steps = path.Split('/');
			if (steps.Length > 2)
			{
				var themePath = string.Join("/", steps[0], steps[1]);
				var theme = Themes.FirstOrDefault(t => t.Id == themePath);
				if (theme != null)
				{
					theme.ModifiedDate = DateTime.UtcNow;
					Update(theme);
				}
				else
				{
					theme = new Theme();
					theme.Id = themePath;
					theme.ThemePath = themePath;
					theme.Name = steps[1];
					theme.CreatedDate = DateTime.UtcNow;
					Add(theme);
				}
			}

			UnitOfWork.Commit();

			return Task.FromResult(true);
		}

		public Task<bool> DeleteContentItem(string path)
		{
			var existingItem = ContentItems.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				Remove(existingItem);
			}

			var steps = path.Split('/');
			if (steps.Length > 2)
			{
				var themePath = string.Join("/", steps[0], steps[1]);
				var theme = Themes.FirstOrDefault(t => t.Id == themePath);
				if (theme != null)
				{
					theme.ModifiedDate = DateTime.UtcNow;
					Update(theme);
				}
			}

			UnitOfWork.Commit();

			return Task.FromResult(true);
		}


		public async Task<bool> DeleteTheme(string path)
		{
			var existingTheme = await Themes.FirstOrDefaultAsync(t => t.Id == path);
			if (existingTheme != null)
			{
				Remove(existingTheme);
				var contentItems = ContentItems.Where(c => c.Path.StartsWith(path));
				foreach (var item in contentItems)
				{
					Remove(item);
				}

				UnitOfWork.Commit();
			}

			return true;
		}
	}
}