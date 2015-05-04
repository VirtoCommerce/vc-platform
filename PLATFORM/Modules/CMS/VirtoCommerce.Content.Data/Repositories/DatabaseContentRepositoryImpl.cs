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
	public class DatabaseContentRepositoryImpl : EFRepositoryBase, IContentRepository, IMenuRepository
	{
		public DatabaseContentRepositoryImpl()
		{
			Database.SetInitializer<DatabaseContentRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		public DatabaseContentRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, null, interceptors)
		{
			Database.SetInitializer<DatabaseContentRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<ContentItem>().HasKey(x => x.Id)
						.Property(x => x.Id);

			modelBuilder.Entity<ContentItem>().ToTable("ContentItem");

			modelBuilder.Entity<Theme>().HasKey(x => x.Id)
						.Property(x => x.Id);

			modelBuilder.Entity<Theme>().ToTable("ContentTheme");

			modelBuilder.Entity<Models.MenuLinkList>().HasKey(x => x.Id)
						.Property(x => x.Id);

			modelBuilder.Entity<Models.MenuLinkList>().ToTable("ContentMenuLinkList");

			modelBuilder.Entity<Models.MenuLink>().HasKey(x => x.Id)
						.Property(x => x.Id);

			modelBuilder.Entity<Models.MenuLink>().HasOptional(m => m.MenuLinkList)
						.WithMany(m => m.MenuLinks)
						.HasForeignKey(m => m.MenuLinkListId).WillCascadeOnDelete(true);

			modelBuilder.Entity<Models.MenuLink>().ToTable("ContentMenuLink");

			modelBuilder.Entity<ContentPage>().HasKey(x => x.Id)
						.Property(x => x.Id);

			modelBuilder.Entity<ContentPage>().ToTable("ContentPage");
		}

		public IQueryable<Models.MenuLinkList> MenuLinkLists
		{
			get { return GetAsQueryable<Models.MenuLinkList>(); }
		}

		public IQueryable<Models.MenuLink> MenuLinks
		{
			get { return GetAsQueryable<Models.MenuLink>(); }
		}

		public IQueryable<ContentItem> ContentItems
		{
			get { return GetAsQueryable<ContentItem>(); }
		}

		public IQueryable<Theme> Themes
		{
			get { return GetAsQueryable<Theme>(); }
		}

		public IQueryable<ContentPage> Pages
		{
			get { return GetAsQueryable<ContentPage>(); }
		}

		public async Task<ContentItem> GetContentItem(string path)
		{
			return await ContentItems.FirstOrDefaultAsync(p => p.Path == path);
		}

		public Task<IEnumerable<Theme>> GetThemes(string storePath)
		{
			var path = string.Format("{0}/", storePath);

			var items = Themes.Where(c => c.ThemePath.StartsWith(storePath));

			return Task.FromResult(items.AsEnumerable());
		}

		public Task<IEnumerable<ContentItem>> GetContentItems(string path, GetThemeAssetsCriteria criteria)
		{
		    var query = ContentItems.Where(i => i.Path.StartsWith(path));

			if (criteria != null && criteria.LastUpdateDate.HasValue)
			{
			    query = query.Where(i => (i.ModifiedDate.HasValue && criteria.LastUpdateDate.Value < i.ModifiedDate.Value) || (criteria.LastUpdateDate.Value < i.CreatedDate));
			}

		    return Task.FromResult(query.AsEnumerable());
		}

		public ContentPage GetPage(string path)
		{
			return Pages.FirstOrDefault(p => p.Path == path);
		}

		public IEnumerable<ContentPage> GetPages(string path, GetPagesCriteria criteria)
		{
			var query = Pages.Where(p => p.Path.StartsWith(path));

			if (criteria != null && criteria.LastUpdateDate.HasValue)
			{
				query = query.Where(i => (i.ModifiedDate.HasValue && criteria.LastUpdateDate.Value < i.ModifiedDate.Value) || (criteria.LastUpdateDate.Value < i.CreatedDate));
			}

			return query.AsEnumerable();
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

		public void SavePage(string path, ContentPage page)
		{
			var existingItem = Pages.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				existingItem.ByteContent = page.ByteContent;
				existingItem.ModifiedDate = DateTime.UtcNow;
				Update(existingItem);
			}
			else
			{
				page.Path = path;
				Add(page);
			}

			UnitOfWork.Commit();
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

		public void DeletePage(string path)
		{
			var existingItem = Pages.FirstOrDefault(p => p.Path == path);
			if (existingItem != null)
			{
				Remove(existingItem);
				UnitOfWork.Commit();
			}
		}

		public IEnumerable<Models.MenuLinkList> GetListsByStoreId(string storeId)
		{
            return MenuLinkLists.Include(s => s.MenuLinks).Where(s => s.StoreId == storeId);
		}

		public Models.MenuLinkList GetListById(string listId)
		{
			return MenuLinkLists.Include(s => s.MenuLinks).FirstOrDefault(s => s.Id == listId);
		}

		public void UpdateList(Models.MenuLinkList list)
		{
			var existingList = GetListById(list.Id);
			if (existingList != null)
			{
				existingList.Attach(list);
			}
			else
			{
				existingList = list;
			}

			AddOrUpdate(existingList);
			UnitOfWork.Commit();

			var ids = new List<string>();

			foreach (var link in existingList.MenuLinks)
			{
				if (!list.MenuLinks.Any(l => l.Id == link.Id))
				{
					ids.Add(link.Id);
				}
			}

			foreach (var id in ids)
			{
				var link = MenuLinks.First(m => m.Id == id);
				Remove(link);
			}

			UnitOfWork.Commit();
		}

		public void DeleteList(string listId)
		{
			var existingList = GetListById(listId);

			Remove(existingList);
			UnitOfWork.Commit();
		}
	}
}
